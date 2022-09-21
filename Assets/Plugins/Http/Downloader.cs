using UnityEngine;

namespace Http
{
    public delegate void ProgressCallback(ulong current, ulong total, float progress);
    public delegate void FinishedCallback(string errMsg);

    internal class Downloader : GuidHolder
    {
        public string Guid { get; set; }
        internal string Url = "";
        internal ProgressCallback MProgressCallback;
        internal FinishedCallback MFinishedCallback;
        internal string FilePath = "";
    }
}