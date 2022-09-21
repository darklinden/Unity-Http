using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Http
{
    public class Request : MonoBehaviour
    {
        private static Request _instance;
        public static Request Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[Http-Request]");
                    _instance = go.AddComponent<Request>();
                }
                return _instance;
            }
        }

        internal int DefaultTimeout = 10;

        private readonly Dictionary<string, GameObject> _senders;
        private Request()
        {
            _senders = new Dictionary<string, GameObject>();
        }

        private StringBuilder m_StringBuilder = null;
        internal StringBuilder StringBuilder
        {
            get
            {
                if (m_StringBuilder == null)
                {
                    m_StringBuilder = new StringBuilder();
                }
                m_StringBuilder.Remove(0, m_StringBuilder.Length);
                return m_StringBuilder;
            }
        }

        internal void AddCachePreventer()
        {
            StringBuilder.Append("t");
            StringBuilder.Append(Time.realtimeSinceStartup);
            StringBuilder.Append("=0");
        }

        internal string NormalizeUrl(string url)
        {
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return url;
            }
            return "http://" + url;
        }

        // private string AttachSender<TSend, TRecv>(
        //     string url,
        //     TSend data,
        //     HttpMethod method,
        //     int timeout,
        //     Action<TRecv, string> completion) where TSend : class where TRecv : class
        // {
        //     var guid = Guid.NewGuid().ToString();

        //     var go = new GameObject(string.Concat("Sender(", method, ")", guid));
        //     UnityEngine.Object.DontDestroyOnLoad(go);

        //     var sender = go.AddComponent<Sender<TSend, TRecv>>();
        //     sender.Guid = guid;



        //     sender.Data = data;
        //     sender.Method = method;
        //     sender.Completion = completion;

        //     _senders[guid] = go;
        //     switch (sender.Method)
        //     {
        //         case HttpMethod.Get:
        //             sender.StartCoroutine(RoutineGet(sender));
        //             break;
        //         case HttpMethod.Post:
        //             sender.StartCoroutine(RoutinePost(sender));
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }

        //     return guid;
        // }

        public static void Clear(string guid)
        {
            GameObject go = null;
            if (!Instance._senders.TryGetValue(guid, out go)) return;
            UnityEngine.Object.Destroy(go.gameObject);
            Instance._senders.Remove(guid);
        }
    }
}