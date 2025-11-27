using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PSE.Model.BOSS;

[Keyless]
[Table("TmpADORDLAT")]
public class TmpAdordlat {
    [Column("CODE")] public string? Code { get; set; }
    [Column("ISTAPPLAT")] public string? IstaPplat { get; set; }
    [Column("ORDPAGLAT")] public string? OrdPagLat { get; set; }
    [Column("NUMORDLAT")] public string? NumOrdLat { get; set; }
    [Column("CODESTLAT")] public string? CodEstLat { get; set; }
    [Column("OPEATTLAT")] public string? OpeAttLat { get; set; }

    // ------------------------
    // TIPIMPLAT (1–30)
    // ------------------------
    [Column("TIPIMPLAT1")] public string? TipImpLat1 { get; set; }
    [Column("TIPIMPLAT2")] public string? TipImpLat2 { get; set; }
    [Column("TIPIMPLAT3")] public string? TipImpLat3 { get; set; }
    [Column("TIPIMPLAT4")] public string? TipImpLat4 { get; set; }
    [Column("TIPIMPLAT5")] public string? TipImpLat5 { get; set; }
    [Column("TIPIMPLAT6")] public string? TipImpLat6 { get; set; }
    [Column("TIPIMPLAT7")] public string? TipImpLat7 { get; set; }
    [Column("TIPIMPLAT8")] public string? TipImpLat8 { get; set; }
    [Column("TIPIMPLAT9")] public string? TipImpLat9 { get; set; }
    [Column("TIPIMPLAT10")] public string? TipImpLat10 { get; set; }
    [Column("TIPIMPLAT11")] public string? TipImpLat11 { get; set; }
    [Column("TIPIMPLAT12")] public string? TipImpLat12 { get; set; }
    [Column("TIPIMPLAT13")] public string? TipImpLat13 { get; set; }
    [Column("TIPIMPLAT14")] public string? TipImpLat14 { get; set; }
    [Column("TIPIMPLAT15")] public string? TipImpLat15 { get; set; }
    [Column("TIPIMPLAT16")] public string? TipImpLat16 { get; set; }
    [Column("TIPIMPLAT17")] public string? TipImpLat17 { get; set; }
    [Column("TIPIMPLAT18")] public string? TipImpLat18 { get; set; }
    [Column("TIPIMPLAT19")] public string? TipImpLat19 { get; set; }
    [Column("TIPIMPLAT20")] public string? TipImpLat20 { get; set; }
    [Column("TIPIMPLAT21")] public string? TipImpLat21 { get; set; }
    [Column("TIPIMPLAT22")] public string? TipImpLat22 { get; set; }
    [Column("TIPIMPLAT23")] public string? TipImpLat23 { get; set; }
    [Column("TIPIMPLAT24")] public string? TipImpLat24 { get; set; }
    [Column("TIPIMPLAT25")] public string? TipImpLat25 { get; set; }
    [Column("TIPIMPLAT26")] public string? TipImpLat26 { get; set; }
    [Column("TIPIMPLAT27")] public string? TipImpLat27 { get; set; }
    [Column("TIPIMPLAT28")] public string? TipImpLat28 { get; set; }
    [Column("TIPIMPLAT29")] public string? TipImpLat29 { get; set; }
    [Column("TIPIMPLAT30")] public string? TipImpLat30 { get; set; }

