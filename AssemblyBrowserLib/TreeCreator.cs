using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib
{
    public class TreeCreator
    {

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
            //Node tmpNode;
            foreach (var field in type.GetFields())
            {
                parent.SubNodes.Add(new Node(Types.Field, field.Name, field.FieldType.ToString()));
            }
            foreach (var property in type.GetProperties())
            {
                parent.SubNodes.Add(new Node(Types.Property, property.Name, property.PropertyType.ToString()));
            }
            SignatureBilder signatureBilder = new SignatureBilder();
            foreach (var method in type.GetMethods())
            {
                parent.SubNodes.Add(new Node(Types.Method, method.Name, signatureBilder.BildSignature(method)));
            }
        }

        private Assembly LoadBuild(String path)
        {
            return Assembly.LoadFile(path);
        }

        private void CreateNewNode(Node parentNode, Type type, Dictionary<string, Node> parentNodes)
        {
            var tmpNode = new Node(GetElemType(type), type.Name, null);

            tmpNode.SubNodes = new List<Node>();
            parentNode.SubNodes.Add(tmpNode);
            parentNodes.Add(tmpNode.Name, tmpNode);
            AddMembers(tmpNode, type);
        }


        private List<Node> ParseAssembly(Assembly assembly)
        {
            
            Node parentNode = null, tmpParent = null;
            String currentNamespace;
            String prevNamespace = "";

            var parentNodes = new Dictionary<string, Node>();
            var result = new List<Node>();
     
            foreach (Type type in assembly.GetTypes())
            {
                currentNamespace = type.Namespace;
                if (currentNamespace != prevNamespace || (parentNode == null))
                {
                    if (!parentNodes.TryGetValue(currentNamespace, out parentNode))
                    {
                        parentNode = new Node(Types.Namespace, currentNamespace, null);
                        parentNode.SubNodes = new List<Node>();

                        result.Add(parentNode);
                    }
                    
                }

                if (!type.IsNested && (type.IsClass || type.IsInterface || type.IsEnum))
                {
                    CreateNewNode(parentNode, type, parentNodes);
                }else if (type.IsNested && parentNodes.TryGetValue(type.Name, out tmpParent))
                {
                    CreateNewNode(tmpParent, type, parentNodes);
                }
                prevNamespace = currentNamespace;
            }
            return result;
        }

        public List<Node> BuildTree(String path)
        {
            var assembly = LoadBuild(path);

            return ParseAssembly(assembly); ;
        }
    }
}
