using System;
using System.Collections.Generic;

namespace ObjectManager.Rest.Tests.Integration.Common
{
    public class SingleChoiceChoiceDefinitions
    {
        public const string Single1 = "2BB4F900-8645-425B-93D3-257CEFB8D38C";
        public const string Single2 = "1087D533-32A9-4945-9C5F-58A42E7B53FB";
        public const string Single3 = "7ACFA152-3C37-435C-885A-3C75C9991CCB";
    }
    public class MultiChoiceChoiceDefinitions
    {
        public const string Multi1 = "FEB13633-0608-46E2-8460-8733204397B5";
        public const string Multi2 = "7BF40F59-35AE-4DC2-8FF1-01A728DA19F9";
        public const string Multi3 = "4C1C596F-A87A-4F80-9757-25A67DE63F89";
    }

    public class LayoutDefinitions
    {
        public const string Default = "0FAE5FF4-053E-4D16-976D-DB4B5676F30F";
    }

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
                    new object []{ FixedLength, "hello world", "hello world"},
                    new object []{ LongText, "hello world", "hello world"},
                    new object []{ Currency, "5,025.30", 5025.30},
                    new object []{ Decimal, "1.05", 1.05},
                    new object []{ WholeNumber, "1", 1L},
                    new object []{ YesNo, true, true},
                    new object []{ Date, dateUnderTest, dateUnderTest}
                };
            }

        }
    }


}
