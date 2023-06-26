﻿namespace PSE.Model.Output.Interfaces
{

    public interface IKeyInformation
    {

        string ClientName { get; set; }

        string ClientNumber { get; set; }

        string Portfolio { get; set; }

        string Service { get; set; }

        string RiskProfile { get; set; }

        string PercentWeightedPerformance { get; set; }

    }

    public interface IAssetType
    {

        string Type { get; set; }

        string MarketValueReportingCurrency { get; set; }

    }

    public interface IAssetExtract
    {

        string AssetClass { get; set; }

        string MarketValueReportingCurrency { get; set; }

        IList<IAssetType> AssetsType { get; set; }

    }
    public interface ISection3Content
    {

        IList<IKeyInformation> KeysInformation { get; set; }

        IList<IAssetExtract> AssetsExtract { get; set; }

    }

}
