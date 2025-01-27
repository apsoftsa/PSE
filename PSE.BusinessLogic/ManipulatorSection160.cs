using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection160 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection160(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection160, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section160 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };            
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {                
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                IShareEconomicSector econSector;                
                ISection160Content sectionContent;
                string sectorDescr;
                decimal totalSum, totalPerc, currPerc;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), Model.Common.Constants.ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();                
                foreach (IDE ideItem in ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    sectionContent = new Section160Content();
                    sectionContent.SubSection16000 = new ShareEconomicSectorSubSection("Shares by economic sector"); 
                    totalSum = 0;
                    IEnumerable<IGrouping<string, POS>> groupByEconomicalSector = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && flt.SubCat4_15 == ((int)PositionClassifications.AZIONI_FONDI_AZIONARI).ToString()).GroupBy(gb => gb.SubCat1_12).OrderBy(ob => ob.Key);
                    foreach (IGrouping<string, POS> economicalSector in groupByEconomicalSector)
                    {
                        sectorDescr = "(Unknown)";
                        extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section160), nameof(ShareEconomicSector.Sector), economicalSector.Key, propertyParams);
                        OnExternalCodifyRequest(extEventArgsAdvisor);
                        if (!extEventArgsAdvisor.Cancel)
                            sectorDescr = extEventArgsAdvisor.PropertyValue;
                        econSector = new ShareEconomicSector()
                        {
                            MarketValueReportingCurrency = Math.Round(economicalSector.Sum(sum => sum.Amount1Base_23).Value, 2),
                            Sector = string.IsNullOrEmpty(sectorDescr) ? "NON CLASSIFICABILI" : sectorDescr,
                            PercentShares = 0
                        };
                        totalSum += Math.Abs(econSector.MarketValueReportingCurrency.Value);
                        sectionContent.SubSection16000.Content.Add(econSector);                        
                    }
                    totalPerc = currPerc = 0;
                    foreach(var sector in sectionContent.SubSection16000.Content)
                    {
                        if (totalSum > 0)
                        {
                            currPerc = Math.Round(sector.MarketValueReportingCurrency.Value * 100.0m / totalSum, 2);
                            totalPerc += currPerc;
                            sector.PercentShares = currPerc;
                        }
                    }
                    sectionContent.SubSection16010 = new ShareEconomicSectorChartSubSection("Shares subdivision by economic sector chart");
                    if (totalPerc > 100.0m)
                    {
                        currPerc = currPerc - (totalPerc - 100.0m);
                        sectionContent.SubSection16000.Content.Last().PercentShares = currPerc;
                    }
                    else if (totalPerc < 100.0m)
                    {
                        currPerc = currPerc + (100.0m - totalPerc);
                        sectionContent.SubSection16000.Content.Last().PercentShares = currPerc;
                    }                    
                    foreach (var sector in sectionContent.SubSection16000.Content)
                    {
                        sectionContent.SubSection16010.Content.Add(new ShareEconomicSectorChart() { Sector = sector.Sector, PercentShares = sector.PercentShares });
                    }
                    output.Content = new Section160Content(sectionContent);
                }
            }    
            return output;
        }

    }

}
