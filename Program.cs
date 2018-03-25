using SqlToEfUnitTestProject.sql;
using System;
using SqlToEf.parser;
using System.Collections.Generic;

namespace SqlToEfUnitTestProject
{
    public
        class Program
    {
        public static void Main(string[] args)
        {
            IDictionary<string, string> parsed = SqlToEfParser.Parse(db.AS_Assets_MP, 6);
            string s = SqlToStorageModels.ConvertTableToEntityType(parsed, 6);
            string c = SqlToConceptualModels.ConvertTableToEntityType(parsed, 6);
            string m = SqlToMapping.ConvertTableToEntityType(parsed, 6);
            Console.Out.WriteLine(s);
            Console.Out.WriteLine(c);
            Console.Out.WriteLine(m);
        }
    }
}
