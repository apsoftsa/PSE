namespace PSE.Model.Fam {

    public class PseReportRequest {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> NumIdeList { get; set; }

        public PseReportRequest(List<string>? numIdeList) {
            StartDate = new DateTime(DateTime.Today.Year, 1, 1, 0, 0, 0);
            EndDate = DateTime.Now;
            NumIdeList = numIdeList != null && numIdeList.Count > 0 ? new List<string>(numIdeList) : new List<string>();    
        }   

    }

    public class PseReportDataInfoRelazione {
        public int Numide { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Moneta { get; set; } = string.Empty;
        public decimal Performance { get; set; }
        public decimal Portafoglio { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class PseReportDataContributoPerformance {
        public Guid Id { get; set; }
        public string NomeModello { get; set; } = string.Empty;
        public string DivisaModello { get; set; } = string.Empty;
        public decimal ContribuzioneNetta { get; set; }
    }

    public class PseReportDataComposizioneMultilinea {
        public DateTime StartDate { get; set; }
        public List<PseReportDataPesoMultilinea> Composizione { get; set; } = [];
    }

    public class PseReportDataPesoMultilinea {
        public Guid Id { get; set; }
        public decimal Peso { get; set; }
    }

    public class PseReportData {
        public PseReportDataInfoRelazione InfoRelazione { get; set; } = new PseReportDataInfoRelazione();
        public List<PseReportDataContributoPerformance> ContributiPerformance { get; set; } = [];
        public List<PseReportDataComposizioneMultilinea> ComposizioniMultilinea { get; set; } = [];

    }

    public class PseReportResult {
        public List<PseReportData> ReportList { get; set; } = [];
    }
   

}
