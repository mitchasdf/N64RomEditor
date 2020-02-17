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
            foreach(var opcode in Opcode.Opcodes)
            {
                Debug.WriteLine($"\r\n\r\nLoaded: {opcode.Name}\r\nBitFields:");

                foreach (var bitField in opcode.BitFields)
                    Debug.WriteLine($"\t{bitField.GetType()} ({bitField.Length})");

                Debug.WriteLine($"\r\nAppearance:");

                foreach (var bitField in opcode.Appearance)
                    Debug.WriteLine($"\t{bitField.GetType()} ({bitField.Length})");
            }
        }
    }
}
