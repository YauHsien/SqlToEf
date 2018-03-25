namespace SqlToEfUnitTestProject.sql
{
    internal partial class db
    {
        internal const string AS_Assets_MP =
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
    }
}
