using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Data
{
    public static class DataExtensions
    {
        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);
    }
}
