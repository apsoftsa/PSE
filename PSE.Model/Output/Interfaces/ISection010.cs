namespace PSE.Model.Output.Interfaces
{

    public interface IKeyInformation
    {

        string Customer { get; set; }

        string CustomerID { get; set; }

        string Portfolio { get; set; }        

        int? RiskProfile { get; set; }

        string EsgProfile { get; set; }

    }           

    public interface ISubSection1000Content
    {

        string Name { get; set; }

        IList<IKeyInformation> Content { get; set; }

    }

    public interface ISubSection1010Content
    {

        string Name { get; set; }

        string ManagementReportFromDate { get; set; }   

        string ManagementReportToDate { get; set; } 

        string PortfolioValueDate { get; set; } 

        decimal? PortfolioValueReportingCurrency { get; set; }   

        decimal? ContributionsValueReportingCurrency { get; set; }   

        decimal? WithdrawalsValueReportingCurrency { get; set; } 

        decimal? PortfolioValueRectifiedReportingCurrency { get; set; }  

        string PortfolioValueDate2 { get; set; }    

        decimal? PortfolioValueReportingCurrency2 { get; set; }  

        decimal? PluslessValueReportingCurrency { get; set; }    

        decimal? PercentWightedPerformance {  get; set; }    

        decimal? PatrimonialFluctuation { get; set; }   

    }

    public interface ISubSection1011Content
    {

        string Name { get; set; }

        decimal? DividendAndInterestValueReportingCurrency { get; set; } 

        decimal? RealizedGainLossesValueReportingCurrency { get; set; }  

        decimal? OngoingRealizedGainLossesValueReportingCurrency { get; set; }

        decimal? OnCurrencyRealizedGainLossesValueReportingCurrency { get; set; }

        decimal? NotRealizedGainLossesValueReportingCurrency { get; set; }

        decimal? OngoingNotRealizedGainLossesValueReportingCurrency { get; set; }

        decimal? OnCurrencyNotRealizedGainLossesValueReportingCurrency { get; set; } 

        decimal? PlusLessValueReportingCurrency { get; set; }    

    }

    public interface ISection010Content
    {

        ISubSection1000Content? SubSection1000 { get; set; }

        ISubSection1010Content? SubSection1010 { get; set; }

        ISubSection1011Content? SubSection1011 { get; set; }

    }

}
