using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalSpecification
{
    class GenerateType2
    {
        static string[] line;
        static string functionName;
        static List<string> outputs;
        //static string[] postCon;

        public static void Initiate(string input)
        {
            line = TestInputHandle.SplitTestInput(input);
            functionName = TestInputHandle.GetFunctionNameOfFunctionString(line[0]);
            outputs = new List<string>();
        }
        public static string GenerateCode(string input, string classname)
        {
            string s = "";
            s += "using System; \n";
            s += "namespace FormalSpecification \n";
            s += "{\n";
            s += "\tpublic class " + classname;
            s += "\n\t{\n";
            Generate(outputs, input, classname);
            for (int i = 0; i < outputs.Count; i++)
            {
                s += outputs[i];
            }
            s += "\n\t}\n";
            s += "}\r";
            return s;
        }

        private static void Generate(List<string> outputs, string input, string classname)
        {
            GenerateInputFunction(outputs, input);
            GenerateOutputFunction(outputs, input);
            GenerateCheckFunction(outputs, input);
            GenerateExecuteFunction(outputs, input);
            GenerateMain(outputs, input, classname);
        }

        private static void GenerateOutputFunction(List<string> outputs, string input)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            string outputFunction = String.Empty;
            outputFunction += "\t\tpublic void Xuat_" + functionName + "(" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + ")\n";
            outputFunction += "\t\t{\n";
            outputFunction += "\t\t\tConsole.WriteLine(\"Ket qua la: {0} \", " + result.FactorName + ");\n";
            outputFunction += "\t\t}\n\n";
            outputs.Add(outputFunction);
        }

        private static void GenerateInputFunction(List<string> outputs, string input)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            string inputFunction = String.Empty;
            inputFunction += "\t\tpublic void Nhap_" + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {

                inputFunction += "ref" + " " + FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName;
                if (i < f.Count - 1)
                {
                    inputFunction += ", ";
                }
            }
            inputFunction += ")\n";
            inputFunction += "\t\t{\n";
            string arrayLength = string.Empty;
            for (int i = 0; i <f.Count; i++)
            {
 
                if (f[i].FactorType == "N" || f[i].FactorType == "Q" || f[i].FactorType == "Z")
                {
                    arrayLength = f[i].FactorName;
                    inputFunction += "\t\t\tConsole.Write(\"Nhap " + f[i].FactorName +": \");\n";
                    inputFunction += "\t\t\t" + f[i].FactorName + " = " + FactorType.ChangeFactorType(f[i].FactorType) + ".Parse(Console.ReadLine());\n";
                }    
            }    
            for (int i = 0; i < f.Count; i++)
            {
                if (f[i].FactorType == "N*" || f[i].FactorType == "Z*" || f[i].FactorType =="R*")
                {
                    inputFunction += "\t\t\tConsole.WriteLine(\"Nhap phan tu cho mang \");\n";
                    inputFunction += "\t\t\t" + f[i].FactorName + " = new " + FactorType.ChangeFactorType(f[i].FactorType).Replace("[","").Replace("]","") +"[" +arrayLength +"];\n";
                    inputFunction += "\t\t\t" + "for (int i = 0; i < " + arrayLength + "; i++)\n";
                    inputFunction += "\t\t\t{\n";
                    inputFunction += "\t\t\t\t" + "Console.Write(\"Nhap phan tu thu {0}: \", i+1);\n";
                    inputFunction += "\t\t\t\t" + f[i].FactorName + "[i] = " + FactorType.ChangeFactorType(f[i].FactorType).Replace("[", "").Replace("]", "") + ".Parse(Console.ReadLine());\n";
                    inputFunction += "\t\t\t}\n";
                }    
            }
            inputFunction += "\t\t}\n\n";
            outputs.Add(inputFunction);
        }

        private static void GenerateMain(List<string> outputs, string input, string classname)
        {
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);

            // main function;
            string mainFunction = String.Empty;
            mainFunction += "\n\t\tpublic static void Main(string[] args)\n";
            mainFunction += "\t\t{\n";
            for (int i = 0; i < f.Count; i++)
            {
                if (f[i].FactorType == "N*" || f[i].FactorType == "Z*" || f[i].FactorType == "R*")
                {
                    mainFunction += "\t\t\t" + FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName + " = {}" + ";\n";
                }
                else if (f[i].FactorType == "char*")
                {
                    mainFunction += "\t\t\t" + FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName + " = null" + ";\n";
                }    
                else
                {
                    mainFunction += "\t\t\t" + FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName + " = 0" + ";\n";
                }    
            }
            if (result.FactorType == "B")
            {
                mainFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = true" + ";\n";
            }
            else if (result.FactorType == "char*")
            {
                mainFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = false" + ";\n";
            }
            else
            {
                mainFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = 0" + ";\n";
            }    
            mainFunction += "\t\t\t" + classname + " p = new " + classname + "();\n";
            mainFunction += "\t\t\t" + "p.Nhap_" + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {
                mainFunction += "ref " + f[i].FactorName;
                if (i < f.Count - 1)
                {
                    mainFunction += ",";
                }
            }

            mainFunction += ");\n";
            mainFunction += "\t\t\tif(" + "p.Kiemtra_" + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {
                mainFunction += f[i].FactorName;
                if (i < f.Count - 1)
                {
                    mainFunction += ",";
                }
            }

            // If statement
            mainFunction += ")== true)\n";
            mainFunction += "\t\t\t{\n";
            mainFunction += "\t\t\t\t" + result.FactorName + " = " + "p." + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {
                mainFunction += f[i].FactorName;
                if (i < f.Count - 1)
                {
                    mainFunction += ",";
                }
            }
            mainFunction += ");\n";
            mainFunction += "\t\t\t\tp.Xuat_" + functionName + "(" + result.FactorName + ");\n";
            mainFunction += "\t\t\t}\n";
            mainFunction += "\t\t\telse\n";
            mainFunction += "\t\t\t{\n";
            mainFunction += "\t\t\t\tConsole.WriteLine" + "(\"Thong tin nhap khong hop le\");\n";
            mainFunction += "\t\t\t}\n";
            mainFunction += "\t\t\tConsole.ReadLine();\n";
            mainFunction += "\t\t}\n";
            outputs.Add(mainFunction);
        }

        private static void GenerateExecuteFunction(List<string> outputs, string input)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            string executeFunction = string.Empty;
            string[] postCon = TestInputHandle.HandlingType2(line[2]);
            executeFunction += "\t\tpublic " + FactorType.ChangeFactorType(result.FactorType) + " " + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {
                executeFunction += FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName;
                if (i < f.Count - 1)
                {
                    executeFunction += ", ";
                }
            }
            executeFunction += ")\n";
            executeFunction += "\t\t{\n";
            if (postCon.Length == 2)
            {
                executeFunction = OneLoopPost(executeFunction, postCon[0]);
            }
            else if (postCon.Length == 3)
            {
                executeFunction = TwoLoopPost(executeFunction, postCon[0], postCon[1]);
            }    
            executeFunction += "\n\t\t}\n";

            outputs.Add(executeFunction);
        }



        private static void GenerateCheckFunction(List<string> outputs, string input)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            string preCon = TestInputHandle.GetPreConditionString(line[1]);
            string checkCondition = string.Empty;
            if (preCon != "")
            {
                checkCondition += "\t\tpublic bool " + "Kiemtra_" + functionName + "(";
                for (int i = 0; i < f.Count; i++)
                {

                    checkCondition += FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName;
                    if (i < f.Count - 1)
                    {
                        checkCondition += ", ";
                    }
                }
                checkCondition += ")\r\n";
                checkCondition += "\t\t{\r\n";
                checkCondition += "\t\t\tif (" + preCon + ")\r\n";
                checkCondition += "\t\t\t{\r\n";
                checkCondition += "\t\t\t\treturn true;\r\n";
                checkCondition += "\t\t\t}\r\n";
                checkCondition += "\t\t\telse return false;\r\n";
                checkCondition += "\t\t}\r\n";
            }
            else
            {
                checkCondition += "\t\tpublic bool " + "Kiemtra_" + functionName + "(";
                for (int i = 0; i < f.Count; i++)
                {

                    checkCondition += FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName;
                    if (i < f.Count - 1)
                    {
                        checkCondition += ", ";
                    }
                }
                checkCondition += ")";
                checkCondition += "\r\n\t\t{\r\n";
                checkCondition += "\t\t\treturn true;";
                checkCondition += "\r\n\t\t}\r\n";

            }
            outputs.Add(checkCondition);
        }



        #region One Loop Execution
        public static string OneLoopPost(string oneLoop, string loopInfor)
        {
            if (loopInfor.Contains("VM"))
            {
                oneLoop = OneLoopVM(oneLoop);
            }    
            else if (loopInfor.Contains("TT"))
            {
                oneLoop = OneLoopTT(oneLoop);
            }    
            return oneLoop;
        }
        private static string OneLoopTT(string oneLoop)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor a = new Factor();
            foreach (var factor in f)
            {
                if (factor.FactorType.Contains("*"))
                {
                    a.FactorName = factor.FactorName;
                }

            }
            string arrayName = a.FactorName.Replace("[", string.Empty).Replace("]", string.Empty);
            string variable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[0]).Replace("(", string.Empty).Replace(")", string.Empty);
            string[] range = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[0]);
            string condition = TestInputHandle.HandlingType2(line[2])[1].Replace("(", "[").Replace(")", "]");
            oneLoop += "\t\t\tfor (int " + variable + " = " + range[0] + "-1; " + variable + "<=" + range[1] + "-1; " + variable + "++)\n";
            oneLoop += "\t\t\t{\n";
            oneLoop += "\t\t\t\tif (" + condition + ")\n";
            oneLoop += "\t\t\t\t{\n";
            oneLoop += "\t\t\t\t\treturn true;\n";
            oneLoop += "\t\t\t\t}\n";
            oneLoop += "\t\t\t}\n";
            oneLoop += "\t\t\treturn false;\n";
            return oneLoop;
        }

        public static string OneLoopVM(string oneLoop)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor a = new Factor();
            foreach (var factor in f)
            {
                if (factor.FactorType.Contains("*"))
                {
                    a.FactorName = factor.FactorName;
                } 
                    
            }
            string arrayName = a.FactorName.Replace("[", string.Empty).Replace("]", string.Empty);
            string variable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[0]).Replace("(",string.Empty).Replace(")",string.Empty);
            string[] range = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[0]);
            string condition = TestInputHandle.HandlingType2(line[2])[1].Replace("(", "[").Replace(")", "]");
     
            oneLoop += "\t\t\tfor (int " + variable + " = " + range[0] + "-1; " + variable + "<=" + range[1] + "-1; " + variable + "++)\n";  
            oneLoop += "\t\t\t{\n";
            oneLoop += "\t\t\t\tif (" + condition + "){}\n";
            oneLoop += "\t\t\t\telse return false;\n";
            oneLoop += "\t\t\t}\n";
            oneLoop += "\t\t\treturn true;";
            return oneLoop;
        }
        #endregion


        #region Two Loop Execution
        private static string TwoLoopPost(string twoLoop, string firstLoopInfor, string secondLoopInfor)
        {
            if (firstLoopInfor.Contains("VM") && secondLoopInfor.Contains("VM"))
            {
                twoLoop = TwoLoopVM_VM(twoLoop);
            }
            else if (firstLoopInfor.Contains("TT") && secondLoopInfor.Contains("TT"))
            {
                twoLoop = TwoLoopTT_TT(twoLoop);
            }
            else if (firstLoopInfor.Contains("TT") && secondLoopInfor.Contains("VM"))
            {
                twoLoop = twoLoopTT_VM(twoLoop);
            }
            else if (firstLoopInfor.Contains("VM") && secondLoopInfor.Contains("TT"))
            {
                twoLoop = twoLoopVM_TT(twoLoop);
            }
            return twoLoop;
        }

        

        private static string TwoLoopVM_VM(string twoLoop)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor a = new Factor();
            foreach (var factor in f)
            {
                if (factor.FactorType.Contains("*"))
                {
                    a.FactorName = factor.FactorName;
                }

            }
            string arrayName = a.FactorName.Replace("[", string.Empty).Replace("]", string.Empty);
            string firstVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[0]).Replace("(", string.Empty).Replace(")", string.Empty);
            string secondVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[1]).Replace("(", string.Empty).Replace(")", string.Empty);
            string[] firstRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[0]);
            string[] secondRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[1]);
            string condition = TestInputHandle.HandlingType2(line[2])[2].Replace("(", "[").Replace(")", "]");
            twoLoop += "\t\t\tfor (int " + firstVariable + " = " + firstRange[0] + "-1; " + firstVariable + "<=" + firstRange[1] + "-1; " + firstVariable + "++)\n";
            twoLoop += "\t\t\t{\n";
            twoLoop += "\t\t\t\tfor (int " + secondVariable + " = " + secondRange[0] + ";" + secondVariable + "<=" + secondRange[1] + "-1;" + secondVariable + "++)\n";
            twoLoop += "\t\t\t\t{\n";
            twoLoop += "\t\t\t\t\tif (" + condition + ") {}\r";
            twoLoop += "\t\t\t\t\telse return false;\r";
            twoLoop += "\t\t\t\t}\n";
            twoLoop += "\t\t\t}\n";
            twoLoop += "\t\t\treturn true;\n";
            return twoLoop;
        }
        private static string TwoLoopTT_TT(string twoLoop)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor a = new Factor();
            foreach (var factor in f)
            {
                if (factor.FactorType.Contains("*"))
                {
                    a.FactorName = factor.FactorName;
                }

            }
            string arrayName = a.FactorName.Replace("[", string.Empty).Replace("]", string.Empty);
            string firstVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[0]).Replace("(", string.Empty).Replace(")", string.Empty);
            string secondVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[1]).Replace("(", string.Empty).Replace(")", string.Empty);
            string[] firstRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[0]);
            string[] secondRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[1]);
            string condition = TestInputHandle.HandlingType2(line[2])[2].Replace("(", "[").Replace(")", "]");
            twoLoop += "\t\t\tfor (int " + firstVariable + " = " + firstRange[0] + "-1; " + firstVariable + "<=" + firstRange[1] + "-1; " + firstVariable + "++)\n";
            twoLoop += "\t\t\t{\n";
            twoLoop += "\t\t\t\tfor (int " + secondVariable + " = " + secondRange[0] + ";" + secondVariable + "<=" + secondRange[1] + "-1;" + secondVariable + "++)\n";
            twoLoop += "\t\t\t\t{\n";
            twoLoop += "\t\t\t\t\tif (" + condition + ") return true;\n";
            twoLoop += "\t\t\t\t}\n";
            twoLoop += "\t\t\t}\n";
            twoLoop += "\t\t\treturn false;\n";
            return twoLoop;
        }

        private static string twoLoopTT_VM(string twoLoop)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor a = new Factor();
            foreach (var factor in f)
            {
                if (factor.FactorType.Contains("*"))
                {
                    a.FactorName = factor.FactorName;
                }

            }
            string arrayName = a.FactorName.Replace("[", string.Empty).Replace("]", string.Empty);
            string firstVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[0]).Replace("(", string.Empty).Replace(")", string.Empty);
            string secondVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[1]).Replace("(", string.Empty).Replace(")", string.Empty);
            string[] firstRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[0]);
            string[] secondRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[1]);
            string condition = TestInputHandle.HandlingType2(line[2])[2].Replace("(", "[").Replace(")", "]");
            twoLoop += "\t\t\tfor (int " + firstVariable + " = " + firstRange[0] + "-1; " + firstVariable + "<=" + firstRange[1] + "-1; " + firstVariable + "++)\n";
            twoLoop += "\t\t\t{\n";
            twoLoop += "\t\t\t\tfor (int " + secondVariable + " = " + secondRange[0] + ";" + secondVariable + "<=" + secondRange[1] + "-1;" + secondVariable + "++)\n";
            twoLoop += "\t\t\t\t{\n";
            twoLoop += "\t\t\t\t\tif ((" + condition + ")==false) break;\n";
            twoLoop += "\t\t\t\t\tif("+ secondVariable+ " == "+ secondRange[1]+ "-1"+")return true;\n";
            twoLoop += "\t\t\t\t}\n";
            twoLoop += "\t\t\t}\n";
            twoLoop += "\t\t\treturn false;\n";
            return twoLoop;
        }

        private static string twoLoopVM_TT(string twoLoop)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor a = new Factor();
            foreach (var factor in f)
            {
                if (factor.FactorType.Contains("*"))
                {
                    a.FactorName = factor.FactorName;
                }

            }
            string arrayName = a.FactorName.Replace("[", string.Empty).Replace("]", string.Empty);
            string firstVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[0]).Replace("(", string.Empty).Replace(")", string.Empty);
            string secondVariable = TestInputHandle.GetIndexName(TestInputHandle.HandlingType2(line[2])[1]).Replace("(", string.Empty).Replace(")", string.Empty);
            string[] firstRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[0]);
            string[] secondRange = TestInputHandle.GetRange(TestInputHandle.HandlingType2(line[2])[1]);
            string condition = TestInputHandle.HandlingType2(line[2])[2].Replace("(", "[").Replace(")", "]");
            twoLoop += "\t\t\tfor (int " + firstVariable + " = " + firstRange[0] + "-1; " + firstVariable + "<=" + firstRange[1] + "-1; " + firstVariable + "++)\n";
            twoLoop += "\t\t\t{\n";
            twoLoop += "\t\t\t\tbool checkCondition = false;\n";
            twoLoop += "\t\t\t\tfor (int " + secondVariable + " = " + secondRange[0] + ";" + secondVariable + "<=" + secondRange[1] + "-1;" + secondVariable + "++)\n";
            twoLoop += "\t\t\t\t{\n";
            twoLoop += "\t\t\t\t\tif(" + condition + ") checkCondition = true;\n";
            twoLoop += "\t\t\t\t}\n";
            twoLoop += "\t\t\t\tif (checkCondition == false) return false;\n";
            twoLoop += "\t\t\t}\n";
            twoLoop += "\t\t\treturn true;\n";
            return twoLoop;
        }
        #endregion
    }


}
