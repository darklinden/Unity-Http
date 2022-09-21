using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Http
{
    public static class RequestExtensionGet
    {
        public static async UniTask<TRecv> AsyncGet<TRecv>(this Request request, string Url, string Data = null, int Timeout = 10) where TRecv : class
        {
            var urlSb = request.StringBuilder;
            urlSb.Append(Url);

            if (Url.IndexOf('?') == -1)
            {
                urlSb.Append("?");
                if (!string.IsNullOrEmpty(Data))
                {
                    urlSb.Append(Data);
                    urlSb.Append("&");
                }
            }
            else
            {
                urlSb.Append("&");
                if (!string.IsNullOrEmpty(Data))
                {
                    urlSb.Append(Data);
                    urlSb.Append("&");
                }
            }
            request.AddCachePreventer();

            var tmpUrl = urlSb.ToString();

            UnityWebRequest getRequest = null;
            if (typeof(TRecv) == typeof(Texture2D))
            {
                getRequest = UnityWebRequestTexture.GetTexture(tmpUrl);
            }
            // else if (typeof(TRecv) == typeof(Texture2D))
            // {
            //     getRequest = UnityWebRequestAssetBundle.GetAssetBundle(tmpUrl);
            // }
            // else if (typeof(TRecv) == typeof(Texture2D))
            // {
            //     getRequest = UnityWebRequestMultimedia.GetAudioClip(tmpUrl);
            // }
            else
            {
                getRequest = UnityWebRequest.Get(tmpUrl);
            }

            getRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            getRequest.timeout = request.DefaultTimeout;

            // Request and wait for the desired page.
            await getRequest.SendWebRequest();

            TRecv result = null;
            switch (getRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Log.E(getRequest.error);
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
                    else if (typeof(TRecv) == typeof(Texture2D))
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
                        var data = getRequest.downloadHandler.data;
                        result = data as TRecv;
                    }
                    break;
                default:
                    break;
            }
            getRequest.Dispose();

            return result;
        }

        public static async UniTask<TRecv> AsyncGet<TRecv>(this Request request, string Url, object Data, int Timeout = 10) where TRecv : class
        {
            var data = JsonConvert.SerializeObject(Data);
            return await request.AsyncGet<TRecv>(Url, data, Timeout);
        }
    }
}