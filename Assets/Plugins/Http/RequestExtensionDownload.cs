namespace Http
{
    // public class HttpDownloadHandler : DownloadHandlerScript
    // {
    //     private readonly FileStream _fileStream;
    //     private readonly ProgressCallback _downloadCallback;
    //     private readonly FinishedCallback _finishedCallback;

    //     public HttpDownloadHandler(
    //         FileStream fileStream,
    //         ProgressCallback downloadCallback,
    //         FinishedCallback finishedCallback)
    //     {
    //         _fileStream = fileStream;
    //         _downloadCallback = downloadCallback;
    //         _finishedCallback = finishedCallback;
    //     }

    //     private ulong _totalLength;
    //     private ulong _downloadedLength;

    //     protected override void CompleteContent()
    //     {
    //         _finishedCallback?.Invoke(null);
    //     }

    //     protected override float GetProgress()
    //     {
    //         return _downloadedLength / (float)_totalLength;
    //     }

    //     protected override void ReceiveContentLengthHeader(ulong contentLength)
    //     {
    //         _totalLength = contentLength;
    //     }

    //     protected override bool ReceiveData(byte[] bytes, int dataLength)
    //     {
    //         _downloadedLength += (ulong)dataLength;
    //         _fileStream.Write(bytes, 0, dataLength);
    //         _downloadCallback?.Invoke(_downloadedLength, _totalLength, GetProgress());
    //         return true;
    //     }
    // }

    // public static class RequestExtensionPost
    // {
    //     public static Request Post<TSend>(this Request request, string url, TSend data) where TSend : class
    //     {
    //         return request.SetMethod(HttpMethod.Post).SetUrl(url).SetData(data);
    //     }

    //     private static IEnumerator RoutineDownload(Downloader sender)
    //     {
    //         var placePath = Path.Combine(Application.persistentDataPath, sender.FilePath);

    //         if (File.Exists(placePath))
    //         {
    //             File.Delete(placePath);
    //         }
    //         else
    //         {
    //             var placeFolder = Path.GetDirectoryName(placePath);
    //             if (placeFolder != null && !Directory.Exists(placeFolder))
    //             {
    //                 Directory.CreateDirectory(placeFolder);
    //             }
    //         }

    //         var tempFilePath = Path.Combine(Application.temporaryCachePath, string.Concat(sender.FilePath, ".tmp"));

    //         var tmpFolder = Path.GetDirectoryName(tempFilePath);

    //         if (tmpFolder != null && !Directory.Exists(tmpFolder))
    //             Directory.CreateDirectory(tmpFolder);

    //         var tempFileInfo = new FileInfo(tempFilePath);

    //         var fileStream = File.Open(tempFilePath, tempFileInfo.Exists ? FileMode.Append : FileMode.CreateNew);

    //         String tmpUrl;
    //         if (sender.Url.IndexOf('?') == -1)
    //         {
    //             tmpUrl = string.Concat(sender.Url, "?", "t", Timestamp(), "=0");
    //         }
    //         else
    //         {
    //             tmpUrl = string.Concat(sender.Url, "&", "t", Timestamp(), "=0");
    //         }

    //         var downRequest = new UnityWebRequest(tmpUrl, UnityWebRequest.kHttpVerbGET,
    //             new HttpDownloadHandler(fileStream, sender.MProgressCallback, null), null);

    //         if (tempFileInfo.Exists)
    //         {
    //             downRequest.SetRequestHeader("RANGE", $"bytes={tempFileInfo.Length}-");
    //         }

    //         yield return downRequest.SendWebRequest();

    //         if (downRequest.result == UnityWebRequest.Result.ConnectionError || downRequest.result == UnityWebRequest.Result.ProtocolError)
    //         {
    //             Debug.Log("Request.RoutineDownload: " + sender.Url + " Failed: " + downRequest.error);
    //             sender.MFinishedCallback(downRequest.error);
    //             Request.Clear(sender.Guid);
    //         }
    //         else if (downRequest.isDone)
    //         {
    //             fileStream.Close();
    //             try
    //             {
    //                 File.Move(tempFilePath, placePath);
    //                 File.Delete(tempFilePath);
    //                 sender.MFinishedCallback?.Invoke(null);
    //                 Debug.Log("Request.RoutineDownload: " + sender.Url + " Success: " + placePath);
    //             }
    //             catch (Exception e)
    //             {
    //                 sender.MFinishedCallback?.Invoke(e.ToString());
    //             }
    //             Request.Clear(sender.Guid);
    //         }
    //         else
    //         {
    //             yield return null;
    //         }
    //     }

    //     public static string Download(
    //     string url,
    //     string toPath,
    //     ProgressCallback progressCallback,
    //     FinishedCallback finishedCallback)
    //     {
    //         var guid = Guid.NewGuid().ToString();

    //         var go = new GameObject(string.Concat("Downloader-", guid));
    //         var sender = go.AddComponent<Downloader>();
    //         sender.Guid = guid;
    //         sender.Url = url;
    //         sender.FilePath = toPath;
    //         sender.MProgressCallback = progressCallback;
    //         sender.MFinishedCallback = finishedCallback;

    //         Instance._senders.Add(guid, go);
    //         sender.StartCoroutine(RoutineDownload(sender));

    //         return guid;
    //     }
    // }
}