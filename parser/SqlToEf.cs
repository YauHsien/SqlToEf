using Library.Common.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlToEf.parser
{
    public class SqlToEfParser
    {
        public static IDictionary<string, string> Parse(string sql, int columnCount)
        {
            IList<string> capTermList = new List<string>()
            {
                "table_name",
            };
            string pat = @"create table \[dbo\]\.\[(?'table_name'[^\]]*)\]\(";
            int i;
            for (i = 0; i < columnCount; i++)
            {
                pat += string.Format(@"(?:\s+"
                    + @"\[(?'def_{0}'[^\[\]]+)\]"
                    + @"\s+\[(?'type_{0}'[^\[\]]+)\]"
                    + @"(?:"
                        + @"(?:\("
                            + @"(?'pre_{0}'[^\(\)]+)"
                            + @",\s*"
                            + @"(?'sca_{0}'[^\(\)]+)"
                        + @"\))"
                        + @"|"
                        + @"(?:\("
                            + @"(?'len_{0}'[^\(\)]+)"
                        + @"\))"
                        + @"|"
                    + @")"
                    + @"(?:\s+(?'ide_{0}'identity\([^\(\)]+\))|)"
                    + @"(?:\s+(?'nul_{0}'not)\s+null|)"
                    + @"(?:.+default\s+\((?:"
                        + @"(?'dft_{0}'getdate\(\))"
                        + @"|"
                        + @"n?(?'dft_{0}''[^']*)'"
                        + @"|"
                        + @"\((?'dft_{0}'[^\(\)]+)\)"
                        + @"|"
                        + @"(?'dft_{0}'[^\(\)]+)"
                    + @")\)|)"
                    + @"(?:(?:(?!,\r).)*)"
                    + @",\r)",
                    i);
                capTermList.Add("def_" + i);
                capTermList.Add("type_" + i);
                capTermList.Add("ide_" + i);
                capTermList.Add("len_" + i);
                capTermList.Add("pre_" + i);
                capTermList.Add("sca_" + i);
                capTermList.Add("nul_" + i);
                capTermList.Add("dft_" + i);
            }
            return sql.MatchAndBind(pat, capTermList);
        }

        public
            static string toHappyCamel(string splittedName)
        {

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            IList<string> list = new List<string>(splittedName.Split('_'));
            if (list.Count() == 1)
                return splittedName;
            else
                return new List<string>(splittedName.Split('_'))
                    .ConvertAll(e => textInfo.ToTitleCase(e))
                    .Aggregate((a, b) => a + b);
        }
    }
}
