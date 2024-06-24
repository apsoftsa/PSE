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

    public class ManipulatorSection23 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection23(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection23, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section23 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };            
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {                
                ExternalCodifyRequestEventArgs extEventArgsAdvisor;
                IEconSector econSector;                
                ISection23Content sectionContent;
                string sectorDescr;
                decimal totalSum, totalPerc, currPerc;
                Dictionary<string, object> propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), Model.Common.Constants.ITALIAN_LANGUAGE_CODE } };
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> posItems = extractedData.Where(flt => flt.RecordType == nameof(POS)).OfType<POS>();                
                foreach (IDE ideItem in ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(ideItem.Language_18))
                        propertyParams[nameof(IDE.Language_18)] = ideItem.Language_18;
                    sectionContent = new Section23Content();
                    sectionContent.ActionByEconSector.Add(new ActionByEconSector());
                    totalSum = 0;
                    IEnumerable<IGrouping<string, POS>> groupByEconomicalSector = posItems.Where(flt => flt.CustomerNumber_2 == ideItem.CustomerNumber_2 && string.IsNullOrEmpty(flt.SubCat1_12) == false).GroupBy(gb => gb.SubCat1_12).OrderBy(ob => ob.Key);
                    foreach (IGrouping<string, POS> economicalSector in groupByEconomicalSector)
                    {
                        sectorDescr = "(Unknown)";
                        extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section23), nameof(EconSector.Sector), economicalSector.Key, propertyParams);
                        OnExternalCodifyRequest(extEventArgsAdvisor);
                        if (!extEventArgsAdvisor.Cancel)
                            sectorDescr = extEventArgsAdvisor.PropertyValue;
                        econSector = new EconSector()
                        {
                            MarketValueReportingCurrency = Math.Round(economicalSector.Sum(sum => sum.Amount1Base_23).Value, 2),
                            Sector = sectorDescr,
                            PercentShares = 0
                        };
                        totalSum += Math.Abs(econSector.MarketValueReportingCurrency!.Value);
                        sectionContent.ActionByEconSector.First().Sectors.Add(econSector);                        
                    }
                    totalPerc = currPerc = 0;
                    foreach(var sector in sectionContent.ActionByEconSector.First().Sectors)
                    {
                        if (totalSum > 0)
                        {
                            currPerc = sector.MarketValueReportingCurrency.HasValue ? Math.Round(sector.MarketValueReportingCurrency.Value * 100.0m / totalSum, 2) : 0;
                            totalPerc += currPerc;
                            sector.PercentShares = currPerc;
                        }
                    }
                    if (totalPerc > 100.0m)
                    {
                        currPerc = currPerc - (totalPerc - 100.0m);
                        sectionContent.ActionByEconSector.First().Sectors.Last().PercentShares = currPerc;
                    }
                    else if (totalPerc < 100.0m)
                    {
                        currPerc = currPerc + (100.0m - totalPerc);
                        sectionContent.ActionByEconSector.First().Sectors.Last().PercentShares = currPerc;
                    }
                    sectionContent.ActionByEconSector.First().TotalPercentShares = 100;
                    sectionContent.ActionByEconSector.First().TotalMarketValueReportingCurrency = totalSum;                    
                    foreach (var sector in sectionContent.ActionByEconSector.First().Sectors)
                    {
                        sectionContent.ChartGraphicEconomicalSector.Add(new EconominalSector() { Sector = sector.Sector, PercentShares = sector.PercentShares });
                    }
                    output.Content = new Section23Content(sectionContent);
                }
            }    
            return output;
        }

    }

}
