using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection18And19 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection18And19(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section18And19 _output = new()
            {
                SectionCode = OUTPUT_SECTION18AND19_CODE,
                SectionName = "Others investments"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IAlternativeProductDetail _altProdDetails;
                IAlternativeProducts _altProdDefinitions;
                ISection18And19Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION18AND19);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section18And19Content();
                        _altProdDefinitions = new AlternativeProducts();
                        foreach (POS _posItem in _posItems)
                        {
                            _altProdDetails = new AlternativeProductDetail()
                            {
                                ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                Currency = _posItem.Currency1_17,
                                Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                DescriptionExtra = string.IsNullOrEmpty(_posItem.ConversionDesc_45) ? "" : _posItem.ConversionDesc_45.Trim(),
                                CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                Isin = _posItem.IsinIban_85,
                                PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                NominalAmmount = 0, // not still recovered (!)
                                UnderlyingDescription = "[UnderlyingDescription]", // not still recovered (!)
                                ExchangeRateImpactPurchase = 0, // not still recovered (!)
                                ExchangeRateImpactYTD = 0, // not still recovered (!)
                                PerformancePurchase = 0, // not still recovered (!)
                                PercentPerformancePurchase = 0, // not still recovered (!)
                                PerformanceYTD = 0, // not still recovered (!)
                                PercentPerformanceYTD = 0, // not still recovered (!)
                                PercentAsset = 0 // not still recovered (!)
                                // cambio storico ?! (!!!!)
                            };
                            // !!!!
                            // come faccio a capire a quale di questi 5 sotto insiemi li devo associare?!
                            //IList<IAlternativeProductDetail> DerivativesOnSecurities { get; set; }
                            //IList<IAlternativeProductDetail> DerivativesOnMetals { get; set; }
                            //IList<IAlternativeProductDetail> DerivativesFutures { get; set; }
                            //IList<IAlternativeProductDetail> Different { get; set; }
                            //IList<IAlternativeProductDetail> DifferentExtra { get; set; }
                            _altProdDefinitions.Different.Add(_altProdDetails); // temporaneo per ora vanno tutti su 'different' !!!!
                        }                                                
                        _sectionContent.AlternativeProducts.Add(_altProdDefinitions);
                        _output.Content = new Section18And19Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
