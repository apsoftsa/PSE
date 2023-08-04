﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSE.Model.Output.Interfaces
{

    public interface IObligationsBase
    {

        string? Description { get; set; }

        int? ValorNumber { get; set; }

        string? Isin { get; set; }

        decimal? NominalAmount { get; set; }

        string? Currency { get; set; }

        string? SPRating { get; set; }

        string? MsciEsg { get; set; }

        decimal? PurchasePrice { get; set; }

        decimal? PriceBeginningYear { get; set; }

        decimal? CurrentPrice { get; set; }

        decimal? ExchangeRateImpactPurchase { get; set; }

        decimal? ExchangeRateImpactYTD { get; set; }

        decimal? PerformancePurchase { get; set; }

        decimal? PercentPerformancePurchase { get; set; }

        decimal? PerformanceYTD { get; set; }

        decimal? PercentPerformanceYTD { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface IFundBase : IObligationsBase { }

    public interface IBondsBase : IObligationsBase
    {

        int? PercentCoupon { get; set; }

        decimal? PercentYTM { get; set; }

        string? Expiration { get; set; }

    }

}