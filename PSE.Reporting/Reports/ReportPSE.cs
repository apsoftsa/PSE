﻿using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace PSE.Reporting.Reports {

    public partial class ReportPSE : XtraReport {

        private string _currencyToApply;
        private string _currentAssetClassSection4000;
        private bool _hasSection80CaptionVisible;
        private bool _hasSection90CaptionVisible;
        private bool _section80NeedPageBreakAtTheEnd;
        private bool _section90NeedPageBreakAtTheEnd;
        private bool _needResetRow;
        int _rowCount;

        private void ManageSection80VisibilityFlags() {
            if (_section80NeedPageBreakAtTheEnd)
                _section80NeedPageBreakAtTheEnd = false;
        }

        private void ManageSection90VisibilityFlags() {
            if (_section90NeedPageBreakAtTheEnd)
                _section90NeedPageBreakAtTheEnd = false;
        }

        public ReportPSE() {
            InitializeComponent();
            _currencyToApply = "?";
            _currentAssetClassSection4000 = string.Empty;
            _hasSection80CaptionVisible = false;
            _hasSection90CaptionVisible = false;    
            _section80NeedPageBreakAtTheEnd = false;
            _section90NeedPageBreakAtTheEnd = false;
            _needResetRow = false;
            _rowCount = 0;
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

        private void pageBreakBeforeReportHeader_BeforePrint(object sender, CancelEventArgs e) {
            if (_section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd) {
                ((ReportHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeader_BeforePrint(object sender, CancelEventArgs e) {
            if (_section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeaderSection80_BeforePrint(object sender, CancelEventArgs e) {
            if (_section80NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection80VisibilityFlags();
            }
        }

        private void pageBreakBeforeGroupHeaderSection90_BeforePrint(object sender, CancelEventArgs e) {
            if (_section90NeedPageBreakAtTheEnd) {
                ((GroupHeaderBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection90VisibilityFlags();
            }
        }

        private void pageBreakBeforeBand_BeforePrint(object sender, CancelEventArgs e) {
            if (_section80NeedPageBreakAtTheEnd || _section90NeedPageBreakAtTheEnd) {
                ((DetailReportBand)sender).PageBreak = PageBreak.BeforeBand;
                ManageSection80VisibilityFlags();
                ManageSection90VisibilityFlags();
            }
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

        private void ContentReportHeaderCliente_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = string.Concat(this.LabelReportHeaderCliente.Text, " ", ((XRLabel)sender).Text);
        }

        private void ContentReportHeaderNumeroCliente_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = string.Concat(this.LabelReportHeaderNumeroCliente.Text, " ", ((XRLabel)sender).Text);
        }

        private void ContentReportHeaderStatoAl_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = string.Concat(this.LabelReportHeaderStatoAl.Text, " ", ((XRLabel)sender).Text);
        }

        private void ContentReportHeaderPortafoglio_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = string.Concat(this.LabelReportHeaderPortafoglio.Text, " ", ((XRLabel)sender).Text);
        }

        private void ContentReportHeaderConsulente_BeforePrint(object sender, CancelEventArgs e) {
            ((XRLabel)sender).Text = string.Concat(this.LabelReportHeaderConsulente.Text, " ", ((XRLabel)sender).Text);
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
        }

        private void DetailReport16000_BeforePrint(object sender, CancelEventArgs e) {
            this.DetailReport16000.HeightF = 0;
            this.PerformLayout();
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
        
    }

}
