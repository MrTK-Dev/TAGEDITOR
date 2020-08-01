using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ID3_Tag_Editor.Scripts.Extensions
{
    public static class Extensions
    {
        #region String

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Chars"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string Item, string[] Chars)
        {
            for (int i = 0; i < Chars.Length; i++)
            {
                if (Item.Contains(Chars[i]))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string RemoveLastChar(this string Input)
        {
            return Input.Substring(0, Input.Length - 1);
        }

        #endregion

        #region JsonElement

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonElement"></param>
        /// <returns></returns>
        public static T ToObject<T>(this JsonElement jsonElement)
        {
            var json = jsonElement.GetRawText();

            return JsonSerializer.Deserialize<T>(json);
        }

        #endregion
    }
}
