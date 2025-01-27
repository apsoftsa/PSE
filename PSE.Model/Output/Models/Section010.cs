﻿using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class KeyInformation : IKeyInformation
    {

        public string Customer { get; set; }

        public string CustomerID { get; set; }

        public string Portfolio { get; set; }

        public string RiskProfile { get; set; }

        public string EsgProfile { get; set; }       

        public KeyInformation() 
        { 
            Customer = string.Empty;
            CustomerID = string.Empty;
            Portfolio = string.Empty;
            EsgProfile = string.Empty;
            RiskProfile = string.Empty;
        }

        public KeyInformation(IKeyInformation source)
        {
            CustomerID = source.CustomerID;
            Customer = source.Customer;
            Portfolio = source.Portfolio;
            EsgProfile = source.EsgProfile;
            RiskProfile = source.RiskProfile;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AssetExtract : IAssetExtract
    {

        public string Entry { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(propertyName: "type", NullValueHandling = NullValueHandling.Ignore)]
        public string? AssetType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrencyContr { get; set; }


        public AssetExtract()
        {
            Entry = string.Empty;  
            AssetType = null;   
            MarketValueReportingCurrency = 0;
            MarketValueReportingCurrencyContr = null;
        }

        public AssetExtract(IAssetExtract source)
        {
            Entry = source.Entry;
            AssetType = source.AssetType;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            MarketValueReportingCurrencyContr = source.MarketValueReportingCurrencyContr;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DividendInterest : IDividendInterest
    {

        public string Entry { get; set; }

        public decimal MarketValueReportingCurrencyT { get; set; }

        [JsonProperty(propertyName: "type", NullValueHandling = NullValueHandling.Ignore)]
        public string? AssetType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }


        public DividendInterest()
        {
            Entry = string.Empty;
            AssetType = null;
            MarketValueReportingCurrencyT = 0;
            MarketValueReportingCurrency = null;
        }

        public DividendInterest(IDividendInterest source)
        {
            Entry = source.Entry;
            AssetType = source.AssetType;
            MarketValueReportingCurrencyT = source.MarketValueReportingCurrencyT;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
        }

    }

    public class FooterInformation : IFooterInformation
    {

        public string Note { get; set; }

        public FooterInformation()
        {
            Note = string.Empty;
        }

        public FooterInformation(IFooterInformation source)
        {
            Note = source.Note;
        }

    }

    public class SubSection1000Content : ISubSection1000Content
    {

        public string Name { get; set; }

        public IList<IKeyInformation> Content { get; set; }

        public SubSection1000Content()
        {
            Name = "Key Information";
            Content = new List<IKeyInformation>(); 
        }

        public SubSection1000Content(ISubSection1000Content source)
        {
            Name = source.Name;
            Content = new List<IKeyInformation>();
            if (source.Content != null && source.Content.Any())
            {
                foreach(var item in source.Content)
                    Content.Add(new KeyInformation(item));
            }
        }

    }

    public class SubSection1010Content : ISubSection1010Content
    {

        public string Name { get; set; }

        public IList<IAssetExtract> Content { get; set; }

        public SubSection1010Content()
        {
            Name = "";
            Content = new List<IAssetExtract>();
        }

        public SubSection1010Content(ISubSection1010Content source)
        {
            Name = source.Name;
            Content = new List<IAssetExtract>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new AssetExtract(item));
            }
        }

    }

    public class SubSection1011Content : ISubSection1011Content
    {

        public string Name { get; set; }

        public IList<IDividendInterest> Content { get; set; }

        public IList<IFooterInformation> Notes { get; set; }

        public SubSection1011Content()
        {
            Name = "";
            Content = new List<IDividendInterest>();
            Notes = new List<IFooterInformation>();
        }

        public SubSection1011Content(ISubSection1011Content source)
        {
            Name = source.Name;
            Content = new List<IDividendInterest>();
            Notes = new List<IFooterInformation>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new DividendInterest(item));
            }
            if (source.Notes != null && source.Notes.Any())
            {
                foreach (var item in source.Notes)
                    Notes.Add(new FooterInformation(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section010Content : ISection010Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection1000Content? SubSection1000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection1010Content? SubSection1010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection1011Content? SubSection1011 { get; set; }

        public Section010Content()
        {
            SubSection1000 = null;   
            SubSection1010 = null;
            SubSection1011 = null;
        }

        public Section010Content(ISection010Content source)
        {
            SubSection1000 = (source.SubSection1000 != null) ? new SubSection1000Content(source.SubSection1000) : null;
            SubSection1010 = (source.SubSection1010 != null) ? new SubSection1010Content(source.SubSection1010) : null;
            SubSection1011 = (source.SubSection1011 != null) ? new SubSection1011Content(source.SubSection1011) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section010 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection010Content Content { get; set; }

        public Section010() : base()
        { 
            Content = new Section010Content();
        }

        public Section010(Section010 source) : base(source) 
        {
            if (source.Content != null)
                Content = new Section010Content(source.Content);
            else
                Content = new Section010Content();
        }

    }

}
