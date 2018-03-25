using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SqlToEf.parser
{
    public class SqlToConceptualModels
    {
        public static string ConvertTableToEntityType(IDictionary<string, string> parsed, int columnCount)
        {
            string result = "";

            for (int i = 0; i < columnCount; i++)
            {
                result +=
                    string.Format("<Property Name=\"{0}\" Type=\"{1}\"{2}{3}{4}{5}{6}{7} />\n",
                        SqlToEfParser.toHappyCamel(parsed["def_" + i]),
                        JudgeType(parsed["type_" + i]),
                        (parsed["pre_" + i] != "") ?
                            string.Format(@" Precision=""{0}"" Scale=""{1}""", parsed["pre_" + i], (parsed["sca_" + i] == "" ? "0" : parsed["sca_" + i]))
                            : (parsed["len_" + i] != "") ?
                                string.Format(@" MaxLength=""{0}""", parsed["len_" + i])
                                : ""
                                ,
                        (parsed["ide_" + i] ?? "") != "" ?
                            " annotation:StoreGeneratedPattern=\"Identity\""
                            : "",
                        parsed["nul_" + i].ToLower() == "not" ?
                            @" Nullable=""false"""
                            : "",
                        parsed["dft_" + i].ToLower() == "getdate()" ?
                            " DefaultValue=\"GETUTCDATE()\""
                            : (parsed["dft_" + i] ?? "") != "" ?
                                string.Format(@" DefaultValue=""{0}""", parsed["dft_" + i].Replace("\'", ""))
                                : "",
                        parsed["type_" + i] == "nvarchar" || parsed["type_" + i] == "ntext" ?
                            " Unicode=\"true\""
                            : parsed["type_" + i] == "varchar" || parsed["type_" + i] == "text" ?
                                " Unicode=\"false\""
                                : "",
                        parsed["type_" + i].StartsWith("nvarc") || parsed["type_" + i].StartsWith("varc") ?
                                " FixedLength=\"false\""
                            : parsed["type_" + i] == "char" ?
                                " FixedLength=\"true\""
                                : ""
                        );
            }

            result = string.Format("<EntityType Name=\"{0}\">\n<Key>\n<PropertyRef Name=\"ID\" />\n</Key>\n{1}</EntityType>", parsed["table_name"], result);

            return result;
        }

        private static string JudgeType(string typeName)
        {
            switch (typeName)
            {
                case "varchar":
                case "nvarchar":
                case "ntext":
                case "text":
                    return "String";
                case "datetime":
                    return "Datetime";
                case "int":
                    return "Int32";
                case "numeric":
                    return "Decimal";
                case "bit":
                    return "Boolean";
                default:
                    return "unknown";
            }
        }
    }
}
