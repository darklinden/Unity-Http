using System.Text;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using SimpleJSON;

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

        public static async UniTask<TRecv> AsyncPost<TSend, TRecv>(this Request request, string Url, TSend Data, string RequestContentType, int Timeout = 10) where TSend : class where TRecv : class
        {
            var postRequest = new UnityWebRequest(Url, UnityWebRequest.kHttpVerbPOST)
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (Data != null)
            {
                if (typeof(TSend) == typeof(byte[]))
                {
                    postRequest.uploadHandler = new UploadHandlerRaw(Data as byte[]);
                }
                else
                {
                    postRequest.uploadHandler = new UploadHandlerRaw(Str2Utf8(Data as string));
                }
            }

            postRequest.SetRequestHeader("Content-Type", RequestContentType);
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

                if (typeof(TRecv) == typeof(byte[]))
                {
                    result = postRequest.downloadHandler.data as TRecv;
                }
                else
                {
                    result = postRequest.downloadHandler.text as TRecv;
                }
            }
            postRequest.Dispose();
            return result;
        }

        public static async UniTask<TRecv> AsyncPostForm<TSend, TRecv>(this Request request, string Url, TSend Data, int Timeout = 10) where TSend : struct where TRecv : class
        {
            var postData = Utils.StructValuesToFormStr(Data);
            var result = await AsyncPost<string, TRecv>(request, Url, postData, RequestContentType.FORM, Timeout);
            return result as TRecv;
        }

        public static async UniTask<JSONNode> AsyncPostJSON<TSend>(this Request request, string Url, TSend Data, int Timeout = 10)
        {
            if (typeof(TSend) == typeof(JSONNode))
            {
                var result = await AsyncPost<string, string>(request, Url, Data as string, RequestContentType.JSON, Timeout);
                return JSON.Parse(result);
            }
            else if (typeof(TSend) == typeof(byte[]))
            {
                var result = await AsyncPost<byte[], string>(request, Url, Data as byte[], RequestContentType.BINARY, Timeout);
                return JSON.Parse(result);
            }
            else
            {
                var postData = Utils.StructValuesToJSON(Data);
                var result = await AsyncPost<string, string>(request, Url, postData, RequestContentType.JSON, Timeout);
                return JSON.Parse(result);
            }
        }
    }
}