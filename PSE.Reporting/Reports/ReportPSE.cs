using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace PSE.Reporting.Reports {

    public partial class ReportPSE : DevExpress.XtraReports.UI.XtraReport {

        public ReportPSE() {
            InitializeComponent();            
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

        private void divisaSection6000_BeforePrint(object sender, CancelEventArgs e) {
            if (((XRLabel)sender).Text.Length > 3) { // it is not a currency code...
                ((XRLabel)sender).StyleName = "gridContentStyleBold";
                this.valoreMercatoSection6000.StyleName = "gridContentStyleRightAlignBold";
                this.section6000LineGrid.Visible = false;
            }
        }
      
    }

}
