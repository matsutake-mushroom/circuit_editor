using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Svg;

namespace 回路図エディタ
{
    class SvgDrawer
    {
        //class変数
        private static SvgDocument document;

        static SvgDrawer()
        {
            document = new SvgDocument
            {
                Width = 1000,
                Height = 500,
            };
        }
        //instance
        private SvgViewBox viewbox;

        public Image Draw(List<CircuitObject> objects)
        {
            document.ViewBox = new SvgViewBox(-250,-250, 250, 250);
            foreach (var obj in objects)
            {
                document.Children.Add(obj.Draw());
            }
            

            return document.Draw();
        }


    }
}
