using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{   

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class ORD : InputRecordB
    {

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(4)]
        public string Portfolio_Number_4 { get; set; }

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(5)]
        public string Sector_5 { get; set; }

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(6)]
        public string Contract_Type_6 { get; set; }

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(7)]
        public string Status_7 { get; set; }

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(8)]
        public string Reference_8 { get; set; }

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(9)]
        public string Type_9 { get; set; }

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(10)]
        public string Direction_10 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        [FieldOrder(11)]
        public DateTime? Limit_Date_End_11 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        [FieldOrder(12)]
        public DateTime? Limit_Date_Start_12 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        [FieldOrder(13)]
        public decimal? Quantity_13 { get; set; }

        [FieldFixedLength(15)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Int64)]
        [FieldOrder(14)]
        public long? Sec_Num_14 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(15)]
        public string Sec_Description_15 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(16)]
        public string Currency_16 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        [FieldOrder(17)]
        public decimal? Quote_17 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        [FieldOrder(18)]
        public decimal? Limit_Price_18 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(19)]
        public string Description_19 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        [FieldOrder(20)]
        public decimal? Average_Quote_20 { get; set; }

        public ORD() : base(INPUT_ORD_MSG_TYPE)
        {
            this.Portfolio_Number_4 = string.Empty;
            this.Sector_5 = string.Empty;
            this.Contract_Type_6 = string.Empty;
            this.Status_7 = string.Empty;
            this.Reference_8 = string.Empty;
            this.Type_9 = string.Empty;
            this.Direction_10 = string.Empty;
            this.Limit_Date_End_11 = null; 
            this.Limit_Date_Start_12 = null;
            this.Quantity_13 = 0;
            this.Sec_Num_14 = 0;
            this.Sec_Description_15 = string.Empty;
            this.Currency_16 = string.Empty;
            this.Quote_17 = 0;
            this.Limit_Price_18 = null;
            this.Description_19 = string.Empty; 
            this.Average_Quote_20 = null;
        }

        public ORD(ORD source) : base(source)
        {
            this.Portfolio_Number_4 = source.Portfolio_Number_4;
            this.Sector_5 = source.Sector_5;
            this.Contract_Type_6 = source.Contract_Type_6;
            this.Status_7 = source.Status_7;
            this.Reference_8 = source.Reference_8;
            this.Type_9 = source.Type_9;
            this.Direction_10 = source.Direction_10;
            this.Limit_Date_End_11 = source.Limit_Date_End_11;
            this.Limit_Date_Start_12 = source.Limit_Date_Start_12;
            this.Quantity_13 = source.Quantity_13;
            this.Sec_Num_14 = source.Sec_Num_14;
            this.Sec_Description_15 = source.Sec_Description_15;
            this.Currency_16 = source.Currency_16;
            this.Quote_17 = source.Quote_17;
            this.Limit_Price_18 = source.Limit_Price_18;
            this.Description_19 = source.Description_19;
            this.Average_Quote_20 = source.Average_Quote_20;
        }

    }

}