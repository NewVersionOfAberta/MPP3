using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib
{
    class SignatureBilder
    {
        private string GetAccessLevel(MethodInfo methodInfo)
        {
            if (methodInfo.IsPublic)
            {
                return "public ";
            }else if (methodInfo.IsPrivate)
            {
                return "private ";
            }else if (methodInfo.IsFamily || methodInfo.IsFamilyAndAssembly || methodInfo.IsFamilyOrAssembly)
            {
                return "protected ";
            }
            return "";
        }

        private string GetGenericSignaturePart(MethodInfo methodInfo)
        {
            if (methodInfo.ContainsGenericParameters)
            {
                StringBuilder result = new StringBuilder("<");
                var genericParams = methodInfo.GetGenericArguments();
                result.Append(genericParams[0].Name);
                for (int i = 1;  i < genericParams.Length; i++)
                {
                    result.Append(", " + genericParams[i].Name);
                }
                result.Append(">");
                return result.ToString();
            }
            else
            {
                return "";
            }
        }

        private void GetParamSignature(MethodInfo methodInfo, StringBuilder stringBuilder)
        {
            foreach (var param in methodInfo.GetParameters())
            {
                if (param.IsOut)
                {
                    stringBuilder.Append("out ");
                }
                stringBuilder.Append(param.ParameterType.Name + " " + param.Name + " ");
            }
            
        }

        public string BildSignature(MethodInfo methodInfo)
        {
            
            StringBuilder stringBuilder = new StringBuilder(GetAccessLevel(methodInfo)
                                                            + methodInfo.ReturnType.Name 
                                                            + " " + methodInfo.Name +
                                                            GetGenericSignaturePart(methodInfo)
                                                            + "(");
            GetParamSignature(methodInfo, stringBuilder);
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }
    }
}
