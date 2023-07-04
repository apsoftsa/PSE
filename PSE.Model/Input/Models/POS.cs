using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class POS : InputRecordB
    {

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        public string PortfolioNumber_4 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string HostPositionType_5 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string HostPositionReference_6 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string HostPositionAgency_7 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string HostPositionCurrency_8 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Boolean, "1", "0")]
        public bool? Visible_9 { get; set; }

        [FieldFixedLength(5)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Type_10 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Category_11 { get; set; }

        [FieldFixedLength(6)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string SubCat1_12 { get; set; }

        [FieldFixedLength(6)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string SubCat2_13 { get; set; }

        [FieldFixedLength(6)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string SubCat3_14 { get; set; }

        [FieldFixedLength(6)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string SubCat4_15 { get; set; }

        [FieldFixedLength(6)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string SubCat5_16 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Currency1_17 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Currency2_18 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? CouponDate_19 { get; set; }

        [FieldFixedLength(3)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Country_20 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string CountryCurrency_21 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Amount1Cur1_22 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Amount1Base_23 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Amount1Euro_24 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Amount1Country_25 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Amount2Cur1_26 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Amount1ProRataHostCur_27 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Quantity_28 { get; set; }

        [FieldFixedLength(15)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string NumSecurity_29 { get; set; }

        [FieldFixedLength(6)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string NumSavingsBook_30 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string MovementKey_31 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Description1_32 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Description2_33 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string CouponFrequency_34 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string CouponText_35 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? MaturityDate_36 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Boolean, "1", "0")]
        public bool? Callable_37 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? CallaDate_38 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Boolean, "1", "0")]
        public bool? OnCall_39 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? AmortizationDate_40 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? ConversionDateStart_41 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? ConversionDateEnd_42 { get; set; }

        [FieldFixedLength(4)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ConversionCurrency_43 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ConversionFactor_44 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ConversionDesc_45 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? IssueDate_46 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? InterestRate_47 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? Quote_48 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        //[FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public string QuoteDate_49 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Boolean, "1", "0")]
        public bool? QuoteOfficial_50 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string QuoteType_51 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string MaturityPrice_52 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal? BuyPriceHistoric_53 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Boolean, "1", "0")]
        public bool? PriceWarning_54 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ProRataCur1_55 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ProRataBase_56 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ProRataEuro_57 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ProRataCountry_58 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Amount2Cur2_59 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Amount2Base_60 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Amount2Euro_61 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Amount2HoustCur_62 { get; set; }

        [FieldFixedLength(7)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string MaturityRoi_63 { get; set; }

        [FieldFixedLength(7)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string DirectRoi_64 { get; set; }

        [FieldFixedLength(7)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string CallRoi_65 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string BuyExchangeRateHistoric_66 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Strike_67 { get; set; }

        [FieldFixedLength(7)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Duration_68 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string MaturityProRataCurr1_69 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string MaturityProRataBase_70 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string MaturityProRataEuro_71 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string MaturityProRataCountry_72 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Lock1_73 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string QuantityLock1_74 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Lock2_75 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string QuantityLock2_76 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Lock3_77 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string QuantityLock3_78 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Lock4_79 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string QuantityLock4_80 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Lock5_81 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string QuantityLock5_82 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string EngagementValueBase_83 { get; set; }

        [FieldFixedLength(8)]
        [FieldOptional]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime? MaturityDateObbl_84 { get; set; }

        [FieldFixedLength(34)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string IsinIban_85 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string TaxType_86 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string BuyPriceAverage_87 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string BuyExchangeRateAverage_88 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string InverseExchangeRate_89 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Amount1Request_90 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Amount2Request_91 { get; set; }

        [FieldFixedLength(17)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ProRataRequest_92 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ConversionBDesc_93 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ConversionCDesc_94 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ConversionDDesc_95 { get; set; }

        [FieldFixedLength(30)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string ConversionEDesc_96 { get; set; }

        [FieldFixedLength(2)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string AgeRat_97 { get; set; }

        [FieldFixedLength(7)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string Rating_98 { get; set; }

        [FieldFixedLength(1)]
        [FieldOptional]
        [FieldTrim(TrimMode.Both)]
        public string OptionsLock_99 { get; set; }

        public POS() : base(INPUT_POS_MSG_TYPE)
        {
            PortfolioNumber_4 = string.Empty;
            HostPositionType_5 = string.Empty;
            HostPositionReference_6 = string.Empty;
            HostPositionAgency_7 = string.Empty;
            HostPositionCurrency_8 = string.Empty;
            Visible_9 = false;
            Type_10 = string.Empty;
            Category_11 = string.Empty;
            SubCat1_12 = string.Empty;
            SubCat2_13 = string.Empty;
            SubCat3_14 = string.Empty;
            SubCat4_15 = string.Empty;
            SubCat5_16 = string.Empty;
            Currency1_17 = string.Empty;
            Currency2_18 = string.Empty;
            CouponDate_19 = null;
            Country_20 = string.Empty;
            CountryCurrency_21 = string.Empty;
            Amount1Cur1_22 = 0;
            Amount1Base_23 = 0;
            Amount1Euro_24 = 0;
            Amount1Country_25 = 0;
            Amount2Cur1_26 = 0;
            Amount1ProRataHostCur_27 = 0;
            Quantity_28 = 0;
            NumSecurity_29 = string.Empty;
            NumSavingsBook_30 = string.Empty;
            MovementKey_31 = string.Empty;
            Description1_32 = string.Empty;
            Description2_33 = string.Empty;
            CouponFrequency_34 = string.Empty;
            CouponText_35 = string.Empty;
            MaturityDate_36 = null;
            Callable_37 = false;
            CallaDate_38 = null;
            OnCall_39 = false;
            AmortizationDate_40 = null;
            ConversionDateStart_41 = null;
            ConversionDateEnd_42 = null;
            ConversionCurrency_43 = string.Empty;
            ConversionFactor_44 = string.Empty;
            ConversionDesc_45 = string.Empty;
            IssueDate_46 = null;
            InterestRate_47 = null;
            Quote_48 = null;
            QuoteDate_49 = string.Empty;
            QuoteOfficial_50 = false;
            QuoteType_51 = string.Empty;
            MaturityPrice_52 = string.Empty;
            BuyPriceHistoric_53 = null;
            PriceWarning_54 = false;
            ProRataCur1_55 = string.Empty;
            ProRataBase_56 = string.Empty;
            ProRataEuro_57 = string.Empty;
            ProRataCountry_58 = string.Empty;
            Amount2Cur2_59 = string.Empty;
            Amount2Base_60 = string.Empty;
            Amount2Euro_61 = string.Empty;
            Amount2HoustCur_62 = string.Empty;
            MaturityRoi_63 = string.Empty;
            DirectRoi_64 = string.Empty;
            CallRoi_65 = string.Empty;
            BuyExchangeRateHistoric_66 = string.Empty;
            Strike_67 = string.Empty;
            Duration_68 = string.Empty;
            MaturityProRataCurr1_69 = string.Empty;
            MaturityProRataBase_70 = string.Empty;
            MaturityProRataEuro_71 = string.Empty;
            MaturityProRataCountry_72 = string.Empty;
            Lock1_73 = string.Empty;
            QuantityLock1_74 = string.Empty;
            Lock2_75 = string.Empty;
            QuantityLock2_76 = string.Empty;
            Lock3_77 = string.Empty;
            QuantityLock3_78 = string.Empty;
            Lock4_79 = string.Empty;
            QuantityLock4_80 = string.Empty;
            Lock5_81 = string.Empty;
            QuantityLock5_82 = string.Empty;
            EngagementValueBase_83 = string.Empty;
            MaturityDateObbl_84 = null;
            IsinIban_85 = string.Empty;
            TaxType_86 = string.Empty;
            BuyPriceAverage_87 = string.Empty;
            BuyExchangeRateAverage_88 = string.Empty;
            InverseExchangeRate_89 = string.Empty;
            Amount1Request_90 = string.Empty;
            Amount2Request_91 = string.Empty;
            ProRataRequest_92 = string.Empty;
            ConversionBDesc_93 = string.Empty;
            ConversionCDesc_94 = string.Empty;
            ConversionDDesc_95 = string.Empty;
            ConversionEDesc_96 = string.Empty;
            AgeRat_97 = string.Empty;
            Rating_98 = string.Empty;
            OptionsLock_99 = string.Empty;
        }

        public POS(POS source) : base(source)
        {
            PortfolioNumber_4 = source.PortfolioNumber_4;
            HostPositionType_5 = source.HostPositionType_5;
            HostPositionReference_6 = source.HostPositionReference_6;
            HostPositionAgency_7 = source.HostPositionAgency_7;
            HostPositionCurrency_8 = source.HostPositionCurrency_8;
            Visible_9 = source.Visible_9;
            Type_10 = source.Type_10;
            Category_11 = source.Category_11;
            SubCat1_12 = source.SubCat1_12;
            SubCat2_13 = source.SubCat2_13;
            SubCat3_14 = source.SubCat3_14;
            SubCat4_15 = source.SubCat4_15;
            SubCat5_16 = source.SubCat5_16;
            Currency1_17 = source.Currency1_17;
            Currency2_18 = source.Currency2_18;
            CouponDate_19 = source.CouponDate_19;
            Country_20 = source.Country_20;
            CountryCurrency_21 = source.CountryCurrency_21;
            Amount1Cur1_22 = source.Amount1Cur1_22;
            Amount1Base_23 = source.Amount1Base_23;
            Amount1Euro_24 = source.Amount1Euro_24;
            Amount1Country_25 = source.Amount1Country_25;
            Amount2Cur1_26 = source.Amount2Cur1_26;
            Amount1ProRataHostCur_27 = source.Amount1ProRataHostCur_27;
            Quantity_28 = source.Quantity_28;
            NumSecurity_29 = source.NumSecurity_29;
            NumSavingsBook_30 = source.NumSavingsBook_30;
            MovementKey_31 = source.MovementKey_31;
            Description1_32 = source.Description1_32;
            Description2_33 = source.Description2_33;
            CouponFrequency_34 = source.CouponFrequency_34;
            CouponText_35 = source.CouponText_35;
            MaturityDate_36 = source.MaturityDate_36;
            Callable_37 = source.Callable_37;
            CallaDate_38 = source.CallaDate_38;
            OnCall_39 = source.OnCall_39;
            AmortizationDate_40 = source.AmortizationDate_40;
            ConversionDateStart_41 = source.ConversionDateStart_41;
            ConversionDateEnd_42 = source.ConversionDateEnd_42;
            ConversionCurrency_43 = source.ConversionCurrency_43;
            ConversionFactor_44 = source.ConversionFactor_44;
            ConversionDesc_45 = source.ConversionDesc_45;
            IssueDate_46 = source.IssueDate_46;
            InterestRate_47 = source.InterestRate_47;
            Quote_48 = source.Quote_48;
            QuoteDate_49 = source.QuoteDate_49;
            QuoteOfficial_50 = source.QuoteOfficial_50;
            QuoteType_51 = source.QuoteType_51;
            MaturityPrice_52 = source.MaturityPrice_52;
            BuyPriceHistoric_53 = source.BuyPriceHistoric_53;
            PriceWarning_54 = source.PriceWarning_54;
            ProRataCur1_55 = source.ProRataCur1_55;
            ProRataBase_56 = source.ProRataBase_56;
            ProRataEuro_57 = source.ProRataEuro_57;
            ProRataCountry_58 = source.ProRataCountry_58;
            Amount2Cur2_59 = source.Amount2Cur2_59;
            Amount2Base_60 = source.Amount2Base_60;
            Amount2Euro_61 = source.Amount2Euro_61;
            Amount2HoustCur_62 = source.Amount2HoustCur_62;
            MaturityRoi_63 = source.MaturityRoi_63;
            DirectRoi_64 = source.DirectRoi_64;
            CallRoi_65 = source.CallRoi_65;
            BuyExchangeRateHistoric_66 = source.BuyExchangeRateHistoric_66;
            Strike_67 = source.Strike_67;
            Duration_68 = source.Duration_68;
            MaturityProRataCurr1_69 = source.MaturityProRataCurr1_69;
            MaturityProRataBase_70 = source.MaturityProRataBase_70;
            MaturityProRataEuro_71 = source.MaturityProRataEuro_71;
            MaturityProRataCountry_72 = source.MaturityProRataCountry_72;
            Lock1_73 = source.Lock1_73;
            QuantityLock1_74 = source.QuantityLock1_74;
            Lock2_75 = source.Lock2_75;
            QuantityLock2_76 = source.QuantityLock2_76;
            Lock3_77 = source.Lock3_77;
            QuantityLock3_78 = source.QuantityLock3_78;
            Lock4_79 = source.Lock4_79;
            QuantityLock4_80 = source.QuantityLock4_80;
            Lock5_81 = source.Lock5_81;
            QuantityLock5_82 = source.QuantityLock5_82;
            EngagementValueBase_83 = source.EngagementValueBase_83;
            MaturityDateObbl_84 = source.MaturityDateObbl_84;
            IsinIban_85 = source.IsinIban_85;
            TaxType_86 = source.TaxType_86;
            BuyPriceAverage_87 = source.BuyPriceAverage_87;
            BuyExchangeRateAverage_88 = source.BuyExchangeRateAverage_88;
            InverseExchangeRate_89 = source.InverseExchangeRate_89;
            Amount1Request_90 = source.Amount1Request_90;
            Amount2Request_91 = source.Amount2Request_91;
            ProRataRequest_92 = source.ProRataRequest_92;
            ConversionBDesc_93 = source.ConversionBDesc_93;
            ConversionCDesc_94 = source.ConversionCDesc_94;
            ConversionDDesc_95 = source.ConversionDDesc_95;
            ConversionEDesc_96 = source.ConversionEDesc_96;
            AgeRat_97 = source.AgeRat_97;
            Rating_98 = source.Rating_98;
            OptionsLock_99 = source.OptionsLock_99;
        }

    }

}