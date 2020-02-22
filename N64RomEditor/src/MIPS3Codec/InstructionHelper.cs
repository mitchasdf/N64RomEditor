using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.MIPS3Codec
{
    public class InstructionHelper
    {
        public static List<InstructionHelper> Helpers { get; set; } = new List<InstructionHelper>();
        public static UnidentifiableInstruction Unidentifiable = new UnidentifiableInstruction();
        public static Dictionary<string, int> NamesToIndices = new Dictionary<string, int>();
        public string Name { get; set; } = "";
        public string Descriptor { get; set; } = "";
        public int ListId { get; set; } = 0;
        public List<BitField> BitFields { get; set; } = new List<BitField>();
        public List<ParameterBitField> Appearance { get; set; } = new List<ParameterBitField>();
        public int BitwiseIdentity { get; set; } = 0;
        public List<OpcodeParamaterUnmaskingMetas> ParamaterUnmaskingMetas { get; set; } = new List<OpcodeParamaterUnmaskingMetas>();
        public bool IsBranchLikely { get; set; }
        private static string[] BranchLikelies = new string[10] {
            "BEQL", "BGEZALL", "BGEZL", "BGTZL", "BLEZL", "BLTZALL", "BLTZL", "BNEL", "BC1FL", "BC1TL"
        };
        public bool IsFloatBranch { get; set; }
        private static string[] FloatBranches = new string[4] { 
            "BC1F", "BC1FL", "BC1T", "BC1TL" 
        };
        public bool IsLoadOrStore { get; set; }
        private static string[] LoadOrStores = new string[28] {
            "LB", "LBU", "LD", "LDL", "LDR","LH", "LHU", "LL", "LLD", "LW", "LWL",
            "LWR","LWU","SB", "SC", "SCD", "SD", "SDL", "SDR", "SH", "SW", "SWL", "SWR",
            "CACHE", "LDC1", "LWC1", "SDC1", "SWC1"
        };
        public bool IsJump { get; set; }
        private static string[] Jumps = new string[4] { 
            "J", "JR", "JAL", "JALR"
        };
        public bool IsBranch { get; set; }
        private static string[] Branches = new string[20] {
            "BEQ", "BEQL", "BGEZ", "BGEZAL", "BGEZALL", "BGEZL", "BGTZ", "BGTZL", "BLEZ", "BLEZL",
            "BLTZ", "BLTZAL", "BLTZALL", "BLTZL", "BNEL", "BNE", "BC1F", "BC1FL", "BC1T", "BC1TL"
        };
        public bool ContainsAddress { get; set; }
        private static string[] ContainersOfAddresses = new string[22] {
            "BC1F", "BC1FL", "BC1T", "BC1TL", "BEQ", "BEQL", "BGEZ", "BGEZAL", "BGEZALL",
            "BGEZL", "BGTZ", "BGTZL", "BLEZ", "BLEZL", "BLTZ", "BLTZAL", "BLTZALL", "BLTZL",
            "BNEL", "BNE", "J", "JAL"
        };
        public bool IsBitShift { get; set; }
        private static string[] BitShifts = new string[9] {
            "DSLL", "DSLL32", "DSRA", "DSRA32", "DSRL", "DSRL32", "SLL", "SRA", "SRL"
        };
        public InstructionHelper(string name, int listId, List<BitField> bitFields, List<ParameterBitField> appearance)
        {
            Name = name;
            ListId = listId;
            BitFields = bitFields;
            Appearance = appearance;

            if (GetType() == typeof(UnidentifiableInstruction)) return;

            NamesToIndices.Add(name, listId);

            IsBranchLikely = CheckIfPartOfGroup(name, BranchLikelies);
            IsFloatBranch = CheckIfPartOfGroup(name, FloatBranches);
            IsLoadOrStore = CheckIfPartOfGroup(name, LoadOrStores);
            IsJump = CheckIfPartOfGroup(name, Jumps);
            IsBranch = CheckIfPartOfGroup(name, Branches);
            ContainsAddress = CheckIfPartOfGroup(name, ContainersOfAddresses);
            IsBitShift = CheckIfPartOfGroup(name, BitShifts);

            if (InstructionDescriptors.Descriptors.TryGetValue(name, out string result))
                Descriptor = result;

            Helpers.Add(this);
        }
        private bool CheckIfPartOfGroup(string name, string[] group) {
            return group.Count(member => name == member) > 0;
        }
        public Instruction GetNewInstructionObject(int byteCode, int index)
        {
            return new Instruction(this, byteCode, index);
        }
        public static bool InstructionExists(string name)
        {
            return NamesToIndices.ContainsKey(name);
        }
        public static InstructionHelper GetHelperByName(string name)
        {
            if(NamesToIndices.TryGetValue(name, out int value))
            {
                return Helpers[value];
            }
            return new UnidentifiableInstruction(name);
        }
    }
    public class UnidentifiableInstruction : InstructionHelper
    {
        public UnidentifiableInstruction() : base("", -1, new List<BitField>(), new List<ParameterBitField>()) { }
        public UnidentifiableInstruction(string name) : base(name, -1, new List<BitField>(), new List<ParameterBitField>()) { }
    }
}
