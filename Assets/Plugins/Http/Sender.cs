namespace Http
{
    internal class Sender<TSend> : GuidHolder where TSend : class
    {
        public string Guid { get; set; }
        internal string Url = "";
        internal TSend Data = null;
    }
}