using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace PSE.Reporting.Reports {

    public partial class ReportPSE : DevExpress.XtraReports.UI.XtraReport {

        private string _currentAssetClassSection4000;
        private bool _hasSection80CaptionVisible;
        private bool _needResetRow;

        public ReportPSE() {
            InitializeComponent();            
            _currentAssetClassSection4000 = string.Empty;
            _hasSection80CaptionVisible = false;
            _needResetRow = false;
        }

        private void hiddenIfZero_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            bool toHidden = false;
            if (string.IsNullOrEmpty(label.Text) == false && double.TryParse(label.Text.Replace("%",""), out double value))
                toHidden = value == 0;
            label.Visible = !toHidden;
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

        private void assetClassSection4000_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel currLabel = (XRLabel)sender;
            if (string.IsNullOrEmpty(_currentAssetClassSection4000) || currLabel.Text != _currentAssetClassSection4000) {               
                _currentAssetClassSection4000 = currLabel.Text;
                _needResetRow = false;
            } else if (currLabel.Text == _currentAssetClassSection4000) {
                currLabel.Text = null;
                _needResetRow = true;
            }
        }

        private void marketValueReportingCurrencyTSection4000_BeforePrint(object sender, CancelEventArgs e) {
            if(_needResetRow) {
                XRLabel currLabel = (XRLabel)sender;
                currLabel.Text = "";
            }            
        }

        private void percentInvestmentTSection4000_BeforePrint(object sender, CancelEventArgs e) {
            if (_needResetRow) {
                XRLabel currLabel = (XRLabel)sender;
                currLabel.Text = "";
            }
        }
        private void DetailReport4000_BeforePrint(object sender, CancelEventArgs e) {
            this.DetailReport4000.HeightF = 0;
            this.PerformLayout();
        }

        private void checkSection80CaptionVisibility_BeforePrint(object sender, CancelEventArgs e) {
            XRLabel label = (XRLabel)sender;
            if (_hasSection80CaptionVisible == false && label.Visible)
                _hasSection80CaptionVisible = true;
            else
                label.Visible = false;
        }       

        private void divisaSection6000_BeforePrint(object sender, CancelEventArgs e) {
            if (((XRLabel)sender).Text.Length > 3) { // it is not a currency code...
                ((XRLabel)sender).StyleName = "gridContentStyleBold";
                this.valoreMercatoSection6000.StyleName = "gridContentStyleRightAlignBold";
                this.section6000LineGrid.Visible = false;
            }
        }      
       
    }

}
