using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Library.Common.Utility;
using Library.Common.Helper;
using System.Text.RegularExpressions;

namespace SqlToEfUnitTestProject
{
    [TestClass]
    public class SqlToEfUnitTest : AbstractUnitTest
    {
        public const string sql =
            @"CREATE TABLE [dbo].[AS_Assets_MP](
                	[ID] [int] IDENTITY(1,1) NOT NULL,
                    [Asset_Number] [varchar](15) NOT NULL,
                     [LicensePlateNumber] [varchar](50) NULL,
                    [Assets_Original_ID] [varchar](30) NULL CONSTRAINT [DF_AS_Assets_MP_Assets_Original_ID]  DEFAULT (''),
                   [Net_Value] [numeric](20, 2) NOT NULL DEFAULT ((0.0)),
                    [Life_Year_End_Date] [datetime] NOT NULL DEFAULT (getdate()),
                 CONSTRAINT [PK_AS_Assets_MP] PRIMARY KEY CLUSTERED
                (

                    [ID] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [UK_AS_Assets_MP] UNIQUE NONCLUSTERED
                (

                    [Asset_Number] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]

                GO";

        public const string expected =
            @"<EntityType Name=""AS_Agent_Flow"">
                    <Key>
                        <PropertyRef Name=""ID"" />
                    </Key>
                    <Property Name=""ID"" Type=""int"" StoreGeneratedPattern=""Identity"" Nullable=""false"" />
                    <Property Name=""Asset_Number"" Type=""varchar"" MaxLength=""4"" Nullable=""false"" />
                    <Property Name=""LicensePlateNumber"" Type=""varchar"" MaxLength=""50"" />
                    <Property Name=""Assets_Original_ID"" Type=""varchar"" MaxLength=""30"" Nullable=""false"" DefaultValue="" />
                    <Property Name=""Net_Value"" Type=""numeric"" Precision=""20"" Scale=""2"" Nullable=""false"" DefaultValue=""0.0"" />
                    <Property Name=""Life_Year_End_Date"" Type=""datetime"" StoreGeneratedPattern=""Computed"" Nullable=""false"" DefaultValue=""0.0"" />
                </EntityType>";

        [TestMethod]
        public void TestConvertTableToEntityType()
        {
            IDictionary<string, string> parsed = new Dictionary<string, string>();
            IList<string> capTermList = new List<string>()
            {
                "table_name",
            };
            string pat = @"create table \[dbo\]\.\[(?'table_name'[^\]]*)\]\(";
            int i;
            for (i = 0; i < 6; i++)
            {
                pat += string.Format(@"(?:\s+"
                    + @"\[(?'def_{0}'[^\[\]]+)\]"
                    + @"\s"
                    + @"\[(?'type_{0}'[^\[\]]+)\]"
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
                    + @"\s*((?:(?!,\r).)+),\r)", i);
                capTermList.Add("def_" + i);
                capTermList.Add("type_" + i);
                capTermList.Add("len_" + i);
                capTermList.Add("pre_" + i);
                capTermList.Add("sca_" + i);
            }
            parsed = sql.MatchAndBind(pat, capTermList);
            Assert.IsTrue(parsed.Count > 0, "取出詞彙數目");

            Assert.AreEqual<string>("AS_Assets_MP", parsed["table_name"]);

            Assert.AreEqual<string>(@"ID", parsed["def_" + i]);
            Assert.AreEqual<string>(@"int", parsed["type_" + i], "ID");

            Assert.AreEqual<string>(@"Asset_Number", parsed["def_" + i]);
            Assert.AreEqual<string>(@"varchar", parsed["type_" + i], "Asset_Number");
            Assert.AreEqual<string>(@"15", parsed["len_" + i], "Asset_Number");

            Assert.AreEqual<string>(@"LicensePlateNumber", parsed["def_" + i]);
            Assert.AreEqual<string>(@"varchar", parsed["type_" + i], "LicensePlateNumber");
            Assert.AreEqual<string>(@"50", parsed["len_" + i], "LicensePlateNumber");

            Assert.AreEqual<string>(@"Assets_Original_ID", parsed["def_" + i]);
            Assert.AreEqual<string>(@"varchar", parsed["type_" + i], "Assets_Original_ID");
            Assert.AreEqual<string>(@"30", parsed["len_" + i], "Assets_Original_ID");

            Assert.AreEqual<string>(@"Net_Value", parsed["def_" + i]);
            Assert.AreEqual<string>(@"numeric", parsed["type_" + i], "Net_Value");
            Assert.AreEqual<string>(@"20", parsed["pre_" + i], "Net_Value");
            Assert.AreEqual<string>(@"2", parsed["sca_" + i], "Net_Value");

            Assert.AreEqual<string>(@"Life_Year_End_Date", parsed["def_" + i]);
            Assert.AreEqual<string>(@"datetime", parsed["type_" + i], "Life_Year_End_Date");
        }

        [TestMethod]
        public void TestConvertTableToEntityType_bare()
        {
            IDictionary<int, string> parsed = new Dictionary<int, string>();
            string pat = @"create table \[dbo\]\.\[(?'table_name'[^\]]*)\]\(";
            int i;
            for (i = 0; i < 6; i++)
            {
                pat += string.Format(@"(?:\s+"
                    + @"\[(?'def_{0}'[^\[\]]+)\]"
                    + @"\s"
                    + @"\[(?'type_{0}'[^\[\]]+)\]"
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
                    + @"\s*((?:(?!,\r).)+),\r)", i);
            }
            Match m = Regex.Match(sql, pat, RegexOptions.IgnoreCase);

            Console.Out.WriteLine(pat);
            Assert.IsTrue(m.Success);

            Console.Out.WriteLine(m.Groups["table_name"].ToString());
            Assert.AreEqual<string>("AS_Assets_MP", m.Groups["table_name"].ToString());

            i = 0;
            //[ID] [int] IDENTITY(1,1) NOT NULL
            Assert.AreEqual<string>(@"ID", m.Groups["def_" + i].ToString());
            Assert.AreEqual<string>(@"int", m.Groups["type_" + i].ToString());

            i = 1;
            //[Asset_Number] [varchar](15) NOT NULL
            Assert.AreEqual<string>(@"Asset_Number", m.Groups["def_" + i].ToString());
            Assert.AreEqual<string>(@"varchar", m.Groups["type_" + i].ToString());
            Assert.AreEqual<string>(@"15", m.Groups["len_" + i].ToString());

            i = 2;
            //[LicensePlateNumber] [varchar](50) NULL
            Assert.AreEqual<string>(@"LicensePlateNumber", m.Groups["def_" + i].ToString());
            Assert.AreEqual<string>(@"varchar", m.Groups["type_" + i].ToString());
            Assert.AreEqual<string>(@"50", m.Groups["len_" + i].ToString());

            i = 3;
            //[Assets_Original_ID] [varchar](30) NULL CONSTRAINT [DF_AS_Assets_MP_Assets_Original_ID]  DEFAULT ('')
            Assert.AreEqual<string>(@"Assets_Original_ID", m.Groups["def_" + i].ToString());
            Assert.AreEqual<string>(@"varchar", m.Groups["type_" + i].ToString());
            Assert.AreEqual<string>(@"30", m.Groups["len_" + i].ToString());

            i = 4;
            //[Net_Value] [numeric](20, 2) NOT NULL DEFAULT ((0.0))
            Assert.AreEqual<string>(@"Net_Value", m.Groups["def_" + i].ToString());
            Assert.AreEqual<string>(@"numeric", m.Groups["type_" + i].ToString());
            Assert.AreEqual<string>(@"20", m.Groups["pre_" + i].ToString(), "Net_Value");
            Assert.AreEqual<string>(@"2", m.Groups["sca_" + i].ToString());

            i = 5;
            //[Life_Year_End_Date] [datetime] NOT NULL DEFAULT (getdate())
            Assert.AreEqual<string>(@"Life_Year_End_Date", m.Groups["def_" + i].ToString());
            Assert.AreEqual<string>(@"datetime", m.Groups["type_" + i].ToString());
        }
    }
}
