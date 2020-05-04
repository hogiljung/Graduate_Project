//
// original code is UnityMainThreadDispatcher.cs
// https://github.com/aws/aws-sdk-net/blob/master/sdk/src/Core/Amazon.Runtime/Pipeline/_unity/UnityMainThreadDispatcher.cs
//
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Threading;
using UnityEngine;
using Logger = Amazon.Runtime.Internal.Util.Logger;
using Amazon.Util.Internal.PlatformServices;
using Amazon.Util.Internal;
using UnityEngine.Networking;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Unity2017.3 aws lambda sdk error workaround.
    ///
    /// How to use.
    /// UnityInitializer.AttachToGameObject(gameObject);
    /// gameObject.AddComponent<CustomUnityMainThreadDispatcher>();
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class CustomUnityMainThreadDispatcher : MonoBehaviour
    {
        private Logger _logger;
        private float _nextUpdateTime;
        private float _updateInterval = 0.1f;
        private NetworkStatus _currentNetworkStatus;

        private FieldInfo _unityWebRequestInstanceFieldInfo;
        private Queue<IUnityHttpRequest> _unityHttpRequestQueue = new Queue<IUnityHttpRequest>();

        /// <summary>
        /// This method is called called when the script instance is
        /// being loaded.
        /// </summary>
        public void Awake()
        {
            _logger = Logger.GetLogger(this.GetType());
            // Call the method to process requests at a regular interval.
            _nextUpdateTime = Time.unscaledTime;
            _nextUpdateTime += _updateInterval;
            //
            Type unityWebRequestWrapper = typeof(UnityWebRequestWrapper);
            _unityWebRequestInstanceFieldInfo =
                unityWebRequestWrapper.GetField("unityWebRequestInstance",
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// This method is called as often as possible.
        /// </summary>
        void Update()
        {
            while (true)
            {
                IUnityHttpRequest request = UnityRequestQueue.Instance.DequeueRequest();
                if (request == null)
                {
                    break;
                }
                _unityHttpRequestQueue.Enqueue(request);
            }

            if (Time.unscaledTime >= _nextUpdateTime)
            {
                ProcessRequests();
                _nextUpdateTime += _updateInterval;
            }
        }

        /// <summary>
        /// This method processes queued web requests.
        /// </summary>
        void ProcessRequests()
        {
            if (_unityHttpRequestQueue.Count == 0)
            {
                return;
            }
            // Make a network call for queued requests on the main thread
            IUnityHttpRequest request = _unityHttpRequestQueue.Dequeue();
            if (request != null)
            {
                StartCoroutine(InvokeRequest(request));
            }
        }

        /// <summary>
        /// Makes a single web request using the WWW or UnityWebRequest API.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>IEnumerator which indicated if the operation is pending.</returns>
        IEnumerator InvokeRequest(IUnityHttpRequest request)
        {
            // Fire the request
            var nr = ServiceFactory.Instance.GetService<INetworkReachability>() as Amazon.Util.Internal.PlatformServices.NetworkReachability;
            if (nr.NetworkStatus != NetworkStatus.NotReachable)
            {
                bool isWwwRequest = (request as UnityWwwRequest) != null;
                if (isWwwRequest)
                {
                    var wwwRequest = new WWW((request as UnityWwwRequest).RequestUri.AbsoluteUri, request.RequestContent, request.Headers);
                    if (wwwRequest == null)
                    {
                        yield return null;
                    }
                    bool uploadCompleted = false;
                    while (!wwwRequest.isDone)
                    {
                        var uploadProgress = wwwRequest.uploadProgress;
                        if (!uploadCompleted)
                        {
                            request.OnUploadProgressChanged(uploadProgress);

                            if (uploadProgress == 1)
                                uploadCompleted = true;
                        }
                        yield return null;
                    }
                    request.WwwRequest = wwwRequest;
                    request.Response = new UnityWebResponseData(wwwRequest);
                }
                else
                {
                    var unityRequest = request as UnityRequest;
                    if (unityRequest == null)
                    {
                        yield return null;
                    }

                    var unityWebRequest = new UnityWebRequestWrapper(
                                              unityRequest.RequestUri.AbsoluteUri,
                                              unityRequest.Method);

                    unityWebRequest.DownloadHandler = new DownloadHandlerBufferWrapper();

                    if (request.RequestContent != null && request.RequestContent.Length > 0)
                        unityWebRequest.UploadHandler = new UploadHandlerRawWrapper(request.RequestContent);

                    bool uploadCompleted = false;

                    foreach (var header in request.Headers)
                    {
                        unityWebRequest.SetRequestHeader(header.Key, header.Value);
                    }

                    // this bug workaround code
                    // https://github.com/aws/aws-sdk-net/issues/820
                    SetUnityWebRequestChunkedTransfer(unityWebRequest, false);

                    var operation = unityWebRequest.Send();
                    while (!operation.isDone)
                    {
                        var uploadProgress = operation.progress;
                        if (!uploadCompleted)
                        {
                            request.OnUploadProgressChanged(uploadProgress);

                            if (uploadProgress == 1)
                                uploadCompleted = true;
                        }
                        yield return null;
                    }
                    request.WwwRequest = unityWebRequest;
                    request.Response = new UnityWebResponseData(unityWebRequest);
                }
            }
            else
            {
                request.Exception = new WebException("Network Unavailable", WebExceptionStatus.ConnectFailure);
            }

            if (request.IsSync)
            {
                // For synchronous calls, signal the wait handle 
                // so that the calling thread which waits on the wait handle
                // is unblocked.
                if (request.Response != null && !request.Response.IsSuccessStatusCode)
                {
                    request.Exception = new HttpErrorResponseException(request.Response);
                }
                request.WaitHandle.Set();
            }
            else
            {
                if (request.Response != null && !request.Response.IsSuccessStatusCode)
                {
                    request.Exception = new HttpErrorResponseException(request.Response);
                }
                // For asychronous calls invoke the callback method with the
                // state object that was originally passed in.

                // Invoke the callback method for the request on the thread pool
                // after the web request is executed. This callback triggers the 
                // post processing of the response from the server.
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    try
                    {
                        request.Callback(request.AsyncResult);
                    }
                    catch (Exception exception)
                    {
                        // The callback method (HttpHandler.GetResponseCallback) and 
                        // subsequent calls to handler callbacks capture any exceptions
                        // thrown from the runtime pipeline during post processing.

                        // Log the exception, in case we get an unhandled exception 
                        // from the callback.
                        _logger.Error(exception,
                            "An exception was thrown from the callback method executed from"
                            + "UnityMainThreadDispatcher.InvokeRequest method.");

                    }
                });
            }
        }

        private void SetUnityWebRequestChunkedTransfer(UnityWebRequestWrapper instance, bool chunkedTransfer)
        {
            UnityWebRequest unityWebRequest =
                _unityWebRequestInstanceFieldInfo.GetValue(instance) as UnityWebRequest;

            unityWebRequest.chunkedTransfer = chunkedTransfer;
        }
    }
}