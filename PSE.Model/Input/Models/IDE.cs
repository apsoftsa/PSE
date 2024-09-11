using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class IDE : InputRecordB
    {

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(4)]
        public string Agency_4 { get; set; }

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(5)]
        public string CustomerNameShort_5 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(6)]
        public string CustomerId_6 { get; set; }

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(7)]
        public string Nature_7 { get; set; }

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(8)]
        public string Manager_8 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Int32, ".")]
        [FieldOrder(9)]
        public int? EmployeeType_9 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(10)]
        public string Employee_10 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(11)]
        public string Mandate_11 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(12)]
        public string PortfolioType_12 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(13)]
        public string ResidenceCurrency_13 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(14)]
        public string Dimension_14 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        [FieldOrder(15)]
        public DateTime? Date_15 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "HHmm")]
        [FieldOrder(16)]
        public DateTime? Time_16 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(17)]
        public string RiskAllocation_17 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(18)]
        public string Language_18 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(19)]
        public string Currency_19 { get; set; }

        [FieldFixedLength(10)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(20)]
        public string BaseOptions_20 { get; set; }

        [FieldFixedLength(5)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(21)]
        public string ModelCode_21 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(22)]
        public string Origin_22 { get; set; }

        [FieldFixedLength(32)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(23)]
        public string CustomerName1_23 { get; set; }

        [FieldFixedLength(32)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(24)]
        public string CustomerName2_24 { get; set; }

        [FieldFixedLength(32)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(25)]
        public string CustomerName3_25 { get; set; }

        [FieldFixedLength(32)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(26)]
        public string CustomerName4_26 { get; set; }

        [FieldFixedLength(32)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(27)]
        public string CustomerName5_27 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(28)]
        public string Manager1_28 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(29)]
        public string Manager2_29 { get; set; }

        [FieldFixedLength(3)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(30)]
        public string Nationality_30 { get; set; }

        [FieldFixedLength(3)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(31)]
        public string Residence_31 { get; set; }

        [FieldFixedLength(3)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(32)]
        public string FiscalResidence_32 { get; set; }

        [FieldFixedLength(3)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(33)]
        public string ClientGroup_33 { get; set; }

        public IDE() : base(INPUT_IDE_MSG_TYPE)
        {
            Agency_4 = string.Empty;
            CustomerNameShort_5 = string.Empty;
            CustomerId_6 = string.Empty;
            Nature_7 = string.Empty;
            Manager_8 = string.Empty;
            EmployeeType_9 = null;
            Employee_10 = string.Empty;
            Mandate_11 = string.Empty;
            PortfolioType_12 = string.Empty;
            ResidenceCurrency_13 = string.Empty;
            Dimension_14 = string.Empty;
            Date_15 = null;
            Time_16 = null;
            RiskAllocation_17 = string.Empty;
            Language_18 = string.Empty;
            Currency_19 = string.Empty;
            BaseOptions_20 = string.Empty;
            ModelCode_21 = string.Empty;
            Origin_22 = string.Empty;
            CustomerName1_23 = string.Empty;
            CustomerName2_24 = string.Empty;
            CustomerName3_25 = string.Empty;
            CustomerName4_26 = string.Empty;
            CustomerName5_27 = string.Empty;
            Manager1_28 = string.Empty;
            Manager2_29 = string.Empty;
            Nationality_30 = string.Empty;
            Residence_31 = string.Empty;
            FiscalResidence_32 = string.Empty;
            ClientGroup_33 = string.Empty;
        }

        public IDE(IDE source) : base(source)
        {
            Agency_4 = source.Agency_4;
            CustomerNameShort_5 = source.CustomerNameShort_5;
            CustomerId_6 = source.CustomerId_6;
            Nature_7 = source.Nature_7;
            Manager_8 = source.Manager_8;
            EmployeeType_9 = source.EmployeeType_9;
            Employee_10 = source.Employee_10;
            Mandate_11 = source.Mandate_11;
            PortfolioType_12 = source.PortfolioType_12;
            ResidenceCurrency_13 = source.ResidenceCurrency_13;
            Dimension_14 = source.Dimension_14;
            Date_15 = source.Date_15;
            Time_16 = source.Time_16;
            RiskAllocation_17 = source.RiskAllocation_17;
            Language_18 = source.Language_18;
            Currency_19 = source.Currency_19;
            BaseOptions_20 = source.BaseOptions_20;
            ModelCode_21 = source.ModelCode_21;
            Origin_22 = source.Origin_22;
            CustomerName1_23 = source.CustomerName1_23;
            CustomerName2_24 = source.CustomerName2_24;
            CustomerName3_25 = source.CustomerName3_25;
            CustomerName4_26 = source.CustomerName4_26;
            CustomerName5_27 = source.CustomerName5_27;
            Manager1_28 = source.Manager1_28;
            Manager2_29 = source.Manager2_29;
            Nationality_30 = source.Nationality_30;
            Residence_31 = source.Residence_31;
            FiscalResidence_32 = source.FiscalResidence_32;
            ClientGroup_33 = source.ClientGroup_33;
        }

    }

}