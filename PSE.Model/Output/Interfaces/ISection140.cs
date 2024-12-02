namespace PSE.Model.Output.Interfaces
{

    public interface IFundAccumulationPlanPayment
    {     

        string Frequency { get; set; }
       
        string Currency { get; set; }

        decimal Amount { get; set; }

        decimal Executed { get; set; }

    }

    public interface IFundAccumulationPlan
    {       

        string Description1 { get; set; }

        string Description2 { get; set; }

        string ExpirationDate { get; set; }

        IList<IFundAccumulationPlanPayment> Payments { get; set; }

        decimal AveragePurchasePrice { get; set; }
       
        decimal SharesPurchased { get; set; }

        decimal Exchange { get; set; }

        decimal MarketValueReportingCurrency { get; set; }

        decimal PercentWeigth { get; set; }

    }

    public interface IFACSubSection
    {

        string Name { get; set; }

        IList<IFundAccumulationPlan> Content { get; set; }

    }

    public interface ISection140Content
    {

        IFACSubSection SubSection14000 { get; set; }

    }

}