using System;
using System.Collections.Generic;

namespace Scripts.StaticData
{
    public static class AbbreviationsNumbers
    {
        public static int Number;
        public static List<string> Chars = new List<string>() { "", "K", "M", "B", "T", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n" };
        public static int _maxChar;
        public static float Value;
        public static int _index;
        //private Coroutine _coroutine;
        private static string v;
        public static string ShortNumber(float value, int maxChar = 0)
        {
            _maxChar = maxChar;
            Number = 0;
            Value = 0;
            Value = value;

            while (Value >= 1000)
            {
                Number++;
                Value = Value / 1000;
            }

            if (_maxChar == 0 && Number > 0)
            {
                if (Value < 10)
                    _maxChar = 2;
                else if (Value < 100)
                    _maxChar = 1;
            }
            string count = Value.ToString();
            _index = count.IndexOf(',');
            v = count;
            v = v.Replace(",", ".");
            if(_index > -1)
            {
                float res = (float)Math.Round(Value, 2);
                v= res.ToString();
                v = v.Replace(",", ".");
            }

            return v + Chars[Number];
        }
    }
}

