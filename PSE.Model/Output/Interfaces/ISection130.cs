namespace PSE.Model.Output.Interfaces
{

    public interface IStockOrder
    {

        string? Order { get; set; }

        long? OrderValue { get; set; }

        string? Description { get; set; }

        string? Operation { get; set; }

        decimal? Amount { get; set; }

        string? Currency { get; set; }

        string LimitStopLoss { get; set; }

        decimal? Price { get; set; }

        string? ExpirationDate { get; set; }

    }

    public interface IStockOrderSubSection
    {

        string Name { get; set; }

        IList<IStockOrder> Content { get; set; }

    }

    public interface ISection130Content
    {

        IStockOrderSubSection SubSection13000 { get; set; }

    }

}