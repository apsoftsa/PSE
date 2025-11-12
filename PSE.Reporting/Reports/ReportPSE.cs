using System.Drawing;
using System.ComponentModel;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace PSE.Reporting.Reports {

    public partial class ReportPSE : XtraReport {

        private const int MAX_ITEMS_SHORT_PALETTE = 8;
        private const string PALETTE_FULL_NAME = "BDSPaletteFull";
        private const string PALETTE_SHORT_NAME = "BDSPaletteShort";

        private string _currencyToApply;
        private string _currentAssetClassSection4000;
        private bool _hasSection70CaptionVisible;
        private bool _hasSection80CaptionVisible;
        private bool _hasSection90CaptionVisible;
        private bool _section70NeedPageBreakAtTheEnd;
        private bool _section80NeedPageBreakAtTheEnd;
        private bool _section90NeedPageBreakAtTheEnd;
        private bool _needResetRow;
        int _rowCount;
        int _currentChartPointsCount;        
        int _currentPointIndex;
        string _currentChartPointAlphaCode;
        int _currentGridChartPointIndex;
        string _currentGridChartPointAlphaCode;

        private void ManageSection70VisibilityFlags() {
            if (_section70NeedPageBreakAtTheEnd)
                _section70NeedPageBreakAtTheEnd = false;
        }

        private void ManageSection80VisibilityFlags() {
            if (_section80NeedPageBreakAtTheEnd)
                _section80NeedPageBreakAtTheEnd = false;
        }

        private void ManageSection90VisibilityFlags() {
            if (_section90NeedPageBreakAtTheEnd)
                _section90NeedPageBreakAtTheEnd = false;
        }

        private static string GetNextLabelAlphaCode(string current) {
            if (string.IsNullOrEmpty(current)) {
                return "A";
            }
            char letter = current[0];
            string numberPart = "";
            for (int i = 1; i < current.Length; i++) {
                if (char.IsDigit(current[i]))
                    numberPart += current[i];
            }
            int number = string.IsNullOrEmpty(numberPart) ? 0 : int.Parse(numberPart);
            if (letter == 'Z') {
                letter = 'A';
                number++;
            } else {
                letter++;
            }
            return number == 0 ? letter.ToString() : $"{letter}{number}";
        }

        private static int GetChartSeriesPointsCount(XRChart chart) {
            int pointsCount = 0;
            if (chart.Series != null && chart.Series.Count > 0 && chart.Series.First().ActualPoints != null)
                pointsCount = chart.Series.First().ActualPoints.Where(f => f.NumericalValue > 0).Count();
            return pointsCount;
        }

        private static Color GetChartSeriesPointColorToApply(XRChart chart, int pointsCount, int pointIndex) {
            Color colorToApply = Color.Transparent; // default
            if (chart.PaletteRepository != null) {
                Palette paletteToUse = chart.PaletteRepository[pointsCount > MAX_ITEMS_SHORT_PALETTE ? PALETTE_FULL_NAME : PALETTE_SHORT_NAME];
                int paletteIndex = pointIndex % paletteToUse.Count;
                colorToApply = paletteToUse[paletteIndex].Color;
            }
            return colorToApply;
        }

        public ReportPSE() {
            InitializeComponent();
            _currencyToApply = "?";
            _currentAssetClassSection4000 = string.Empty;
            _hasSection70CaptionVisible = false;
            _hasSection80CaptionVisible = false;
            _hasSection90CaptionVisible = false;
            _section70NeedPageBreakAtTheEnd = false;
            _section80NeedPageBreakAtTheEnd = false;
            _section90NeedPageBreakAtTheEnd = false;
            _needResetRow = false;
            _rowCount = 0;
        }

        private void languageToApply_BeforePrint(object sender, CancelEventArgs e) {
            string customerLanguage = ((XRLabel)sender).Text;
            if (string.IsNullOrWhiteSpace(customerLanguage) || string.IsNullOrEmpty(customerLanguage))
                customerLanguage = "E";
            customerLanguage = customerLanguage.Trim().ToUpper() switch {
                "E" => "en-CH",
                "F" => "fr-CH",
                "G" => "de-CH",
                _ => "it-CH",
            };
            this.ApplyLocalization(customerLanguage);
        }

        private void currencyToApply_BeforePrint(object sender, CancelEventArgs e) {
            _currencyToApply = ((XRLabel)sender).Text;
        }

        private void hiddenIfZero_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            bool toHidden = false;
            if (string.IsNullOrEmpty(label.Text) == false && double.TryParse(label.Text.Replace("%",""), out double value))
                toHidden = value == 0;
            label.Visible = !toHidden;
        }

        private void checkIfCambioNotMeaningful_BeforePrint(object sender, CancelEventArgs e) {
            if (double.TryParse(((XRLabel)sender).Text, out double exchange) && exchange == 1.0d) 
                ((XRLabel)sender).Text = "-";
        }

        private void hiddenIfCambioNotMeaningful_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            bool toHidden = false;
            if (string.IsNullOrEmpty(label.Text) == false && double.TryParse(label.Text.Replace("%", ""), out double value))
                toHidden = value == 0;
            if (toHidden)
                ((XRLabel)sender).Text = "-";
        }

        private void applyCurrency_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            label.Text = label.Text.Replace("{0}", _currencyToApply);
        }

        private void pageBreakBeforeGroupHeaderSection80_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeaderSection90_BeforePrint(object sender, CancelEventArgs e) {
            if (_section80NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection80VisibilityFlags();
            }
        }

        private void pageBreakBeforeReportHeader_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd) {
                ((ReportHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeader_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
            }
        }
      
        private void pageBreakBeforeBand_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd) {
                ((DetailReportBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
            }
        }

        private void chartDoughnut_BeforePrint(object sender, CancelEventArgs e) {
            _currentChartPointsCount = GetChartSeriesPointsCount((XRChart)sender);
            _currentPointIndex = 0;
        }

        private void chartDoughnut_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e) {
            if (_currentChartPointsCount > 0) {                
                e.SeriesDrawOptions.Color = GetChartSeriesPointColorToApply((XRChart)sender, _currentChartPointsCount, _currentPointIndex);
                _currentPointIndex++;
            }
        }

        private void chartDoughnutWithLabelAlphaCode_BeforePrint(object sender, CancelEventArgs e) {
            _currentChartPointsCount = GetChartSeriesPointsCount((XRChart)sender);
            _currentPointIndex = 0;
            _currentChartPointAlphaCode = string.Empty;
        }

        private void chartDoughnutWithLabelAlphaCode_CustomDrawSeriesPointWith(object sender, CustomDrawSeriesPointEventArgs e) {
            if (_currentChartPointsCount > 0) {
                _currentChartPointAlphaCode = GetNextLabelAlphaCode(_currentChartPointAlphaCode);
                e.LabelText += " (" + _currentChartPointAlphaCode + ")";
                e.SeriesDrawOptions.Color = GetChartSeriesPointColorToApply((XRChart)sender, _currentChartPointsCount, _currentPointIndex);
                _currentPointIndex++;
            }
        }

        private void contentHeaderRow1_BeforePrint(object sender, CancelEventArgs e) {
            string tmpText = ((XRLabel)sender).Text;
            tmpText = tmpText.Replace("{0}", this.labelHeaderStatoAl.Text);
            tmpText = tmpText.Replace("{1}", this.labelHeaderValutazioneIn.Text + " " + _currencyToApply);
            ((XRLabel)sender).Text = tmpText;
        }

        private void contentHeaderRow2_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = string.Concat(this.labelHeaderNumeroCliente.Text, " ", ((XRLabel)sender).Text);
        }

        private void BDSLogoPageHeader_PrintOnPage(object sender, PrintOnPageEventArgs e) {
            ((XRPictureBox)sender).Visible = e.PageIndex != e.PageCount - 1;
        }

        private void contentHeaderRow1_PrintOnPage(object sender, PrintOnPageEventArgs e) {
            ((XRLabel)sender).Visible = e.PageIndex > 0 && e.PageIndex != e.PageCount - 1;
        }

        private void contentHeaderRow2_PrintOnPage(object sender, PrintOnPageEventArgs e) {
            ((XRLabel)sender).Visible = e.PageIndex > 0 && e.PageIndex != e.PageCount - 1;
        }

        private void linePageHeader_PrintOnPage(object sender, PrintOnPageEventArgs e) {
            ((XRLine)sender).Visible = e.PageIndex > 0 && e.PageIndex != e.PageCount - 1;
        }       

        private void LabelRendicontoGestioneSection1000_BeforePrint(object sender, CancelEventArgs e) {
            string tmpText = ((XRLabel)sender).Text;
            tmpText = tmpText.Replace("{0}", this.ContentManagementReportFromDate.Text).Replace("{1}", this.ContentManagementReportToDate.Text);
            ((XRLabel)sender).Text = tmpText;
        }

        private void LabelSection1000ValorePortafoglio_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = ((XRLabel)sender).Text.Replace("{0}", this.ContentManagementReportPortfolioDate1.Text);
        }

        private void LabelSection1000ValoreDelPortafoglio_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = ((XRLabel)sender).Text.Replace("{0}", this.ContentManagementReportPortfolioDate2.Text);
        }

        private void ContentSection1000ValoreDelPortafoglio_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            bool toHidden = false;
            if (string.IsNullOrEmpty(label.Text) == false && double.TryParse(label.Text.Replace("%", ""), out double value))
                toHidden = value == 0;
            label.Visible = !toHidden;
            if (label.Visible && this.contentPatrimonialFluctuation.Value != null && double.TryParse(this.contentPatrimonialFluctuation.Value.ToString(), out double valuePF) && valuePF != 0)
                label.Text += "*";
        }

        private void labelOscillazionePatrimoniale_BeforePrint(object sender, CancelEventArgs e) {
            if (this.contentPatrimonialFluctuation.Value != null && double.TryParse(this.contentPatrimonialFluctuation.Value.ToString(), out double value) && value != 0) {
                string tmpText = ((XRLabel)sender).Text;
                tmpText = tmpText.Replace("{1}", this.contentPatrimonialFluctuation.Text).Replace("{0}", _currencyToApply).Replace("{2}", this.ContentManagementReportPortfolioDate2.Text);
                ((XRLabel)sender).Visible = true;
                ((XRLabel)sender).Text = tmpText;
            }
            else
                ((XRLabel)sender).Visible = false;  
        }

        private void DetailReportSection4000_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
        }        

        private void DetailReport4000_BeforePrint(object sender, CancelEventArgs e) {            
            this.DetailReport4000.HeightF = 0;
            this.PerformLayout();
        }

        private void assetClassSection4000Fake_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel currLabel = (XRLabel)sender;
            if (string.IsNullOrEmpty(_currentAssetClassSection4000) || currLabel.Text != _currentAssetClassSection4000) {
                _currentAssetClassSection4000 = currLabel.Text;
                _needResetRow = false;
            } else if (currLabel.Text == _currentAssetClassSection4000) {
                _needResetRow = true;
            }
        }

        private void lineUpperSection4000_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLine)sender).Visible = _needResetRow == false && this.DetailReportSection4000.CurrentRowIndex > 0;
            if (((XRLine)sender).Visible && this.DetailReportSection4000.CurrentRowIndex == _rowCount - 1)
                ((XRLine)sender).StyleName = "headerFooterLineStyle";
        }

        private void assetClassSection4000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel currLabel = (XRLabel)sender;
            if (_needResetRow)
                currLabel.Text = "";
            else if (this.DetailReportSection4000.CurrentRowIndex == _rowCount - 1) 
                currLabel.StyleName = "gridContentStyleBoldItalic";
        }

        private void marketValueReportingCurrencyTSection4000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel currLabel = (XRLabel)sender;
            if (_needResetRow) 
                currLabel.Text = "";
            else if (this.DetailReportSection4000.CurrentRowIndex == _rowCount - 1)
                currLabel.StyleName = "gridContentStyleRightAlignBoldItalic";
        }

        private void percentInvestmentTSection4000_BeforePrint(object sender, CancelEventArgs e) {
            if (_needResetRow) {
                XRLabel currLabel = (XRLabel)sender;
                currLabel.Text = "";
            }
        }

        private void lineMiddleSection4000_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLine)sender).Visible = this.classSection4000.Text != "Total";
        }

        private void checkSection70CaptionVisibility_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_hasSection70CaptionVisible == false && label.Visible) {
                _hasSection70CaptionVisible = true;
                _section70NeedPageBreakAtTheEnd = true;
            } else
                label.Visible = false;
        }

        private void checkSection80CaptionVisibility_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_hasSection80CaptionVisible == false && label.Visible) {
                _hasSection80CaptionVisible = true;
                _section80NeedPageBreakAtTheEnd = true;
            } else
                label.Visible = false;
        }

        private void checkSection90CaptionVisibility_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_hasSection90CaptionVisible == false && label.Visible) {
                _hasSection90CaptionVisible = true;
                _section90NeedPageBreakAtTheEnd = true;
            } else
                label.Visible = false;
        }

        private void DetailReportSection6000_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
            _currentGridChartPointIndex = 0;
        }

        private void bookmarkChartPercPesoSection6000_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                label.BackColor = Color.White;
                label.ForeColor = Color.White;
                if (double.TryParse(label.Value.ToString(), out double value)) {
                    if (value > 0 && value < 100) {
                        label.BackColor = GetChartSeriesPointColorToApply(this.chartSection6010, _currentChartPointsCount, _currentGridChartPointIndex);
                        label.ForeColor = label.BackColor;
                        _currentGridChartPointIndex++;
                    } 
                }
            }
        }

        private void bookmarkChartPercAlphaCodeSharesSection16000_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                label.Visible = false;
                if (double.TryParse(label.Value.ToString(), out double value)) {
                    if (value > 0 && value < 100) {
                        _currentGridChartPointAlphaCode = GetNextLabelAlphaCode(_currentGridChartPointAlphaCode);
                        label.Text = "(" + _currentGridChartPointAlphaCode.ToString() + ")";
                        label.Visible = true;
                    } 
                }
            }
        }

        private void divisaSection6000_BeforePrint(object sender, CancelEventArgs e) {
            if (this.DetailReportSection6000.CurrentRowIndex == _rowCount - 1) {
                ((XRLabel)sender).StyleName = "gridContentStyleBold";
                this.valoreMercatoSection6000.StyleName = "gridContentStyleRightAlignBold";
                this.section6000LineGrid.Visible = false;
            }
        }

        private void DetailReportSection160_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
            _currentGridChartPointIndex = 0;
            _currentGridChartPointAlphaCode = string.Empty;
        }

        private void DetailReport16000_BeforePrint(object sender, CancelEventArgs e) {
            this.DetailReport16000.HeightF = 0;
            this.PerformLayout();
        }

        private void bookmarkChartPercSharesSection16000_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                if (double.TryParse(label.Value.ToString(), out double value)) {
                    if (value > 0 && value < 100) {
                        label.BackColor = GetChartSeriesPointColorToApply(this.chartSection16010, _currentChartPointsCount, _currentGridChartPointIndex);
                        label.ForeColor = label.BackColor;
                        _currentGridChartPointIndex++;
                    } else {
                        label.BackColor = Color.White;
                        label.ForeColor = Color.White;
                    }
                }
            }
        }

        private void percentShares16000_BeforePrint(object sender, CancelEventArgs e) {
            if (this.DetailReportSection160.CurrentRowIndex == _rowCount - 1) {
                this.section16000LineGridUpper.StyleName = "headerFooterLineStyle";
                this.section16000LineGridUpper.LineWidth = 2;
                this.sector16000.StyleName = "gridContentStyleBold";
                this.marketValue16000.StyleName = "gridContentStyleRightAlignBold";
                this.percentShares16000Fake.StyleName = "gridContentStyleRightAlignBold";
                this.percentShares16000Fake.TextFormatString = "{0:0}%";
            } else {
                this.section16000LineGridUpper.StyleName = "gridRowLineStyle";
                this.section16000LineGridUpper.LineWidth = 1;
                this.sector16000.StyleName = "gridContentStyle";
                this.marketValue16000.StyleName = "gridContentStyleRightAlign";
                this.percentShares16000Fake.StyleName = "gridContentStyleRightAlign";
                this.percentShares16000Fake.TextFormatString = "{0:0.00}%";
            }
            this.section16000LineGridUpper.Visible = this.DetailReportSection160.CurrentRowIndex > 0;
            this.section16000LineGridDown.Visible = this.DetailReportSection160.CurrentRowIndex == _rowCount - 1;
        }

        private void section17000LineGridUpper_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLine)sender).Visible = this.DetailReportSection170.CurrentRowIndex > 0;
        }

        private void DetailReportSubSection170_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
        }

        private void subSection17000LineGridDown_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLine)sender).Visible = this.DetailReportSubSection170.CurrentRowIndex < _rowCount - 1;
        }

        private void Detail19010Objects_BeforePrint(object sender, CancelEventArgs e) {
            this.Detail19010Objects.HeightF = 0;
            this.Detail19010Objects.PerformLayout();
        }

        private void DetailReportSubSection19000_BeforePrint(object sender, CancelEventArgs e) {
            int count = this.DetailReportSubSection19000.RowCount;
            this.panelSection19000HeaderContainer.Visible = count > 0;
            this.panelSection19000FooterContainer.Visible = count > 0;
            this.lineSeparator19000.Visible = count > 0;
            this.labelHeaderSection19000.Visible = !(count == 0 && this.DetailReportSubSection19010.RowCount == 0);
            if (!this.labelHeaderSection19000.Visible) {
                this.DetailReportSubSection19000.HeightF = 0;
                this.DetailReportSubSection19000.PerformLayout();
            }
        }

        private void DetailReportSubSection190Objects_BeforePrint(object sender, CancelEventArgs e) {
            if (this.DetailReportSubSection19010.RowCount > 0 || (this.DetailReportSubSection19010.RowCount == 0 && this.DetailReportSubSection19000.RowCount == 0))
                this.DetailReportSubSection190Objects.ReportPrintOptions.PrintOnEmptyDataSource = false;
        }

        private void DetailReportSubSection19010_BeforePrint(object sender, CancelEventArgs e) {
            int count = this.DetailReportSubSection19010.RowCount;
            if (count > 0) {
                XRTableRow row;
                XRTableCell cellObject, cellDescription, cellAddressBook, cellCurrency, cellCurrentBalance, cellMarketValueReportingCurrency;
                this.xrTableSection19010Objects.Visible = true;
                this.panelSection19010HeaderContainer.Visible = true;
                this.panelSection19010FooterContainer.Visible = true;
                this.xrTableSection19010Objects.BeginInit();
                this.xrTableSection19010Objects.HeightF = this.xrTableRowSection19010Objects.HeightF * count;
                this.xrTableSection19010Objects.CanShrink = false;
                for (int i = 1; i < count; i++) {
                    row = new XRTableRow {
                        CanShrink = false,
                        HeightF = this.xrTableRowSection19010Objects.HeightF,
                        BorderColor = this.xrTableRowSection19010Objects.BorderColor,
                        Borders = i < count - 1 ? this.xrTableRowSection19010Objects.Borders : DevExpress.XtraPrinting.BorderSide.None
                    };
                    cellObject = new XRTableCell {                        
                        CanGrow = this.xrTableCellObject.CanGrow,
                        CanShrink = this.xrTableCellObject.CanShrink,
                        Multiline = this.xrTableCellObject.Multiline,
                        Name = this.xrTableCellObject.Name + i.ToString(),
                        StyleName = this.xrTableCellObject.StyleName,
                        WidthF = this.xrTableCellObject.WidthF
                    };
                    cellDescription = new XRTableCell {
                        CanGrow = this.xrTableCellDescription.CanGrow,
                        CanShrink = this.xrTableCellDescription.CanShrink,
                        Multiline = this.xrTableCellDescription.Multiline,
                        Name = this.xrTableCellDescription.Name + i.ToString(),
                        StyleName = this.xrTableCellDescription.StyleName,
                        WidthF = this.xrTableCellDescription.WidthF
                    };
                    cellAddressBook = new XRTableCell {
                        CanGrow = this.xrTableCellAddressBook.CanGrow,
                        CanShrink = this.xrTableCellAddressBook.CanShrink,
                        Multiline = this.xrTableCellAddressBook.Multiline,
                        Name = this.xrTableCellAddressBook.Name + i.ToString(),
                        StyleName = this.xrTableCellAddressBook.StyleName,
                        WidthF = this.xrTableCellAddressBook.WidthF
                    };
                    cellCurrency = new XRTableCell {
                        CanGrow = this.xrTableCellCurrency.CanGrow,
                        CanShrink = this.xrTableCellCurrency.CanShrink,
                        Multiline = this.xrTableCellCurrency.Multiline,
                        Name = this.xrTableCellCurrency.Name + i.ToString(),
                        StyleName = this.xrTableCellCurrency.StyleName,
                        WidthF = this.xrTableCellCurrency.WidthF
                    };
                    cellCurrentBalance = new XRTableCell {
                        CanGrow = this.xrTableCellCurrentBalance.CanGrow,
                        CanShrink = this.xrTableCellCurrentBalance.CanShrink,
                        Multiline = this.xrTableCellCurrentBalance.Multiline,
                        Name = this.xrTableCellCurrentBalance.Name + i.ToString(),
                        StyleName = this.xrTableCellCurrentBalance.StyleName,
                        TextFormatString = this.xrTableCellCurrentBalance.TextFormatString,
                        WidthF = this.xrTableCellCurrentBalance.WidthF
                    };
                    cellMarketValueReportingCurrency = new XRTableCell {
                        CanGrow = this.xrTableCellMarketValueReportingCurrency.CanGrow,
                        CanShrink = this.xrTableCellMarketValueReportingCurrency.CanShrink,
                        Multiline = this.xrTableCellMarketValueReportingCurrency.Multiline,
                        Name = this.xrTableCellMarketValueReportingCurrency.Name + i.ToString(),
                        StyleName = this.xrTableCellMarketValueReportingCurrency.StyleName,
                        TextFormatString = this.xrTableCellMarketValueReportingCurrency.TextFormatString,
                        WidthF = this.xrTableCellMarketValueReportingCurrency.WidthF
                    };
                    row.Cells.AddRange([
                        cellObject,
                        cellDescription,
                        cellAddressBook,
                        cellCurrency,
                        cellCurrentBalance,
                        cellMarketValueReportingCurrency]);
                    row.Dpi = 254F;
                    row.Name = "xrTableRow" + i.ToString() + "Section19010Objects";
                    row.StylePriority.UseBorderColor = false;
                    row.StylePriority.UseBorders = false;                    
                    this.xrTableSection19010Objects.Rows.Add(row);                    
                }
                this.panelSection19010FooterContainer.TopF = this.xrTableSection19010Objects.TopF + this.xrTableSection19010Objects.HeightF;
                this.xrTableSection19010Objects.EndInit();
                this.xrTableSection19010Objects.PerformLayout();
            } else {
                this.xrTableSection19010Objects.Visible = false;
                this.panelSection19010HeaderContainer.Visible = false;  
                this.panelSection19010FooterContainer.Visible = false;                
            }
        }

        private void ContentDetail19010Object_BeforePrint(object sender, CancelEventArgs e) {
            int index = this.DetailReportSubSection19010.CurrentRowIndex;
            if (this.xrTableSection19010Objects.Rows.Count > index) {
                this.xrTableSection19010Objects.Rows[index].Cells[0].Text = this.ContentDetail19010Object.Text;
                this.xrTableSection19010Objects.Rows[index].Cells[1].Text = this.ContentDetail19010Description.Text;
                this.xrTableSection19010Objects.Rows[index].Cells[2].Text = this.ContentDetail19010AddressBook.Text;
                this.xrTableSection19010Objects.Rows[index].Cells[3].Text = this.ContentDetail19010Currency.Text;
                this.xrTableSection19010Objects.Rows[index].Cells[4].Text = (string.IsNullOrEmpty(this.ContentDetail19010CurrentBalance.Text) == false && double.TryParse(this.ContentDetail19010CurrentBalance.Text, out double valueCB) && valueCB != 0) ? this.ContentDetail19010CurrentBalance.Text : string.Empty;
                this.xrTableSection19010Objects.Rows[index].Cells[5].Text = (string.IsNullOrEmpty(this.ContentDetail19010MarketValue.Text) == false && double.TryParse(this.ContentDetail19010MarketValue.Text, out double valueMV) && valueMV != 0) ? this.ContentDetail19010MarketValue.Text : string.Empty;
            }
        }       

        private void DetailReportSubSection200_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
        }

        private void contentTipoClasseSection20020_BeforePrint(object sender, CancelEventArgs e) {
            if (this.DetailReportSubSection200.CurrentRowIndex >= _rowCount - 2) {
                ((XRLabel)sender).StyleName = "gridContentStyleBoldItalic";
            }
        }

        private void contentValoreMercatoSection20020_BeforePrint(object sender, CancelEventArgs e) {
            if (this.DetailReportSubSection200.CurrentRowIndex >= _rowCount - 3) {
                ((XRLabel)sender).StyleName = "gridContentStyleRightAlignBoldItalic";
            }
        }

        private void contentInvestimentiSection20020_BeforePrint(object sender, CancelEventArgs e) {
            if (this.DetailReportSubSection200.CurrentRowIndex >= _rowCount - 3) {
                ((XRLabel)sender).StyleName = "gridContentStyleRightAlignBoldItalic";
            }
        }

        private void pageFooterContainer_PrintOnPage(object sender, PrintOnPageEventArgs e) {
            ((XRPanel)sender).Visible = !(e.PageIndex == 0 || e.PageIndex == e.PageCount - 1);
        }
       
    }

}
