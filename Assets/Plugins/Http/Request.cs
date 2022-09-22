using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Http
{
    public class Request<TRecv>
    {
        public TRecv Result { get; set; }
        public string Error { get; set; }

        public static Request<TRecv> Make()
        {
            return new Request<TRecv>();
        }
    }
}