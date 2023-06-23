using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class PER : InputRecordB
    {

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        public string PortfolioNumber_4 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Int32, ".")]
        public int? Type_5 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? StartDate_6 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? EndDate_7 { get; set; }

        [FieldFixedLength(17)]        
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? StartValue_8 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? EndValue_9 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? CashIn_10 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? CashOut_11 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? SecIn_12 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? SecOut_13 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? TWR_14 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Interest_15 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? PlRealEquity_16 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? PlRealCurrency_17 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? PlNonRealEquity_18 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? PlNonRealCurrency_19 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Currency_20 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Boolean, "1", "0")]
        public bool? Consolidate_21 { get; set; }

        public PER() : base(INPUT_PER_MSG_TYPE)
        {
            PortfolioNumber_4 = string.Empty;
            Type_5 = 0;
            StartDate_6 = null;
            EndDate_7 = null;
            StartValue_8 = 0;
            EndValue_9 = 0;
            CashIn_10 = 0;
            CashOut_11 = 0;
            SecIn_12 = 0;
            SecOut_13 = 0;
            TWR_14 = 0;
            Interest_15 = 0;
            PlRealEquity_16 = 0;
            PlRealCurrency_17 = 0;
            PlNonRealEquity_18 = 0;
            PlNonRealCurrency_19 = 0;
            Currency_20 = string.Empty;
            Consolidate_21 = false;
        }

        public PER(PER source) : base(source)
        {
            PortfolioNumber_4 = source.PortfolioNumber_4;
            Type_5 = source.Type_5;
            StartDate_6 = source.StartDate_6;
            EndDate_7 = source.EndDate_7;
            StartValue_8 = source.StartValue_8;
            EndValue_9 = source.EndValue_9;
            CashIn_10 = source.CashIn_10;
            CashOut_11 = source.CashOut_11;
            SecIn_12 = source.SecIn_12;
            SecOut_13 = source.SecOut_13;
            TWR_14 = source.TWR_14;
            Interest_15 = source.Interest_15;
            PlRealEquity_16 = source.PlRealEquity_16;
            PlRealCurrency_17 = source.PlRealCurrency_17;
            PlNonRealEquity_18 = source.PlNonRealEquity_18;
            PlNonRealCurrency_19 = source.PlNonRealCurrency_19;
            Currency_20 = source.Currency_20;
            Consolidate_21 = source.Consolidate_21;
        }

    }

}