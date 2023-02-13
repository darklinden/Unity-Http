/* * * * *
 * A simple JSON Parser / builder
 * ------------------------------
 * 
 * It mainly has been written as a simple JSON parser. It can build a JSON string
 * from the node-tree, or generate a node tree from any valid JSON string.
 * 
 * Written by Bunny83 
 * 2012-06-09
 * 
 * Changelog now external. See Changelog.txt
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2012-2022 Markus Göbel (Bunny83)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * * * * */
using System.Globalization;
using System.Text;

namespace SimpleJSON
{
    // End of JSONNull

    internal partial class JSONLazyCreator : JSONNode
    {
        private JSONNode m_Node = null;
        private string m_Key = null;
        public override JSONNodeType Tag { get { return JSONNodeType.None; } }
        public override Enumerator GetEnumerator() { return new Enumerator(); }

        public JSONLazyCreator(JSONNode aNode)
        {
            m_Node = aNode;
            m_Key = null;
        }

        public JSONLazyCreator(JSONNode aNode, string aKey)
        {
            m_Node = aNode;
            m_Key = aKey;
        }

        private T Set<T>(T aVal) where T : JSONNode
        {
            if (m_Key == null)
                m_Node.Add(aVal);
            else
                m_Node.Add(m_Key, aVal);
            m_Node = null; // Be GC friendly.
            return aVal;
        }

        public override JSONNode this[int aIndex]
        {
            get { return new JSONLazyCreator(this); }
            set { Set(new JSONArray()).Add(value); }
        }

        public override JSONNode this[string aKey]
        {
            get { return new JSONLazyCreator(this, aKey); }
            set { Set(new JSONObject()).Add(aKey, value); }
        }

        public override void Add(JSONNode aItem)
        {
            Set(new JSONArray()).Add(aItem);
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            Set(new JSONObject()).Add(aKey, aItem);
        }

        public static bool operator ==(JSONLazyCreator a, object b)
        {
            if (b == null)
                return true;
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JSONLazyCreator a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return true;
            return System.Object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override int AsInt
        {
            get { Set(new JSONNumber(0)); return 0; }
            set { Set(new JSONNumber(value)); }
        }

        public override float AsFloat
        {
            get { Set(new JSONNumber(0.0f)); return 0.0f; }
            set { Set(new JSONNumber(value)); }
        }

        public override double AsDouble
        {
            get { Set(new JSONNumber(0.0)); return 0.0; }
            set { Set(new JSONNumber(value)); }
        }

        public override long AsLong
        {
            get
            {
                if (longAsString)
                    Set(new JSONString("0"));
                else
                    Set(new JSONNumber(0.0));
                return 0L;
            }
            set
            {
                if (longAsString)
                    Set(new JSONString(value.ToString(CultureInfo.InvariantCulture)));
                else
                    Set(new JSONNumber(value));
            }
        }

        public override ulong AsULong
        {
            get
            {
                if (longAsString)
                    Set(new JSONString("0"));
                else
                    Set(new JSONNumber(0.0));
                return 0L;
            }
            set
            {
                if (longAsString)
                    Set(new JSONString(value.ToString(CultureInfo.InvariantCulture)));
                else
                    Set(new JSONNumber(value));
            }
        }

        public override bool AsBool
        {
            get { Set(new JSONBool(false)); return false; }
            set { Set(new JSONBool(value)); }
        }

        public override JSONArray AsArray
        {
            get { return Set(new JSONArray()); }
        }

        public override JSONObject AsObject
        {
            get { return Set(new JSONObject()); }
        }
        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append("null");
        }
    }
}