    // ------------------------
    // UNIIMPLAT (1–30)
    // ------------------------
    [Column("UNIIMPLAT1")] public string? UniImpLat1 { get; set; }
    [Column("UNIIMPLAT2")] public string? UniImpLat2 { get; set; }
    [Column("UNIIMPLAT3")] public string? UniImpLat3 { get; set; }
    [Column("UNIIMPLAT4")] public string? UniImpLat4 { get; set; }
    [Column("UNIIMPLAT5")] public string? UniImpLat5 { get; set; }
    [Column("UNIIMPLAT6")] public string? UniImpLat6 { get; set; }
    [Column("UNIIMPLAT7")] public string? UniImpLat7 { get; set; }
    [Column("UNIIMPLAT8")] public string? UniImpLat8 { get; set; }
    [Column("UNIIMPLAT9")] public string? UniImpLat9 { get; set; }
    [Column("UNIIMPLAT10")] public string? UniImpLat10 { get; set; }
    [Column("UNIIMPLAT11")] public string? UniImpLat11 { get; set; }
    [Column("UNIIMPLAT12")] public string? UniImpLat12 { get; set; }
    [Column("UNIIMPLAT13")] public string? UniImpLat13 { get; set; }
    [Column("UNIIMPLAT14")] public string? UniImpLat14 { get; set; }
    [Column("UNIIMPLAT15")] public string? UniImpLat15 { get; set; }
    [Column("UNIIMPLAT16")] public string? UniImpLat16 { get; set; }
    [Column("UNIIMPLAT17")] public string? UniImpLat17 { get; set; }
    [Column("UNIIMPLAT18")] public string? UniImpLat18 { get; set; }
    [Column("UNIIMPLAT19")] public string? UniImpLat19 { get; set; }
    [Column("UNIIMPLAT20")] public string? UniImpLat20 { get; set; }
    [Column("UNIIMPLAT21")] public string? UniImpLat21 { get; set; }
    [Column("UNIIMPLAT22")] public string? UniImpLat22 { get; set; }
    [Column("UNIIMPLAT23")] public string? UniImpLat23 { get; set; }
    [Column("UNIIMPLAT24")] public string? UniImpLat24 { get; set; }
    [Column("UNIIMPLAT25")] public string? UniImpLat25 { get; set; }
    [Column("UNIIMPLAT26")] public string? UniImpLat26 { get; set; }
    [Column("UNIIMPLAT27")] public string? UniImpLat27 { get; set; }
    [Column("UNIIMPLAT28")] public string? UniImpLat28 { get; set; }
    [Column("UNIIMPLAT29")] public string? UniImpLat29 { get; set; }
    [Column("UNIIMPLAT30")] public string? UniImpLat30 { get; set; }

    // ------------------------
    // IMPIMPLAT (1–30)
    // ------------------------
    [Column("IMPIMPLAT1")] public double? ImpImpLat1 { get; set; }
    [Column("IMPIMPLAT2")] public double? ImpImpLat2 { get; set; }
    [Column("IMPIMPLAT3")] public double? ImpImpLat3 { get; set; }
    [Column("IMPIMPLAT4")] public double? ImpImpLat4 { get; set; }
    [Column("IMPIMPLAT5")] public double? ImpImpLat5 { get; set; }
    [Column("IMPIMPLAT6")] public double? ImpImpLat6 { get; set; }
    [Column("IMPIMPLAT7")] public double? ImpImpLat7 { get; set; }
    [Column("IMPIMPLAT8")] public double? ImpImpLat8 { get; set; }
    [Column("IMPIMPLAT9")] public double? ImpImpLat9 { get; set; }
    [Column("IMPIMPLAT10")] public double? ImpImpLat10 { get; set; }
    [Column("IMPIMPLAT11")] public double? ImpImpLat11 { get; set; }
    [Column("IMPIMPLAT12")] public double? ImpImpLat12 { get; set; }
    [Column("IMPIMPLAT13")] public double? ImpImpLat13 { get; set; }
    [Column("IMPIMPLAT14")] public double? ImpImpLat14 { get; set; }
    [Column("IMPIMPLAT15")] public double? ImpImpLat15 { get; set; }
    [Column("IMPIMPLAT16")] public double? ImpImpLat16 { get; set; }
    [Column("IMPIMPLAT17")] public double? ImpImpLat17 { get; set; }
    [Column("IMPIMPLAT18")] public double? ImpImpLat18 { get; set; }
    [Column("IMPIMPLAT19")] public double? ImpImpLat19 { get; set; }
    [Column("IMPIMPLAT20")] public double? ImpImpLat20 { get; set; }
    [Column("IMPIMPLAT21")] public double? ImpImpLat21 { get; set; }
    [Column("IMPIMPLAT22")] public double? ImpImpLat22 { get; set; }
    [Column("IMPIMPLAT23")] public double? ImpImpLat23 { get; set; }
    [Column("IMPIMPLAT24")] public double? ImpImpLat24 { get; set; }
    [Column("IMPIMPLAT25")] public double? ImpImpLat25 { get; set; }
    [Column("IMPIMPLAT26")] public double? ImpImpLat26 { get; set; }
    [Column("IMPIMPLAT27")] public double? ImpImpLat27 { get; set; }
    [Column("IMPIMPLAT28")] public double? ImpImpLat28 { get; set; }
    [Column("IMPIMPLAT29")] public double? ImpImpLat29 { get; set; }
    [Column("IMPIMPLAT30")] public double? ImpImpLat30 { get; set; }

