using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection11 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection11(CultureInfo? culture = null) : base(PositionClassifications.OPERAZIONI_CAMBI_A_TERMINE, ManipolationTypes.AsSection11, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section11 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _tmp1, _tmp2, _customerSumAmounts, _currentBaseValue;
                IProfitLossOperation _profitLossOperation;
                ISection11Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                foreach (IDE _ideItem in _ideItems)
                {
                    _customerSumAmounts = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.Amount1Base_23.HasValue).Sum(_sum => _sum.Amount1Base_23.Value);
                    _customerSumAmounts += extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.ProRataBase_56.HasValue).Sum(_sum => _sum.ProRataBase_56.Value);
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section11Content();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            _currentBaseValue = _posItem.Amount1Base_23.HasValue ? _posItem.Amount1Base_23.Value : 0;
                            _currentBaseValue += _posItem.ProRataBase_56.HasValue ? _posItem.ProRataBase_56.Value : 0;
                            _tmp1 = _posItem.Amount1Base_23 != null ? _posItem.Amount1Base_23.Value : 0;
                            _tmp2 = _posItem.Amount2Base_60 != null ? _posItem.Amount2Base_60.Value : 0;
                            _profitLossOperation = new ProfitLossOperation()
                            {
                                CurrencyLoss = _posItem.Currency1_17,
                                Currency2 = _posItem.Currency2_18,
                                AmountLoss = _posItem.Amount1Cur1_22 != null ? _posItem.Amount1Cur1_22.Value : 0,
                                ExpirationDate = _posItem.MaturityDate_36 != null ? ((DateTime)_posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                CurrentExchangeRate = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                Change = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                Amount2 = _posItem.Amount2Cur2_59 != null ? _posItem.Amount2Cur2_59.Value : 0,
                                ProfitlossReportingCurrency = _tmp1 + _tmp2,
                                PercentAssets = _customerSumAmounts != 0 && _currentBaseValue != 0 ? Math.Round(_currentBaseValue / _customerSumAmounts * 100m, DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            _posItem.AlreadyUsed = true;
                            _sectionContent.Operations.Add(_profitLossOperation);
                        }
                        _output.Content = new Section11Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
