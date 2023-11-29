using System.Globalization;
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

    public class ManipulatorSection7 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection7(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection7, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section7 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {                
                ISection7Content _sectionContent;
                IInvestment _investment;
                IEnumerable<CUR> _curItems;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();                
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE _ideItem in _ideItems)
                {
                    _sectionContent = new Section7Content();
                    _curItems = extractedData.Where(_flt => _flt.RecordType == nameof(CUR)).OfType<CUR>().Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2);
                    IEnumerable<IGrouping<string, POS>> _groupByCurrency = _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2).GroupBy(_gb => _gb.Currency1_17).OrderBy(_ob => _ob.Key);
                    if (_groupByCurrency != null && _groupByCurrency.Any())
                    {
                        foreach (IGrouping<string, POS> _currency in _groupByCurrency)
                        {                                                        
                            _investment = new Investment()
                            {
                                Amount = Math.Round(_currency.Sum(_sum => _sum.Amount1Cur1_22).Value, 2),
                                Currency = _currency.Key,
                                MarketValueReportingCurrency = Math.Round(_currency.Sum(_sum => _sum.Amount1Base_23).Value, 2),
                                Exchange = (_curItems != null && _curItems.Any(_flt => _flt.Currency_5 == _currency.Key && _flt.Rate_6 != null)) ? Math.Round(_curItems.First(_flt => _flt.Currency_5 == _currency.Key && _flt.Rate_6 != null).Rate_6.Value, 3) : 0
                            };
                            _sectionContent.Investments.Add(_investment);
                        }                        
                        decimal? _totalAmount = _sectionContent.Investments.Sum(_sum => _sum.Amount);
                        if (_totalAmount != null && _totalAmount != 0)
                        {
                            foreach (IInvestment _invToUpgrd in _sectionContent.Investments)
                            {
                                _invToUpgrd.PercentAsset = Math.Round((_invToUpgrd.Amount / _totalAmount.Value * 100m).Value, 2);
                                _sectionContent.ChartInvestments.Add(new ChartInvestment() { Currency = _invToUpgrd.Currency, PercentAsset = _invToUpgrd.PercentAsset });
                            }
                            _investment = new Investment()
                            {
                                Amount = null,
                                Currency = "Total",
                                MarketValueReportingCurrency = Math.Round(_totalAmount.Value, 2),
                                PercentAsset = 100m,
                                Exchange = null
                            };
                            _sectionContent.Investments.Add(_investment);
                        }                        
                    }
                    _output.Content = new Section7Content(_sectionContent);
                }
            }
            return _output;
        }

    }

}
