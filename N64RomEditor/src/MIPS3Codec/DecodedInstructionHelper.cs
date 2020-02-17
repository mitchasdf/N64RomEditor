using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.MIPS3Codec
{
    public class DecodedInstructionHelper
    {
        public static List<DecodedInstructionHelper> Instructions { get; set; } = new List<DecodedInstructionHelper>();
        public static UnidentifiableInstruction Unidentifiable = new UnidentifiableInstruction();
        public string Name { get; set; } = "";
        public int ListId { get; set; } = 0;
        public List<BitField> BitFields { get; set; } = new List<BitField>();
        public List<ParameterBitField> Appearance { get; set; } = new List<ParameterBitField>();
        public int BitwiseIdentity { get; set; } = 0;
        public List<OpcodeParamaterUnmaskingMetas> ParamaterUnmaskingMetas { get; set; } = new List<OpcodeParamaterUnmaskingMetas>();

        public DecodedInstructionHelper()
        {
            if (GetType() == typeof(UnidentifiableInstruction)) return;
            Instructions.Add(this);
        }
    }
    public class UnidentifiableInstruction : DecodedInstructionHelper
    {
        public UnidentifiableInstruction() : base() { }
    }
}
