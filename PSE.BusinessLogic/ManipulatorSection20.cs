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

    public class ManipulatorSection20 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection20(CultureInfo? culture = null) : base(PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO, ManipolationTypes.AsSection20, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section20 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _customerSumAmounts, _currentBaseValue;
                IMetalPhysicalMetalAccount _metPhyMetAcc;
                ISection20Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                foreach (IDE _ideItem in _ideItems)
                {
                    _customerSumAmounts = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.Amount1Base_23.HasValue).Sum(_sum => _sum.Amount1Base_23.Value);
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section20Content();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            _currentBaseValue = _posItem.Amount1Base_23.HasValue ? _posItem.Amount1Base_23.Value : 0;
                            _currentBaseValue += _posItem.ProRataBase_56.HasValue ? _posItem.ProRataBase_56.Value : 0;
                            _metPhyMetAcc = new MetalPhysicalMetalAccount()
                            {
                                Account = _posItem.HostPositionReference_6,
                                CurrentBalance = _posItem.Amount1Cur1_22 != null ? _posItem.Amount1Cur1_22.Value : 0,
                                MarketValueReportingCurrency = _posItem.Amount1Base_23 != null ? _posItem.Amount1Base_23.Value : 0,
                                Amount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                PurchasingCourse = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                CostPrice = 0, // not still recovered (!)
                                PercentDifference = 0, // not still recovered (!)
                                PercentAsset = _customerSumAmounts != 0 && _currentBaseValue != 0 ? Math.Round(_currentBaseValue / _customerSumAmounts * 100m, Model.Common.Constants.DEFAULT_MEANINGFUL_DECIMAL_DIGITS_FOR_CALCULATION) : 0
                            };
                            _sectionContent.MetalPhysicalMetalAccounts.Add(_metPhyMetAcc);
                            _posItem.AlreadyUsed = true;    
                        }
                        _output.Content = new Section20Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
