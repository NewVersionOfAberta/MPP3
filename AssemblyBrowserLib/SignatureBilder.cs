using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib
{
    class SignatureBilder
    {
        private string GetAccessLevel(MethodBase methodInfo)
        {
            if (methodInfo.IsPublic)
            {
                return "public ";
            }
            else if (methodInfo.IsPrivate)
            {
                return "private ";
            }
            else if (methodInfo.IsFamily || methodInfo.IsFamilyAndAssembly || methodInfo.IsFamilyOrAssembly)
            {
                return "protected ";
            }
            return "";
        }

        private string GetGenericSignaturePart(MethodBase methodInfo)
        {
            if (methodInfo.ContainsGenericParameters)
            {
                StringBuilder result = new StringBuilder("<");
                var genericParams = methodInfo.GetGenericArguments();
                if (genericParams.Length > 0)
                {
                    result.Append(genericParams[0].Name);
                    for (int i = 1; i < genericParams.Length; i++)
                    {
                        result.Append(", " + genericParams[i].Name);
                    }
                    result.Append(">");
                }
                return result.ToString();
            }
            else
            {
                return "";
            }
        }

        private void GetParamSignature(MethodBase methodInfo, StringBuilder stringBuilder)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length > 0)
            {
                stringBuilder.Append(parameters[0].ParameterType.Name + " " + parameters[0].Name);
                for (var i = 1; i < parameters.Length; i++)
                {
                    if (parameters[i].IsOut)
                    {
                        stringBuilder.Append("out ");
                    }
                    stringBuilder.Append(", " + parameters[i].ParameterType.Name + " " + parameters[i].Name);
                }
            }
            

        }
        

        public string BildSignature(MethodBase methodInfo)
        {
            string returnType = "";
            string name = methodInfo.DeclaringType.Name;
            if (methodInfo is MethodInfo)
            {
                returnType = ((MethodInfo)methodInfo).ReturnType.Name + " ";
                name = methodInfo.Name;
            }
            StringBuilder stringBuilder = new StringBuilder(GetAccessLevel(methodInfo)
                                                            + returnType
                                                            + name +
                                                            GetGenericSignaturePart(methodInfo)
                                                            + "(");
            GetParamSignature(methodInfo, stringBuilder);
            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }
    }
}
