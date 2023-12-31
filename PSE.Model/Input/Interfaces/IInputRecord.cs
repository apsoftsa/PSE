﻿namespace PSE.Model.Input.Interfaces
{

    public interface IInputRecord
    {

        string RecordType { get; }

        bool AlreadyUsed { get; set; }

        string MessageType_1 { get; set; }

        string ToString();

    }

    public interface IInputRecordA : IInputRecord
    {
        
        string Block_2 { get; set; }

        string CustomerNumber_3 { get; set; }

    }

    public interface IInputRecordB : IInputRecord
    {

        string CustomerNumber_2 { get; set; }

        string Grouping_3 { get; set; }

    }

}
