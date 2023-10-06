namespace PSE.Model.Params
{

    public record ExchangeRateChangeValueParams
    {

        public string Sign { get; init; }
        public decimal Change1 { get; init; }
        public decimal Change2 { get; init; }
        public decimal Percentage { get; init; }

        public ExchangeRateChangeValueParams(string sign, decimal change1, decimal change2, decimal percentage)
        {
            Sign = sign;
            Change1 = change1;
            Change2 = change2;
            Percentage = percentage;
        }

        public ExchangeRateChangeValueParams()
        {
            Sign = Common.Constants.NEGATIVE_SIGN;
            Change1 = 0;
            Change2 = 0;
            Percentage = 0;
        }

        public virtual bool IsValid => string.IsNullOrEmpty(Sign) == false
            && Change1 != 0
            && Change2 != 0;

    }

    public record GlobalVariationValueParams : ExchangeRateChangeValueParams
    {

        public decimal Trend1 { get; init; }
        public decimal Trend2 { get; init; }

        public GlobalVariationValueParams(string sign, decimal trend1, decimal trend2, decimal change1, decimal change2, decimal percentage) : base(sign, change1, change2, percentage)
        {
            Trend1 = trend1;
            Trend2 = trend2;
        }

        public GlobalVariationValueParams() : base()
        {
            Trend1 = 0;
            Trend2 = 0;
        }

        public override bool IsValid => string.IsNullOrEmpty(Sign) == false
            && Trend1 != 0 && Change1 != 0;

    }

    public record CourseChangeValueParams 
    {
        public string Sign { get; init; }
        public decimal Trend1 { get; init; }
        public decimal Trend2 { get; init; }

        public CourseChangeValueParams(string sign, decimal trend1, decimal trend2) 
        {
            Sign = sign;
            Trend1 = trend1;
            Trend2 = trend2;
        }

        public CourseChangeValueParams() 
        {
            Sign = string.Empty;
            Trend1 = 0;
            Trend2 = 0;
        }

        public virtual bool IsValid => string.IsNullOrEmpty(Sign) == false && Trend1 > 0;

    }

    public record PriceDifferenceValueParams : CourseChangeValueParams
    {

        public PriceDifferenceValueParams(string sign, decimal trend1, decimal trend2) : base(sign, trend1, trend2) { }

        public PriceDifferenceValueParams() : base() { }

        public override bool IsValid => string.IsNullOrEmpty(Sign) == false;

    }

    public record ExchangeRateDifferenceValueParams
    {

        public string Code { get; init; }
        public string Sign { get; init; }
        public decimal ChangeAcq { get; init; }
        public decimal Change { get; init; }
        public decimal ChangeAcqM { get; init; }
        public decimal ChangeM { get; init; }
        public decimal Reverse { get; init; }

        public ExchangeRateDifferenceValueParams(string code, string sign, decimal changeAcq, decimal change, decimal changeAcqM, decimal changeM, decimal reverse)
        {
            Code = code;
            Sign = sign;
            ChangeAcq = changeAcq;
            Change = change;
            ChangeAcqM = changeAcqM;
            ChangeM = changeM;
            Reverse = reverse;
        }

        public ExchangeRateDifferenceValueParams()
        {
            Code = string.Empty;
            Sign = Common.Constants.NEGATIVE_SIGN;
            ChangeAcq = 0;
            Change = 0;
            ChangeAcqM = 0;
            ChangeM = 0;
            Reverse = 0;
        }

        public virtual bool IsValid => string.IsNullOrEmpty(Code) == false
            && string.IsNullOrEmpty(Sign) == false;

    }

    public record GlobalDifferenceValueParams : ExchangeRateDifferenceValueParams
    {

        public decimal TrendAcq { get; init; }
        public decimal Trend { get; init; }

        public GlobalDifferenceValueParams(string code, string sign, decimal trendAcq, decimal trend, decimal changeAcq, decimal change, decimal changeAcqM, decimal changeM, decimal reverse)
            : base(code, sign, changeAcq, change, changeAcqM, changeM, reverse)
        {
            TrendAcq = trendAcq;
            Trend = trend;
        }

        public GlobalDifferenceValueParams() : base()
        {
            TrendAcq = 0;
            Trend = 0;
        }

    }
    
    public record UnrealizedValueParams
    {

        public decimal ImpCtv { get; init; }
        public string Code { get; init; }
        public string Sign { get; init; }
        public decimal TrendAcq { get; init; }
        public decimal Trend { get; init; }
        public decimal ChangeAcq { get; init; }
        public decimal Change { get; init; }
        public decimal ChangeAcqM { get; init; }
        public decimal ChangeM { get; init; }
        public decimal Reverse { get; init; }

        public UnrealizedValueParams(decimal impCtv, string code, string sign, decimal trendAcq, decimal trend, decimal changeAcq, decimal change, decimal changeAcqM, decimal changeM, decimal reverse)
        {
            ImpCtv = impCtv;
            Code = code;
            Sign = sign;
            TrendAcq = trendAcq;
            Trend = trend;
            ChangeAcq = changeAcq;
            Change = change;
            ChangeAcqM = changeAcqM;
            ChangeM = changeM;
            Reverse = reverse;
        }

        public UnrealizedValueParams()
        {
            ImpCtv = 0;
            Code = string.Empty;
            Sign = Common.Constants.NEGATIVE_SIGN;
            TrendAcq = 0;
            Trend = 0;
            ChangeAcq = 0;
            Change = 0;
            ChangeAcqM = 0;
            ChangeM = 0;
            Reverse = 0;
        }

        public bool IsValid => string.IsNullOrEmpty(Code) == false
            && string.IsNullOrEmpty(Sign) == false;

    }
    
}
