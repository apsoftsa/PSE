using System.Drawing;
using System.ComponentModel;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace PSE.Reporting.Reports {

    public partial class ReportPSE : XtraReport {

        private const int MAX_ITEMS_SHORT_PALETTE = 9;
        private const int MAX_CHART_ELEMENT_COUNT = 11;
        private const string PALETTE_FULL_NAME = "BDSPaletteFull";
        private const string PALETTE_SHORT_NAME = "BDSPaletteShort";
        private const string CLASS_ITEM_ENTRY = "Entry";
        private const string CLASS_ITEM_TOTAL = "Total";

        private readonly char _lastLetterChart;

        private string _customerLanguage;
        private string _customerCulture;
        private string _currencyToApply;
        private string _currentAssetClassSection4000;
        private bool _hasSection70CaptionVisible;
        private bool _hasSection80CaptionVisible;
        private bool _hasSection90CaptionVisible;
        private bool _hasSection100CaptionVisible;
        private bool _hasSection110CaptionVisible;
        private bool _hasSection120CaptionVisible;
        private bool _section70NeedPageBreakAtTheEnd;
        private bool _section80NeedPageBreakAtTheEnd;
        private bool _section90NeedPageBreakAtTheEnd;
        private bool _section100NeedPageBreakAtTheEnd;
        private bool _section110NeedPageBreakAtTheEnd;
        private bool _section120NeedPageBreakAtTheEnd;
        private bool _needResetRow;
        private int _rowCount;
        private int _currentChartPointsCount;
        private int _currentPointIndex;
        private string _currentChartPointAlphaCode;
        private int _currentGridChartPointIndex;
        private string _currentGridChartPointAlphaCode;
        private int _multilinePeriodCount;
        private int _chartBarsCount;
        private bool _chartHasMeaningfulData;
        private bool _hasNotTransfered;

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

        private void ManageSection100VisibilityFlags() {
            if (_section100NeedPageBreakAtTheEnd)
                _section100NeedPageBreakAtTheEnd = false;
        }

        private void ManageSection110VisibilityFlags() {
            if (_section110NeedPageBreakAtTheEnd)
                _section110NeedPageBreakAtTheEnd = false;
        }

        private void ManageSection120VisibilityFlags() {
            if (_section120NeedPageBreakAtTheEnd)
                _section120NeedPageBreakAtTheEnd = false;
        }

        private string GetNextLabelAlphaCode(string current, bool groupToLast = false) {
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
                if (groupToLast) {
                    if (letter < _lastLetterChart)
                        letter++;
                    else
                        letter = _lastLetterChart;
                } else
                    letter++;
            }
            return number == 0 ? letter.ToString() : $"{letter}{number}";
        }

        private static int GetChartSeriesPointsCount(XRChart chart) {
            int pointsCount = 0;
            if (chart.Series != null && chart.Series.Count > 0 && chart.Series.First().ActualPoints != null)
                pointsCount = chart.Series.First().ActualPoints.Count();
            return pointsCount;
        }

        private static Color GetChartSeriesPointColorToApply(XRChart chart, int pointsCount, int pointIndex, bool groupToLast = false) {
            Color colorToApply = Color.Transparent; // default
            if (chart.PaletteRepository != null) {
                Palette paletteToUse = chart.PaletteRepository[pointsCount > MAX_ITEMS_SHORT_PALETTE ? PALETTE_FULL_NAME : PALETTE_SHORT_NAME];
                int paletteIndex = pointIndex % paletteToUse.Count;
                if (groupToLast && pointIndex >= MAX_CHART_ELEMENT_COUNT - 1)
                    paletteIndex = MAX_CHART_ELEMENT_COUNT - 1;
                colorToApply = paletteToUse[paletteIndex].Color;
            }
            return colorToApply;
        }

        public ReportPSE() {
            InitializeComponent();
            _customerLanguage = "E";
            _customerCulture = "en-CH";
            _currencyToApply = "?";
            _currentAssetClassSection4000 = string.Empty;
            _hasSection70CaptionVisible = false;
            _hasSection80CaptionVisible = false;
            _hasSection90CaptionVisible = false;
            _hasSection100CaptionVisible = false;
            _hasSection110CaptionVisible = false;
            _hasSection120CaptionVisible = false;
            _section70NeedPageBreakAtTheEnd = false;
            _section80NeedPageBreakAtTheEnd = false;
            _section90NeedPageBreakAtTheEnd = false;
            _section100NeedPageBreakAtTheEnd = false;
            _section110NeedPageBreakAtTheEnd = false;
            _section120NeedPageBreakAtTheEnd = false;
            _needResetRow = false;
            _hasNotTransfered = false;
            _chartHasMeaningfulData = false;
            _currentGridChartPointAlphaCode = string.Empty;
            _currentChartPointAlphaCode = string.Empty;
            _rowCount = 0;
            _chartBarsCount = 0;
            _multilinePeriodCount = 0;
            _lastLetterChart = 'A';
            for (int c = 0; c < MAX_CHART_ELEMENT_COUNT - 1; c++)
                _lastLetterChart++;
        }

        private void languageToApply_BeforePrint(object sender, CancelEventArgs e) {
            _customerLanguage = ((XRLabel)sender).Text;
            if (string.IsNullOrWhiteSpace(_customerLanguage) || string.IsNullOrEmpty(_customerLanguage))
                _customerLanguage = "E";
            _customerCulture = _customerLanguage.Trim().ToUpper() switch {
                "E" => "en-CH",
                "F" => "fr-CH",
                "G" => "de-CH",
                _ => "it-CH",
            };
            this.ApplyLocalization(_customerCulture);
        }

        private void currencyToApply_BeforePrint(object sender, CancelEventArgs e) {
            _currencyToApply = ((XRLabel)sender).Text;
        }

        private void hiddenIfZero_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            bool toHidden = false;
            if (string.IsNullOrEmpty(label.Text) == false && double.TryParse(label.Text.Replace("%", ""), out double value))
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

        private void checkChartHasMeaningfulData_BeforePrint(object sender, CancelEventArgs e) {
            _chartHasMeaningfulData = false;
            if (bool.TryParse(((XRLabel)sender).Text, out bool tmpCheck))
                _chartHasMeaningfulData = tmpCheck;
        }

        private void injectDateAfterLabelText_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (label.Tag != null) {
                label.Text = label.Text.Split(" ").First() + " " + label.Tag.ToString();
            }
        }

        private void pageBreakBeforeGroupHeaderSection80_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeaderSection90_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeaderSection100_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeaderSection110_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd || _section100NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
                ManageSection100VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeaderSection120_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd || _section100NeedPageBreakAtTheEnd || _section110NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
                ManageSection100VisibilityFlags();
                ManageSection110VisibilityFlags();
            }
        }

        private void pageBreakBeforeReportHeader_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd || _section100NeedPageBreakAtTheEnd || _section110NeedPageBreakAtTheEnd || _section120NeedPageBreakAtTheEnd) {
                ((ReportHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
                ManageSection100VisibilityFlags();
                ManageSection110VisibilityFlags();
                ManageSection120VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeader_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd || _section100NeedPageBreakAtTheEnd || _section110NeedPageBreakAtTheEnd || _section120NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
                ManageSection100VisibilityFlags();
                ManageSection110VisibilityFlags();
                ManageSection120VisibilityFlags();
            }
        }

        private void pageBreakBeforeBand_BeforePrint(object sender, CancelEventArgs e) {
            if (_section70NeedPageBreakAtTheEnd || _section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd || _section100NeedPageBreakAtTheEnd || _section110NeedPageBreakAtTheEnd || _section120NeedPageBreakAtTheEnd) {
                ((DetailReportBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection70VisibilityFlags();
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
                ManageSection100VisibilityFlags();
                ManageSection110VisibilityFlags();
                ManageSection120VisibilityFlags();
            }
        }

        private void chartDoughnut_BeforePrint(object sender, CancelEventArgs e) {
            _currentChartPointsCount = GetChartSeriesPointsCount((XRChart)sender);
            _currentPointIndex = 0;
        }

        private void chartDoughnutValid_BeforePrint(object sender, CancelEventArgs e) {
            if (_chartHasMeaningfulData) {
                _currentChartPointsCount = GetChartSeriesPointsCount((XRChart)sender);
                _currentPointIndex = 0;
            } else
                e.Cancel = true;
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

        private void chartDoughnutValidWithLabelAlphaCode_BeforePrint(object sender, CancelEventArgs e) {
            if (_chartHasMeaningfulData) {
                _currentChartPointsCount = GetChartSeriesPointsCount((XRChart)sender);
                _currentPointIndex = 0;
                _currentChartPointAlphaCode = string.Empty;
            } else
                e.Cancel = true;
        }

        private void chartBarInvalid_BeforePrint(object sender, CancelEventArgs e) {
            if (!_chartHasMeaningfulData) {
                XRChart chart = (XRChart)sender;
                _currentChartPointsCount = GetChartSeriesPointsCount(chart);
                _currentPointIndex = 0;
                if (chart.Series != null && chart.Series.Count > 0) {
                    SideBySideBarSeriesView sideBySideBarSeriesView1 = (SideBySideBarSeriesView)chart.Series[0].View;
                    if (_chartBarsCount < 2)
                        sideBySideBarSeriesView1.BarWidth = 0.3;
                    else if (_chartBarsCount < 3)
                        sideBySideBarSeriesView1.BarWidth = 0.6;
                    else if (_chartBarsCount < 4)
                        sideBySideBarSeriesView1.BarWidth = 0.7;
                    else if (_chartBarsCount < 6)
                        sideBySideBarSeriesView1.BarWidth = 0.8;
                    else
                        sideBySideBarSeriesView1.BarWidth = 0.9;
                }
                chart.Visible = true;
            } else
                e.Cancel = true;
        }

        private void chartBarInvalidWithLabelAlphaCode_BeforePrint(object sender, CancelEventArgs e) {
            if (!_chartHasMeaningfulData) {
                XRChart chart = (XRChart)sender;
                _currentChartPointsCount = GetChartSeriesPointsCount(chart);
                _currentPointIndex = 0;
                _currentChartPointAlphaCode = string.Empty;
                if (chart.Series != null && chart.Series.Count > 0) {
                    SideBySideBarSeriesView sideBySideBarSeriesView1 = (SideBySideBarSeriesView)chart.Series[0].View;
                    if (_chartBarsCount < 2)
                        sideBySideBarSeriesView1.BarWidth = 0.3;
                    else if (_chartBarsCount < 3)
                        sideBySideBarSeriesView1.BarWidth = 0.6;
                    else if (_chartBarsCount < 4)
                        sideBySideBarSeriesView1.BarWidth = 0.7;
                    else if (_chartBarsCount < 6)
                        sideBySideBarSeriesView1.BarWidth = 0.8;
                    else
                        sideBySideBarSeriesView1.BarWidth = 0.9;
                }
                chart.Visible = true;
            } else
                e.Cancel = true;
        }

        private void chartDoughnutWithLabelAlphaCode_CustomDrawSeriesPointWith(object sender, CustomDrawSeriesPointEventArgs e) {
            if (_currentChartPointsCount > 0) {
                _currentChartPointAlphaCode = GetNextLabelAlphaCode(_currentChartPointAlphaCode);
                e.LabelText += " (" + _currentChartPointAlphaCode + ")";
                e.SeriesDrawOptions.Color = GetChartSeriesPointColorToApply((XRChart)sender, _currentChartPointsCount, _currentPointIndex);
                _currentPointIndex++;
            }
        }

        private void chartBarWithLabelAlphaCode_CustomDrawSeriesPointWith(object sender, CustomDrawSeriesPointEventArgs e) {
            if (_currentChartPointsCount > 0) {
                _currentChartPointAlphaCode = GetNextLabelAlphaCode(_currentChartPointAlphaCode);
                e.LabelText += " (" + _currentChartPointAlphaCode + ")";
                e.SeriesDrawOptions.Color = GetChartSeriesPointColorToApply((XRChart)sender, _currentChartPointsCount, _currentPointIndex);
                _currentPointIndex++;
            }
        }

        private void chartBar_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e) {
            if (_currentChartPointsCount > 0) {
                e.SeriesDrawOptions.Color = GetChartSeriesPointColorToApply((XRChart)sender, _currentChartPointsCount, _currentPointIndex);
                _currentPointIndex++;
            }
        }

        private void detectChartBarsCount_BeforePrint(object sender, CancelEventArgs e) {
            _chartBarsCount = 0;
            if (int.TryParse(((XRLabel)sender).Text, out int tmpCount))
                _chartBarsCount = tmpCount;
        }

        private void chartBarWidth_BeforePrint(object sender, CancelEventArgs e) {
            XRChart chart = (XRChart)sender;
            if (chart.Series != null && chart.Series.Count > 0) {
                SideBySideBarSeriesView sideBySideBarSeriesView1 = (SideBySideBarSeriesView)chart.Series[0].View;
                if (_chartBarsCount < 2)
                    sideBySideBarSeriesView1.BarWidth = 0.3;
                else if (_chartBarsCount < 3)
                    sideBySideBarSeriesView1.BarWidth = 0.6;
                else if (_chartBarsCount < 4)
                    sideBySideBarSeriesView1.BarWidth = 0.7;
                else if (_chartBarsCount < 6)
                    sideBySideBarSeriesView1.BarWidth = 0.8;
                else
                    sideBySideBarSeriesView1.BarWidth = 0.9;
                AxisLabelPosition labelPos = AxisLabelPosition.Outside;
                foreach (SeriesPoint point in chart.Series[0].Points) {
                    if (point.GetNumericValue(ValueLevel.Value) < 0) {
                        labelPos = AxisLabelPosition.Inside;
                        break;
                    }
                }
                ((XYDiagram)chart.Diagram).AxisX.LabelPosition = labelPos;
            }
        }

        private void CheckPortafoglioContent_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (label.Value != null && label.Text.Trim().ToLower().EndsWith("- n/a"))
                label.Text = label.Text.Trim().Substring(0, label.Text.Trim().Length - 5).Trim();
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

        private void labelAsteriscoOscillazionePatrimoniale_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            bool toHidden = false;
            if (label.Tag != null && double.TryParse(label.Tag.ToString(), out double value))
                toHidden = value == 0;
            label.Visible = !toHidden;
            if (label.Visible && this.contentPatrimonialFluctuation.Value != null && double.TryParse(this.contentPatrimonialFluctuation.Value.ToString(), out double valuePF) && valuePF != 0)
                label.Text = "*";
        }

        private void labelOscillazionePatrimoniale_BeforePrint(object sender, CancelEventArgs e) {
            if (this.contentPatrimonialFluctuation.Value != null && double.TryParse(this.contentPatrimonialFluctuation.Value.ToString(), out double value) && value != 0) {
                string tmpText = ((XRLabel)sender).Text;
                tmpText = tmpText.Replace("{1}", this.contentPatrimonialFluctuation.Text).Replace("{0}", _currencyToApply).Replace("{2}", this.ContentManagementReportPortfolioDate2.Text);
                ((XRLabel)sender).Visible = true;
                ((XRLabel)sender).Text = tmpText;
            } else
                ((XRLabel)sender).Visible = false;
        }

        private void labelESGProfileDettPortafoglio_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel labelESGProfile = (XRLabel)sender;
            if (labelESGProfile.Tag != null && labelESGProfile.Tag.ToString() == labelESGProfile.Text)
                labelESGProfile.StyleName = "labelESGProfileSelected";
            else
                labelESGProfile.StyleName = "labelESGProfile";
        }

        private void contentMultilinePeriodCount_BeforePrint(object sender, CancelEventArgs e) {
            _multilinePeriodCount = int.Parse(((XRLabel)sender).Text);
        }

        private void chartSection3020_BeforePrint(object sender, CancelEventArgs e) {
            XRChart chart = (XRChart)sender;
            chart.Series.Clear();
            chart.DataMember = "section30.content.subSection3020.content";
            chart.SeriesTemplate.ChangeView(ViewType.StackedBar);
            chart.SeriesTemplate.SeriesDataMember = "ElementIndex";
            chart.SeriesTemplate.SetDataMembers("Period", "PercentNetContribution");
            chart.SeriesTemplate.Label.BackColor = Color.Transparent;
            chart.SeriesTemplate.Label.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chart.SeriesTemplate.Label.TextColor = Color.White;
            chart.SeriesTemplate.Label.TextPattern = "{V}%";
            chart.SeriesTemplate.Label.DXFont = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            StackedBarSeriesView view = (StackedBarSeriesView)chart.SeriesTemplate.View;
            if (_multilinePeriodCount < 2)
                view.BarWidth = 0.3;
            else if (_multilinePeriodCount < 3)
                view.BarWidth = 0.6;
            else if (_multilinePeriodCount < 4)
                view.BarWidth = 0.7;
            else if (_multilinePeriodCount < 6)
                view.BarWidth = 0.8;
            else
                view.BarWidth = 0.9;
        }

        private void chartSection3020_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e) {
            XRChart chart = (XRChart)sender;
            int i = e.Series.Points.Select(p => p.Argument).Distinct().Count();
            if (int.TryParse(e.Series.Name, out int currElementIndex) && currElementIndex > 0)
                e.SeriesDrawOptions.Color = chart.PaletteRepository[chart.PaletteName][currElementIndex - 1].Color;
        }

        private void LabelPatrimonioInfChiaveMultilinea_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            label.Text = label.Tag != null ? label.Text.Replace("{0}", label.Tag.ToString()) : label.Text.Replace("{0}", "").Trim();
        }

        private void contentSection3000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            label.StyleName = label.Tag != null && label.Tag.ToString() == CLASS_ITEM_TOTAL ? "gridContentStyleBold" : "gridContentStyle";
        }

        private void contentContributoNettoSection3000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            label.StyleName = label.Tag != null && label.Tag.ToString() == CLASS_ITEM_TOTAL ? "gridContentStyleRightAlignBold" : "gridContentStyleRightAlign";
        }

        private void bookmarkModelloSection3000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (label.Tag != null && label.Tag.ToString() != CLASS_ITEM_TOTAL && int.TryParse(label.Tag.ToString(), out int currElementIndex) && currElementIndex > 0) {
                label.Visible = true;
                label.ForeColor = Color.Transparent;
                label.BackColor = this.chartSection3020.PaletteRepository[this.chartSection3020.PaletteName][currElementIndex - 1].Color;
            } else
                label.Visible = false;
        }

        private void DetailReportSection4000_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
            _currentGridChartPointIndex = 0;
            _currentGridChartPointAlphaCode = string.Empty;
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
                ((XRLine)sender).StyleName = "gridHeaderLine";
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
            ((XRLine)sender).Visible = this.classSection4000.Text != CLASS_ITEM_TOTAL;
        }

        private void bookmarkChartPercInvSection4000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_needResetRow == false && _currentChartPointsCount > 0) {
                label.BackColor = Color.White;
                label.ForeColor = Color.White;
                if (label.Value != null && label.Tag != null && label.Tag.ToString() == CLASS_ITEM_ENTRY && double.TryParse(label.Value.ToString(), out double _)) {
                    label.BackColor = GetChartSeriesPointColorToApply(this.chartSection4010, _currentChartPointsCount, _currentGridChartPointIndex);
                    label.ForeColor = label.BackColor;
                    label.Visible = true;
                    _currentGridChartPointIndex++;
                }
            } else
                label.Visible = false;
        }

        private void bookmarkChartPercAlphaCodeInvSection4000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_needResetRow == false && _currentChartPointsCount > 0) {
                label.Visible = false;
                if (label.Value != null && label.Tag != null && label.Tag.ToString() == CLASS_ITEM_ENTRY && double.TryParse(label.Value.ToString(), out double _)) {
                    _currentGridChartPointAlphaCode = GetNextLabelAlphaCode(_currentGridChartPointAlphaCode);
                    label.Text = "(" + _currentGridChartPointAlphaCode.ToString() + ")";
                    label.Visible = true;
                }
            } else
                label.Visible = false;
        }

        private void ibanSection7000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (string.IsNullOrEmpty(label.Text) == false && label.Text.Trim().Length == 21) {
                string tmp = label.Text.Trim();
                label.Text = tmp.Substring(0, 4) + " " + tmp.Substring(4, 4) + " " + tmp.Substring(8, 4) + " " + tmp.Substring(12, 4) + " " + tmp.Substring(16, 4) + " " + tmp.Substring(tmp.Length - 1);
            }
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

        private void checkSection100CaptionVisibility_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_hasSection100CaptionVisible == false && label.Visible) {
                _hasSection100CaptionVisible = true;
                _section100NeedPageBreakAtTheEnd = true;
            } else
                label.Visible = false;
        }

        private void checkSection110CaptionVisibility_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_hasSection110CaptionVisible == false && label.Visible) {
                _hasSection110CaptionVisible = true;
                _section110NeedPageBreakAtTheEnd = true;
            } else
                label.Visible = false;
        }

        private void checkSection120CaptionVisibility_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_hasSection120CaptionVisible == false && label.Visible) {
                _hasSection120CaptionVisible = true;
                _section120NeedPageBreakAtTheEnd = true;
            } else
                label.Visible = false;
        }

        private void order13000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (string.IsNullOrEmpty(label.Text) == false && label.Text.Length > 3)
                label.Text = label.Text.Substring(3);
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
                if (label.Tag != null && label.Tag.ToString() == CLASS_ITEM_ENTRY && double.TryParse(label.Value.ToString(), out double _)) {
                    label.BackColor = GetChartSeriesPointColorToApply(this.chartSection6010, _currentChartPointsCount, _currentGridChartPointIndex);
                    label.ForeColor = label.BackColor;
                    label.Visible = true;
                    _currentGridChartPointIndex++;
                }
            }
        }

        private void bookmarkChartPercAlphaCodeSharesSection16000_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                label.Visible = false;
                if (label.Tag != null && label.Tag.ToString() != CLASS_ITEM_TOTAL && double.TryParse(label.Value.ToString(), out double _)) {
                    _currentGridChartPointAlphaCode = GetNextLabelAlphaCode(_currentGridChartPointAlphaCode, true);
                    label.Text = "(" + _currentGridChartPointAlphaCode.ToString() + ")";
                    label.Visible = true;
                }
            }
        }

        private void sector16000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (label.Text.Trim() == "<OTHER>") {
                label.Text = _customerLanguage.Trim().ToUpper() switch {
                    "E" => "OTHER",
                    "F" => "AUTRE",
                    "G" => "ANDERE",
                    _ => "ALTRO",
                };
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
                label.BackColor = Color.White;
                label.ForeColor = Color.White;
                if (double.TryParse(label.Value.ToString(), out double value)) {
                    if (label.Tag != null && label.Tag.ToString() != CLASS_ITEM_TOTAL && value > 0) {
                        label.BackColor = GetChartSeriesPointColorToApply(this.chartSection16010, _currentChartPointsCount, _currentGridChartPointIndex, true);
                        label.ForeColor = label.BackColor;
                        label.Visible = true;
                        _currentGridChartPointIndex++;
                    }
                }
            }
        }

        private void percentShares16000_BeforePrint(object sender, CancelEventArgs e) {
            if (this.DetailReportSection160.CurrentRowIndex == _rowCount - 1) {
                this.section16000LineGridUpper.StyleName = "gridHeaderLine";
                this.section16000LineGridUpper.LineWidth = 1;
                this.sector16000.StyleName = "gridContentStyleBold";
                this.marketValue16000.StyleName = "gridContentStyleRightAlignBold";
                this.percentShares16000Fake.StyleName = "gridContentStyleRightAlignBold";
            } else {
                this.section16000LineGridUpper.StyleName = "gridRowLineStyle";
                this.section16000LineGridUpper.LineWidth = 1;
                this.sector16000.StyleName = "gridContentStyle";
                this.marketValue16000.StyleName = "gridContentStyleRightAlign";
                this.percentShares16000Fake.StyleName = "gridContentStyleRightAlign";
            }
            this.section16000LineGridUpper.Visible = this.DetailReportSection160.CurrentRowIndex > 0;
            this.section16000LineGridDown.Visible = this.DetailReportSection160.CurrentRowIndex == _rowCount - 1;
        }

        private void DetailReportSection170_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
            _currentGridChartPointIndex = 0;
            _currentGridChartPointAlphaCode = string.Empty;
        }

        private void DetailSection17000_BeforePrint(object sender, CancelEventArgs e) {
            this.DetailSection17000.HeightF = 0;
            this.DetailSection17000.PerformLayout();
        }

        private void section17000LineGridUpper_BeforePrint(object sender, CancelEventArgs e) {
            XRLine line = (XRLine)sender;
            line.Visible = this.DetailReportSection170.CurrentRowIndex > 0;
            if (line.Visible) {
                if (line.Tag != null && line.Tag.ToString() == CLASS_ITEM_TOTAL)
                    line.StyleName = "gridHeaderLine";
                else
                    line.StyleName = "gridRowLineStyle";
            }
        }

        private void section17000LineGridDown_BeforePrint(object sender, CancelEventArgs e) {
            XRLine line = (XRLine)sender;
            if (line.Tag != null && line.Tag.ToString() == CLASS_ITEM_TOTAL)
                line.StyleName = "gridHeaderLine";
            else
                line.StyleName = "gridRowLineStyle";
        }

        private void bookmarkChartPercSharesSection17000_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                label.BackColor = Color.White;
                label.ForeColor = Color.White;
                if (label.Tag != null && label.Tag.ToString() == CLASS_ITEM_ENTRY && double.TryParse(label.Value.ToString(), out double value)) {
                    if (value > 0) {
                        label.BackColor = GetChartSeriesPointColorToApply(this.chartSection17010, _currentChartPointsCount, _currentGridChartPointIndex);
                        label.ForeColor = label.BackColor;
                        label.Visible = true;
                        _currentGridChartPointIndex++;
                    }
                }
            }
        }

        private void bookmarkChartPercAlphaCodeSharesSection17000_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                label.Visible = false;
                if (label.Tag != null && label.Tag.ToString() == CLASS_ITEM_ENTRY && double.TryParse(label.Value.ToString(), out double value)) {
                    if (value > 0) {
                        _currentGridChartPointAlphaCode = GetNextLabelAlphaCode(_currentGridChartPointAlphaCode);
                        label.Text = "(" + _currentGridChartPointAlphaCode.ToString() + ")";
                        label.Visible = true;
                    }
                }
            }
        }

        private void DetailSubSection17000_BeforePrint(object sender, CancelEventArgs e) {
            this.DetailSubSection17000.HeightF = 0;
            this.DetailSubSection17000.PerformLayout();
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

        private void contentNotTransferedCount_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            _hasNotTransfered = string.IsNullOrEmpty(label.Text) == false && int.TryParse(label.Text, out int count) && count > 0;
        }

        private void panelSection19010HeaderContainer_BeforePrint(object sender, CancelEventArgs e) {
            if (!_hasNotTransfered) {
                ((XRPanel)sender).LocationF = new PointF(0, ((XRPanel)sender).LocationF.Y);
            }
        }

        private void xrTableSection19010Objects_BeforePrint(object sender, CancelEventArgs e) {
            if (!_hasNotTransfered) {
                ((XRTable)sender).LocationF = new PointF(0, ((XRTable)sender).LocationF.Y);
            }
        }

        private void panelSection19010FooterContainer_BeforePrint(object sender, CancelEventArgs e) {
            if (!_hasNotTransfered) {
                ((XRPanel)sender).LocationF = new PointF(0, ((XRPanel)sender).LocationF.Y);
            }
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
                if (this.DetailReportSubSection19010.RowCount == 1) {
                    this.xrTableSection19010Objects.Rows[index].Cells[0].Borders = DevExpress.XtraPrinting.BorderSide.None;
                    this.xrTableSection19010Objects.Rows[index].Cells[1].Borders = DevExpress.XtraPrinting.BorderSide.None;
                    this.xrTableSection19010Objects.Rows[index].Cells[2].Borders = DevExpress.XtraPrinting.BorderSide.None;
                    this.xrTableSection19010Objects.Rows[index].Cells[3].Borders = DevExpress.XtraPrinting.BorderSide.None;
                    this.xrTableSection19010Objects.Rows[index].Cells[4].Borders = DevExpress.XtraPrinting.BorderSide.None;
                    this.xrTableSection19010Objects.Rows[index].Cells[5].Borders = DevExpress.XtraPrinting.BorderSide.None;
                }
            }
        }

        private void DetailReportSubSection200_BeforePrint(object sender, CancelEventArgs e) {
            _rowCount = ((DetailReportBand)sender).RowCount;
            _currentGridChartPointIndex = 0;
            _currentGridChartPointAlphaCode = string.Empty;
        }

        private void bookmarkChartPercInvSection20020_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                label.BackColor = Color.White;
                label.ForeColor = Color.White;
                if (label.Tag != null && label.Tag.ToString() == CLASS_ITEM_ENTRY && double.TryParse(label.Value.ToString(), out double _)) {
                    label.BackColor = GetChartSeriesPointColorToApply(this.chartSection20010, _currentChartPointsCount, _currentGridChartPointIndex);
                    label.ForeColor = label.BackColor;
                    label.Visible = true;
                    _currentGridChartPointIndex++;
                }
            }
        }

        private void bookmarkChartPercAlphaCodeInvSection20020_BeforePrint(object sender, CancelEventArgs e) {
            if (_currentChartPointsCount > 0) {
                XRLabel label = (XRLabel)sender;
                label.Visible = false;
                if (label.Tag != null && label.Tag.ToString() == CLASS_ITEM_ENTRY && double.TryParse(label.Value.ToString(), out double _)) {
                    _currentGridChartPointAlphaCode = GetNextLabelAlphaCode(_currentGridChartPointAlphaCode);
                    label.Text = "(" + _currentGridChartPointAlphaCode.ToString() + ")";
                    label.Visible = true;
                }
            }
        }

        private void labelESGProfileKeyInformation_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel labelESGProfile = (XRLabel)sender;
            if (labelESGProfile.Tag != null && labelESGProfile.Tag.ToString() == labelESGProfile.Text)
                labelESGProfile.StyleName = "labelESGProfileSelected";
            else
                labelESGProfile.StyleName = "labelESGProfile";
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
