using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection11 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection11(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section11 _output = new()
            {
                SectionCode = OUTPUT_SECTION11_CODE,
                SectionName = "Forward Exchange Transactions (PROFIT/LOSS)"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _tmp1, _tmp2;
                IProfitLossOperation _profitLossOperation;
                ISection11Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION11);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section11Content();
                        foreach (POS _posItem in _posItems)
                        {
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
                                PercentAssets = 0 // not still recovered (!)
                            };
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
