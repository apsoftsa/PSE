using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection3 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection3(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section3 _output = new Section3()
            {
                SectionCode = OUTPUT_SECTION3_CODE,
                SectionName = "Portfolio Details"
            };
            if(extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(PER))) 
            {
                decimal _tmpValue1, _tmpValue2, _tot1, _tot2;
                bool _hasValue, _hasValue1, _hasValue2;
                IKeyInformation _currKeyInf;
                IAssetExtract _currAsstExtr;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                List<PER> _perItems = extractedData.Where(_flt => _flt.RecordType == nameof(PER)).OfType<PER>().ToList();
                foreach (IDE _ideItem in _ideItems) 
                {
                    if (_perItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        // it is necessary to take only the PER item having the property Type_5 value smallest
                        PER _perItem = _perItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2).OrderBy(_ob => _ob.Type_5).First();
                        _currKeyInf = new KeyInformation()
                        {
                            ClientName = _ideItem.CustomerNameShort_5,
                            ClientNumber = _ideItem.CustomerNumber_2,
                            Portfolio = _ideItem.ModelCode_21,
                            Service = _ideItem.Mandate_11,
                            RiskProfile = "[RiskProfile]",
                            PercentWeightedPerformance = _perItem.TWR_14
                        };
                        _output.Content.KeysInformation.Add(new KeyInformation(_currKeyInf));
                        if (_perItem.StartValue_8 != null || _perItem.StartDate_6 != null)
                        {
                            _currAsstExtr = new AssetExtract()
                            {
                                AssetClass = _perItem.StartDate_6 != null ? "Portfolio Value " + ((DateTime)_perItem.StartDate_6).ToString("dd/MM/yyyy", _culture) : string.Empty,
                                MarketValueReportingCurrency = _perItem.StartValue_8
                            };
                            _currAsstExtr.AssetsType.Add(new AssetType()
                            {
                                Type = "contributions",
                                MarketValueReportingCurrency = _perItem.CashIn_10
                            });
                            _currAsstExtr.AssetsType.Add(new AssetType()
                            {
                                Type = "withdrawals",
                                MarketValueReportingCurrency = _perItem.CashOut_11
                            });
                            _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                        }
                        _hasValue = false;
                        _tot1 = _tot2 = _tmpValue1 = _tmpValue2 = 0;
                        if (_perItem.StartValue_8 != null)
                        {
                            _tmpValue1 = (decimal)_perItem.StartValue_8;
                            _hasValue = true;
                        }
                        if (_perItem.CashOut_11 != null)
                        {
                            _tmpValue2 = (decimal)_perItem.CashOut_11;
                            _hasValue = true;
                        }
                        _tot1 = _tmpValue1 + _tmpValue2;
                        _currAsstExtr = new AssetExtract()
                        {
                            AssetClass = "Portfolio Value Rectified",
                            MarketValueReportingCurrency = _hasValue ? _tot1 : null
                        };
                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                        _hasValue = false;
                        _tmpValue1 = _tmpValue2 = 0;
                        if (_perItem.EndValue_9 != null)
                        {
                            _tmpValue1 = (decimal)_perItem.EndValue_9;
                            _hasValue = true;
                        }
                        if (_perItem.CashIn_10 != null)
                        {
                            _tmpValue2 = (decimal)_perItem.CashIn_10;
                            _hasValue = true;
                        }
                        _tot2 = _tmpValue1 + _tmpValue2;
                        _currAsstExtr = new AssetExtract()
                        {
                            AssetClass = _perItem.EndDate_7 != null ? "Portfolio Value " + ((DateTime)_perItem.EndDate_7).ToString("dd/MM/yyyy", _culture) : string.Empty,
                            MarketValueReportingCurrency = _hasValue ? _tot2 : null
                        };
                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                        _currAsstExtr = new AssetExtract()
                        {
                            AssetClass = "Plus/Less Value",
                            MarketValueReportingCurrency = _tot2 - _tot1
                        };
                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                        _currAsstExtr = new AssetExtract()
                        {
                            AssetClass = "Dividend and Interest",
                            MarketValueReportingCurrency = _perItem.Interest_15
                        };
                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                        _hasValue = _hasValue1 = _hasValue2 = false;
                        _tot1 = _tmpValue1 = _tmpValue2 = 0;
                        if (_perItem.PlRealEquity_16 != null)
                        {
                            _tmpValue1 = (decimal)_perItem.PlRealEquity_16;
                            _hasValue = _hasValue1 = true;
                        }
                        if (_perItem.PlRealCurrency_17 != null)
                        {
                            _tmpValue2 = (decimal)_perItem.PlRealCurrency_17;
                            _hasValue = _hasValue2 = true;
                        }
                        _tot1 = _tmpValue1 + _tmpValue2;
                        _currAsstExtr = new AssetExtract()
                        {
                            AssetClass = "Realized Gains/Losses",
                            MarketValueReportingCurrency = _hasValue ? _tot1 : null
                        };
                        _currAsstExtr.AssetsType.Add(new AssetType()
                        {
                            Type = "of with on course",
                            MarketValueReportingCurrency = _hasValue1 ? _tmpValue1 : null
                        });
                        _currAsstExtr.AssetsType.Add(new AssetType()
                        {
                            Type = "of with on currency",
                            MarketValueReportingCurrency = _hasValue2 ? _tmpValue2 : null
                        });
                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                        _hasValue = _hasValue1 = _hasValue2 = false;
                        _tot2 = _tmpValue1 = _tmpValue2 = 0;
                        if (_perItem.PlNonRealEquity_18 != null)
                        {
                            _tmpValue1 = (decimal)_perItem.PlNonRealEquity_18;
                            _hasValue = _hasValue1 = true;
                        }
                        if (_perItem.PlNonRealCurrency_19 != null)
                        {
                            _tmpValue2 = (decimal)_perItem.PlNonRealCurrency_19;
                            _hasValue = _hasValue2 = true;
                        }
                        _tot2 = _tmpValue1 + _tmpValue2;
                        _currAsstExtr = new AssetExtract()
                        {
                            AssetClass = "Not Realized Gains/Losses",
                            MarketValueReportingCurrency = _hasValue ? _tot2 : null
                        };
                        _currAsstExtr.AssetsType.Add(new AssetType()
                        {
                            Type = "of with on course",
                            MarketValueReportingCurrency = _hasValue1 ? _tmpValue1 : null
                        });
                        _currAsstExtr.AssetsType.Add(new AssetType()
                        {
                            Type = "of with on currency",
                            MarketValueReportingCurrency = _hasValue2 ? _tmpValue2 : null
                        });
                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                        _currAsstExtr = new AssetExtract()
                        {
                            AssetClass = "Plus/Less Value",
                            MarketValueReportingCurrency = _perItem.Interest_15 + _tot1 + _tot2
                        };
                        _output.Content.AssetsExtract.Add(new AssetExtract(_currAsstExtr));
                    }
                }
            }
            return _output;
        }

    }

}
