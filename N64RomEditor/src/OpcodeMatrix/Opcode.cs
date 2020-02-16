using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace N64RomEditor.src.OpcodeMatrix
{
    public class Opcode
    {
        public const int InstructionSizeInBits = 32;
        public static List<Opcode> Opcodes { get; set; } = new List<Opcode>();
        public static bool Loaded { get; set; } = false;

        public string Name { get; set; } = "";
        public List<BitField> BitFields { get; set; } = new List<BitField>();
        public List<ParameterBitField> Appearance { get; set; } = new List<ParameterBitField>();
        public int BitwiseIdentity { get; set; } = 0;
        public int ListId { get; set; } = 0;
        public List<OpcodeParamaterMaskingMetas> ParamaterMaskingMetas { get; set; } = new List<OpcodeParamaterMaskingMetas>();
        public Opcode(string name, List<BitField> bitFields, List<ParameterBitField> appearance)
        {
            // Check that all items within both BitField arrays are valid
            int invalidCount = appearance.Count(x => bitFields.Count(y => y.GetType() == x.GetType()) == 0);
            if (invalidCount > 0)
            {
                throw new Exception(
                        $"All items within the appearance list must also be within the bitFields list.\r\n" +
                        $"Failed [{invalidCount}/{appearance.Count}]");
            }
            Name = name;
            Matrix.Fit(Opcodes.Count, bitFields, appearance, this);
            Opcodes.Add(this);
        }
        public static void InstantiateOpcodes()
        {
            if (!Loaded)
            {
                string filePath = Directory.GetCurrentDirectory() + "\\..\\..\\src\\OpcodeMatrix\\";
                OpcodeFileParser.LoadOpcodesFromFile("opcodes.ini");
                Loaded = true;
            }
        }
    }
    public class OpcodeParamaterMaskingMetas
    {
        public int BitMaskAfterShift { get; set; } = 0;
        public int ShiftAmount { get; set; } = 0;
        public int ListId { get; set; } = 0;
        public OpcodeParamaterMaskingMetas(int mask, int shift, int listId)
        {
            BitMaskAfterShift = mask;
            ShiftAmount = shift;
            ListId = ListId;
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
