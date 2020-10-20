using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyBrowserLib
{
    public class Node
    {
        Types type;
        List<Node> subNodes;
        String name;
        String typeOrSignature;

        public Node() {
            
        }

        public Node(Types type, string name, string typeOrSignature)
        {
            this.type = type;
            this.name = name;
            this.typeOrSignature = typeOrSignature;
            
        }

        public List<Node> SubNodes { get => subNodes; set => subNodes = value; }
        public string Name { get => name; set => name = value; }
        public string TypeOrSignature { get => typeOrSignature; set => typeOrSignature = value; }
        internal Types Type { get => type; set => type = value; }
    }
}
