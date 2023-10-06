using PSE.BusinessLogic.Interfaces;
using PSE.Model.Params;
using PSE.Model.Common;

namespace PSE.BusinessLogic.Utility
{

    public class BondsCalculation : IBondsCalculation
    {

        private readonly string _classGroup; // (????) significato ed origine da chiarire...
        private readonly bool _zeroHistoricalPurchasePriceSet; // (????) significato sconosciuto...        

        public BondsCalculation()
        {
            _classGroup = string.Empty;
            _zeroHistoricalPurchasePriceSet = false;
        }

        public BondsCalculation(string classGroup, bool zeroHistoricalPurchasePriceSet = false) 
        { 
            _classGroup = classGroup;
            _zeroHistoricalPurchasePriceSet = zeroHistoricalPurchasePriceSet;   
        }

        // to-do (???)
        // corrisponde a: "FlagCambioMercato"
        // implementazione sconosciuta...
        internal bool StockMarketToChange(string code) { return string.IsNullOrEmpty(code); }

        // Corrisponde a: "ValoreVariazioneGlobale"
        public decimal GetGlobalVariationValue(GlobalVariationValueParams recParams)
        {
            decimal _globalVariationValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                decimal _divider;
                bool _isOpposite = false;
                decimal _change1c = 0;
                decimal _change2c = 0;
                if (recParams.Percentage == -1)
                    _isOpposite = true;
                if (_isOpposite)
                {
                    if (recParams.Change1 > 0 && recParams.Change2 > 0)
                    {
                        _divider = 1;
                        _change1c = _divider / recParams.Change1;
                        _change2c = _divider / recParams.Change2;
                    }
                }
                else
                {
                    _change1c = recParams.Change1;
                    _change2c = recParams.Change2;
                }
                _divider = recParams.Trend1 * _change1c * (decimal)100;
                if (_divider > 0)
                {
                    if (recParams.Sign == Constants.NEGATIVE_SIGN)
                        _globalVariationValue = (recParams.Trend1 * _change1c - recParams.Trend2 * _change2c) / _divider;
                    else
                        _globalVariationValue = (recParams.Trend2 * _change2c - recParams.Trend1 * _change1c) / _divider;
                }
            }
            return _globalVariationValue;
        }

