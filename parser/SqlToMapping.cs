using System.Collections.Generic;

namespace SqlToEf.parser
{
    public class SqlToMapping
    {
        public static string ConvertTableToEntityType(IDictionary<string, string> parsed, int columnCount)
        {
            string result = "";

            for (int i = 0; i < columnCount; i++)
            {
                result +=
                    string.Format("<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />\n",
                        SqlToEfParser.toHappyCamel(parsed["def_" + i]),
                        parsed["def_" + i]
                        );
            }

            result = string.Format("<EntityTypeMapping  TypeName=\"context.{0}\">\n<MappingFragment StoreEntitySet=\"{0}\">\n{1}</MappingFragment>\n</EntityType>", parsed["table_name"], result);

            return result;
        }
    }
}