    // ------------------------
    // VIMIMPLAT (1–30)
    // ------------------------
    [Column("VIMIMPLAT1")] public string? VimImpLat1 { get; set; }
    [Column("VIMIMPLAT2")] public string? VimImpLat2 { get; set; }
    [Column("VIMIMPLAT3")] public string? VimImpLat3 { get; set; }
    [Column("VIMIMPLAT4")] public string? VimImpLat4 { get; set; }
    [Column("VIMIMPLAT5")] public string? VimImpLat5 { get; set; }
    [Column("VIMIMPLAT6")] public string? VimImpLat6 { get; set; }
    [Column("VIMIMPLAT7")] public string? VimImpLat7 { get; set; }
    [Column("VIMIMPLAT8")] public string? VimImpLat8 { get; set; }
    [Column("VIMIMPLAT9")] public string? VimImpLat9 { get; set; }
    [Column("VIMIMPLAT10")] public string? VimImpLat10 { get; set; }
    [Column("VIMIMPLAT11")] public string? VimImpLat11 { get; set; }
    [Column("VIMIMPLAT12")] public string? VimImpLat12 { get; set; }
    [Column("VIMIMPLAT13")] public string? VimImpLat13 { get; set; }
    [Column("VIMIMPLAT14")] public string? VimImpLat14 { get; set; }
    [Column("VIMIMPLAT15")] public string? VimImpLat15 { get; set; }
    [Column("VIMIMPLAT16")] public string? VimImpLat16 { get; set; }
    [Column("VIMIMPLAT17")] public string? VimImpLat17 { get; set; }
    [Column("VIMIMPLAT18")] public string? VimImpLat18 { get; set; }
    [Column("VIMIMPLAT19")] public string? VimImpLat19 { get; set; }
    [Column("VIMIMPLAT20")] public string? VimImpLat20 { get; set; }
    [Column("VIMIMPLAT21")] public string? VimImpLat21 { get; set; }
    [Column("VIMIMPLAT22")] public string? VimImpLat22 { get; set; }
    [Column("VIMIMPLAT23")] public string? VimImpLat23 { get; set; }
    [Column("VIMIMPLAT24")] public string? VimImpLat24 { get; set; }
    [Column("VIMIMPLAT25")] public string? VimImpLat25 { get; set; }
    [Column("VIMIMPLAT26")] public string? VimImpLat26 { get; set; }
    [Column("VIMIMPLAT27")] public string? VimImpLat27 { get; set; }
    [Column("VIMIMPLAT28")] public string? VimImpLat28 { get; set; }
    [Column("VIMIMPLAT29")] public string? VimImpLat29 { get; set; }
    [Column("VIMIMPLAT30")] public string? VimImpLat30 { get; set; }

    // ------------------------
    // UNILAT / CTV / TIPUNI / VALUNI (1–5)
    // ------------------------
    [Column("CAUUNILAT1")] public string? CauUniLat1 { get; set; }
    [Column("CAUUNILAT2")] public string? CauUniLat2 { get; set; }
    [Column("CAUUNILAT3")] public string? CauUniLat3 { get; set; }
    [Column("CAUUNILAT4")] public string? CauUniLat4 { get; set; }
    [Column("CAUUNILAT5")] public string? CauUniLat5 { get; set; }

    [Column("UNIUNILAT1")] public string? UniUniLat1 { get; set; }
    [Column("UNIUNILAT2")] public string? UniUniLat2 { get; set; }
    [Column("UNIUNILAT3")] public string? UniUniLat3 { get; set; }
    [Column("UNIUNILAT4")] public string? UniUniLat4 { get; set; }
    [Column("UNIUNILAT5")] public string? UniUniLat5 { get; set; }

    [Column("CTVUNILAT1")] public string? CtvUniLat1 { get; set; }
    [Column("CTVUNILAT2")] public string? CtvUniLat2 { get; set; }
    [Column("CTVUNILAT3")] public string? CtvUniLat3 { get; set; }
    [Column("CTVUNILAT4")] public string? CtvUniLat4 { get; set; }
    [Column("CTVUNILAT5")] public string? CtvUniLat5 { get; set; }

