using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 回路図エディタ
{
    public enum Order
    {
        p,
        n,
        u,
        m,
        k,
        M,
        G,
        T
    }
    public enum Unit
    {
        Ohm,
        F,
        H,
        V,
        A
    }


    public class CircuitCompiler
    {
        private readonly Regex regex = new Regex(@"(?<=[(（\[]).*?(?=[)）\]])");
        private readonly Regex find_node = new Regex(@"(\*|R|C|OP|SW|NPN|PNP|MFET)[0-9]*?(\[[^\[\]]*\])??");
        public void Compile(string source)
        {
            var lines = source.Split('\n');
            foreach (var line in lines)
            {
                var trials = find_node.Matches(line);
                foreach (Match trial in trials)
                {
                    Console.Write(trial.Value + ",");
                }

                

                var nodes = line.Split(new string[] { "--" },StringSplitOptions.RemoveEmptyEntries);

                bool contains_VCC = false;
                bool contains_GND = false;
                var nodelist = new List<CircuitObject>();

                foreach (var nodeName in nodes)
                {
                    switch (nodeName[0])
                    {
                        case '*':
                            Console.WriteLine("*");
                            Spot sp = new Spot();
                            break;
                        case 'R':
                            Console.WriteLine("Register");
                            Register r = new Register();
                            var arguments = regex.Match(nodeName).Value;
                            var arg_list = arguments.Split(',');
                            int argc = arg_list.Length;
                            for(int i =0; i<argc; ++i)
                            {
                                string arg = arg_list[i];
                                Console.WriteLine(arg);
                                switch (i)
                                {
                                    case 0:
                                        try
                                        {
                                            r.value = Convert.ToDouble(arg);
                                        }
                                        catch (Exception ee)
                                        {
                                            Console.WriteLine(ee.Message);
                                            r.value = 0.0;
                                        }

                                        break;
                                    case 1:
                                        r.name = arg;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            nodelist.Append(r);
                            break;
                        case 'C':
                            Console.WriteLine("Capacitor");
                            Capacitor c = new Capacitor();

                            var arguments2 = regex.Match(nodeName).Value;
                            var arg_list2 = arguments2.Split(',');
                            int argc2 = arg_list2.Length;
                            for (int i = 0; i < argc2; ++i)
                            {
                                string arg = arg_list2[i];
                                Console.WriteLine(arg);
                                switch (i)
                                {
                                    case 0:
                                        try
                                        {
                                            c.value = Convert.ToDouble(arg);
                                        }
                                        catch (Exception ee)
                                        {
                                            Console.WriteLine(ee.Message);
                                            c.value = 0.0;
                                        }

                                        break;
                                    case 1:
                                        c.name = arg;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            nodelist.Append(c);
                            break;
                        case 'G':
                            if (nodeName.Substring(0, 3) == "GND")
                            {
                                Console.WriteLine("GND");
                                GND gnd = new GND();
                                contains_GND = true;
                                nodelist.Append(gnd);
                            }
                            break;

                        default:
                            break;
                    }
                }

                //位置整形


            }
        }
    }


    public class Node
    {
        public int x;
        public int y;
        public bool connected;

        public Node(int _x, int _y)
        {
            x = _x;
            y = _y;
            connected = false;
        }

        public void Connect(ref Node other)
        {
            this.connected = true;
            other.connected = true;
        }
    }





    public class CircuitObject
    {
        private int x;
        private int y;
        private int height;
        private int width;
        public string name;
        private List<Node> nodes;

        public void Move(int dx, int dy)
        {
            x += dx;
            y += dy;
            foreach (var n in nodes)
            {
                n.x += dx;
                n.y += dy;
            }
        }


        public void Rotate90()
        {
            int prev_x = x, prev_y = y, prev_h = height;
            x = -prev_y;
            y = prev_x;
            height = width;
            width = prev_h;

            foreach (var node in nodes)
            {
                prev_x = node.x;
                node.x = -node.y;
                node.y = prev_x;
            }
        }

        public void Symmetry()
        {

        }


        public CircuitObject(int _height, int _width)
        {
            x = 0;
            y = 0;
            height = _height;
            width = _width;
        }
    }

    public class Spot : CircuitObject
    {
        public Spot() : base(0, 0)
        {
            ;
        }
    }

    public class Register : CircuitObject
    {
        public double value;
        private Order order;
        private Unit unit;
        

        public Register() : base(2, 8)
        {
            ;
        }

    }
    public class Capacitor : CircuitObject
    {
        public double value;
        private Order order;
        private Unit unit;


        public Capacitor() : base(2, 8)
        {
            ;
        }

    }

    public class GND : CircuitObject
    {
        public GND() : base(4, 4)
        {
            ;
        }
    }

}