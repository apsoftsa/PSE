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

    public class ManipulatorSection6 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection6(CultureInfo? culture = null) : base(PositionClassifications.UNKNOWN, ManipolationTypes.AsSection6, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section6 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                ExternalCodifyRequestEventArgs _extEventArgsAdvisor;
                IAsset _asset;                
                ISection6Content _sectionContent;
                Dictionary<string, object> _propertyParams = new Dictionary<string, object>() { { nameof(IDE.Language_18), Model.Common.Constants.ITALIAN_LANGUAGE_CODE } };
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>();
                foreach (IDE _ideItem in _ideItems)
                {
                    if (ManipulatorOperatingRules.CheckInputLanguage(_ideItem.Language_18))
                        _propertyParams[nameof(IDE.Language_18)] = _ideItem.Language_18;
                    _sectionContent = new Section6Content();
                    IEnumerable<IGrouping<string, POS>> _groupByCategory = _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2).GroupBy(_gb => _gb.SubCat3_14).OrderBy(_ob => _ob.Key);
                    IEnumerable<IGrouping<string, POS>> _groupBySubCategory = _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2).GroupBy(_gb => _gb.SubCat4_15).OrderBy(_ob => _ob.Key);
                    if (_groupBySubCategory != null && _groupBySubCategory.Any())
                    {
                        string? _categoryDescr = string.Empty;
                        string _prevCategory = string.Empty;
                        string _currCategory = string.Empty;
                        foreach (IGrouping<string, POS> _subCategory in _groupBySubCategory)
                        {                            
                            _currCategory = _subCategory.First().SubCat3_14;
                            if (_currCategory != _prevCategory)
                            {
                                _categoryDescr = "(Unknown)";
                                _extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section6), nameof(Asset.AssetClass), _currCategory, _propertyParams);
                                OnExternalCodifyRequest(_extEventArgsAdvisor);
                                if (!_extEventArgsAdvisor.Cancel)
                                    _categoryDescr = _extEventArgsAdvisor.PropertyValue;
                                _prevCategory = _currCategory;
                            }
                            _extEventArgsAdvisor = new ExternalCodifyRequestEventArgs(nameof(Section6), nameof(Asset.TypeInvestment), _subCategory.Key, _propertyParams);
                            OnExternalCodifyRequest(_extEventArgsAdvisor);
                            if (!_extEventArgsAdvisor.Cancel)
                            {
                                _asset = new Asset()
                                {
                                    MarketValueReportingCurrency = Math.Round(_subCategory.Sum(_sum => _sum.Amount1Cur1_22).Value, 2),
                                    AssetClass = _categoryDescr,
                                    TypeInvestment = _extEventArgsAdvisor.PropertyValue,
                                    MarketValueReportingCurrencyT = Math.Round(_groupByCategory.First(_flt => _flt.Key == _subCategory.First().SubCat3_14).Sum(_sum => _sum.Amount1Cur1_22).Value, 2)
                                };
                                _sectionContent.Assets.Add(_asset);
                            }                            
                        }                        
                        decimal? _totalSum = _sectionContent.Assets.Sum(_sum => _sum.MarketValueReportingCurrency);
                        if (_totalSum != 0)
                        {
                            foreach (IAsset _assetToUpgrd in _sectionContent.Assets)
                            {
                                _assetToUpgrd.PercentInvestment = Math.Round((_assetToUpgrd.MarketValueReportingCurrency / _totalSum.Value * 100m).Value, 2);
                            }
                            IEnumerable<IGrouping<string, IAsset?>> _groupByAssetClasses = _sectionContent.Assets.GroupBy(_gb => _gb.AssetClass);
                            foreach (IGrouping<string, IAsset> _groupByAssetClass in _groupByAssetClasses)
                            {
                                foreach(IAsset _assetToUpgrd in _sectionContent.Assets.Where(_flt => _flt.AssetClass == _groupByAssetClass.Key))
                                {
                                    _assetToUpgrd.PercentInvestmentT = _groupByAssetClass.Sum(_sum => _sum.PercentInvestment);
                                }
                                _sectionContent.ChartAssets.Add(new ChartAsset() { AssetClass = _groupByAssetClass.Key, PercentInvestment = _groupByAssetClass.Sum(_sum => _sum.PercentInvestment) });
                            }
                        }
                    }
                    _output.Content = new Section6Content(_sectionContent);
                }
            }
            return _output;
        }

    }

}
