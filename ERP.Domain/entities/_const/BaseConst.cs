using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.entities
{
    public class BaseConst
    {
        public const string INT = "int";
        public const string BIGINT = "bigint";

        public const string BIT = "bit";

        public const string DATE = "date";
        public const string DATETIME = "datetime";

        public const string MONEY = "money";

        public const string UNIQUE = "uniqueidentifier";

        public const string FLOAT = "float";

        #region [decimal]

        public const string DECIMAL_8_2 = "decimal(8, 2)";
        public const string DECIMAL_8_6 = "decimal(8, 6)";
        public const string DECIMAL_10_2 = "decimal(10, 2)";
        public const string DECIMAL_10_6 = "decimal(10, 6)";
        public const string DECIMAL_12_2 = "decimal(12, 2)";
        public const string DECIMAL_12_6 = "decimal(12, 6)";
        public const string DECIMAL_15_2 = "decimal(15, 2)";
        public const string DECIMAL_15_6 = "decimal(15, 6)";
        public const string DECIMAL_18_0 = "decimal(18, 0)";
        public const string DECIMAL_18_2 = "decimal(18, 2)";
        public const string DECIMAL_18_6 = "decimal(18, 6)";
        public const string DECIMAL_26_2 = "decimal(26, 2)";
        public const string DECIMAL_26_6 = "decimal(26, 6)";
        public const string DECIMAL_28_2 = "decimal(28, 2)";
        public const string DECIMAL_28_6 = "decimal(28, 6)";

        #endregion [decimal]

        #region [varchar]
        public const string CHAR_1 = "char(1)";
        public const string CHAR_2 = "char(2)";
        public const string CHAR_3 = "char(3)";
        public const string CHAR_4 = "char(4)";
        public const string CHAR_5 = "char(5)";
        public const string CHAR_6 = "char(6)";
        public const string CHAR_7 = "char(7)";
        public const string CHAR_8 = "char(8)";
        public const string CHAR_9 = "char(9)";

        public const string VARCHAR_1 = "varchar(1)";
        public const string VARCHAR_2 = "varchar(2)";
        public const string VARCHAR_3 = "varchar(3)";
        public const string VARCHAR_4 = "varchar(4)";
        public const string VARCHAR_5 = "varchar(5)";
        public const string VARCHAR_6 = "varchar(6)";
        public const string VARCHAR_7 = "varchar(7)";
        public const string VARCHAR_8 = "varchar(8)";
        public const string VARCHAR_9 = "varchar(9)";
        public const string VARCHAR_10 = "varchar(10)";
        public const string VARCHAR_11 = "varchar(11)";
        public const string VARCHAR_12 = "varchar(12)";
        public const string VARCHAR_13 = "varchar(13)";
        public const string VARCHAR_14 = "varchar(14)";
        public const string VARCHAR_15 = "varchar(15)";
        public const string VARCHAR_16 = "varchar(16)";
        public const string VARCHAR_17 = "varchar(17)";
        public const string VARCHAR_18 = "varchar(18)";
        public const string VARCHAR_19 = "varchar(19)";
        public const string VARCHAR_20 = "varchar(20)";
        public const string VARCHAR_25 = "varchar(25)";
        public const string VARCHAR_30 = "varchar(30)";
        public const string VARCHAR_40 = "varchar(40)";
        public const string VARCHAR_50 = "varchar(50)";
        public const string VARCHAR_100 = "varchar(100)";
        public const string VARCHAR_128 = "varchar(128)";
        public const string VARCHAR_150 = "varchar(150)";
        public const string VARCHAR_200 = "varchar(200)";
        public const string VARCHAR_250 = "varchar(250)";
        public const string VARCHAR_300 = "varchar(300)";
        public const string VARCHAR_350 = "varchar(350)";
        public const string VARCHAR_400 = "varchar(400)";
        public const string VARCHAR_500 = "varchar(500)";
        public const string VARCHAR_550 = "varchar(550)";
        public const string VARCHAR_650 = "varchar(650)";
        public const string VARCHAR_1000 = "varchar(1000)";
        public const string VARCHAR_2000 = "varchar(2000)";
        public const string VARCHAR_4000 = "varchar(4000)";
        public const string VARCHAR_MAX = "varchar(max)";
        #endregion [varchar]

        #region [nvarchar]
        public const string NCHAR_1 = "nchar(1)";
        public const string NCHAR_2 = "nchar(2)";
        public const string NCHAR_3 = "nchar(3)";
        public const string NCHAR_4 = "nchar(4)";
        public const string NCHAR_5 = "nchar(5)";
        public const string NCHAR_6 = "nchar(6)";
        public const string NCHAR_7 = "nchar(7)";
        public const string NCHAR_8 = "nchar(8)";
        public const string NCHAR_9 = "nchar(9)";

        public const string NVARCHAR_1 = "nvarchar(1)";
        public const string NVARCHAR_2 = "nvarchar(2)";
        public const string NVARCHAR_3 = "nvarchar(3)";
        public const string NVARCHAR_4 = "nvarchar(4)";
        public const string NVARCHAR_5 = "nvarchar(5)";
        public const string NVARCHAR_6 = "nvarchar(6)";
        public const string NVARCHAR_7 = "nvarchar(7)";
        public const string NVARCHAR_8 = "nvarchar(8)";
        public const string NVARCHAR_9 = "nvarchar(9)";
        public const string NVARCHAR_10 = "nvarchar(10)";
        public const string NVARCHAR_11 = "nvarchar(11)";
        public const string NVARCHAR_12 = "nvarchar(12)";
        public const string NVARCHAR_13 = "nvarchar(13)";
        public const string NVARCHAR_14 = "nvarchar(14)";
        public const string NVARCHAR_15 = "nvarchar(15)";
        public const string NVARCHAR_16 = "nvarchar(16)";
        public const string NVARCHAR_17 = "nvarchar(17)";
        public const string NVARCHAR_18 = "nvarchar(18)";
        public const string NVARCHAR_19 = "nvarchar(19)";
        public const string NVARCHAR_20 = "nvarchar(20)";
        public const string NVARCHAR_25 = "nvarchar(25)";
        public const string NVARCHAR_30 = "nvarchar(30)";
        public const string NVARCHAR_40 = "nvarchar(40)";
        public const string NVARCHAR_50 = "nvarchar(50)";
        public const string NVARCHAR_100 = "nvarchar(100)";
        public const string NVARCHAR_128 = "nvarchar(128)";
        public const string NVARCHAR_150 = "nvarchar(150)";
        public const string NVARCHAR_200 = "nvarchar(200)";
        public const string NVARCHAR_250 = "nvarchar(250)";
        public const string NVARCHAR_255 = "nvarchar(250)";
        public const string NVARCHAR_300 = "nvarchar(300)";
        public const string NVARCHAR_350 = "nvarchar(350)";
        public const string NVARCHAR_500 = "nvarchar(500)";
        public const string NVARCHAR_550 = "nvarchar(550)";
        public const string NVARCHAR_1000 = "nvarchar(1000)";
        public const string NVARCHAR_2000 = "nvarchar(2000)";
        public const string NVARCHAR_4000 = "nvarchar(4000)";
        public const string NVARCHAR_MAX = "nvarchar(max)";
        #endregion [nvarchar]
    }
}
