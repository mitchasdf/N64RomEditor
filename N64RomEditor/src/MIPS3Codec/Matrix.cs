using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.MIPS3Codec
{
    public class Matrix
    {
        private static int Stack = 0;
        private static int[] MatrixArray = new int[50000];
        public const int MatrixLayerSize = 64;
        public static void Fit(int id, List<BitField> bitFields, List<ParameterBitField> parameters, Opcode opcode)
        {
            int bitsAddressed = 0;
            int i = 0;
            List<List<OpcodeIdentifierFitmentMetas>> opcodeIdentifierFitmentMetas = new List<List<OpcodeIdentifierFitmentMetas>>();
            for (; i <= BitField.maximumPrecedenceSetting; i++)
                opcodeIdentifierFitmentMetas.Add(new List<OpcodeIdentifierFitmentMetas>());

            // Sort the paramaters from the identifiers and various other work for data required later
            foreach (BitField bf in bitFields)
            {
                if (bf.GetType().BaseType == typeof(IdentifierBitField))
                {
                    IdentifierBitField idbf = (IdentifierBitField)bf;
                    int identifier = idbf.Identifier;
                    if(!(bf.GetType() == typeof(ZeroBitField)))
                        opcode.BitwiseIdentity += identifier << (Opcode.InstructionSizeInBits - (bitsAddressed + bf.Length));
                    opcodeIdentifierFitmentMetas[idbf.Precedence].Add(new OpcodeIdentifierFitmentMetas(identifier, bf.Length, bitsAddressed));
                }
                bitsAddressed += bf.Length;
            }
            if (bitsAddressed != Opcode.InstructionSizeInBits)
                throw new Exception($"Instruction {opcode.Name} has {bitsAddressed} bits when Opcode.InstructionSizeInBits is {Opcode.InstructionSizeInBits}.");

            // The parameter unmasking metas should appear in order of the appearance parameters
            foreach (ParameterBitField pbf in parameters)
            {
                bitsAddressed = 0;
                foreach (BitField bf in bitFields)
                {
                    if (bf.GetType() == pbf.GetType())
                    {
                        int shiftAmount = Opcode.InstructionSizeInBits - (bf.Length + bitsAddressed);
                        int bitMaskAfterShift = (1 << bf.Length) - 1;
                        opcode.ParamaterUnmaskingMetas.Add(new OpcodeParamaterUnmaskingMetas(bitMaskAfterShift, shiftAmount, (ParameterBitField)bf));
                        break;
                    }
                    bitsAddressed += bf.Length;
                }
            }

            // Take all ZeroBitFields over the length of 6 and split them up into more ZeroBitFields with a max length of 6 each
            // The reason for this is 0b111_111 = 63 = the max length of our defined matrix array layers
            // Also, or else the one code with a ZeroBitField(20) would require a layer 0b1111_1111_1111_1111_1111 = 1,048,575‬ elements long
            // The algorithm also requires consistency among the layers, each will be 66 elements long, element 65 and 66 reserved for bit 
            //   masking data detailing how to traverse the particular layer.
            // It is a case of performance < RAM consumption, though this algorithm may be able to be tweaked, who knows.
            i = 0;
            // ZeroBitFields should always be at the max setting. They should be tested against after everything else.
            int max = BitField.maximumPrecedenceSetting;
            while (i < opcodeIdentifierFitmentMetas[max].Count)
            {
                int length = opcodeIdentifierFitmentMetas[max][i].Length;
                bitsAddressed = opcodeIdentifierFitmentMetas[max][i].BitsAddressed;
                if(length > 6)
                {
                    opcodeIdentifierFitmentMetas[max][i].Length = 6;
                    int amountOver = length - 6;
                    opcodeIdentifierFitmentMetas[max].Insert(i + 1, new OpcodeIdentifierFitmentMetas(0, amountOver, bitsAddressed + 6));
                }
                i++;
            }
            List<OpcodeIdentifierFitmentMetas> orderedIdentifierBitFieldMetas = new List<OpcodeIdentifierFitmentMetas>();
            foreach (List<OpcodeIdentifierFitmentMetas> bitFieldList in opcodeIdentifierFitmentMetas)
                orderedIdentifierBitFieldMetas.AddRange(bitFieldList);

            // Now the in the presence of the required data in the correct order, the matrix can be fitted
            i = 0;
            int offset = 0;
            foreach (OpcodeIdentifierFitmentMetas fitmentMeta in orderedIdentifierBitFieldMetas)
            {
                bool isFinalElement = ++i == orderedIdentifierBitFieldMetas.Count();
                int shiftAmount = Opcode.InstructionSizeInBits - (fitmentMeta.Length + fitmentMeta.BitsAddressed);
                int bitMask = (1 << fitmentMeta.Length) - 1;
                int offsetNewIndex = offset + fitmentMeta.NewTargetMatrixIndex;
                if (MatrixArray[offset + MatrixLayerSize] == 0)
                {
                    MatrixArray[offset + MatrixLayerSize] = bitMask;
                    MatrixArray[offset + MatrixLayerSize + 1] = shiftAmount;
                }
                if (isFinalElement)
                {
                    if (MatrixArray[offsetNewIndex] != 0) // This is bad
                    {
                        string errMsg = $"There's been a short while fitting the matrix.\n\n" +
                                        $"Instruction attempted to fit: {opcode.Name}\n";
                        if (MatrixArray[offsetNewIndex] < 0)
                            errMsg += $"Instruction that would've been written over: {Opcode.Opcodes[-MatrixArray[offsetNewIndex]].Name}";
                        else
                            errMsg += $"A part of the pathway to 1 more more instructions would've been overwritten.";
                        throw new Exception(errMsg);
                    }
                    MatrixArray[offsetNewIndex] = -id;
                    break;
                }
                if (MatrixArray[offsetNewIndex] == 0)
                {
                    Stack += MatrixLayerSize + 2; // +2 for the 2 bitMasking fields
                    MatrixArray[offsetNewIndex] = Stack;
                    offset = Stack;
                }
                else
                    offset = MatrixArray[offsetNewIndex];
            }
        }
        public static Opcode FindOpcodeFromByteCode(int fourBytesOfCode)
        {
            if (fourBytesOfCode == 0)
                return Opcode.Opcodes[0];
            int offset = 0;
            int value;
            while (true)
            {
                value = MatrixArray[
                    offset + 
                    ((fourBytesOfCode >> MatrixArray[offset + 65]) &
                    MatrixArray[offset + 64])
                ];
                if (value > 0)
                {
                    offset = value;
                    continue;
                }
                if (value < 0)
                    return Opcode.Opcodes[-value];
                return Opcode.Unidentifiable;
            }
        }
    }
}