        // Corrisponde a: "ValoreDifferenzaCorso"
        public decimal GetPriceDifferenceValue(PriceDifferenceValueParams recParams)
        {
            decimal _priceDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (string.IsNullOrEmpty(this._classGroup) == false && _classGroup == "D") // derivati
                {
                    _priceDifferenceValue = GetCourseChangeValue5(new CourseChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.Trend1,
                        Trend2 = recParams.Trend2
                    });
                }
                else
                {
                    _priceDifferenceValue = GetCourseChangeValue(new CourseChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.Trend1,
                        Trend2 = recParams.Trend2
                    });
                }
            }
            return _priceDifferenceValue;
        }

        // Corrisponde a: "ValoreVariazioneCorso"
        public decimal GetCourseChangeValue(CourseChangeValueParams recParams)
        {
            decimal _courseChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                if (!(recParams.Trend1 < (decimal)0.01 || recParams.Trend2 < (decimal)0.01))
                {
                    if (recParams.Sign == Constants.NEGATIVE_SIGN)
                        _courseChangeValue = (recParams.Trend1 - recParams.Trend2) / recParams.Trend1 * (decimal)100;
                    else
                        _courseChangeValue = (recParams.Trend2 - recParams.Trend1) / recParams.Trend1 * (decimal)100;
                }
            }
            return _courseChangeValue;
        }

        // Corrisponde a: "ValoreVariazioneCorso5"
        public decimal GetCourseChangeValue5(CourseChangeValueParams recParams)
        {
            decimal _courseChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                if (!(recParams.Trend1 < (decimal)0.00001 || recParams.Trend2 < (decimal)0.00001))
                {
                    if (recParams.Sign == Constants.NEGATIVE_SIGN)
                        _courseChangeValue = (recParams.Trend1 - recParams.Trend2) / recParams.Trend1 * (decimal)100;
                    else
                        _courseChangeValue = (recParams.Trend2 - recParams.Trend1) / recParams.Trend1 * (decimal)100;
                }
            }
            return _courseChangeValue;
        }

        // Corrisponde a: "ValoreDifferenzaGlobale"
        public decimal GetGlobalDifferenceValue(GlobalDifferenceValueParams recParams)
        {
            decimal _globalDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (StockMarketToChange(recParams.Code))
                {
                    _globalDifferenceValue = GetGlobalVariationValue(new GlobalVariationValueParams() { 
                        Sign = recParams.Sign,
                        Trend1 = recParams.TrendAcq,
                        Trend2 = recParams.Trend,
                        Change1 = recParams.ChangeAcqM,
                        Change2 = recParams.ChangeM,
                        Percentage = recParams.Reverse
                    });
                }
                else
                {
                    _globalDifferenceValue = GetGlobalVariationValue(new GlobalVariationValueParams()
                    {
                        Sign = recParams.Sign,
                        Trend1 = recParams.TrendAcq,
                        Trend2 = recParams.Trend,
                        Change1 = recParams.ChangeAcq,
                        Change2 = recParams.Change,
                        Percentage = recParams.Reverse
                    });
                }
            }
            return _globalDifferenceValue;
        }

        // Corrisponde a: "ValoreDifferenzaCambio"
        public decimal GetExchangeRateDifferenceValue(ExchangeRateDifferenceValueParams recParams)
        {
            decimal _exchangeRateDifferenceValue = 0;
            if (recParams.IsValid)
            {
                if (StockMarketToChange(recParams.Code))
                {
                    _exchangeRateDifferenceValue = GetExchangeRateChangeValue(new ExchangeRateChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Change1 = recParams.ChangeAcqM,
                        Change2 = recParams.ChangeM,
                        Percentage = recParams.Reverse
                    });
                }
                else
                {
                    _exchangeRateDifferenceValue = GetExchangeRateChangeValue(new ExchangeRateChangeValueParams()
                    {
                        Sign = recParams.Sign,
                        Change1 = recParams.ChangeAcq,
                        Change2 = recParams.Change,
                        Percentage = recParams.Reverse
                    });
                }
            }
            return _exchangeRateDifferenceValue;
        }

        // Corrisponde a: "ValoreNonRealizzato"
        public decimal GetUnrealizedValue(UnrealizedValueParams recParams)
        {
            decimal _unrealizedValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                bool _continue = true;
                if (_zeroHistoricalPurchasePriceSet)
                {
                    decimal? _change1, _change2;
                    decimal? _trend1 = recParams.TrendAcq;
                    decimal? _trend2 = recParams.Trend;
                    if (StockMarketToChange(recParams.Code))
                    {
                        _change1 = recParams.ChangeAcqM;
                        _change2 = recParams.ChangeM;
                    }
                    else
                    {
                        _change1 = recParams.ChangeAcq;
                        _change2 = recParams.Change;
                    }
                    if (_change1.HasValue && _change2.HasValue && _trend1.HasValue && _trend2.HasValue)
                    {
                        if (_trend1 == 0 && _change1 != 0 && _trend2 != 0 && _change2 != 0)
                        {
                            if (recParams.Sign == Constants.NEGATIVE_SIGN)
                                _unrealizedValue = Math.Abs(recParams.ImpCtv) * (decimal)-1;
                            else
                                _unrealizedValue = recParams.ImpCtv;
                            _continue = false;
                        }
                    }
                }
                if (_continue)
                {
                    decimal _global = GetGlobalDifferenceValue(new GlobalDifferenceValueParams(
                        recParams.Code, 
                        recParams.Sign, 
                        recParams.TrendAcq, 
                        recParams.Trend, 
                        recParams.ChangeAcq, 
                        recParams.Change, 
                        recParams.ChangeAcqM,
                        recParams.ChangeM, 
                        recParams.Reverse)
                    );
                    _global /= (decimal)100;
                    if (_global == -1) // ???
                        _unrealizedValue = 0 - recParams.ImpCtv;
                    else if(_global != 0)
                    {
                        if (recParams.Sign == Constants.NEGATIVE_SIGN)
                            _unrealizedValue = Math.Abs(recParams.ImpCtv) * (_global / ((decimal)1 - _global));
                        else
                            _unrealizedValue = recParams.ImpCtv * (_global / ((decimal)1 + _global));
                    }
                }
            }
            return _unrealizedValue;
        }

        // Corrisponde a: "ValoreVariazioneCambio"
        public decimal GetExchangeRateChangeValue(ExchangeRateChangeValueParams recParams)
        {
            decimal _exchangeRateChangeValue = 0;
            if (recParams != null && recParams.IsValid)
            {
                decimal _change1c, _change2c;                
                bool _isOpposite = false;                
                if (recParams.Percentage == -1)
                    _isOpposite = true;
                if (_isOpposite)
                {
                    decimal _divider = 1;
                    _change1c = _divider / recParams.Change1;
                    _change2c = _divider / recParams.Change2;
                }
                else
                {
                    _change1c = recParams.Change1;
                    _change2c = recParams.Change2;
                }
                if (recParams.Sign == Constants.NEGATIVE_SIGN)
                    _exchangeRateChangeValue = (_change1c - _change2c) / _change1c * (decimal)100;
                else
                    _exchangeRateChangeValue = (_change2c - _change1c) / _change1c * (decimal)100;
            }
            return _exchangeRateChangeValue;
        }

    }

}
