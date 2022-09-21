namespace Http
{

    // public static byte[] Str2Utf8(string string_)
    // {
    //     if (string_ == "")
    //     {
    //         return new byte[] { 0x00 };
    //     }

    //     var tmp = Encoding.UTF8.GetBytes(string_);
    //     return tmp;
    // }


    // public static class RequestExtensionPost {
    //     public static Request Post<TSend>(this Request request, string url, TSend data) where TSend : class {
    //         return request.SetMethod(HttpMethod.Post).SetUrl(url).SetData(data);
    //     }
    //      public static IEnumerator RoutinePost<TSend, TRecv>(Sender<TSend, TRecv> sender) where TSend : class where TRecv : class
    //     {
    //         var postRequest = new UnityWebRequest(sender.Url, UnityWebRequest.kHttpVerbPOST)
    //         {
    //             downloadHandler = new DownloadHandlerBuffer()
    //         };

    //         if (sender.Data != null)
    //         {
    //             if (typeof(TSend) == typeof(string))
    //             {
    //                 postRequest.uploadHandler = new UploadHandlerRaw(Str2Utf8(sender.Data as string));
    //             }
    //             else if (typeof(TSend) == typeof(byte[]))
    //             {
    //                 postRequest.uploadHandler = new UploadHandlerRaw(sender.Data as byte[]);
    //             }
    //             else
    //             {
    //                 var json = Newtonsoft.Json.JsonConvert.SerializeObject(sender.Data);
    //                 postRequest.uploadHandler = new UploadHandlerRaw(Str2Utf8(json));
    //             }
    //         }

    //         postRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    //         postRequest.timeout = DefaultTimeout;

    //         yield return postRequest.SendWebRequest();

    //         if (postRequest.result == UnityWebRequest.Result.ConnectionError || postRequest.result == UnityWebRequest.Result.ProtocolError)
    //         {
    //             Debug.Log("Request.RoutinePost: " + sender.Url + " Data: " + sender.Data + " Failed: " + postRequest.error);
    //             sender.Completion?.Invoke(null, postRequest.error);

    //             postRequest.Dispose();
    //             Request.Clear(sender.Guid);
    //         }
    //         else if (postRequest.isDone)
    //         {
    //             Debug.Log("Request.RoutinePost: " + sender.Url + " Data: " + sender.Data + " Success: " + postRequest.downloadHandler.text);

    //             if (typeof(TRecv) == typeof(string))
    //             {
    //                 sender.Completion?.Invoke(postRequest.downloadHandler.text as TRecv, null);
    //             }
    //             else if (typeof(TRecv) == typeof(byte[]))
    //             {
    //                 sender.Completion?.Invoke(postRequest.downloadHandler.data as TRecv, null);
    //             }
    //             else
    //             {
    //                 var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TRecv>(postRequest.downloadHandler.text);
    //                 sender.Completion?.Invoke(obj, null);
    //             }

    //             postRequest.Dispose();
    //             Request.Clear(sender.Guid);
    //         }
    //         else
    //         {
    //             yield return null;
    //         }
    //     }
    // }

    // // callBack<result, error>
    //     public static string Post<TSend, TRecv>(
    //         string url,
    //         TSend data,
    //         Action<TRecv, string> callBack) where TSend : class where TRecv : class
    //     {
    //         return Instance.AttachSender(url, data, HttpMethod.Post, DefaultTimeout, callBack);
    //     }

    //     public static async Task<TRecv> Post<TSend, TRecv>(string url, TSend data) where TSend : class where TRecv : class
    //     {
    //         var t = new TaskCompletionSource<TRecv>();
    //         Post<TSend, TRecv>(url, data, (result, err) => t.TrySetResult(result));
    //         return await t.Task;
    //     }
}