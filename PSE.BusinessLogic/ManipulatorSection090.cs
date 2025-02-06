using System.Globalization;
using PSE.BusinessLogic.Calculations;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection090 : ManipulatorBase, IManipulator
    {

        private readonly SharesCalculation _calcShares;

        public ManipulatorSection090(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>() { PositionClassifications.SHARES }, ManipolationTypes.AsSection090, culture) 
        {
            _calcShares = new SharesCalculation(calcSettings);
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section090 output = new()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)) && extractedData.Any(flt => flt.RecordType == nameof(POS)))
            {
                ISection090Content sectionContent;
                List<IDE> ideItems = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                foreach (IDE ideItem in ideItems)
                {
                    sectionContent = new Section090Content();
                    IEnumerable<IGrouping<string, POS>> posItemsGroupBySubCat = extractedData.Where(flt => flt.AlreadyUsed == false && flt.RecordType == nameof(POS)).OfType<POS>().Where(fltSubCat => fltSubCat.CustomerNumber_2 == ideItem.CustomerNumber_2 && ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, fltSubCat.SubCat4_15)).GroupBy(gb => gb.SubCat4_15).OrderBy(ob => ob.Key);
                    if (posItemsGroupBySubCat != null && posItemsGroupBySubCat.Any())
                    {
                        foreach (IGrouping<string, POS> subCategoryItems in posItemsGroupBySubCat)
                        {
                            switch ((PositionClassifications)int.Parse(subCategoryItems.Key))
                            {
                                case PositionClassifications.SHARES:
                                    {
                                        IShareDetail shareDetail;
                                        ISummaryTo summaryTo;
                                        ISummaryBeginningYear summaryBeginningYear;
                                        ISummaryPurchase summaryPurchase;
                                        sectionContent.Subsection9010 = new ShareSubSection("Shares");
                                        foreach (POS posItem in subCategoryItems)
                                        {
                                            summaryTo = new SummaryTo()
                                            {
                                                ValuePrice = posItem.Quote_48,
                                                PercentPrice = 0m,
                                                ProfitLossNotRealizedValue = 0m
                                            };
                                            summaryBeginningYear = new SummaryBeginningYear()
                                            {
                                                ValuePrice = posItem.BuyPriceAverage_87,
                                                ExchangeValue = posItem.BuyExchangeRateAverage_88
                                            };
                                            summaryPurchase = new SummaryPurchase()
                                            {
                                                ValuePrice = posItem.BuyPriceHistoric_53,
                                                ExchangeValue = posItem.BuyExchangeRateHistoric_66
                                            };
                                            shareDetail = new ShareDetail()
                                            {
                                                Currency = AssignRequiredString(posItem.Currency1_17),
                                                Description1 = string.Concat(AssignRequiredString(posItem.Description1_32), " ", AssignRequiredString(posItem.Description2_33)),
                                                Description2 = AssignRequiredString(posItem.IsinIban_85),
                                                Amount = AssignRequiredDecimal(posItem.Quantity_28),
                                                CapitalMarketValueReportingCurrency = AssignRequiredDecimal(posItem.Amount1Base_23),
                                                PercentWeight = 0 // ??                                                 
                                            };
                                            CalculateSummaries(summaryTo, summaryBeginningYear, summaryPurchase, posItem.Quantity_28);
                                            shareDetail.SummaryTo.Add(summaryTo);
                                            shareDetail.SummaryBeginningYear.Add(summaryBeginningYear);
                                            shareDetail.SummaryPurchase.Add(summaryPurchase);
                                            sectionContent.Subsection9010.Content.Add(shareDetail);
                                            posItem.AlreadyUsed = true;
                                        }                                        
                                        break;
                                    }
                            }
                        }
                    }
                    output.Content = new Section090Content(sectionContent);                    
                }
            }
            return output;
        }

    }

}
