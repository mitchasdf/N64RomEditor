using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.MIPS3Codec
{
    public class RegisterMap
    {
        public static Dictionary<string, int> RegistersEncode { get; set; } = new Dictionary<string, int>();
        public static string[][] RegistersDecode { get; set; } = new string[4][] {
            new string[32], new string[32], new string[32], new string[32]
        };
        public static int EncodeRegister(string name)
        {
            int result;
            if (RegistersEncode.TryGetValue(name, out result))
            {
                return result;
            }
            return -1;
        }
        public static string DecodeRegister(int dimension1Index, int dimension2Index)
        {
            return RegistersDecode[dimension1Index][dimension2Index];
        }
        public static FunctionalRegister[][] Registers { get; set; } = new FunctionalRegister[4][] {
            new FunctionalRegister[32] {
                new FunctionalRegister("R0"),
                new FunctionalRegister("AT"),
                new FunctionalRegister("V0"),
                new FunctionalRegister("V1"),
                new FunctionalRegister("A0"),
                new FunctionalRegister("A1"),
                new FunctionalRegister("A2"),
                new FunctionalRegister("A3"),
                new FunctionalRegister("T0"),
                new FunctionalRegister("T1"),
                new FunctionalRegister("T2"),
                new FunctionalRegister("T3"),
                new FunctionalRegister("T4"),
                new FunctionalRegister("T5"),
                new FunctionalRegister("T6"),
                new FunctionalRegister("T7"),
                new FunctionalRegister("S0"),
                new FunctionalRegister("S1"),
                new FunctionalRegister("S2"),
                new FunctionalRegister("S3"),
                new FunctionalRegister("S4"),
                new FunctionalRegister("S5"),
                new FunctionalRegister("S6"),
                new FunctionalRegister("S7"),
                new FunctionalRegister("T8"),
                new FunctionalRegister("T9"),
                new FunctionalRegister("K0"),
                new FunctionalRegister("K1"),
                new FunctionalRegister("GP"),
                new FunctionalRegister("SP"),
                new FunctionalRegister("S8"),
                new FunctionalRegister("RA")
            },
            new FunctionalRegister[32] {
                new FunctionalRegister("F0"),
                new FunctionalRegister("F1"),
                new FunctionalRegister("F2"),
                new FunctionalRegister("F3"),
                new FunctionalRegister("F4"),
                new FunctionalRegister("F5"),
                new FunctionalRegister("F6"),
                new FunctionalRegister("F7"),
                new FunctionalRegister("F8"),
                new FunctionalRegister("F9"),
                new FunctionalRegister("F10"),
                new FunctionalRegister("F11"),
                new FunctionalRegister("F12"),
                new FunctionalRegister("F13"),
                new FunctionalRegister("F14"),
                new FunctionalRegister("F15"),
                new FunctionalRegister("F16"),
                new FunctionalRegister("F17"),
                new FunctionalRegister("F18"),
                new FunctionalRegister("F19"),
                new FunctionalRegister("F20"),
                new FunctionalRegister("F21"),
                new FunctionalRegister("F22"),
                new FunctionalRegister("F23"),
                new FunctionalRegister("F24"),
                new FunctionalRegister("F25"),
                new FunctionalRegister("F26"),
                new FunctionalRegister("F27"),
                new FunctionalRegister("F28"),
                new FunctionalRegister("F29"),
                new FunctionalRegister("F30"),
                new FunctionalRegister("F31")
            },
            new FunctionalRegister[32] {
                new FunctionalRegister("INDEX"),
                new FunctionalRegister("RANDOM"),
                new FunctionalRegister("ENTRYLO0"),
                new FunctionalRegister("ENTRYLO1"),
                new FunctionalRegister("CONTEXT"),
                new FunctionalRegister("PAGEMASK"),
                new FunctionalRegister("WIRED"),
                new FunctionalRegister("RESERVED0"),
                new FunctionalRegister("BADVADDR"),
                new FunctionalRegister("COUNT"),
                new FunctionalRegister("ENTRYHI"),
                new FunctionalRegister("COMPARE"),
                new FunctionalRegister("STATUS"),
                new FunctionalRegister("CAUSE"),
                new FunctionalRegister("EXCEPTPC"),
                new FunctionalRegister("PREVID"),
                new FunctionalRegister("CONFIG"),
                new FunctionalRegister("LLADDR"),
                new FunctionalRegister("WATCHLO"),
                new FunctionalRegister("WATCHHI"),
                new FunctionalRegister("XCONTEXT"),
                new FunctionalRegister("RESERVED1"),
                new FunctionalRegister("RESERVED2"),
                new FunctionalRegister("RESERVED3"),
                new FunctionalRegister("RESERVED4"),
                new FunctionalRegister("RESERVED5"),
                new FunctionalRegister("PERR"),
                new FunctionalRegister("CACHEERR"),
                new FunctionalRegister("TAGLO"),
                new FunctionalRegister("TAGHI"),
                new FunctionalRegister("ERROREPC"),
                new FunctionalRegister("RESERVED6")
            },
            new FunctionalRegister[32] {
                new FunctionalRegister("V00"),
                new FunctionalRegister("V01"),
                new FunctionalRegister("V02"),
                new FunctionalRegister("V03"),
                new FunctionalRegister("V04"),
                new FunctionalRegister("V05"),
                new FunctionalRegister("V06"),
                new FunctionalRegister("V07"),
                new FunctionalRegister("V08"),
                new FunctionalRegister("V09"),
                new FunctionalRegister("V10"),
                new FunctionalRegister("V11"),
                new FunctionalRegister("V12"),
                new FunctionalRegister("V13"),
                new FunctionalRegister("V14"),
                new FunctionalRegister("V15"),
                new FunctionalRegister("V16"),
                new FunctionalRegister("V17"),
                new FunctionalRegister("V18"),
                new FunctionalRegister("V19"),
                new FunctionalRegister("V20"),
                new FunctionalRegister("V21"),
                new FunctionalRegister("V22"),
                new FunctionalRegister("V23"),
                new FunctionalRegister("V24"),
                new FunctionalRegister("V25"),
                new FunctionalRegister("V26"),
                new FunctionalRegister("V27"),
                new FunctionalRegister("V28"),
                new FunctionalRegister("V29"),
                new FunctionalRegister("V30"),
                new FunctionalRegister("V31")
            }
        };
    }

    public class FunctionalRegister
    {
        string Name { get; set; }
        public FunctionalRegister(string name)
        {
            Name = name;
            new RegisterMapper(name);
        }
    }

    public class RegisterMapper
    {
        private static int Count = 0;
        public RegisterMapper(string name)
        {
            RegisterMap.RegistersDecode[Count / 32][Count % 32] = name;
            RegisterMap.RegistersEncode.Add(name, Count++ % 32);
        }
    }
}
