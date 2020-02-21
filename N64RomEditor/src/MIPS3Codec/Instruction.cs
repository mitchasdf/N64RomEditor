using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.MIPS3Codec
{
    public class Instruction
    {
        public InstructionHelper Helper { get; set; }
        public int AssociatedByteCode { get; set; }
        public Instruction(InstructionHelper helper, int byteCode)
        {
            Helper = helper;
            AssociatedByteCode = byteCode;
        }
    }
    public class FunctionalParam
    {
        public enum Types
        {
            Register,
            Number
        }
        public ParameterBitField Parameter { get; set; }
        public RegisterBitField RegisterType { get; set; }
        public NumericBitField NumericType { get; set; }
        public int BitShift { get; set; }
        public int BitMask { get; set; }
        public int EraserBitMask { get; set; }
        public Instruction Owner { get; set; }
        public Types Type { get; set; }

        public FunctionalParam(Instruction owner, ParameterBitField param, int bitShift, int bitMask)
        {
            Owner = owner;
            Parameter = param;
            BitShift = bitShift;
            BitMask = bitMask;
            EraserBitMask = bitMask << bitShift;
            if (param.GetType().BaseType == typeof(NumericBitField))
            {
                NumericType = (NumericBitField)param;
                Type = Types.Number;
            }
            else if (param.GetType().BaseType.BaseType == typeof(RegisterBitField))
            {
                RegisterType = (RegisterBitField)param;
                Type = Types.Register;
            }
        }
        public int BitExtract()
        {
            return (Owner.AssociatedByteCode >> BitShift) & BitMask;
        }
        public string Decode()
        {
            if (Type == Types.Register)
            {
                int mapEntry = BitExtract();
                return RegisterMap.DecodeRegister(RegisterType.RegisterMapEntry, mapEntry);
            }
            else // if (Type == Types.Number)
            {
                return Convert.ToString(BitExtract(), 16);
            }
        }
        public int Encode()
        {

        }
    }
}
