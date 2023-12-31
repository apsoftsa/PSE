﻿using System.Globalization;
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

    public class ManipulatorSection4 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection4(CultureInfo? culture = null) : base(ManipolationTypes.AsSection4, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section4 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(PER)))
            {
                IHistoryEvolutionPerformanceCurrency _historyEvo;
                IChartPerformanceEvolution _chartEvo;
                ISection4Content _sectionContent;
                decimal _tmpInpOut;
                string _tmpPeriod;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<PER> _perItems = extractedData.Where(_flt => _flt.RecordType == nameof(PER)).OfType<PER>();
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_perItems != null && _perItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section4Content();
                        foreach (PER _perItem in _perItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2).OrderByDescending(_ob => _ob.StartDate_6))
                        {
                            if (_perItem.StartDate_6 != null && _perItem.EndDate_7 != null
                                && _perItem.StartDate_6.Value.Year == _perItem.EndDate_7.Value.Year
                                && _perItem.StartDate_6.Value.Month == 1 && _perItem.EndDate_7.Value.Month == 12
                                && _perItem.StartDate_6.Value.Day == 1 && _perItem.EndDate_7.Value.Day == 31)
                                _tmpPeriod = _perItem.StartDate_6.Value.Year.ToString();
                            else if (_perItem.StartDate_6 != null && _perItem.EndDate_7 != null)
                                _tmpPeriod = _perItem.StartDate_6.Value.ToString(DEFAULT_DATE_FORMAT, _culture) + "-" + _perItem.EndDate_7.Value.ToString(DEFAULT_DATE_FORMAT, _culture);
                            else if (_perItem.StartDate_6 != null)
                                _tmpPeriod = _perItem.StartDate_6.Value.ToString(DEFAULT_DATE_FORMAT, _culture);
                            else if (_perItem.EndDate_7 != null)
                                _tmpPeriod = _perItem.EndDate_7.Value.ToString(DEFAULT_DATE_FORMAT, _culture);
                            else
                                _tmpPeriod = string.Empty;
                            _tmpInpOut = _perItem.CashIn_10 != null ? _perItem.CashIn_10.Value : 0;
                            _tmpInpOut += _perItem.CashOut_11 != null ? _perItem.CashOut_11.Value : 0;
                            _tmpInpOut += _perItem.SecIn_12 != null ? _perItem.SecIn_12.Value : 0;
                            _tmpInpOut += _perItem.SecOut_13 != null ? _perItem.SecOut_13.Value : 0;
                            _historyEvo = new HistoryEvolutionPerformanceCurrency()
                            {
                                Period = _tmpPeriod,
                                PercentPerformance = _perItem.TWR_14 != null ? _perItem.TWR_14.Value : 0,
                                InitialAmount = _perItem.StartValue_8 != null ? _perItem.StartValue_8.Value : 0,
                                FinalAmount = _perItem.EndValue_9 != null ? _perItem.EndValue_9.Value : 0,
                                InputsOutputs = _tmpInpOut
                            };
                            _sectionContent.HistoryEvolutionPerformancesCurrency.Add(_historyEvo);
                            _chartEvo = new ChartPerformanceEvolution()
                            {
                                Year = _tmpPeriod,
                                PercentPerformance = _perItem.TWR_14 != null ? _perItem.TWR_14.Value : 0
                            };
                            _sectionContent.ChartPerformanceEvolutions.Insert(0, _chartEvo);
                        }
                        _output.Content = new Section4Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
