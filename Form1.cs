using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FormalSpecification
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select a text file",
                Filter = "Text files (*.txt)|*.txt",
                Title = "Open text file"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                string readFile = File.ReadAllText(fileName);
                rtbInput.Text = readFile;
                txtClassName.Text = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                SettingCheckBox(cbImportInput, Color.Green);
            }
        }

        private void generatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rtbInput.Text.Contains("TH"))
            {
                GenerateType2.Initiate(rtbInput.Text);
                rtbOutput.Text = GenerateType2.GenerateCode(rtbOutput.Text, txtClassName.Text);        
            }
            else
            {
                GenerateTypeI.Initiate(rtbInput.Text);
                rtbOutput.Text = GenerateTypeI.GenerateCode(rtbInput.Text, txtClassName.Text);
            }
            SettingCheckBox(checkboxGenerate, Color.Green);
        }


        private void Build_Click(object sender, EventArgs e)
        {
            CSharpCodeProvider code = new CSharpCodeProvider();
            ICodeCompiler iCodeCompiler = code.CreateCompiler();
            string program = txbExeFileName.Text + ".exe";

            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = program;

            CompilerResults results = iCodeCompiler.CompileAssemblyFromSource(parameters, rtbOutput.Text);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError error in results.Errors)
                {
                    Console.WriteLine(error.Line + "\t" + error.ErrorText + Environment.NewLine);
                }
            }
            else
            {
                Process.Start(program);
                lbSuccess.Visible = true;
            }

        }

        private void hightlightCode(RichTextBox rtb)
        {
    
            string keywords = @"\b(public|private|partial|static|namespace|class|using|void|foreach|in|pre|post)\b";
            MatchCollection keywordMatches = Regex.Matches(rtb.Text, keywords);

         
            string types = @"\b(Console)\b";
            MatchCollection typeMatches = Regex.Matches(rtb.Text, types);

           
   
            string comments = @"(\/\/.+?$|\/\*.+?\*\/)";
            MatchCollection commentMatches = Regex.Matches(rtb.Text, comments, RegexOptions.Multiline);

  
            string strings = "\".+?\"";
            MatchCollection stringMatches = Regex.Matches(rtb.Text, strings);

            string conditions = @"\b(if|else|return|for)\b";
            MatchCollection conditionMatches = Regex.Matches(rtb.Text, conditions);


            string others = @"\b(WriteLine|ReadLine|Parse|Write|Read)\b";
            MatchCollection otherMatches = Regex.Matches(rtb.Text, others);

            // saving the original caret position + forecolor
            int originalIndex = rtb.SelectionStart;
            int originalLength = rtb.SelectionLength;
            Color originalColor = Color.Black;

            // MANDATORY - focuses a label before highlighting (avoids blinking)
            rtb.Focus();

            // removes any previous highlighting (so modified words won't remain highlighted)
            rtb.SelectionStart = 0;
            rtb.SelectionLength = rtb.Text.Length;
            rtb.SelectionColor = originalColor;

            // scanning...
            foreach (Match m in keywordMatches)
            {
                rtb.SelectionStart = m.Index;
                rtb.SelectionLength = m.Length;
                rtb.SelectionColor = Color.Blue;
            }

            foreach (Match m in typeMatches)
            {
                rtb.SelectionStart = m.Index;
                rtb.SelectionLength = m.Length;
                rtb.SelectionColor = Color.DarkCyan;
            }

            foreach (Match m in commentMatches)
            {
                rtb.SelectionStart = m.Index;
                rtb.SelectionLength = m.Length;
                rtb.SelectionColor = Color.Green;
            }

            foreach (Match m in stringMatches)
            {
                rtb.SelectionStart = m.Index;
                rtb.SelectionLength = m.Length;
                rtb.SelectionColor = Color.Brown;
            }

            foreach (Match m in conditionMatches)
            {
                rtb.SelectionStart = m.Index;
                rtb.SelectionLength = m.Length;
                rtb.SelectionColor = Color.BlueViolet;
            }         
            foreach (Match m in otherMatches)
            {
                rtb.SelectionStart = m.Index;
                rtb.SelectionLength = m.Length;
                rtb.SelectionColor = Color.Coral;
            }
            // restoring the original colors, for further writing
            rtb.SelectionStart = originalIndex;
            rtb.SelectionLength = originalLength;
            rtb.SelectionColor = originalColor;

            // giving back the focus
            rtb.Focus();
        }

        private void SettingCheckBox(CheckBox checkBox, Color color)
        {
            checkBox.Checked = true;
            checkBox.ForeColor = color;
        }
        private void rtbOutput_TextChanged(object sender, EventArgs e)
        {
            hightlightCode(rtbOutput);
    
        }

        private void rtbInput_TextChanged(object sender, EventArgs e)
        {
            hightlightCode(rtbInput);
        }

        private void New_Click(object sender, EventArgs e)
        {
            rtbInput.SelectionColor = Color.Black;
            if (rtbInput.Text == string.Empty && rtbInput.Text == string.Empty)
                return;
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn lưu file này không?", "Chưa lưu!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                SaveFile();
            }
            else if (dialogResult == DialogResult.No)
            {
                txtClassName.Text = "";
                rtbInput.Text = "";
                rtbOutput.Text = "";
                cbImportInput.Checked = false;
                checkboxGenerate.Checked = false;
            }
        }
        private void SaveFile()
        {
            saveFileDialog1.Filter = String.Format("{0}|.txt", txtClassName.Text);
            if (saveFileDialog1.Filter == "")
                saveFileDialog1.Filter = "Program|.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                sw.WriteLine(rtbOutput.Text);
                sw.Close();
                DialogResult dialogResult2 = MessageBox.Show("Đã lưu thành công", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dialogResult2 == DialogResult.OK)
                {
                    return;
                }
                txtClassName.Text = "";
                rtbInput.Text = "";
                rtbOutput.Text = "";
                cbImportInput.Checked = false;
                checkboxGenerate.Checked = false;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

    }
}
