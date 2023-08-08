namespace PSE.Model.Output.Interfaces
{

    public interface IExchange
    {

        int? ExchangeOrder { get; set; }

        int? ExchangeValue { get; set; }

        string? Description { get; set; }

        string? Operation { get; set; }

        int? Quantity { get; set; }

        string? Currency { get; set; }

        string? LimitStopLoss { get; set; }

        decimal? CourseCost { get; set; }

        string? ExpirationDate { get; set; }

    }

    public interface ISection24Content
    {

        IList<IExchange> Exchange { get; set; }

    }

}