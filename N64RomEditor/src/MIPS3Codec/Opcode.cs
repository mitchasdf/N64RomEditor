using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace N64RomEditor.src.MIPS3Codec
{
    public class Opcode
    {
        public const int InstructionSizeInBits = 32;
        public static bool Loaded { get; set; } = false;
        public string Name { get; set; } = "";
        public int ListId { get; set; } = 0;
        public Opcode(string name, List<BitField> bitFields, List<ParameterBitField> appearance)
        {
            if (name == "")
                return;
            // Check that all items within both BitField arrays are valid
            int invalidCount = appearance.Count(x => bitFields.Count(y => y.GetType() == x.GetType()) == 0);
            if (invalidCount > 0)
            {
                throw new Exception(
                        $"All items within the appearance list must also be within the bitFields list.\r\n" +
                        $"Failed [{invalidCount}/{appearance.Count}]");
            }
            ListId = DecodedInstructionHelper.Instructions.Count;
            Name = name;
            Matrix.Fit(this, bitFields, appearance);
        }
        public static void InstantiateOpcodes()
        {
            if (!Loaded)
            {
                OpcodeFileParser.LoadOpcodesFromFile("MIPS3Opcodes.ini");
                Loaded = true;
            }
        }
    }
    public class OpcodeParamaterUnmaskingMetas
    {
        public int BitMaskAfterShift { get; set; }
        public int ShiftAmount { get; set; }
        public ParameterBitField Parameter { get; set; }
        public OpcodeParamaterUnmaskingMetas(int mask, int shift, ParameterBitField param)
        {
            BitMaskAfterShift = mask;
            ShiftAmount = shift;
            Parameter = param;
        }
    }
    public class OpcodeIdentifierFitmentMetas
    {
        public int NewTargetMatrixIndex { get; set; }
        public int Length { get; set; }
        public int BitsAddressed { get; set; }
        public OpcodeIdentifierFitmentMetas(int target, int len, int addressed)
        {
            NewTargetMatrixIndex = target;
            Length = len;
            BitsAddressed = addressed;
        }
    }
}
