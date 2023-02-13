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
using System;
using System.Globalization;
using System.Text;

namespace SimpleJSON
{
    public partial class JSONNumber : JSONNode
    {
        private double m_Data;

        public override JSONNodeType Tag { get { return JSONNodeType.Number; } }
        public override bool IsNumber { get { return true; } }
        public override Enumerator GetEnumerator() { return new Enumerator(); }

        public override string Value
        {
            get { return m_Data.ToString(CultureInfo.InvariantCulture); }
            set
            {
                double v;
                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
                    m_Data = v;
            }
        }

        public override double AsDouble
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
        public override long AsLong
        {
            get { return (long)m_Data; }
            set { m_Data = value; }
        }
        public override ulong AsULong
        {
            get { return (ulong)m_Data; }
            set { m_Data = value; }
        }

        public JSONNumber(double aData)
        {
            m_Data = aData;
        }

        public JSONNumber(string aData)
        {
            Value = aData;
        }

        public override JSONNode Clone()
        {
            return new JSONNumber(m_Data);
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append(Value);
        }
        private static bool IsNumeric(object value)
        {
            return value is int || value is uint
                || value is float || value is double
                || value is decimal
                || value is long || value is ulong
                || value is short || value is ushort
                || value is sbyte || value is byte;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (base.Equals(obj))
                return true;
            JSONNumber s2 = obj as JSONNumber;
            if (s2 != null)
                return m_Data == s2.m_Data;
            if (IsNumeric(obj))
                return Convert.ToDouble(obj) == m_Data;
            return false;
        }
        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }
        public override void Clear()
        {
            m_Data = 0;
        }
    }
}
