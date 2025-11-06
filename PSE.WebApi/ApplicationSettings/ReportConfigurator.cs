using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.Json;

namespace PSE.WebApi.ApplicationSettings {

    internal static class ReportConfigurator {

        private static IEnumerable<T> AllControls<T>(this XRControl root) where T : XRControl {
            foreach (XRControl c in root.Controls) {
                if (c is T t)
                    yield return t;
                foreach (var nested in AllControls<T>(c))
                    yield return nested;
            }
        }

        public static void FixJsonConnections(XtraReport report, string jsonPayload, string? rootElement = null) {
            foreach (var jds in report.ComponentStorage.OfType<JsonDataSource>()) {
                jds.ConnectionName = null;                            
                jds.JsonSource = new CustomJsonSource(jsonPayload);
                if (!string.IsNullOrWhiteSpace(rootElement))
                    jds.RootElement = rootElement;
                jds.Fill();
            }
            var main = new JsonDataSource { JsonSource = new CustomJsonSource(jsonPayload) };
            if (!string.IsNullOrWhiteSpace(rootElement))
                main.RootElement = rootElement;
            main.Fill();
            report.DataSource = main;
            foreach (var sub in EnumerateSubreports(report)) {
                if (sub.ReportSource is XtraReport sr) {
                    sr.DataSource = main;
                    sr.RequestParameters = false;
                    foreach (var jds in sr.ComponentStorage.OfType<JsonDataSource>()) {
                        jds.ConnectionName = null;
                        jds.JsonSource = new CustomJsonSource(jsonPayload);
                        if (!string.IsNullOrWhiteSpace(rootElement))
                            jds.RootElement = rootElement;
                        jds.Fill();
                    }
                }
                sub.BeforePrint += (_, __) => {
                    if (sub.ReportSource is XtraReport sr2) {
                        sr2.DataSource = main;
                        foreach (var jds in sr2.ComponentStorage.OfType<JsonDataSource>()) {
                            jds.ConnectionName = null;
                            jds.JsonSource = new CustomJsonSource(jsonPayload);
                            if (!string.IsNullOrWhiteSpace(rootElement))
                                jds.RootElement = rootElement;
                            jds.Fill();
                        }
                    }
                };
            }
            foreach (var chart in report.AllControls<XRChart>()) {
                try {
                    chart.DataSource = report.DataSource; // evita che puntino a un jds di design
                } catch {
                    chart.DataSource = null;
                    chart.Visible = false;  
                }
            }
            report.RequestParameters = false;
        }

        private static IEnumerable<XRSubreport> EnumerateSubreports(XtraReport report) {
            foreach (var band in report.Bands.Cast<Band>()) {
                foreach (var sub in EnumerateSubreports(band))
                    yield return sub;
            }
        }

        private static IEnumerable<XRSubreport> EnumerateSubreports(Band band) {
            foreach (XRControl ctrl in band.Controls) {
                if (ctrl is XRSubreport s)
                    yield return s;
                foreach (var nested in EnumerateSubreports(ctrl))
                    yield return nested;
            }
        }

        private static IEnumerable<XRSubreport> EnumerateSubreports(XRControl root) {
            foreach (XRControl c in root.Controls) {
                if (c is XRSubreport s)
                    yield return s;
                foreach (var nested in EnumerateSubreports(c))
                    yield return nested;
            }
        }

    }

}


