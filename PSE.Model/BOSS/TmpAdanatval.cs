using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PSE.Model.BOSS {

    [Keyless]
    [Table("TmpADANATVAL")]
    public class TmpAdanatval {

        [Column("CODE")] public string? Code { get; set; }
        [Column("NUMVALANT")] public string? NumValAnt { get; set; }
        [Column("DIVVALANT")] public string? DivValAnt { get; set; }
        [Column("GRUINTANT")] public string? GruIntAnt { get; set; }
        [Column("NUMDECANT")] public string? NumDecAnt { get; set; }
        [Column("NUMSOCANT")] public string? NumSocAnt { get; set; }
        [Column("CODMERANT")] public string? CodMerAnt { get; set; }
        [Column("DATEMIANT")] public DateTime? DatEmiAnt { get; set; }
        [Column("DATSCAANT")] public DateTime? DatScaAnt { get; set; }
        [Column("TAGMINANT")] public double? TagMinAnt { get; set; }
        [Column("TITUSAANT")] public string? TitUsaAnt { get; set; }
        [Column("FATVALANT")] public string? FatValAnt { get; set; }
        [Column("CODBLOANT")] public string? CodBloAnt { get; set; }
        [Column("PROCONANT")] public string? ProConAnt { get; set; }
        [Column("CAMVARANT")] public string? CamVarAnt { get; set; }
        [Column("CODMUTANT")] public string? CodMutAnt { get; set; }
        [Column("DATMUTANT")] public DateTime? DatMutAnt { get; set; }
        [Column("PROAGGANT")] public string? ProAggAnt { get; set; }
        [Column("TASSOANT")] public double? TassoAnt { get; set; }
        [Column("DATINTANT")] public DateTime? DatIntAnt { get; set; }
        [Column("PERPAGANT")] public string? PerPagAnt { get; set; }
        [Column("PROPAGANT")] public DateTime? ProPagAnt { get; set; }
        [Column("INIPROANT")] public DateTime? IniProAnt { get; set; }
        [Column("FINPROANT")] public DateTime? FinProAnt { get; set; }
        [Column("GIOPREANT")] public string? GioPreAnt { get; set; }
        [Column("DIVINTANT")] public string? DivIntAnt { get; set; }
        [Column("DIVNOMANT")] public string? DivNomAnt { get; set; }
        [Column("NOMPAGANT")] public double? NomPagAnt { get; set; }
        [Column("POOLFAANT")] public double? PoolFaAnt { get; set; }
        [Column("ULTCEDANT")] public string? UltCedAnt { get; set; }
        [Column("NOMINAANT")] public double? NominaAnt { get; set; }
        [Column("INIRIMANT")] public DateTime? IniRimAnt { get; set; }
        [Column("PRERIMANT")] public double? PreRimAnt { get; set; }
        [Column("INIPUTANT")] public DateTime? IniPutAnt { get; set; }
        [Column("PREPUTANT")] public double? PrePutAnt { get; set; }
        [Column("INICONANT")] public DateTime? IniConAnt { get; set; }
        [Column("FINCONANT")] public DateTime? FinConAnt { get; set; }
        [Column("DIVCONANT")] public string? DivConAnt { get; set; }
        [Column("PRECONANT")] public double? PreConAnt { get; set; }
        [Column("PERVARANT")] public string? PerVarAnt { get; set; }
        [Column("STATUSANT")] public string? StatusAnt { get; set; }
        [Column("CODORDANT")] public string? CodOrdAnt { get; set; }
        [Column("NUMOPEANT")] public string? NumOpeAnt { get; set; }
        [Column("TITSUBANT")] public string? TitSubAnt { get; set; }
        [Column("COMANAANT")] public string? ComAnaAnt { get; set; }
        [Column("DATGODANT")] public DateTime? DatGodAnt { get; set; }
        [Column("DATLIBANT")] public DateTime? DatLibAnt { get; set; }
        [Column("CATDEBANT")] public string? CatDebAnt { get; set; }
        [Column("COMVALANT")] public string? ComValAnt { get; set; }
        [Column("TIPVALANT")] public string? TipValAnt { get; set; }
        [Column("GRUVALANT")] public string? GruValAnt { get; set; }
        [Column("LINABBANT")] public string? LinAbbAnt { get; set; }
        [Column("TESABBANT")] public string? TesAbbAnt { get; set; }
        [Column("LINESTANT")] public string? LinEstAnt { get; set; }
        [Column("TESIN1ANT")] public string? TesIn1Ant { get; set; }
        [Column("TESIN2ANT")] public string? TesIn2Ant { get; set; }
        [Column("TESIN3ANT")] public string? TesIn3Ant { get; set; }
        [Column("TESIN4ANT")] public string? TesIn4Ant { get; set; }
        [Column("FINPUTANT")] public DateTime? FinPutAnt { get; set; }
        [Column("DATAPEANT")] public DateTime? DatApeAnt { get; set; }
        [Column("DATESTANT")] public DateTime? DatEstAnt { get; set; }
        [Column("TIPQTAANT")] public string? TipQtaAnt { get; set; }
        [Column("TIPQUOANT")] public string? TipQuoAnt { get; set; }
        [Column("EXCODEANT")] public string? ExCodeAnt { get; set; }
        [Column("DIRCUSANT")] public string? DirCusAnt { get; set; }
        [Column("SIMTLKANT")] public string? SimTlkAnt { get; set; }

        [Column("TIPCORANT1")] public string? TipCorAnt1 { get; set; }
        [Column("TIPCORANT2")] public string? TipCorAnt2 { get; set; }
        [Column("TIPCORANT3")] public string? TipCorAnt3 { get; set; }
        [Column("TIPCORANT4")] public string? TipCorAnt4 { get; set; }

        [Column("DIVCORANT1")] public string? DivCorAnt1 { get; set; }
        [Column("DIVCORANT2")] public string? DivCorAnt2 { get; set; }
        [Column("DIVCORANT3")] public string? DivCorAnt3 { get; set; }
        [Column("DIVCORANT4")] public string? DivCorAnt4 { get; set; }

        [Column("DATCORANT1")] public DateTime? DatCorAnt1 { get; set; }
        [Column("DATCORANT2")] public DateTime? DatCorAnt2 { get; set; }
        [Column("DATCORANT3")] public DateTime? DatCorAnt3 { get; set; }
        [Column("DATCORANT4")] public DateTime? DatCorAnt4 { get; set; }

        [Column("BORQUOANT1")] public string? BorQuoAnt1 { get; set; }
        [Column("BORQUOANT2")] public string? BorQuoAnt2 { get; set; }
        [Column("BORQUOANT3")] public string? BorQuoAnt3 { get; set; }
        [Column("BORQUOANT4")] public string? BorQuoAnt4 { get; set; }

        [Column("CORSOANT1")] public double? CorsoAnt1 { get; set; }
        [Column("CORSOANT2")] public double? CorsoAnt2 { get; set; }
        [Column("CORSOANT3")] public double? CorsoAnt3 { get; set; }
        [Column("CORSOANT4")] public double? CorsoAnt4 { get; set; }

        [Column("TIPCALANT")] public string? TipCalAnt { get; set; }
        [Column("TIPMERANT")] public string? TipMerAnt { get; set; }
        [Column("DIVRIMANT")] public string? DivRimAnt { get; set; }
        [Column("DATRIAANT")] public DateTime? DatRiaAnt { get; set; }
        [Column("GIORIMANT")] public string? GioRimAnt { get; set; }
        [Column("RIMFINANT")] public double? RimFinAnt { get; set; }
        [Column("GRUESTANT")] public string? GruEstAnt { get; set; }
        [Column("TASNSANT")] public double? TasNsAnt { get; set; }
        [Column("COMCEDANT")] public double? ComCedAnt { get; set; }
        [Column("COMRIMANT")] public double? ComRimAnt { get; set; }
        [Column("IMPPREANT")] public string? ImpPreAnt { get; set; }
        [Column("IMPSUPANT")] public string? ImpSupAnt { get; set; }
        [Column("USANZAANT")] public string? UsanzaAnt { get; set; }
        [Column("NRISINANT")] public string? NrisinAnt { get; set; }
        [Column("NONVERANT")] public double? NonVerAnt { get; set; }
        [Column("PUTCALANT")] public string? PutCalAnt { get; set; }
        [Column("STRIKEANT")] public double? StrikeAnt { get; set; }
        [Column("QTACONANT")] public double? QtaConAnt { get; set; }
        [Column("DIVUL1ANT")] public string? DivUl1Ant { get; set; }
        [Column("DIVUL2ANT")] public string? DivUl2Ant { get; set; }
        [Column("LIMMERANT")] public string? LimMerAnt { get; set; }
        [Column("POSPDEANT")] public DateTime? PosPdeAnt { get; set; }
        [Column("QTAOPEANT")] public string? QtaOpeAnt { get; set; }
        [Column("DAULCOANT")] public DateTime? DaUlCoAnt { get; set; }
        [Column("STILEANT")] public string? StileAnt { get; set; }
        [Column("NUMTLKANT")] public string? NumTlkAnt { get; set; }
        [Column("TIPPDEANT")] public string? TipPdeAnt { get; set; }
        [Column("CREUTERSANT")] public string? CreutersAnt { get; set; }
        [Column("VALBASANT")] public string? ValBasAnt { get; set; }
        [Column("UETAXANT")] public string? UeTaxAnt { get; set; }

        [Column("CORTISANT1")] public double? CorTisAnt1 { get; set; }
        [Column("CORTISANT2")] public double? CorTisAnt2 { get; set; }
        [Column("CORTISANT3")] public double? CorTisAnt3 { get; set; }
        [Column("CORTISANT4")] public double? CorTisAnt4 { get; set; }

        [Column("AFFIDAANT")] public string? AffidaAnt { get; set; }
        [Column("LICENZAANT")] public string? LicenzaAnt { get; set; }
        [Column("TIPBOLANT")] public string? TipBolAnt { get; set; }
        [Column("BOLAPPANT")] public string? BolAppAnt { get; set; }
        [Column("CONVQTAANT")] public double? ConvQtaAnt { get; set; }
        [Column("ACTTYPANT")] public string? ActTypAnt { get; set; }
        [Column("VALUCAANT")] public DateTime? ValUcaAnt { get; set; }
        [Column("EXDTACAANT")] public DateTime? ExDtaCaAnt { get; set; }
        [Column("NEWCOMANT")] public string? NewComAnt { get; set; }
        [Column("NEWNUMANT")] public string? NewNumAnt { get; set; }
        [Column("CONVPRICEANT")] public double? ConvPriceAnt { get; set; }
        [Column("CURRCAANT")] public string? CurrCaAnt { get; set; }
        [Column("DESCCAANT")] public string? DescCaAnt { get; set; }
        [Column("TRANDATAANT")] public DateTime? TranDataAnt { get; set; }
        [Column("STAOPEANT")] public string? StaOpeAnt { get; set; }
        [Column("FIREANT")] public string? FireAnt { get; set; }
        [Column("CBLOOMBERGANT")] public string? CBloombergAnt { get; set; }
        [Column("MONTITANT")] public string? MonTitAnt { get; set; }
        [Column("IMPCEDANT")] public string? ImpCedAnt { get; set; }
        [Column("RUBTCREANT")] public string? RubtCreAnt { get; set; }
        [Column("EBKFLAGANT")] public string? EbkFlagAnt { get; set; }
        [Column("TAGINCANT")] public double? TagIncAnt { get; set; }
        [Column("FATCAANT")] public string? FatCaAnt { get; set; }
        [Column("COCOBOANT")] public string? CocoBoAnt { get; set; }
        [Column("WRDOWNANT")] public string? WrDownAnt { get; set; }
        [Column("FOARMOANT")] public string? FoArmoAnt { get; set; }

        [Column("TESIBRANT1")] public string? TesiBrAnt1 { get; set; }
        [Column("TESIBRANT2")] public string? TesiBrAnt2 { get; set; }
        [Column("TESIBRANT3")] public string? TesiBrAnt3 { get; set; }
        [Column("TESIBRANT4")] public string? TesiBrAnt4 { get; set; }

        [Column("CLRISKANT")] public string? ClRiskAnt { get; set; }
        [Column("FINFRAGANT")] public string? FinFragAnt { get; set; }
        [Column("CLSSWXANT")] public string? ClsSwxAnt { get; set; }
        [Column("TYPSTRANT")] public string? TypStrAnt { get; set; }
        [Column("IRS871ANT")] public string? Irs871Ant { get; set; }
        [Column("APPLICANT")] public string? ApplicAnt { get; set; }
        [Column("OBJECTANT")] public string? ObjectAnt { get; set; }
        [Column("PARTICANT")] public string? ParticAnt { get; set; }
        [Column("PECULIANT")] public string? PeculiAnt { get; set; }
        [Column("UNIVERSANT")] public string? UniversAnt { get; set; }
        [Column("TIPOINVES")] public string? TipoInvEs { get; set; }
        [Column("CLASSIFIC")] public string? Classific { get; set; }

        [Column("CLITYPRETAILANT")] public string? CliTypRetailAnt { get; set; }
        [Column("CLITYPPROFCLIANT")] public string? CliTypProfCliAnt { get; set; }
        [Column("CLITYPELIGCOUNTERANT")] public string? CliTypeLigCounterAnt { get; set; }

        [Column("KNOWEXPBASICANT")] public string? KnowExpBasicAnt { get; set; }
        [Column("KNOWEXPINFINVESTANT")] public string? KnowExpInfInvestAnt { get; set; }
        [Column("KNOWEXPINVEXPANT")] public string? KnowExpInvExpAnt { get; set; }

        [Column("ABILOSSFULLPROTANT")] public string? AbiLossFullProtAnt { get; set; }
        [Column("ABILOSSPARTIALPROTANT")] public string? AbiLossPartialProtAnt { get; set; }
        [Column("ABILOSSPERCLOSSDEFANT")] public string? AbiLossPercLossDefAnt { get; set; }
        [Column("ABILOSSNOPROTANT")] public string? AbiLossNoProtAnt { get; set; }
        [Column("ABILOSSLOSSMOREANT")] public string? AbiLossLossMoreAnt { get; set; }

        [Column("INVOBJSHORTANT")] public string? InvObjShortAnt { get; set; }
        [Column("INVOBJMEDIUMANT")] public string? InvObjMediumAnt { get; set; }
        [Column("INVOBJLONGANT")] public string? InvObjLongAnt { get; set; }
        [Column("INVOBJVSHORTANT")] public string? InvObjVShortAnt { get; set; }
        [Column("INVOBJRHPANT")] public string? InvObjRhpAnt { get; set; }
        [Column("INVOBJRHPAMOUNTANT")] public string? InvObjRhpAmountAnt { get; set; }
        [Column("INVOBJGREENANT")] public string? InvObjGreenAnt { get; set; }
        [Column("INVOBJETHICALANT")] public string? InvObjEthicalAnt { get; set; }
        [Column("INVOBJISLAMICANT")] public string? InvObjIslamicAnt { get; set; }
        [Column("INVOBJESGANT")] public string? InvObjEsgAnt { get; set; }

        [Column("DISSTRAEXEONLYRETANT")] public string? DisStraExeOnlyRetAnt { get; set; }
        [Column("DISSTRAEXEONLYPROANT")] public string? DisStraExeOnlyProAnt { get; set; }
        [Column("DISSTRAEXEONLYBOTHANT")] public string? DisStraExeOnlyBothAnt { get; set; }
        [Column("DISSTRAADVISEDRETANT")] public string? DisStraAdvisedRetAnt { get; set; }
        [Column("DISSTRAADVISEDPROANT")] public string? DisStraAdvisedProAnt { get; set; }
        [Column("DISSTRAADVISEDBOTHANT")] public string? DisStraAdvisedBothAnt { get; set; }
        [Column("DISSTRAPORTMANRETANT")] public string? DisStraPortManRetAnt { get; set; }
        [Column("DISSTRAPORTMANPROANT")] public string? DisStraPortManProAnt { get; set; }
        [Column("DISSTRAPORTMANBOTHANT")] public string? DisStraPortManBothAnt { get; set; }
        [Column("DISSTRANOADVISERETANT")] public string? DisStraNoAdviseRetAnt { get; set; }
        [Column("DISSTRANOADVISEPROANT")] public string? DisStraNoAdviseProAnt { get; set; }
        [Column("DISSTRANOADVISEBOTHANT")] public string? DisStraNoAdviseBothAnt { get; set; }

        [Column("INDISRIOFPRIIPSVALANT")] public string? InDisRiOfPriipsValAnt { get; set; }
        [Column("INDISRRIOFUCITSVALANT")] public string? InDisRriOfUcitsValAnt { get; set; }
        [Column("INDIMANPROVRISKFACANT")] public string? IndiManProvRiskFacAnt { get; set; }
        [Column("COMPLEXINDEXVALANT")] public string? ComplexIndexValAnt { get; set; }
        [Column("COMPLEXNOINDEXVALANT")] public string? ComplexNoIndexValAnt { get; set; }
        [Column("MIRISKANT")] public string? MiRiskAnt { get; set; }

        [Column("AGERATANT1")] public string? AgeRatAnt1 { get; set; }
        [Column("AGERATANT2")] public string? AgeRatAnt2 { get; set; }
        [Column("AGERATANT3")] public string? AgeRatAnt3 { get; set; }
        [Column("AGERATANT4")] public string? AgeRatAnt4 { get; set; }
        [Column("AGERATANT5")] public string? AgeRatAnt5 { get; set; }

        [Column("RATINGANT1")] public string? RatingAnt1 { get; set; }
        [Column("RATINGANT2")] public string? RatingAnt2 { get; set; }
        [Column("RATINGANT3")] public string? RatingAnt3 { get; set; }
        [Column("RATINGANT4")] public string? RatingAnt4 { get; set; }
        [Column("RATINGANT5")] public string? RatingAnt5 { get; set; }

        [Column("DATRATANT1")] public DateTime? DatRatAnt1 { get; set; }
        [Column("DATRATANT2")] public DateTime? DatRatAnt2 { get; set; }
        [Column("DATRATANT3")] public DateTime? DatRatAnt3 { get; set; }
        [Column("DATRATANT4")] public DateTime? DatRatAnt4 { get; set; }
        [Column("DATRATANT5")] public DateTime? DatRatAnt5 { get; set; }

        [Column("PRDBCAANT")] public string? PrDbCaAnt { get; set; }
        [Column("DATELOCK")] public string? DateLock { get; set; }
        [Column("FUTOPTTYPEANT")] public string? FutOptTypeAnt { get; set; }
        [Column("NOPREMIOANT")] public string? NoPremioAnt { get; set; }
        [Column("SFDRARTANT")] public string? SfdrArtAnt { get; set; }
        [Column("DIRTYFLAGANT")] public string? DirtyFlagAnt { get; set; }
        [Column("CFICODEANT")] public string? CfiCodeAnt { get; set; }
        [Column("CFICLAANT")] public string? CfiClaAnt { get; set; }
        [Column("CFISIXCODEANT")] public string? CfiSixCodeAnt { get; set; }
        [Column("CFISIXANT")] public string? CfiSixAnt { get; set; }

        [Column("THERMALPCTANT")] public double? ThermalPctAnt { get; set; }
        [Column("UNCONVPCTANT")] public double? UnConvPctAnt { get; set; }

        [Column("UFFCODEANT")] public string? UffCodeAnt { get; set; }
        [Column("FLGSIXANT")] public string? FlgSixAnt { get; set; }
        [Column("PTPCLAANT")] public string? PtpClaAnt { get; set; }
        [Column("PTPAPPANT")] public string? PtpAppAnt { get; set; }
        [Column("PNOACCANT")] public string? PnoAccAnt { get; set; }
        [Column("CRYPTOANT")] public string? CryptoAnt { get; set; }

    }

}