using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N64RomEditor.src.OpcodeMatrix
{
    public class Matrix
    {
        private int Stack = 0;
        private int[] MatrixArray = new int[50000];
        public void Fit(int id, List<BitField> bitFields, List<ParameterBitField> parameters)
        {
            List<BitField> appearanceFromBitFields = new List<BitField>();
        }
    }
}
