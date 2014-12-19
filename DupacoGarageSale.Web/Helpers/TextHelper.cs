using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DupacoGarageSale.Web.Helpers
{
    public static class TextHelper
    {
        public static string EncodeText(string text)
        {
            return HttpContext.Current.Server.HtmlEncode(text);
        }

        public static string DecodeText(string text)
        {
            return HttpContext.Current.Server.HtmlDecode(text);
        }
    }
}