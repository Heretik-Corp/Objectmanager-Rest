using System;
using System.Collections.Generic;

namespace ObjectManager.Rest.Tests.Integration.Common
{
    public class DocumentFieldDefinitions
    {
        public const string FixedLength = "C7FCC7C1-89BD-49D5-B6F4-1DE147EC3EC4";
        public const string LongText = "5D7F851C-2432-4B24-A49B-BB53E68ECFC6";
        public const string Date = "713192D2-3B73-4B6F-AFC8-E8BD5202008D";
        public const string WholeNumber = "301BB2F9-8E42-4A3B-8AD3-5E23D8AD24D5";
        public const string Decimal = "01C2C398-16BC-4BAF-B61B-314EAA31712A";
        public const string Currency = "A37BF251-D9C1-4390-9D1E-ED6367DDFDA1";
        public const string YesNo = "1EE3A20E-F2A9-4233-8C81-5D399A0CFF8C";
        public const string SingleChoice = "CFE7ADBC-4B30-4975-928D-2D9779743BCD";
        public const string Multichoice = "72FDCAFC-1ABA-4E79-8D8B-EB1FB553E413";

        public static IEnumerable<object[]> FieldTestData
        {
            get
            {
                var dateUnderTest = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));

                return new[]
                {
                    new object []{DocumentFieldDefinitions.FixedLength, "hello world", "hello world"},
                    new object []{DocumentFieldDefinitions.LongText, "hello world", "hello world"},
                    new object []{DocumentFieldDefinitions.Currency, "5,025.30", 5025.30},
                    new object []{DocumentFieldDefinitions.Decimal, "1.05", 1.05},
                    new object []{DocumentFieldDefinitions.WholeNumber, "1", 1L},
                    new object []{DocumentFieldDefinitions.YesNo, true, true},
                    new object []{DocumentFieldDefinitions.Date, dateUnderTest, dateUnderTest},
                };
            }

        }
    }


}
