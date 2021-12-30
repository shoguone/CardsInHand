using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CardsInHand.Scripts.Web
{
    public static class WebRequestProvider
    {
        private class WebRequestMonoBehaviour : MonoBehaviour { }

        private static WebRequestMonoBehaviour _webRequestHelper;

        private static WebRequestMonoBehaviour WebRequestHelperSingleton
        {
            get
            {
                if (_webRequestHelper == null)
                {
                    var gameObject = new GameObject("WebRequestHelperSingleton");
                    _webRequestHelper = gameObject.AddComponent<WebRequestMonoBehaviour>();
                }

                return _webRequestHelper;
            }
        }


        public static void Get(string url, Action<string> onError, Action<string> onSuccess)
        {
            WebRequestHelperSingleton.StartCoroutine(GetCoroutine(url, onError, onSuccess));
        }

        public static void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
        {
            WebRequestHelperSingleton.StartCoroutine(GetTextureCoroutine(url, onError, onSuccess));
        }

        private static IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSuccess)
        {
            using var uwr = UnityWebRequest.Get(url);
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError
                || uwr.result == UnityWebRequest.Result.DataProcessingError
                || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(uwr.error);
            }
            else
            {
                onSuccess(uwr.downloadHandler.text);
            }
        }

        private static IEnumerator GetTextureCoroutine(string url, Action<string> onError, Action<Texture2D> onSuccess)
        {
            using var uwr = UnityWebRequestTexture.GetTexture(url);
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError
                || uwr.result == UnityWebRequest.Result.DataProcessingError
                || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(uwr.error);
            }
            else
            {
                var handlerTexture = uwr.downloadHandler as DownloadHandlerTexture;
                if (handlerTexture == null)
                {
                    onError($"{nameof(UnityWebRequest.downloadHandler)} is not {nameof(DownloadHandlerTexture)}!");
                }
                else
                {
                    onSuccess(handlerTexture.texture);
                }
            }
        }

    }
}
