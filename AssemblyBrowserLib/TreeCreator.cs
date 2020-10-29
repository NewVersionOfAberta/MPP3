using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AssemblyBrowserLib
{
    public class TreeCreator
    {

        private List<MethodInfo> extensionMethods = new List<MethodInfo>();

        private Types GetElemType(Type type)
        {
            if (type.IsClass)
            {
                return Types.Class;
            }else if (type.IsEnum)
            {
                return Types.Enum;
            }else if (type.IsInterface)
            {
                return Types.Interface;
            }
            return Types.Default;
        }



        private void AddMembers(Node parent, Type type)
        {
            var bindingParam = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
            foreach (var field in type.GetFields(bindingParam).Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null))
            {
                parent.SubNodes.Add(new Node(Types.Field, field.FieldType.Name + " " + field.Name));
            }
            foreach (var property in type.GetProperties(bindingParam))
            {
                parent.SubNodes.Add(new Node(Types.Property, property.PropertyType.Name + " " + property.Name));
            }
            SignatureBilder signatureBilder = new SignatureBilder();
            
            foreach (var method in type.GetMethods(bindingParam).Where(m => m.GetCustomAttribute<CompilerGeneratedAttribute>() == null))
            {
                if (method.IsDefined(typeof(ExtensionAttribute), false))
                {
                    extensionMethods.Add(method);
                }
                else
                {
                    parent.SubNodes.Add(new Node(Types.Method, signatureBilder.BildSignature(method)));
                }
            }
           
            foreach (var constructor in type.GetConstructors().Where(constuctor => constuctor.GetCustomAttribute<CompilerGeneratedAttribute>() == null))
            {
                parent.SubNodes.Add(new Node(Types.Constructor, signatureBilder.BildSignature(constructor)));
            }
           
        }

        private Assembly LoadBuild(String path)
        {
            return Assembly.LoadFile(path);
        }

        private void CreateNewNode(Node parentNode, Type type, Dictionary<string, Node> parentNodes)
        {
            var tmpNode = new Node(GetElemType(type), type.Name);

            tmpNode.SubNodes = new System.Collections.ObjectModel.ObservableCollection<Node>();
            parentNode.SubNodes.Add(tmpNode);
            if (!parentNodes.ContainsKey(tmpNode.Name))
            {
                parentNodes.Add(tmpNode.Name, tmpNode);
            }
            AddMembers(tmpNode, type);
        }

        private void ParseExtensionMethods(Dictionary<string, Node> classes)
        {
            Node tmpNode, newNode;
            SignatureBilder signatureBilder = new SignatureBilder();
            string exClassName;
            foreach (var method in extensionMethods)
            {
                exClassName = method.GetParameters()[0].ParameterType.Name;
                if (classes.TryGetValue(exClassName, out tmpNode))
                {
                    exClassName = "";   
                }
                else
                {
                    if (!classes.TryGetValue(method.DeclaringType.Name, out tmpNode)) {
                        continue;
                    }
                    else
                    {
                        exClassName += " ";
                    }
                }
                newNode = new Node(Types.Extension, exClassName + signatureBilder.BildSignature(method));
                tmpNode.SubNodes.Add(newNode);
            }
        }


        private List<Node> ParseAssembly(Assembly assembly)
        {
            
            Node parentNode = null, tmpParent = null;
            String currentNamespace;
            String prevNamespace = "";

            var parentNodes = new Dictionary<string, Node>();
            var result = new List<Node>();
     
            foreach (Type type in assembly.GetTypes().Where(m => m.GetCustomAttribute<CompilerGeneratedAttribute>() == null))
            {
                currentNamespace = type.Namespace ?? "global";
                if (currentNamespace != prevNamespace || (parentNode == null))
                {
                    if (!parentNodes.TryGetValue(currentNamespace, out parentNode))
                    {
                        parentNode = new Node(Types.Namespace, currentNamespace);
                        parentNode.SubNodes = new System.Collections.ObjectModel.ObservableCollection<Node>();

                        result.Add(parentNode);
                        parentNodes.Add(currentNamespace, parentNode);
                    }
                    
                }

                if (!type.IsNested && (type.IsClass || type.IsInterface || type.IsEnum))
                {
                    CreateNewNode(parentNode, type, parentNodes);
                }else if (type.IsNested && parentNodes.TryGetValue(type.DeclaringType.Name, out tmpParent))
                {
                    CreateNewNode(tmpParent, type, parentNodes);
                }
                prevNamespace = currentNamespace;
            }
            if (extensionMethods.Count > 0)
                ParseExtensionMethods(parentNodes);
            return result;
        }

        public List<Node> BuildTree(String path)
        {
            var assembly = LoadBuild(path);

            return ParseAssembly(assembly); ;
        }
    }
}
