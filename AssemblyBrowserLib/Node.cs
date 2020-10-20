using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AssemblyBrowserLib
{
    public class Node
    {
        Types type;
        ObservableCollection<Node> subNodes;
        String name;

        public Node() {
            
        }

        public Node(Types type, string name)
        {
            this.type = type;
            this.name = name;
           
            
        }

        public ObservableCollection<Node> SubNodes { get => subNodes; set => subNodes = value; }
        public string Name { get => name; set => name = value; }
        
        internal Types Type { get => type; set => type = value; }
    }
}
