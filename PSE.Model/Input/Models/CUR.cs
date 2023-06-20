﻿using FileHelpers;
using PSE.Model.Input.Common;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Input.Models
{

    [FixedLengthRecord(FixedMode.AllowMoreChars)]
    [IgnoreEmptyLines()]
    public class CUR : InputRecord
    {

        // to-do ...
        [FieldFixedLength(898)]
        public string? ToDefine { get; set; }

        public CUR() : base(INPUT_CUR_MSG_TYPE)
        {
        }

        public CUR(CUR source) : base(source)
        {
        }

    }

}