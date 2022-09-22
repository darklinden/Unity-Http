using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Http
{
    public static class RequestExtensionPost
    {
        public static byte[] Str2Utf8(string string_)
        {
            if (string_ == "")
            {
                return new byte[] { 0x00 };
            }

            var tmp = Encoding.UTF8.GetBytes(string_);
            return tmp;
        }

        public static IEnumerator Post<TSend, TRecv>(
            this Request<TRecv> request,
            string Url,
            TSend Data,
            int Timeout = 10) where TRecv : class
        {
            if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
            {
                Url = "http://" + Url;
            }

            var postRequest = new UnityWebRequest(Url, UnityWebRequest.kHttpVerbPOST)
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (Data != null)
            {
                if (typeof(TSend) == typeof(string))
                {
                    postRequest.uploadHandler = new UploadHandlerRaw(Str2Utf8(Data as string));
                }
                else if (typeof(TSend) == typeof(byte[]))
                {
                    postRequest.uploadHandler = new UploadHandlerRaw(Data as byte[]);
                }
                else
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
                    postRequest.uploadHandler = new UploadHandlerRaw(Str2Utf8(json));
                }
            }

            postRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            postRequest.timeout = Timeout;

            yield return postRequest.SendWebRequest();

            if (postRequest.result == UnityWebRequest.Result.ConnectionError
                || postRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                request.Error = postRequest.error;
                Log.E("Request.AsyncPost:", Url, "Data:", Data + "Failed:", postRequest.error);
            }
            else if (postRequest.isDone)
            {
                Log.D("Request.AsyncPost:", Url, "Data:", Data + "Success:", postRequest.downloadHandler.text);

                if (typeof(TRecv) == typeof(string))
                {
                    request.Result = postRequest.downloadHandler.text as TRecv;
                }
                else if (typeof(TRecv) == typeof(byte[]))
                {
                    request.Result = postRequest.downloadHandler.data as TRecv;
                }
                else
                {
                    request.Result = JsonConvert.DeserializeObject<TRecv>(postRequest.downloadHandler.text);
                }
            }
            postRequest.Dispose();
        }

        public static IEnumerator PostWithForm(
            this Request<Dictionary<string, object>> request,
            string Url,
            Dictionary<string, object> Data,
            int Timeout = 10)
        {
            yield return Post(request, Url, UrlUtil.DictionaryToFormStr(Data), Timeout);
        }

        public static IEnumerator PostWithForm<TSend, TRecv>(
            this Request<TRecv> request,
            string Url,
            TSend Data,
            int Timeout = 10) where TRecv : class
        {
            yield return Post(request, Url, UrlUtil.StructValuesToFormStr(Data), Timeout);
        }
    }
}