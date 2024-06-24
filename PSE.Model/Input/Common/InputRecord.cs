using System.Text;
using System.Reflection;
using FileHelpers;
using PSE.Model.Input.Interfaces;
using Newtonsoft.Json.Linq;

namespace PSE.Model.Input.Common
{

    public abstract class InputRecord : IInputRecord
    {

        [FieldHidden]
        public string RecordType 
        { 
            get
            {
                string recordType = "";
                if(string.IsNullOrEmpty(MessageType_1) == false && MessageType_1.Length >= 3)
                    recordType = MessageType_1[..3].ToUpper();
                return recordType;
            }
            private set { }
        }

        [FieldHidden]
        public bool AlreadyUsed
        {
            get; set;
        }

        [FieldFixedLength(6)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(1)]
        public string MessageType_1 { get; set; }

        protected InputRecord(string messageType)
        {
            AlreadyUsed = false;
            MessageType_1 = messageType;
        }

        protected InputRecord(InputRecord source)
        {
            AlreadyUsed = source.AlreadyUsed;
            MessageType_1 = source.MessageType_1;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            //sb.Append("----------------------------------------");
            sb.Append(Environment.NewLine);
            sb.Append(RecordType);
            sb.Append(Environment.NewLine);
            sb.Append("----------------------------------------");
            sb.Append(Environment.NewLine);
            object? objOrder;
            int orderIndex;
            List<(string name, int order, object? value)> propsOrderAndValue = new();
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                orderIndex = 0;
                objOrder = null;
                if (property.CustomAttributes != null && property.CustomAttributes.Any()
                && property.CustomAttributes.Last().NamedArguments != null && property.CustomAttributes.Last().ConstructorArguments.Any()
                && property.CustomAttributes.Last().ConstructorArguments.Last().Value != null)
                    objOrder = property.CustomAttributes.Last().ConstructorArguments.Last().Value;
                if(objOrder != null && int.TryParse(objOrder.ToString(), out orderIndex))
                    propsOrderAndValue.Add((property.Name, orderIndex, property.GetValue(this, null)));
            }
            foreach(var (name, order, value) in propsOrderAndValue.OrderBy(ob => ob.order))
            {
                sb.Append(name);
                sb.Append(": ");
                sb.Append(value);
                sb.Append(Environment.NewLine);
            }
            /*
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                sb.Append(property.Name);
                sb.Append(": ");
                sb.Append(property.GetValue(this, null));
                sb.Append(Environment.NewLine);
            }
            */            
            sb.Append("----------------------------------------");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

    }

    public abstract class InputRecordA : InputRecord, IInputRecordA
    {

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(2)]
        public string Block_2 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(3)]
        public string CustomerNumber_3 { get; set; }

        protected InputRecordA(string messageType) : base(messageType)
        {
            Block_2 = "";
            CustomerNumber_3 = "";
        }

        protected InputRecordA(InputRecordA source) : base(source)
        {
            Block_2 = source.Block_2;
            CustomerNumber_3 = source.CustomerNumber_3;
        }

    }

    public abstract class InputRecordB : InputRecord, IInputRecordB
    {
 
        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(2)] 
        public string CustomerNumber_2 { get; set; }

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both)]
        [FieldOrder(3)]
        public string Grouping_3 { get; set; }

        protected InputRecordB(string messageType) : base(messageType)
        {
            CustomerNumber_2 = "";
            Grouping_3 = "";
        }

        protected InputRecordB(InputRecordB source) : base(source)
        {
            CustomerNumber_2 = source.CustomerNumber_2;
            Grouping_3 = source.Grouping_3;
        }

    }

}
