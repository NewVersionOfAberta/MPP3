using AssemblyBrowserLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserMVVM
{
    //"E:/University_needs/СПП/lab1/TracerLib/bin/Debug/netstandard2.0/TracerLib.dll"
    class DllParser
    {
        ObservableCollection<Node> subNodes;

        
        public ObservableCollection<Node> SubNodes { get => subNodes; set => subNodes = value; }

        public void Parse(String path)
        {
            TreeCreator treeCreator = new TreeCreator();
            subNodes = new ObservableCollection<Node>(treeCreator.BuildTree(path));
            
        }
    }
}