    [Column("TIPUNILAT1")] public string? TipUniLat1 { get; set; }
    [Column("TIPUNILAT2")] public string? TipUniLat2 { get; set; }
    [Column("TIPUNILAT3")] public string? TipUniLat3 { get; set; }
    [Column("TIPUNILAT4")] public string? TipUniLat4 { get; set; }
    [Column("TIPUNILAT5")] public string? TipUniLat5 { get; set; }

    [Column("VALUNILAT1")] public double? ValUniLat1 { get; set; }
    [Column("VALUNILAT2")] public double? ValUniLat2 { get; set; }
    [Column("VALUNILAT3")] public double? ValUniLat3 { get; set; }
    [Column("VALUNILAT4")] public double? ValUniLat4 { get; set; }
    [Column("VALUNILAT5")] public double? ValUniLat5 { get; set; }

    // ------------------------
    // TIPCOMLAT / MEZCOMLAT / RIFCOMLAT (1–5)
    // ------------------------
    [Column("TIPCOMLAT1")] public string? TipComLat1 { get; set; }
    [Column("TIPCOMLAT2")] public string? TipComLat2 { get; set; }
    [Column("TIPCOMLAT3")] public string? TipComLat3 { get; set; }
    [Column("TIPCOMLAT4")] public string? TipComLat4 { get; set; }
    [Column("TIPCOMLAT5")] public string? TipComLat5 { get; set; }

    [Column("MEZCOMLAT1")] public string? MezComLat1 { get; set; }
    [Column("MEZCOMLAT2")] public string? MezComLat2 { get; set; }
    [Column("MEZCOMLAT3")] public string? MezComLat3 { get; set; }
    [Column("MEZCOMLAT4")] public string? MezComLat4 { get; set; }
    [Column("MEZCOMLAT5")] public string? MezComLat5 { get; set; }

    [Column("RIFCOMLAT1")] public string? RifComLat1 { get; set; }
    [Column("RIFCOMLAT2")] public string? RifComLat2 { get; set; }
    [Column("RIFCOMLAT3")] public string? RifComLat3 { get; set; }
    [Column("RIFCOMLAT4")] public string? RifComLat4 { get; set; }
    [Column("RIFCOMLAT5")] public string? RifComLat5 { get; set; }

    // ------------------------
    // LEG ANALAT / IDIMPA / CAURAP / LEGRAP / IDIMPR (1–5)
    // ------------------------
    [Column("LEGANALAT1")] public string? LegAnalAt1 { get; set; }
    [Column("LEGANALAT2")] public string? LegAnalAt2 { get; set; }
    [Column("LEGANALAT3")] public string? LegAnalAt3 { get; set; }
    [Column("LEGANALAT4")] public string? LegAnalAt4 { get; set; }
    [Column("LEGANALAT5")] public string? LegAnalAt5 { get; set; }

    [Column("IDIMPALAT1")] public string? IdImpaLat1 { get; set; }
    [Column("IDIMPALAT2")] public string? IdImpaLat2 { get; set; }
    [Column("IDIMPALAT3")] public string? IdImpaLat3 { get; set; }
    [Column("IDIMPALAT4")] public string? IdImpaLat4 { get; set; }
    [Column("IDIMPALAT5")] public string? IdImpaLat5 { get; set; }

    [Column("CAURAPLAT1")] public string? CauRapLat1 { get; set; }
    [Column("CAURAPLAT2")] public string? CauRapLat2 { get; set; }
    [Column("CAURAPLAT3")] public string? CauRapLat3 { get; set; }
    [Column("CAURAPLAT4")] public string? CauRapLat4 { get; set; }
    [Column("CAURAPLAT5")] public string? CauRapLat5 { get; set; }

    [Column("LEGRAPLAT1")] public string? LegRapLat1 { get; set; }
    [Column("LEGRAPLAT2")] public string? LegRapLat2 { get; set; }
    [Column("LEGRAPLAT3")] public string? LegRapLat3 { get; set; }
    [Column("LEGRAPLAT4")] public string? LegRapLat4 { get; set; }
    [Column("LEGRAPLAT5")] public string? LegRapLat5 { get; set; }

    [Column("IDIMPRLAT1")] public string? IdImprLat1 { get; set; }
    [Column("IDIMPRLAT2")] public string? IdImprLat2 { get; set; }
    [Column("IDIMPRLAT3")] public string? IdImprLat3 { get; set; }
    [Column("IDIMPRLAT4")] public string? IdImprLat4 { get; set; }
    [Column("IDIMPRLAT5")] public string? IdImprLat5 { get; set; }

