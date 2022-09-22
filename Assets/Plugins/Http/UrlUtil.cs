using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Http
{
    public static class UrlUtil
    {
        private static StringBuilder m_StringBuilder = null;
        internal static StringBuilder ReuseSB()
        {
            if (m_StringBuilder == null)
            {
                m_StringBuilder = new StringBuilder();
            }
            else
            {
                m_StringBuilder.Length = 0;
            }
            return m_StringBuilder;
        }

        public static string GetUrl(string Url, string Data)
        {
            var urlSb = ReuseSB();
            if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
            {
                urlSb.Append("http://");
            }

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
            urlSb.Append("t");
            urlSb.Append(Time.realtimeSinceStartup);
            urlSb.Append("=0");
            return urlSb.ToString();
        }

        public static string DictionaryToFormStr(Dictionary<string, object> dict)
        {
            var sb = ReuseSB();
            foreach (var item in dict)
            {
                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(item.Value);
                sb.Append("&");
            }
            return sb.ToString();
        }

        public static string StructValuesToFormStr(object obj)
        {
            var sb = ReuseSB();
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
    }
}