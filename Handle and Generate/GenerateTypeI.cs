using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalSpecification
{
    class GenerateTypeI
    {
        static string[] line;
        static string functionName;
        static List<string> outputs;

        public static void Initiate(string input)
        {
            line = TestInputHandle.SplitTestInput(input);
            functionName = TestInputHandle.GetFunctionNameOfFunctionString(line[0]);
            outputs = new List<string>();
        }
        public static string GenerateCode(string input, string classname)
        {
            string s = "";
            s += "using System; \r\n";
            s += "namespace FormalSpecification \r\n";
            s += "{\r\n";
            s += "\tpublic class " + classname;
            s += "\r\n\t{\r\n";
            Generate(outputs, input, classname);
            for (int i = 0; i < outputs.Count; i++)
            {
                s += outputs[i];
            }
            s += "\r\n\t}\r\n";
            s += "}\r";
            return s;
        }

        public static void Generate(List<string> outputs, string input, string classname)
        {
            GenerateInputFunction(outputs, input);
            GenerateOutputFunction(outputs, input);
            GenerateCheckFunction(outputs, input);
            GenerateExecuteFunction(outputs, input);
            GenerateMain(outputs, input, classname);
        }

        private static void GenerateMain(List<string> outputs, string input, string classname)
        {
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);

            // main function;
            string mainFunction = String.Empty;
            mainFunction += "\r\n\t\tpublic static void Main(string[] args)\r\n";
            mainFunction += "\t\t{\r\n";
            for (int i = 0; i < f.Count; i++)
            {
                mainFunction += "\t\t\t" + FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName + " = 0"+";\r\n";
            }
            if (result.FactorType == "char*")
            {
                mainFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = null" + ";\r\n";
            }
            else if (result.FactorType == "B")
            {
                mainFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = false" + ";\r\n";
            }
            else
            {
                mainFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = 0" + ";\r\n";
            }
            mainFunction += "\t\t\t" + classname + " p = new " + classname + "();\r\n";
            mainFunction += "\t\t\t" + "p.Nhap_" + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {
                mainFunction += "ref " + f[i].FactorName;
                if (i < f.Count - 1)
                {
                    mainFunction += ",";
                }
            }
            
            mainFunction += ");\r\n";
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
            mainFunction += ")== true)\r\n";
            mainFunction += "\t\t\t{\r\n";
            mainFunction += "\t\t\t\t" + result.FactorName + " = " + "p." + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {
                mainFunction += f[i].FactorName;
                if (i < f.Count - 1)
                {
                    mainFunction += ",";
                }
            }
            mainFunction += ");\r\n";
            mainFunction += "\t\t\t\tp.Xuat_" + functionName + "(" + result.FactorName + ");\r\n";
            mainFunction += "\t\t\t}\r\n";
            mainFunction += "\t\t\telse\r\n";
            mainFunction += "\t\t\t{\r\n";
            mainFunction += "\t\t\t\tConsole.WriteLine" + "(\"Thong tin nhap khong hop le\");\r\n";
            mainFunction += "\t\t\t}\r\n";
            mainFunction += "\t\t\tConsole.ReadLine();\r\n";
            mainFunction += "\t\t}\r\n";
            outputs.Add(mainFunction);
        }

        private static void GenerateExecuteFunction(List<string> outputs, string input)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            string executeFunction = string.Empty;
            string[] postCon = TestInputHandle.HandlingType1(line[2]);
            executeFunction += "\t\tpublic " + FactorType.ChangeFactorType(result.FactorType) + " " + functionName + "(";
            for (int i = 0; i < f.Count; i++)
            {
                executeFunction += FactorType.ChangeFactorType(f[i].FactorType) + " " + f[i].FactorName;
                if (i < f.Count - 1)
                {
                    executeFunction += ", ";
                }
            }
            executeFunction += ")";
            executeFunction += "\r\n\t\t{\r\n";
            if (result.FactorType == "char*")
            {
                executeFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = null" + ";\r\n";
            }
            else if (result.FactorType == "B")
            {
                executeFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = false" + ";\r\n";
            }
            else
            {
                executeFunction += "\t\t\t" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + " = 0" + ";\r\n";
            }
            for (int i = 0; i < postCon.Length; i++)
            {
               
                string[] subPostCon = postCon[i].Split(new[] { "&&" }, StringSplitOptions.None);
               
                if (subPostCon.Length == 3)
                {
                    subPostCon[1] = "("+ subPostCon[1]+")" + "&&" + "("+ subPostCon[2] +")";
                }
                
                if (subPostCon[0].Contains("=="))
                {
                    subPostCon[0] = subPostCon[0].Replace("==", "=");
                }    
                if (subPostCon.Length <= 1)
                {
                    executeFunction += "\t\t\t" + subPostCon[0] + ";";
                    //executeFunction += "\r\n\t\t\treturn " + result.FactorName + ";";
                }
                else
                {
                    string replaceChar = string.Empty;
                    if (i < 1)
                    {
                        executeFunction += "\t\t\tif (" + subPostCon[1] + ")\r\n";
                        executeFunction += "\t\t\t{\r\n";
                        executeFunction += "\t\t\t\t" + subPostCon[0] + ";\r\n";
                        executeFunction += "\t\t\t}\r\n";
                       
                    }
                    else
                    {
                        executeFunction += "\t\t\telse if (" + subPostCon[1] + ")\r\n";
                        executeFunction += "\t\t\t{\r\n";
                        executeFunction += "\t\t\t\t" + subPostCon[0] + ";";
                        executeFunction = executeFunction.Replace("!==", "!=");
                        executeFunction = executeFunction.Replace(">==", ">=");
                        executeFunction = executeFunction.Replace("<==", "<=");
                        executeFunction += "\r\n\t\t\t}\r\n";
                        
                    }
                    
                }
            }
            executeFunction += "\r\n\t\t\treturn" + " " + result.FactorName +";";
            executeFunction += "\r\n\t\t}\r\n\r\n";
            executeFunction = executeFunction.Replace("TRUE", "true");
            executeFunction = executeFunction.Replace("FALSE", "false");
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
                checkCondition += ")";
                checkCondition += "\r\n\t\t{\r\n";
                checkCondition += "\t\t\tif (" + preCon + ")";
                checkCondition += "\r\n\t\t\t{\r\n";
                checkCondition += "\t\t\t\treturn true;\r\n\t\t\t}\r\n";
                checkCondition += "\t\t\telse return false;\r\n";
                checkCondition += "\r\n\t\t}\r\n\r\n";
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

        private static void GenerateOutputFunction(List<string> outputs, string input)
        {
            List<Factor> f = TestInputHandle.GetVariableStringOfFunctionString(line[0]);
            Factor result = TestInputHandle.GetResultStringOfFunctionString(line[0]);
            string outputFunction = String.Empty;
            outputFunction += "\t\tpublic void Xuat_" + functionName + "(" + FactorType.ChangeFactorType(result.FactorType) + " " + result.FactorName + ")\r\n";
            outputFunction += "\t\t{\r\n";
            outputFunction += "\t\t\tConsole.WriteLine(\"Ket qua la: {0} \", " + result.FactorName + ");\r\n";
            outputFunction += "\t\t}\r\n\r\n";
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
            inputFunction += ")\r\n";
            inputFunction += "\t\t{\r\n";
            for (int i = 0; i < f.Count; i++)
            {
                inputFunction += "\t\t\tConsole.Write(\"Nhap " + f[i].FactorName + ": \");\r\n";
                inputFunction += "\t\t\t" + f[i].FactorName + " = " + FactorType.ChangeFactorType(f[i].FactorType) + ".Parse(Console.ReadLine());\r\n";
            }
            inputFunction += "\t\t}\r\n\r\n";
            outputs.Add(inputFunction);
        }


   
    }
}
