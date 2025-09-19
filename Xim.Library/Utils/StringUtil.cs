using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Utils
{
    public static class StringUtil
    {
        static Dictionary<char, string> _unicodeChs = new Dictionary<char, string>()
        {
            { 'e', "éèẻẹẽêếềểệễ"},
            { 'y', "ýỳỷỵỹ"},
            { 'u', "úùủụũưứừửựữ"},
            { 'i', "íìỉịĩ"},
            { 'o', "óòỏọõôốồổộỗơớờởợỡ"},
            { 'a', "áàảạãâấầẩậẫăắằẳặẵ"},
        };

        /// <summary>
        /// Xử lý loại bỏ ký tự unicode
        /// </summary>
        public static string NoneUnicode(string input)
        {
            var rs = input;
            foreach (var item in _unicodeChs)
            {
                foreach (var c in item.Value)
                {
                    rs = rs.Replace(c, item.Key);
                }
            }

            return rs;
        }
    }
}
