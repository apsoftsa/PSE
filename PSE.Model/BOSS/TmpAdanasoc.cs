using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PSE.Model.BOSS {

    [Keyless]
    [Table("TmpADANASOC")]
    public class TmpAdanasoc {

        [Column("CODE")]
        public string? Code { get; set; }

        [Column("NUMSOCSOC")]
        public string? NumSocSoc { get; set; }

        [Column("CODNAZSOC")]
        public string? CodNazSoc { get; set; }

        [Column("STATUSSOC")]
        public string? StatusSoc { get; set; }

        [Column("SETATTSOC")]
        public string? SetAttSoc { get; set; }

        [Column("NUMOPESOC")]
        public string? NumOpeSoc { get; set; }

        [Column("DATAPESOC")]
        public DateTime? DatApeSoc { get; set; }

        [Column("DATMUTSOC")]
        public DateTime? DatMutSoc { get; set; }

        [Column("DATESTSOC")]
        public DateTime? DatEstSoc { get; set; }

        [Column("TESABBSOC")]
        public string? TesAbbSoc { get; set; }

        [Column("TESIN1SOC")]
        public string? TesIn1Soc { get; set; }

        [Column("TESIN2SOC")]
        public string? TesIn2Soc { get; set; }

        [Column("TESIN3SOC")]
        public string? TesIn3Soc { get; set; }

        [Column("TESIN4SOC")]
        public string? TesIn4Soc { get; set; }

        [Column("GRUPPOSOC")]
        public string? GruppoSoc { get; set; }

        [Column("NOTE01SOC")]
        public string? Note01Soc { get; set; }

        [Column("NOTE02SOC")]
        public string? Note02Soc { get; set; }

        [Column("NOTE03SOC")]
        public string? Note03Soc { get; set; }

        [Column("CANTONESOC")]
        public string? CantoneSoc { get; set; }

        [Column("INDIR1SOC")]
        public string? Indir1Soc { get; set; }

        [Column("INDIR2SOC")]
        public string? Indir2Soc { get; set; }

        [Column("INDIR3SOC")]
        public string? Indir3Soc { get; set; }

        [Column("INDIR4SOC")]
        public string? Indir4Soc { get; set; }

        [Column("INDIR5SOC")]
        public string? Indir5Soc { get; set; }

        [Column("INDABBSOC")]
        public string? IndAbbSoc { get; set; }

        [Column("NUMPARSOC")]
        public string? NumParSoc { get; set; }

        [Column("SWEMITSOC")]
        public string? SwEmitSoc { get; set; }

        [Column("NUMANASOC")]
        public string? NumAnaSoc { get; set; }

        [Column("DATVERLEGSOC")]
        public DateTime? DatVerLegSoc { get; set; }

        [Column("NUMTLKSOC")]
        public string? NumTlkSoc { get; set; }

    }

}