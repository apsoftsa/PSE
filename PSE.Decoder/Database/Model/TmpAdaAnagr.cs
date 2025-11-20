using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PSE.Decoder.Database.Model {

    [Table("TmpADAANAGR")]
    [Keyless]
    internal class TmpAdaAnagr {

        [Column("ANNUMIDE")]
        public string? AnNumIde { get; set; }

        [Column("ANRISKPR")]
        public string? AnRiskPr { get; set; }

    }

}
