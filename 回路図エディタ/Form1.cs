using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 回路図エディタ
{
    public partial class Form1 : Form
    {
        private CircuitCompiler compiler;
        private TextFormatter formatter;
        private SvgDrawer drawer;
        public Form1()
        {
            InitializeComponent();
            compiler = new CircuitCompiler();
            formatter = new TextFormatter(richTextBox1);
            drawer = new SvgDrawer();
            
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                formatter.exec();
                var output = compiler.Compile(richTextBox1.Text);
                pictureBox1.Image = drawer.Draw(output);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
