using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Library.Common.Helper
{
    public static class RegexHelper
    {
        public static IDictionary<string, string> MatchAndBind(this string value, string pattern, IEnumerable<string> capTermList)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            Match m = Regex.Match(value, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                foreach (string term in capTermList)
                    result.Add(term, m.Groups[term].ToString());
            }
            return result;
        }


    }
}
