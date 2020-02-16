using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.OpcodeMatrix
{
    public class BitField
    {
        public int Length { get; set; }
        public BitField(int len)
        {
            Length = len;
        }
    }

    public class IdentifierBitField : BitField
    {
        public int Precedence { get; set; }
        public int Identifier { get; set; }
        public IdentifierBitField(int len, int precedence, int identifier) : base(len)
        {
            Precedence = precedence;
            Identifier = identifier;
        }
    }

    public class ParameterBitField : BitField
    {
        public ParameterBitField(int len) : base(len) { }
    }

    // Identifier BitFields
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
    public class OpBitField : ParameterBitField
    {
        public OpBitField() : base(5) { }
    }
    public class CoBitField : ParameterBitField
    {
        public CoBitField() : base(1) { }
    }
    public class EsBitField : ParameterBitField
    {
        public EsBitField() : base(2) { }
    }
    public class ZeroBitField : IdentifierBitField
    {
        public ZeroBitField(int len) : base(len, 3, 0) { }
    }

    // Parameter BitFields
    public class OffsetBitField : ParameterBitField
    {
        public OffsetBitField() : base(16) { }
    }
    public class VOffsetBitField : ParameterBitField
    {
        public VOffsetBitField() : base(6) { }
    }
    public class AddressBitField : ParameterBitField
    {
        public AddressBitField() : base(26) { }
    }
    public class ImmediateBitField : ParameterBitField
    {
        public ImmediateBitField() : base(16) { }
    }
    public class BaseBitField : ParameterBitField
    {
        public BaseBitField() : base(5) { }
    }
    public class RtBitField : ParameterBitField
    {
        public RtBitField() : base(5) { }
    }
    public class RdBitField : ParameterBitField
    {
        public RdBitField() : base(5) { }
    }
    public class RsBitField : ParameterBitField
    {
        public RsBitField() : base(5) { }
    }
    public class FtBitField : ParameterBitField
    {
        public FtBitField() : base(5) { }
    }
    public class FdBitField : ParameterBitField
    {
        public FdBitField() : base(5) { }
    }
    public class FsBitField : ParameterBitField
    {
        public FsBitField() : base(5) { }
    }
    public class CsBitField : ParameterBitField
    {
        public CsBitField() : base(5) { }
    }
    public class SaBitField : ParameterBitField
    {
        public SaBitField() : base(5) { }
    }
    public class Code10BitField : ParameterBitField
    {
        public Code10BitField() : base(10) { }
    }
    public class Code20BitField : ParameterBitField
    {
        public Code20BitField() : base(20) { }
    }
    public class VsBitField : ParameterBitField
    {
        public VsBitField() : base(5) { }
    }
    public class VdBitField : ParameterBitField
    {
        public VdBitField() : base(5) { }
    }
    public class VtBitField : ParameterBitField
    {
        public VtBitField() : base(5) { }
    }
    public class ModBitField : ParameterBitField
    {
        public ModBitField() : base(4) { }
    }
}
