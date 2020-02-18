using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace N64RomEditor.src.MIPS3Codec
{
    public class BitField
    {
        public const int maximumPrecedenceSetting = 3;
        public int Length { get; set; }
        public BitField(int len)
        {
            Length = len;
        }
    }

    // Identifier BitFields
    public class IdentifierBitField : BitField
    {
        public int Precedence { get; set; }
        public int Identifier { get; set; }
        public IdentifierBitField(int len, int precedence, int identifier) : base(len)
        {
            if(precedence > maximumPrecedenceSetting)
            {
                throw new Exception("Precedence set too high");
            }
            Precedence = precedence;
            Identifier = identifier;
        }
    }
    public class OpcodeBitField : IdentifierBitField
    {
        public OpcodeBitField(int identifier) : base(6, 0, identifier) { }
    }
    public class ExOpcodeBitField : IdentifierBitField
    {
        public ExOpcodeBitField(int identifier) : base(5, 2, identifier) { }
    }
    public class STypeBitField : IdentifierBitField
    {
        public STypeBitField(int identifier) : base(5, 1, identifier) { }
    }
    public class FmtBitField : IdentifierBitField
    {
        public FmtBitField(int identifier) : base(5, 1, identifier) { }
    }
    public class CondBitField : IdentifierBitField
    {
        public CondBitField(int identifier) : base(4, 1, identifier) { }
    }
    public class ZeroBitField : IdentifierBitField
    {
        public ZeroBitField(int len) : base(len, maximumPrecedenceSetting, 0) { }
    }

    // Parameter BitFields
    public class ParameterBitField : BitField
    {
        public ParameterBitField(int len) : base(len) { }
        public bool Is(ParameterBitField pbf)
        {
            return GetType() == pbf.GetType();
        }
    }

    public class RegisterBitField : ParameterBitField
    {
        public int RegisterMapEntry { get; set; }
        public RegisterBitField(int len) : base(len) 
        {
            Type thisType = GetType();
            if (thisType == typeof(MainRegisterBitField)) RegisterMapEntry = 0;
            if (thisType == typeof(FloatRegisterBitField)) RegisterMapEntry = 1;
            if (thisType == typeof(VectorRegisterBitField)) RegisterMapEntry = 2;
            if (thisType == typeof(CP0RegisterBitField)) RegisterMapEntry = 3;
        }
    }

    // Main Register Parameter BitFields
    public class MainRegisterBitField : RegisterBitField
    {
        public MainRegisterBitField(int len) : base(len) { }
    }
    public class BaseBitField : MainRegisterBitField
    {
        public BaseBitField() : base(5) { }
    }
    public class RtBitField : MainRegisterBitField
    {
        public RtBitField() : base(5) { }
    }
    public class RdBitField : MainRegisterBitField
    {
        public RdBitField() : base(5) { }
    }
    public class RsBitField : MainRegisterBitField
    {
        public RsBitField() : base(5) { }
    }

    // Float Register Parameter BitFields
    public class FloatRegisterBitField : RegisterBitField
    {
        public FloatRegisterBitField(int len) : base(len) { }
    }
    public class FtBitField : FloatRegisterBitField
    {
        public FtBitField() : base(5) { }
    }
    public class FdBitField : FloatRegisterBitField
    {
        public FdBitField() : base(5) { }
    }
    public class FsBitField : FloatRegisterBitField
    {
        public FsBitField() : base(5) { }
    }

    // Vector Register Parameter BitFields
    public class VectorRegisterBitField : RegisterBitField
    {
        public VectorRegisterBitField(int len) : base(len) { }
    }
    public class VsBitField : VectorRegisterBitField
    {
        public VsBitField() : base(5) { }
    }
    public class VdBitField : VectorRegisterBitField
    {
        public VdBitField() : base(5) { }
    }
    public class VtBitField : VectorRegisterBitField
    {
        public VtBitField() : base(5) { }
    }

    // Co-Processor 0 Register Parameter BitField(s)
    public class CP0RegisterBitField : RegisterBitField
    {
        public CP0RegisterBitField(int len) : base(len) { }
    }
    public class CsBitField : CP0RegisterBitField
    {
        public CsBitField() : base(5) { }
    }

    // Numeric Parameter BitFields
    public class NumericBitField : ParameterBitField
    {
        public int MaximumValue { get; set; }
        public int MinimumDigits { get; set; }
        public NumericBitField(int len, int maxValue, int minDigits) : base(len)
        {
            MaximumValue = maxValue;
            MinimumDigits = minDigits;
        }
    }
    public class OffsetBitField : NumericBitField
    {
        public OffsetBitField() : base(16, 0xFFFF, 8) { }
    }
    public class VOffsetBitField : NumericBitField
    {
        public VOffsetBitField() : base(6, 0x3F, 2) { }
    }
    public class AddressBitField : NumericBitField
    {
        public AddressBitField() : base(26, 0x3FFFFFF, 8) { }
    }
    public class ImmediateBitField : NumericBitField
    {
        public ImmediateBitField() : base(16, 0xFFFF, 4) { }
    }
    public class SaBitField : NumericBitField
    {
        public SaBitField() : base(5, 0x1F, 2) { }
    }
    public class Code10BitField : NumericBitField
    {
        public Code10BitField() : base(10, 0x3FF, 3) { }
    }
    public class Code20BitField : NumericBitField
    {
        public Code20BitField() : base(20, 0xFFFFF, 5) { }
    }
    public class ModBitField : NumericBitField
    {
        public ModBitField() : base(4, 0xF, 1) { }
    }
    public class OpBitField : NumericBitField
    {
        public OpBitField() : base(5, 0x1F, 2) { }
    }
    public class CoBitField : NumericBitField
    {
        public CoBitField() : base(1, 1, 1) { }
    }
    public class EsBitField : NumericBitField
    {
        public EsBitField() : base(2, 3, 1) { }
    }
}
