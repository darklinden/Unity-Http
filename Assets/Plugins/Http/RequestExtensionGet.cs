using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Http
{
    public static class RequestExtensionGet
    {
        internal static void AddCachePreventer(StringBuilder sb)
        {
            sb.Append("t");
            sb.Append(Time.realtimeSinceStartup);
            sb.Append("=0");
        }

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

            AddCachePreventer(urlSb);

            var tmpUrl = urlSb.ToString();

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

            // Request and wait for the desired page.
            try
            {
                await getRequest.SendWebRequest().ToUniTask();
            }
            catch (System.Exception)
            {
                // Log.E("Request.AsyncGet:", Url, "Data:", Data + "Failed:", e);
            }

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
            return await AsyncGet<TRecv>(request, Url, data, Timeout);
        }
    }
}