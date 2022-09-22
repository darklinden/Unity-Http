using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
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

        public static async UniTask<TRecv> AsyncPost<TSend, TRecv>(this Request request, string Url, TSend Data, int Timeout = 10) where TSend : class where TRecv : class
        {
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

            try
            {
                await postRequest.SendWebRequest().ToUniTask();
            }
            catch (System.Exception)
            {
                // Log.E("Request.AsyncPost:", Url, "Data:", Data + "Failed:", postRequest.error);
            }

            TRecv result = null;
            if (postRequest.result == UnityWebRequest.Result.ConnectionError
                || postRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Log.E("Request.AsyncPost:", Url, "Data:", Data + "Failed:", postRequest.error);
            }
            else if (postRequest.isDone)
            {
                Log.D("Request.AsyncPost:", Url, "Data:", Data + "Success:", postRequest.downloadHandler.text);

                if (typeof(TRecv) == typeof(string))
                {
                    result = postRequest.downloadHandler.text as TRecv;
                }
                else if (typeof(TRecv) == typeof(byte[]))
                {
                    result = postRequest.downloadHandler.data as TRecv;
                }
                else
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TRecv>(postRequest.downloadHandler.text);
                    result = obj as TRecv;
                }
            }
            postRequest.Dispose();
            return result;
        }

        private static string DictionaryToFormStr(Dictionary<string, object> dict)
        {
            var sb = new StringBuilder();
            foreach (var item in dict)
            {
                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(item.Value);
                sb.Append("&");
            }
            return sb.ToString();
        }

        public static async UniTask<Dictionary<string, object>> AsyncPostForm(this Request request, string Url, Dictionary<string, object> Data, int Timeout = 10)
        {
            var result = await AsyncPost<string, string>(request, Url, DictionaryToFormStr(Data), Timeout);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
        }

        private static string StructValuesToFormStr(object obj)
        {
            var sb = new StringBuilder();
            var type = obj.GetType();
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                if (value == null)
                {
                    continue;
                }
                sb.Append(field.Name);
                sb.Append("=");
                sb.Append(value);
                sb.Append("&");
            }
            return sb.ToString();
        }

        public static async UniTask<TRecv> AsyncPostForm<TSend, TRecv>(this Request request, string Url, TSend Data, int Timeout = 10) where TSend : struct where TRecv : struct
        {
            var postData = StructValuesToFormStr(Data);
            var result = await AsyncPost<string, string>(request, Url, postData, Timeout);
            return JsonConvert.DeserializeObject<TRecv>(result);
        }
    }
}