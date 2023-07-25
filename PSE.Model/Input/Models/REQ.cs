using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class REQ : InputRecordA
    {

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(4)]
        public string Grouping_4 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(5)]
        public string GroupingNumberRequested_5 { get; set; }

        [FieldFixedLength(3)]
        [FieldConverter(ConverterKind.Int32, ".")]
        [FieldOrder(6)]
        public int CopiesNumber_6 { get; set; }

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(7)]
        public string Currency_7 { get; set; }

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(8)]
        public string ModelCode_8 { get; set; }

        [FieldFixedLength(8)]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        [FieldOrder(9)]
        public DateTime? Date_9 { get; set; }

        [FieldFixedLength(4)]
        [FieldConverter(ConverterKind.Int32, ".")]
        [FieldOrder(10)]
        public int Time_10 { get; set; }

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(11)]
        public string RiskAllocation_11 { get; set; }

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(12)]
        public string Language_12 { get; set; }

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(13)]
        public string Usage_13 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(14)]
        public string Origin_14 { get; set; }

        [FieldFixedLength(3)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(15)]
        public string CurrencyAggregation_15 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(16)]
        public string OutputType_16 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(17)]
        public string PerformanceType_17 { get; set; }

        [FieldFixedLength(60)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(18)]
        public string Options_18 { get; set; }

        public REQ() : base(INPUT_REQ_MSG_TYPE)
        {
            Grouping_4 = string.Empty;
            GroupingNumberRequested_5 = string.Empty;
            CopiesNumber_6 = 0;
            Currency_7 = string.Empty;
            ModelCode_8 = string.Empty;
            Date_9 = null;
            Time_10 = 0;
            RiskAllocation_11 = string.Empty;
            Language_12 = string.Empty;
            Usage_13 = string.Empty;
            Origin_14 = string.Empty;
            CurrencyAggregation_15 = string.Empty;
            OutputType_16 = string.Empty;
            PerformanceType_17 = string.Empty;
            Options_18 = string.Empty;
        }

        public REQ(REQ source) : base(source)
        {
            Grouping_4 = source.Grouping_4;
            GroupingNumberRequested_5 = source.GroupingNumberRequested_5;
            CopiesNumber_6 = source.CopiesNumber_6;
            Currency_7 = source.Currency_7;
            ModelCode_8 = source.ModelCode_8;
            Date_9 = source.Date_9;
            Time_10 = source.Time_10;
            RiskAllocation_11 = source.RiskAllocation_11;
            Language_12 = source.Language_12;
            Usage_13 = source.Usage_13;
            Origin_14 = source.Origin_14;
            CurrencyAggregation_15 = source.CurrencyAggregation_15;
            OutputType_16 = source.OutputType_16;
            PerformanceType_17 = source.PerformanceType_17;
            Options_18 = source.Options_18;
        }

    }

}