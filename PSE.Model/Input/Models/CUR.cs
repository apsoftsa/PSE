using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{   

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class CUR : InputRecordB
    {

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(4)]
        public string PortfolioNumber_4 { get; set; }

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(5)]
        public string Currency_5 { get; set; }

        [FieldFixedLength(17)]
        [FieldConverter(ConverterKind.Decimal, ".")]
        [FieldOrder(6)]
        public decimal? Rate_6 { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldOrder(7)]
        public string Model_1 { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldOrder(8)]
        public string Model_2 { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldOrder(9)]
        public string Model_3 { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldOrder(10)]
        public string Model_4 { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldOrder(11)]
        public string Model_5 { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldOrder(12)]
        public string Model_6 { get; set; }

        public CUR() : base(INPUT_CUR_MSG_TYPE)
        {
            this.PortfolioNumber_4 = string.Empty;
            this.Currency_5 = string.Empty;
            this.Rate_6 = 0;
            this.Model_1 = string.Empty;
            this.Model_2 = string.Empty;
            this.Model_3 = string.Empty;
            this.Model_4 = string.Empty;
            this.Model_5 = string.Empty;
            this.Model_6 = string.Empty;
        }

        public CUR(CUR source) : base(source)
        {
            this.PortfolioNumber_4 = source.PortfolioNumber_4;
            this.Currency_5 = source.Currency_5;
            this.Rate_6 = source.Rate_6;
            this.Model_1 = source.Model_1;
            this.Model_2 = source.Model_2;
            this.Model_3 = source.Model_3;
            this.Model_4 = source.Model_4;
            this.Model_5 = source.Model_5;
            this.Model_6 = source.Model_6;
        }

    }

}