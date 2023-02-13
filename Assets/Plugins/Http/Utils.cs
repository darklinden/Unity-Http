using System;
using System.Text;
using SimpleJSON;

namespace Http
{
    public static class Utils
    {
        internal static string StructValuesToFormStr(object obj)
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

                if (sb.Length > 0) sb.Append("&");
                sb.Append(field.Name);
                sb.Append("=");
                sb.Append(value);
            }
            return sb.ToString();
        }

        internal static string StructValuesToJSON(object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields();
            var jsonNode = new JSONObject();
            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                if (value == null)
                {
                    continue;
                }
                // assert value is string or number
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                        jsonNode[field.Name] = (int)value;
                        break;
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        jsonNode[field.Name] = (long)value;
                        break;
                    case TypeCode.Single:
                        jsonNode[field.Name] = (float)value;
                        break;
                    case TypeCode.Double:
                        jsonNode[field.Name] = (double)value;
                        break;
                    case TypeCode.String:
                        jsonNode[field.Name] = (string)value;
                        break;
                    default:
                        Log.E("value type not supported: ", value, value.GetType().ToString());
                        break;
                }
            }
            return jsonNode.ToString();
        }
    }
}