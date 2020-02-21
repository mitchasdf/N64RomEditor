using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace N64RomEditor.src.MIPS3Codec
{
    public class OpcodeFileParser
    {
        // A map of BitField type by bit field code
        private static readonly Dictionary<string, Type> TypeByBitFieldCode = new Dictionary<string, Type>()
        {
            { "OPCODE", typeof(OpcodeBitField) },
            { "EX_OPCODE", typeof(ExOpcodeBitField) },
            { "BASE", typeof(BaseBitField) },
            { "SA", typeof(SaBitField) },
            { "OP", typeof(OpBitField) },
            { "CS", typeof(CsBitField) },
            { "CO", typeof(CoBitField) },
            { "RS", typeof(RsBitField) },
            { "RT", typeof(RtBitField) },
            { "RD", typeof(RdBitField) },
            { "FS", typeof(FsBitField) },
            { "FT", typeof(FtBitField) },
            { "FD", typeof(FdBitField) },
            { "VS", typeof(VsBitField) },
            { "VT", typeof(VtBitField) },
            { "VD", typeof(VdBitField) },
            { "MOD", typeof(ModBitField) },
            { "OFFSET", typeof(OffsetBitField) },
            { "VOFFSET", typeof(VOffsetBitField) },
            { "FMT", typeof(FmtBitField) },
            { "CODE_10", typeof(Code10BitField) },
            { "CODE_20", typeof(Code20BitField) },
            { "IMMEDIATE", typeof(ImmediateBitField) },
            { "ADDRESS", typeof(AddressBitField) },
            //TODO: Add any missing BitField types (I added all of the ones used in the old opcodes)
        };

        /// <summary>
        /// Opens an file containing opcodes and parses all opcodes into a new List of Opcode
        /// </summary>
        /// <param name="filePath">The path to the opcodes file. Can be relative (e.g. "opcodes.ini")</param>
        /// <returns>Returns a list of `Opcode` objects</returns>
        public static List<Opcode> LoadOpcodesFromFile(string filePath)
        {
            List<Opcode> result = new List<Opcode>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("The opcodes file cannot be found at the given path... \n" +
                    "Make sure it is located in the same folder as the executable.");

            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                string trim = line.Trim();

                // if it's just an empty line or a comment, skip it
                if (trim.Length == 0 || trim.StartsWith("#"))
                    continue;

                // e.g. ADD
                string mnemonic = trim.Substring(1, trim.LastIndexOf("\'")-1);

                // e.g. [OPCODE, 0], RS, RT, RD, 5, [OPCODE, 32]
                string encoding = trim.Substring(trim.IndexOf('[') + 1).Replace(" ","");
                encoding = encoding.Substring(0, encoding.LastIndexOf("],["));

                // e.g. RD, RS, RT
                string appearance = trim.Substring(trim.LastIndexOf('[') + 1);
                appearance = appearance.Substring(0, appearance.LastIndexOf(']'));

                result.Add(GetOpcodeFromInfo(mnemonic, encoding, appearance));
            }

            return result;
        }

        private static string FlattenEncoding(string encoding, int level = 0)
        {
            int nextBracket = encoding.LastIndexOf('[');

            if (nextBracket == -1)
            {
                if (level > 0)
                {
                    return encoding.Replace(" ", "").Replace(",", "~");
                }
                else
                {
                    return encoding;
                }
            }

            string innerMostPart = encoding.Substring(nextBracket + 1);
            innerMostPart = innerMostPart.Substring(0, innerMostPart.IndexOf(']'));

            encoding = encoding.Replace($"[{innerMostPart}]", FlattenEncoding(innerMostPart, level + 1));
            while (encoding.LastIndexOf('[') != -1)
            {
                encoding = FlattenEncoding(encoding);
            }
            return encoding;
        }

        private static Opcode GetOpcodeFromInfo(string mnemonic, string encoding, string appearanceInfo)
        {
            // e.g. OPCODE~0, RS, RT, RD, 5, OPCODE~32
            encoding = FlattenEncoding(encoding);

            List<BitField> bitFields = new List<BitField>();
            List<ParameterBitField> appearance = new List<ParameterBitField>();

            foreach (var entry in encoding.Replace(" ", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                bitFields.Add(GetBitFieldFromFlattenedEntry<BitField>(entry));

            foreach (var entry in appearanceInfo.Replace(" ", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                appearance.Add(GetBitFieldFromFlattenedEntry<ParameterBitField>(entry));

            return new Opcode(mnemonic, bitFields, appearance);
        }

        private static T GetBitFieldFromFlattenedEntry<T>(string entry) where T : BitField
        {
            string bitField = entry.Split('~')[0];
            int number = -1;
            if (entry.Contains("~"))
                number = int.Parse(entry.Split('~')[1]);

            if (int.TryParse(bitField, out int zeroBitCount))
            {
                return (T)(BitField)new ZeroBitField(zeroBitCount);
            }
            else if (TypeByBitFieldCode.ContainsKey(bitField))
            {
                ConstructorInfo ctor = null;
                if (number != -1)
                {
                    ctor = TypeByBitFieldCode[bitField].GetConstructor(new Type[] { typeof(int) });

                    if (ctor == null)
                    {
                        ctor = TypeByBitFieldCode[bitField].GetConstructor(new Type[] { });
                        if (ctor != null)
                            return (T)ctor.Invoke(new object[] { });
                    }

                    if (ctor != null)
                        return (T)ctor.Invoke(new object[] { number });
                }
                else
                {
                    ctor = TypeByBitFieldCode[bitField].GetConstructor(new Type[] { });

                    if (ctor != null)
                        return (T)ctor.Invoke(new object[] { });
                }

                if (ctor == null)
                {
                    throw new Exception($"Unable to find contructor for class with the signature `{TypeByBitFieldCode[bitField].Name}({(number == -1 ? "" : "int")})`");
                }
            }
            else
            {
                throw new Exception($"`TypeByBitFieldCode` collection does not contain a BitField value for key `{bitField}`");
            }
            return null;
        }
    }
}