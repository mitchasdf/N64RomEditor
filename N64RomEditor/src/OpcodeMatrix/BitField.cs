using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace N64RomEditor.src.OpcodeMatrix
{
    public class BitField
    {
        // So this causes a stack overflow
        //public List<BitField> HandyListOfAllBitFields = new List<BitField>()
        //{
        //    {new OpcodeBitField          (1)},
        //    {new ExOpcodeBitField        (1)},
        //    {new STypeBitField           (1)},
        //    {new FmtBitField             (1)},
        //    {new CondBitField            (1)},
        //    {new ZeroBitField            (1)},
        //    {new OffsetBitField           ()},
        //    {new VOffsetBitField          ()},
        //    {new AddressBitField          ()},
        //    {new ImmediateBitField        ()},
        //    {new BaseBitField             ()},
        //    {new RtBitField               ()},
        //    {new RdBitField               ()},
        //    {new RsBitField               ()},
        //    {new FtBitField               ()},
        //    {new FdBitField               ()},
        //    {new FsBitField               ()},
        //    {new CsBitField               ()},
        //    {new SaBitField               ()},
        //    {new Code10BitField           ()},
        //    {new Code20BitField           ()},
        //    {new VsBitField               ()},
        //    {new VdBitField               ()},
        //    {new VtBitField               ()},
        //    {new ModBitField              ()},
        //    {new OpBitField               ()},
        //    {new CoBitField               ()},
        //    {new EsBitField               () }
        //};
        public const int maximumPrecedenceSetting = 3;
        public int ListId { get; set; }
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
            if(precedence > maximumPrecedenceSetting)
            {
                throw new Exception("Precedence set too high");
            }
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
        public OpcodeBitField(int identifier) : base(6, 0, identifier) { ListId = 0; }
    }
    public class ExOpcodeBitField : IdentifierBitField
    {
        public ExOpcodeBitField(int identifier) : base(5, 2, identifier) { ListId = 1;}
    }
    public class STypeBitField : IdentifierBitField
    {
        public STypeBitField(int identifier) : base(5, 1, identifier) { ListId = 2;}
    }
    public class FmtBitField : IdentifierBitField
    {
        public FmtBitField(int identifier) : base(5, 1, identifier) { ListId = 3;}
    }
    public class CondBitField : IdentifierBitField
    {
        public CondBitField(int identifier) : base(4, 1, identifier) { ListId = 4;}
    }
    public class ZeroBitField : IdentifierBitField
    {
        public ZeroBitField(int len) : base(len, maximumPrecedenceSetting, 0) { ListId = 5;}
    }

    // Parameter BitFields
    public class OffsetBitField : ParameterBitField
    {
        public OffsetBitField() : base(16) { ListId = 6;}
    }
    public class VOffsetBitField : ParameterBitField
    {
        public VOffsetBitField() : base(6) { ListId = 7;}
    }
    public class AddressBitField : ParameterBitField
    {
        public AddressBitField() : base(26) { ListId = 8;}
    }
    public class ImmediateBitField : ParameterBitField
    {
        public ImmediateBitField() : base(16) { ListId = 9;}
    }
    public class BaseBitField : ParameterBitField
    {
        public BaseBitField() : base(5) { ListId = 10;}
    }
    public class RtBitField : ParameterBitField
    {
        public RtBitField() : base(5) { ListId = 11;}
    }
    public class RdBitField : ParameterBitField
    {
        public RdBitField() : base(5) { ListId = 12;}
    }
    public class RsBitField : ParameterBitField
    {
        public RsBitField() : base(5) { ListId = 13;}
    }
    public class FtBitField : ParameterBitField
    {
        public FtBitField() : base(5) { ListId = 14;}
    }
    public class FdBitField : ParameterBitField
    {
        public FdBitField() : base(5) { ListId = 15;}
    }
    public class FsBitField : ParameterBitField
    {
        public FsBitField() : base(5) { ListId = 16;}
    }
    public class CsBitField : ParameterBitField
    {
        public CsBitField() : base(5) { ListId = 17;}
    }
    public class SaBitField : ParameterBitField
    {
        public SaBitField() : base(5) { ListId = 18;}
    }
    public class Code10BitField : ParameterBitField
    {
        public Code10BitField() : base(10) { ListId = 19;}
    }
    public class Code20BitField : ParameterBitField
    {
        public Code20BitField() : base(20) { ListId = 20;}
    }
    public class VsBitField : ParameterBitField
    {
        public VsBitField() : base(5) { ListId = 21;}
    }
    public class VdBitField : ParameterBitField
    {
        public VdBitField() : base(5) { ListId = 22;}
    }
    public class VtBitField : ParameterBitField
    {
        public VtBitField() : base(5) { ListId = 23;}
    }
    public class ModBitField : ParameterBitField
    {
        public ModBitField() : base(4) { ListId = 24;}
    }
    public class OpBitField : ParameterBitField
    {
        public OpBitField() : base(5) { ListId = 25;}
    }
    public class CoBitField : ParameterBitField
    {
        public CoBitField() : base(1) { ListId = 26;}
    }
    public class EsBitField : ParameterBitField
    {
        public EsBitField() : base(2) { ListId = 27;}
    }
}
