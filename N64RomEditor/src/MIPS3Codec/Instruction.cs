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
        public int Index { get; set; }
        public List<FunctionalParam> Params { get; set; } = new List<FunctionalParam>();
        public bool Erroneous { get; set; } = false;
        public Instruction(InstructionHelper helper, int byteCode, int index)
        {
            Helper = helper;
            AssociatedByteCode = byteCode;
            Index = index;
            foreach(OpcodeParamaterUnmaskingMetas pMetas in helper.ParamaterUnmaskingMetas)
            {
                Params.Add(new FunctionalParam(this, pMetas.Parameter, pMetas.ShiftAmount, pMetas.BitMaskAfterShift));
            }
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
        public int LocalByteCode { get; set; }
        public string LocalDecodeText { get; set; }
        public bool Erroneous { get; set; } = false;
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
            else
            {
                throw new Exception($"Typeless FunctionalParam of {param.GetType().ToString()} in Instruction {owner.Helper.Name}");
            }
        }
        public int BitExtract()
        {
            return (Owner.AssociatedByteCode >> BitShift) & BitMask;
        }
        public void Decode()
        {
            Erroneous = false;
            if (Type == Types.Register)
            {
                int mapEntry = BitExtract();
                LocalDecodeText = RegisterMap.DecodeRegister(RegisterType.RegisterMapEntry, mapEntry);
            }
            else // if (Type == Types.Number)
            {
                LocalDecodeText = Convert.ToString(BitExtract(), 16);
            }
        }
        public void Encode()
        {
            try
            {
                int newByteCode = 0;
                if (Type == Types.Register)
                {
                    newByteCode = RegisterMap.EncodeRegister(LocalDecodeText);

                }
                else // if (Type == Types.Number)
                {
                    newByteCode = Convert.ToInt32(LocalDecodeText, 16);
                    if (newByteCode > NumericType.MaximumValue)
                    {
                        throw new Exception("");
                    }
                }
                if (newByteCode < 0)
                {
                    throw new Exception("");
                }
                LocalByteCode = newByteCode;
                Erroneous = false;
            }
            catch (Exception e)
            {
                Erroneous = true;
            }
        }
    }
}
