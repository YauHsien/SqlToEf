using System.Collections.Generic;

namespace SqlToEf.parser
{
    public class SqlToStorageModels
    {
        public static string ConvertTableToEntityType(IDictionary<string, string> parsed, int columnCount)
        {
            string result = "";

            for (int i = 0; i < columnCount; i++)
            {
                result +=
                    string.Format("<Property Name=\"{0}\" Type=\"{1}\"{2}{3}{4}{5} />\n",
                        parsed["def_" + i],
                        parsed["type_" + i],
                        (parsed["pre_" + i] != "") ?
                            string.Format(@" Precision=""{0}"" Scale=""{1}""", parsed["pre_" + i], (parsed["sca_" + i] == "" ? "0" : parsed["sca_" + i]))
                            : (parsed["len_" + i] != "") ?
                                string.Format(@" MaxLength=""{0}""", parsed["len_" + i])
                                : ""
                                ,
                        (parsed["ide_" + i] ?? "") != "" ?
                            " StoreGeneratedPattern=\"Identity\""
                            : "",
                        parsed["nul_" + i].ToLower() == "not" ?
                            @" Nullable=""false"""
                            : "",
                        parsed["dft_" + i].ToLower() == "getdate()" ?
                            " DefaultValue=\"GETUTCDATE()\""
                            : (parsed["dft_" + i] ?? "") != "" ?
                                string.Format(@" DefaultValue=""{0}""", parsed["dft_" + i].Replace("\'", ""))
                                : ""
                        );
            }

            result = string.Format("<EntityType Name=\"{0}\">\n<Key>\n<PropertyRef Name=\"ID\" />\n</Key>\n{1}</EntityType>", parsed["table_name"], result);

            return result;
        }
    }
}
