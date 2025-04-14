using System.Text;
using UnityEngine;

namespace AGGtH.Runtime.Extensions
{
    public static class ColorExtensions
    {
        public static string ColorString(string text, Color color)
        {
            var str = new StringBuilder();
            str.Append("<color=#").Append(ColorUtility.ToHtmlStringRGBA(color)).Append(">").Append(text)
                .Append("</color>");
            return str.ToString();
        }

    }
}