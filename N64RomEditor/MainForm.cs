using N64RomEditor.src.MIPS3Codec;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace N64RomEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Opcode.InstantiateOpcodes();
            Debug.WriteLine($"\r\n\r\nFitted Instructions = {DecodedInstructionHelper.Instructions.Count}");
            Debug.WriteLine($"Matrix layer count = {Matrix.Stack / (Matrix.MatrixLayerSize + 2)}");
            Debug.WriteLine($"Matrix array element consumption = {Matrix.Stack + Matrix.MatrixLayerSize + 2}");
            
            foreach(var instruction in DecodedInstructionHelper.Instructions)
            {
                Debug.WriteLine($"\r\n\r\nLoaded: {instruction.Name}");
                Debug.WriteLine($"Index = {instruction.ListId}");
                Debug.WriteLine("BitFields:");

                foreach (var bitField in instruction.BitFields)
                    Debug.WriteLine($"\t{bitField.GetType()} ({bitField.Length})");

                Debug.WriteLine($"\r\nAppearance:");

                foreach (var bitField in instruction.Appearance)
                    Debug.WriteLine($"\t{bitField.GetType()} ({bitField.Length})");
            }
        }
    }
}
