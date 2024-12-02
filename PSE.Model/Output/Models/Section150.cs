using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PossibleCommitment : IPossibleCommitment
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OpeningDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int AccruedInterestReportingCurrency { get; set; }

        public PossibleCommitment()
        {
            Description1 = string.Empty;    
            Description2 = string.Empty;
            OpeningDate = string.Empty;
            ExpirationDate = string.Empty;
            CurrentBalance = 0;
            MarketValueReportingCurrency = 0;
            AccruedInterestReportingCurrency = 0;
        }

        public PossibleCommitment(IPossibleCommitment source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            OpeningDate = source.OpeningDate;
            ExpirationDate = source.ExpirationDate;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            AccruedInterestReportingCurrency = source.AccruedInterestReportingCurrency;
        }

    }
   
    public class PossibleCommitmentSubSection : IPossibleCommitmentSubSection
    {

        public string Name { get; set; }

        public IList<IPossibleCommitment> Content { get; set; }


        public PossibleCommitmentSubSection(string name)
        {
            Name = name;
            Content = new List<IPossibleCommitment>();
        }

        public PossibleCommitmentSubSection(IPossibleCommitmentSubSection source)
        {
            Name = source.Name; 
            Content = new List<IPossibleCommitment>();
            if (source != null)
            {
                if (source.Content != null && source.Content.Any())
                {
                    foreach (IPossibleCommitment possComm in source.Content)
                    {
                        Content.Add(new PossibleCommitment(possComm));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section150Content : ISection150Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IPossibleCommitmentSubSection SubSection15000 { get; set; }

        public Section150Content()
        {
            SubSection15000 = new PossibleCommitmentSubSection("Possible commitments");
        }

        public Section150Content(ISection150Content source)
        {
            SubSection15000 = new PossibleCommitmentSubSection(source.SubSection15000);
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section150 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection150Content Content { get; set; }

        public Section150() : base()
        {
            Content = new Section150Content();
        }

        public Section150(Section150 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section150Content(source.Content);
            else
                Content = new Section150Content();
        }

    }

}
