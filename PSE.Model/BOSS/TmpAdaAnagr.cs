using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PSE.Model.BOSS {

    [Table("TmpADAANAGR")]
    [Keyless]
    public class TmpAdaAnagr {

        [Column("ANNUMIDE")]
        public string? AnNumIde { get; set; }

        [Column("ANRISKPR")]
        public string? AnRiskPr { get; set; }

    }

}
