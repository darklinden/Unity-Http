using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Http
{
    public static class RequestExtensionGet
    {
        public static IEnumerator Get<TRecv>(
            this Request<TRecv> request,
            string Url,
            string Data = null,
            int Timeout = 10) where TRecv : class
        {
            var tmpUrl = UrlUtil.GetUrl(Url, Data);

            UnityWebRequest getRequest = null;
            if (typeof(TRecv) == typeof(Texture2D))
            {
                getRequest = UnityWebRequestTexture.GetTexture(tmpUrl);
            }
            else if (typeof(TRecv) == typeof(AssetBundle))
            {
                getRequest = UnityWebRequestAssetBundle.GetAssetBundle(tmpUrl);
            }
            else if (typeof(TRecv) == typeof(AudioClip))
            {
                getRequest = UnityWebRequestMultimedia.GetAudioClip(tmpUrl, AudioType.MPEG);
            }
            else
            {
                getRequest = UnityWebRequest.Get(tmpUrl);
            }

            getRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            getRequest.timeout = Timeout;

            yield return getRequest.SendWebRequest();

            TRecv result = null;
            switch (getRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    request.Error = getRequest.error;
                    Log.D("Request.RoutineGet: " + tmpUrl + " Failed: " + getRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Log.D("Request.RoutineGet: " + tmpUrl + " Success: " + getRequest.downloadHandler.text);
                    if (typeof(TRecv) == typeof(Texture2D))
                    {
                        var texture = DownloadHandlerTexture.GetContent(getRequest);
                        result = texture as TRecv;
                    }
                    else if (typeof(TRecv) == typeof(AssetBundle))
                    {
                        var assetBundle = DownloadHandlerAssetBundle.GetContent(getRequest);
                        result = assetBundle as TRecv;
                    }
                    else if (typeof(TRecv) == typeof(AudioClip))
                    {
                        var audioClip = DownloadHandlerAudioClip.GetContent(getRequest);
                        result = audioClip as TRecv;
                    }
                    else if (typeof(TRecv) == typeof(string))
                    {
                        var text = getRequest.downloadHandler.text;
                        result = text as TRecv;
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<TRecv>(getRequest.downloadHandler.text);
                    }
                    break;
                default:
                    break;
            }
            getRequest.Dispose();
            request.Result = result;
        }

        public static IEnumerator GetWithForm(
            this Request<Dictionary<string, object>> request,
            string Url,
            Dictionary<string, object> Data,
            int Timeout = 10)
        {
            yield return Get(request, Url, UrlUtil.DictionaryToFormStr(Data), Timeout);
        }

        public static IEnumerator GetWithForm<TSend, TRecv>(
            this Request<TRecv> request,
            string Url,
            TSend Data,
            int Timeout = 10) where TSend : class where TRecv : class
        {
            yield return Get(request, Url, UrlUtil.StructValuesToFormStr(Data), Timeout);
        }
    }
}