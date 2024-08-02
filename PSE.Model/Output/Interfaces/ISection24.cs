namespace PSE.Model.Output.Interfaces
{

    public interface IExchange
    {

        string ExchangeOrder { get; set; }

        long? ExchangeValue { get; set; }

        string? Description { get; set; }

        string? Operation { get; set; }

        decimal? Quantity { get; set; }

        string? Currency { get; set; }

        string LimitStopLoss { get; set; }

        decimal? CourseCost { get; set; }

        string? ExpirationDate { get; set; }

    }

    public interface ISection24Content
    {

        IList<IExchange> Exchange { get; set; }

    }

}