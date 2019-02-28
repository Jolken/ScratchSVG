using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGRepair
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        
        

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog
            {
                Title = "Выберите файл",
                Filter = "Scalable Vector Graphics|*.svg",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fdlg.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SVGRepair(textBox1.Text);
        }

        static void SVGRepair(string file)
        {
            string readContent = "";
            using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
            {
                for (int i = 1; !streamReader.EndOfStream; i++)
                {
                    string line = streamReader.ReadLine();
                    if (i == 2)
                    {
                        if (line.StartsWith("<!--fixed-->"))
                        {
                            MessageBox.Show("Файл уже починен, если он не работает, свяжитесь с создателем", "Файл уже починен");
                            return;
                        }
                        else
                        {
                            line = "<!--fixed-->\n" + line;
                        }
                    }
                    readContent += line + "\n";
                }
                
            }
           
            List<string> st = new List<string>() { };
            Regex rx = new Regex(@"\.st.*?{(.*)}");
            MatchCollection matches = rx.Matches(readContent);
            for (int i = 0; i < matches.Count; i++)
            {
                string str = matches[i].ToString().Split('{')[1].Split('}')[0];   
                st.Add(str.Replace(":", "=\"").Replace(";", "\" "));
            }
            for (int i = 0; i < st.Count; i++)
            {
                readContent = Regex.Replace(readContent, $"st{i}\"", $"st{i}\" {st[i]}");
            }
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                outputFile.Write(readContent);
            }
        }
    }
}