    // ------------------------
    // SINGOLI CAMPI
    // ------------------------
    [Column("LEGORDLAT")] public string? LegOrdLat { get; set; }
    [Column("LEGTEXLAT")] public string? LegTexLat { get; set; }
    [Column("LOCEFFLAT")] public string? LocEffLat { get; set; }
    [Column("LOCAMMLAT")] public string? LocAmmLat { get; set; }
    [Column("LOCESELAT")] public string? LocEseLat { get; set; }
    [Column("STAORDLAT")] public string? StaOrdLat { get; set; }
    [Column("OPESTOLAT")] public string? OpeStoLat { get; set; }
    [Column("CODSTOLAT")] public string? CodStoLat { get; set; }
    [Column("NUMGESLAT")] public string? NumGesLat { get; set; }
    [Column("NUMCODLAT")] public string? NumCodLat { get; set; }
    [Column("UFFESELAT")] public string? UffEseLat { get; set; }
    [Column("NUMOPELAT")] public string? NumOpeLat { get; set; }
    [Column("RISPAELAT")] public string? RisPaeLat { get; set; }
    [Column("SPESELAT")] public string? SpeseLat { get; set; }
    [Column("CAUESTLAT")] public string? CauEstLat { get; set; }
    [Column("CODFORLAT")] public string? CodForLat { get; set; }

    [Column("DAT000LAT")] public DateTime? Dat000Lat { get; set; }
    [Column("DATRICLAT")] public DateTime? DatRicLat { get; set; }
    [Column("DATCARLAT")] public DateTime? DatCarLat { get; set; }
    [Column("DATINILAT")] public DateTime? DatIniLat { get; set; }
    [Column("DATESELAT")] public DateTime? DatEseLat { get; set; }
    [Column("DATFINLAT")] public DateTime? DatFinLat { get; set; }
    [Column("DATESTLAT")] public DateTime? DatEstLat { get; set; }

    [Column("COD010LAT")] public string? Cod010Lat { get; set; }
    [Column("COD020LAT")] public string? Cod020Lat { get; set; }
    [Column("COD030LAT")] public string? Cod030Lat { get; set; }
    [Column("COD040LAT")] public string? Cod040Lat { get; set; }
    [Column("COD050LAT")] public string? Cod050Lat { get; set; }
    [Column("COD060LAT")] public string? Cod060Lat { get; set; }
    [Column("COD070LAT")] public string? Cod070Lat { get; set; }
    [Column("COD080LAT")] public string? Cod080Lat { get; set; }
    [Column("COD090LAT")] public string? Cod090Lat { get; set; }
    [Column("COD100LAT")] public string? Cod100Lat { get; set; }
    [Column("COD110LAT")] public string? Cod110Lat { get; set; }

    [Column("DATCONTLAT")] public DateTime? DatContLat { get; set; }

    [Column("SUDDIVLAT")] public string? SudDivLat { get; set; }
    [Column("TERMIDLAT")] public string? TermIdLat { get; set; }
    [Column("COD120LAT")] public string? Cod120Lat { get; set; }
    [Column("COD130LAT")] public string? Cod130Lat { get; set; }
    [Column("COD140LAT")] public string? Cod140Lat { get; set; }
    [Column("COD150LAT")] public string? Cod150Lat { get; set; }
    [Column("SUDIV1LAT")] public string? SuDiv1Lat { get; set; }

    [Column("COD160LAT")] public double? Cod160Lat { get; set; }

    [Column("COD170LAT")] public string? Cod170Lat { get; set; }
    [Column("COD180LAT")] public string? Cod180Lat { get; set; }
    [Column("COD190LAT")] public string? Cod190Lat { get; set; }

    [Column("CBANKOPELAT")] public string? CbankOpeLat { get; set; }
    [Column("CINSTRUCTLAT1")] public string? CInstructLat1 { get; set; }
    [Column("CINSTRUCTLAT2")] public string? CInstructLat2 { get; set; }
    [Column("CINSTRUCTLAT3")] public string? CInstructLat3 { get; set; }
    [Column("CINSTRUCTLAT4")] public string? CInstructLat4 { get; set; }
    [Column("CINSTRUCTLAT5")] public string? CInstructLat5 { get; set; }

    [Column("CTRANTYPELAT")] public string? CtranTypeLat { get; set; }
    [Column("DATSPELAT")] public string? DatSpeLat { get; set; }
}
