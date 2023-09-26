using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSE.Decoder.Database.Model
{

    [Table("TmpADAAUIDE")]
    [Keyless]
    internal class TmpAdaAuIde
    {

        [Column("CODE")]
        public string? Code { get; set; }

        [Column("AUITIPIDE")]
        public string? AuiTipIde { get; set; }

        [Column("AUINUMPER")]
        public string? AuiNumPer { get; set; }

        [Column("AUINOME")]
        public string? AuiNome { get; set; }

        [Column("AUILOCALI")]
        public string? AuiLocali { get; set; }

        [Column("AUICODUFF")]
        public string? AuiCodUff { get; set; }

        [Column("AUIGRADO")]
        public string? AuiGrado { get; set; }

        [Column("AUIDATENT")]
        public DateTime? AuiDatEnt { get; set; }

        [Column("AUIDATUSC")]
        public DateTime? AuiDatUsc { get; set; }

        [Column("AUICUSRTYP")]
        public string? AuiCusrTyp { get; set; }

        [Column("AUIGESTOLD")]
        public string? AuiGestOld { get; set; }

        [Column("AUIFIDPRO")]
        public string? AuiFidPro { get; set; }

        [Column("AUIFIDCON")]
        public string? AuiFidCon { get; set; }

        [Column("AUINOMLOG")]
        public string? AuiNomLog { get; set; }

        [Column("AUITIPCOM")]
        public string? AuiTipCom { get; set; }

        [Column("AUICENRES")]
        public string? AuiCenRes { get; set; }

        [Column("AUIOSSERVAZIONE1")]
        public string? AuiOsservazione1 { get; set; }

    }

}
