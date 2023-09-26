using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSE.Decoder.Database.Model
{

    [Table("TmpTABELLE")]
    [Keyless]
    internal class TmpTabelle
    {

        [Column("CMD")]
        public string? Cmd { get; set; }

        [Column("TAB")]
        public string? Tab { get; set; }

        [Column("CODE")]
        public string? Code { get; set; }

        [Column("TEXT_I")]
        public string? TextI { get; set; }

        [Column("TEXT_T")]
        public string? TextT { get; set; }

        [Column("TEXT_F")]
        public string? TextF { get; set; }

        [Column("TEXT_E")]
        public string? TextE { get; set; }

        [Column("COL_6")]
        public string? Col6 { get; set; }

        [Column("COL_7")]
        public string? Col7 { get; set; }

        [Column("COL_8")]
        public string? Col8 { get; set; }

        [Column("COL_9")]
        public string? Col9 { get; set; }

        [Column("COL_10")]
        public string? Col10 { get; set; }

        [Column("COL_11")]
        public string? Col11 { get; set; }

        [Column("COL_12")]
        public string? Col12 { get; set; }

        [Column("COL_13")]
        public string? Col13 { get; set; }

        [Column("COL_14")]
        public string? Col14 { get; set; }

        [Column("COL_15")]
        public string? Col15 { get; set; }

    }

}
