using System.Globalization;
using PSE.Model.Common;
using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection3 : ManipulatorBase
    {

        public ManipulatorSection3(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsSection3, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = Utility.ManipulatorOperatingRules.GetDestinationSection(this);
            Section3 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(PER)))
            {
                bool _hasValue;
                IKeyInformation _currKeyInf;
                IAssetExtract _currAsstExtr;
                ExternalCodifyRequestEventArgs _extEventArgsPortfolio;
                ExternalCodifyRequestEventArgs _extEventArgsService;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                List<PER> _perItems = extractedData.Where(_flt => _flt.RecordType == nameof(PER)).OfType<PER>().ToList();
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_perItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        // it is necessary to take only the PER item having the property Type_5 value smallest (!!!!)
                        PER _perItem = _perItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2).OrderBy(_ob => _ob.Type_5).First();
                        _extEventArgsPortfolio = new ExternalCodifyRequestEventArgs(nameof(Section3), nameof(KeyInformation.Portfolio), string.IsNullOrEmpty(_ideItem.ModelCode_21) ? "" : _ideItem.ModelCode_21[..1]);
                        OnExternalCodifyRequest(_extEventArgsPortfolio);
                        if (!_extEventArgsPortfolio.Cancel)
                        {
                            _extEventArgsService = new ExternalCodifyRequestEventArgs(nameof(Section3), nameof(KeyInformation.Service), _ideItem.Mandate_11);
                            OnExternalCodifyRequest(_extEventArgsService);
                            if (!_extEventArgsService.Cancel)
                            {
                                _currKeyInf = new KeyInformation()
                                {
                                    CustomerName = _ideItem.CustomerNameShort_5,
                                    CustomerNumber = _ideItem.CustomerNumber_2,
                                    Portfolio = _extEventArgsPortfolio.PropertyValue,
                                    Service = _extEventArgsService.PropertyValue,
                                    RiskProfile = "[RiskProfile]", // not still recovered (!)
                                    PercentWeightedPerformance = _perItem.TWR_14 != null ? _perItem.TWR_14.Value : null,
                                };
                                _output.Content.KeysInformation.Add(new KeyInformation(_currKeyInf));
                                if (_perItem.StartValue_8 != null && _perItem.StartDate_6 != null)
                                {
                                    decimal _startValue, _cashIn, _cashOut, _secIn, _secOut, _portfolioValueRectified;
                                    _cashIn = _cashOut = _secIn = _secOut = 0;
                                    _startValue = _perItem.StartValue_8.Value;
                                    _currAsstExtr = new AssetExtract()
                                    {
                                        AssetClass = "Portfolio Value " + ((DateTime)_perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture),
                                        MarketValueReportingCurrencyT = _startValue,
                                        AssetType = "+ Contributions"
                                    };
                                    _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                                    _hasValue = false;
                                    if (_perItem.CashOut_11 != null)
                                    {
                                        _cashOut = _perItem.CashOut_11.Value;
                                        _hasValue = true;
                                    }
                                    if (_perItem.SecOut_13 != null)
                                    {
                                        _secOut = _perItem.SecOut_13.Value;
                                        _hasValue = true;
                                    }
                                    if (_hasValue)
                                    {
                                        _currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Portfolio Value " + ((DateTime)_perItem.StartDate_6).ToString(DEFAULT_DATE_FORMAT, _culture),
                                            MarketValueReportingCurrencyT = _startValue,
                                            AssetType = "- Withdrawals",
                                            MarketValueReportingCurrency = _cashOut + _secOut
                                        };
                                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                                        if (_perItem.CashIn_10 != null)
                                            _cashIn = _perItem.CashIn_10.Value;
                                        if (_perItem.SecIn_12 != null)
                                            _secIn = _perItem.SecIn_12.Value;
                                        _portfolioValueRectified = _startValue + (_cashIn + _secIn + _cashOut + _secOut);
                                        _currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Portfolio Value Rectified",
                                            MarketValueReportingCurrencyT = _portfolioValueRectified
                                        };
                                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                                        if (_perItem.EndValue_9 != null && _perItem.EndDate_7 != null)
                                        {
                                            _currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Portfolio Value " + ((DateTime)_perItem.EndDate_7).ToString(DEFAULT_DATE_FORMAT, _culture),
                                                MarketValueReportingCurrencyT = _perItem.EndValue_9.Value
                                            };
                                            _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                                            _currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Plus/less value",
                                                MarketValueReportingCurrencyT = _perItem.EndValue_9.Value - _portfolioValueRectified
                                            };
                                            _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));

                                        }
                                    }
                                }
                                if (_perItem.Interest_15 != null)
                                {
                                    decimal _interest, _realEquity, _realCurr, _nonRealEquity, _nonRealCurrency;
                                    _realEquity = _realCurr = _nonRealEquity = _nonRealCurrency = 0;
                                    _interest = _perItem.Interest_15.Value;
                                    _currAsstExtr = new AssetExtract()
                                    {
                                        AssetClass = "Dividend and Interest",
                                        MarketValueReportingCurrencyT = _interest,
                                    };
                                    _output.Content.DividendsInterests.Add(new AssetExtract(_currAsstExtr));
                                    _hasValue = false;
                                    if (_perItem.PlRealEquity_16 != null)
                                    {
                                        _realEquity = _perItem.PlRealEquity_16.Value;
                                        _hasValue = true;
                                    }
                                    if (_perItem.PlRealCurrency_17 != null)
                                    {
                                        _realCurr = _perItem.PlRealCurrency_17.Value;
                                        _hasValue = true;
                                    }
                                    if (_hasValue)
                                    {
                                        _currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Realized gains/losses",
                                            MarketValueReportingCurrencyT = _realEquity + _realCurr,
                                            AssetType = "on which ongoing",
                                            MarketValueReportingCurrency = _realEquity
                                        };
                                        _output.Content.DividendsInterests.Add(new AssetExtract(_currAsstExtr));
                                        _currAsstExtr = new AssetExtract()
                                        {
                                            AssetClass = "Realized gains/losses",
                                            MarketValueReportingCurrencyT = _realEquity + _realCurr,
                                            AssetType = "on which on currency",
                                            MarketValueReportingCurrency = _realCurr
                                        };
                                        _output.Content.DividendsInterests.Add(new AssetExtract(_currAsstExtr));
                                        _hasValue = false;
                                        if (_perItem.PlNonRealEquity_18 != null)
                                        {
                                            _nonRealEquity = _perItem.PlNonRealEquity_18.Value;
                                            _hasValue = true;
                                        }
                                        if (_perItem.PlNonRealCurrency_19 != null)
                                        {
                                            _nonRealCurrency = _perItem.PlNonRealCurrency_19.Value;
                                            _hasValue = true;
                                        }
                                        if (_hasValue)
                                        {
                                            _currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Not realized gains/losses",
                                                MarketValueReportingCurrencyT = _nonRealEquity + _nonRealCurrency,
                                                AssetType = "on which ongoing",
                                                MarketValueReportingCurrency = _nonRealEquity
                                            };
                                            _output.Content.DividendsInterests.Add(new AssetExtract(_currAsstExtr));
                                            _currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Not realized gains/losses",
                                                MarketValueReportingCurrencyT = _nonRealEquity + _nonRealCurrency,
                                                AssetType = "on which on currency",
                                                MarketValueReportingCurrency = _nonRealCurrency
                                            };
                                            _output.Content.DividendsInterests.Add(new AssetExtract(_currAsstExtr));
                                            _currAsstExtr = new AssetExtract()
                                            {
                                                AssetClass = "Plus/less value",
                                                MarketValueReportingCurrencyT = _interest + _realEquity + _realCurr +
                                                                                _nonRealEquity + _nonRealCurrency
                                            };
                                            _output.Content.DividendsInterests.Add(new AssetExtract(_currAsstExtr));
                                        }
                                    }
                                }
                                _output.Content.FooterInformation.Add(new FooterInformation() { Footer1 = "[footer1]", Footer2 = "[footer2]" }); // not still recovered (!)
                            }
                        }
                    }
                }
            }
            return _output;
        }

    }

}
