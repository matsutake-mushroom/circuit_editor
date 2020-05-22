using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 回路図エディタ
{
    class TextFormatter
    {
        private RichTextBox textbox;
        private Regex node_finder = new Regex(@"((\*|R|C|OP|SW|NPN|PNP|MFET)[0-9]*?)(\[[^\[\]]*\])??--");

        public TextFormatter(RichTextBox box)
        {
            textbox = box;
        }

        public void exec()
        {
            int current_start = textbox.SelectionStart;
            int current_length = textbox.SelectionLength;

            int pos = 0;
            int counter = 0;

            foreach (var line in textbox.Lines)
            {
                //コメント処理
                pos = line.IndexOf("//");
                if(pos>=0)
                {
                    textbox.Select(counter + pos, line.Length - pos);
                    textbox.SelectionColor = Color.DarkGreen;
                }
                //ノード処理
                var nodes = node_finder.Matches(line, 0);
                foreach(Match node in nodes)
                {
                    Console.WriteLine("node:" + node.Index + "," +node.Length);
                    pos = node.Index;
                    textbox.Select(counter + pos, node.Length-2);

                    var fontbase = textbox.SelectionFont;
                    textbox.SelectionFont = new Font(fontbase.FontFamily, fontbase.Size, fontbase.Style | FontStyle.Bold);
                }


                counter += line.Length + 1;

            }
            textbox.Select(current_start, current_length);

        }
    }
}
