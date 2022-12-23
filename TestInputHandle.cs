using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormalSpecification
{
    class TestInputHandle
    {


        public static string[] SplitTestInput(string input)
        {

            string[] arrInput = input.Split(new[] { "\n" }, StringSplitOptions.None);
            if (arrInput.Length > 3)
            {
                for (int i = 3; i < arrInput.Length; i++)
                {
                    arrInput[2] += arrInput[i];
                }
            }
            foreach (var lineInput in arrInput)
            {
                ReplaceAndRemove.ReplaceEndline(lineInput);
                ReplaceAndRemove.ReplaceTab(lineInput);
                ReplaceAndRemove.ReplaceSpace(lineInput);
            }

            return arrInput;
        }

        public static string GetSubInput(string input, int index)
        {
            return SplitTestInput(input)[index];
        }
        public static string GetFunctionNameOfFunctionString(string functionString)
        {
            int firstIndex = 0;
            int lastIndex = functionString.IndexOf("(");
            string functionName = functionString.Substring(firstIndex, lastIndex).Trim();
            return functionName;
        }

        public static List<Factor> GetVariableStringOfFunctionString(string functionString)
        {
            List<Factor> factor = new List<Factor>();
            int firstIndex = functionString.IndexOf("(") + 1;
            int lastIndex = functionString.IndexOf(")") - firstIndex;
            string variableString = functionString.Substring(firstIndex, lastIndex).Trim();
            MessageBox.Show(variableString);
            string[] variables = variableString.Split(',');

            for (int i = 0; i < variables.Length; i++)
            {
                string variableName = variables[i].Substring(0, variables[i].IndexOf(":")).Trim();
                string varableType = variables[i].Substring(variables[i].IndexOf(":") + 1).Trim();
                var v = new Factor(variableName, varableType);
                factor.Add(v);
            }
            return factor;
        }

        public static Factor GetResultStringOfFunctionString(string functionString)
        {
            Factor factor = new Factor();
            int firstIndex = functionString.IndexOf(")") + 1;
            string result = functionString.Substring(firstIndex).Trim();
            string[] resultString = result.Split(new[] { ":" }, StringSplitOptions.None);
            factor.FactorName = resultString[0].Trim();
            factor.FactorType = resultString[1].Trim();
            return factor;
        }
        public static string GetPreConditionString(string pre)
        {
            int firstIndex = pre.IndexOf("pre") + 3;
            string preCondition = pre.Substring(firstIndex).Trim();
            return preCondition;
        }
        public static string GetPosConditionString(string post)
        {
            int firstIndex = post.IndexOf("post") + 4;
            string postResult = post.Substring(firstIndex).Trim();
            postResult = postResult.Replace("=", "==");
            postResult = postResult.Replace("!===", "!=");

            return postResult;
        }

        public static string[] HandlingType1(string postType1)
        {
            string post = GetPosConditionString(postType1);
            string[] postType1Result = post.Split(new[] { "||" }, StringSplitOptions.None);
            for (int i = 0; i < postType1Result.Length; i++)
            {
                postType1Result[i].Trim().Replace(" ", string.Empty);
                postType1Result[i] = string.Join(String.Empty, postType1Result[i].Split('(', ')'));
            }
            return postType1Result;
        }
        public static string[] HandlingType2(string postType2)
        {
            string post = GetPosConditionString(postType2);
            string[] postType2Result = post.Split(new[] { "}." }, StringSplitOptions.None);
            postType2Result[0] = postType2Result[0].Remove(0, postType2Result[0].IndexOf("=") + 1);
            postType2Result[postType2Result.Length - 1] = postType2Result[postType2Result.Length - 1].Substring(0, postType2Result[postType2Result.Length - 1].LastIndexOf(")"));
            for (int i = 0; i < postType2Result.Length; i++)
            {
                postType2Result[i].Trim().Replace(" ", string.Empty);
                postType2Result[i] = postType2Result[i].Replace("==", "=");
                postType2Result[i] = postType2Result[i].Replace("..", "*");
                postType2Result[i] = postType2Result[i].Replace(" ", string.Empty);     
            }
            
            return postType2Result;
        }
        public static string[] GetResultConditionType2(string resultCondition)
        {
            
            if (resultCondition.Contains("{"))
            {
                string[] resultConditionType = resultCondition.Split(new[] { "{" }, StringSplitOptions.None);
                return resultConditionType;
            }
            else { return null; }
        }

        public static string GetIndexName(string postCondition)
        {
            if (postCondition.Contains("TH{"))
            {
                int index = postCondition.IndexOf("TH{");
                postCondition = postCondition.Substring(0,index);
            }    
            return postCondition.Replace("VM", String.Empty).Replace("TT", String.Empty).Replace("TH", String.Empty).Replace("=", String.Empty).Replace(" ", string.Empty).Trim();
        }

        public static string[] GetRange(string loopCondition)
        {
            if (loopCondition.Contains("TH{")) 
            {
                int index = loopCondition.IndexOf("TH{")+3;
                loopCondition= loopCondition.Substring(index);
            }
            
            string[] range = loopCondition.Split(new[] { "*" }, StringSplitOptions.None);
            return range;
        }

    } 
    
}
