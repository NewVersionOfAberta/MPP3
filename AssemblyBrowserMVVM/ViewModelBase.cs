using AssemblyBrowserLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserMVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {


        IDialogService dialogService;

        public ViewModelBase()
        {
            dialogService = new DialogService();
        }

        public ObservableCollection<Node> Parse(string path)
        {
            TreeCreator treeCreator = new TreeCreator();
            
            return new ObservableCollection<Node>(treeCreator.BuildTree(path));
            
        }

        

        ObservableCollection<Node> subNodes;

        public ObservableCollection<Node> SubNodes
        {
            get => subNodes;
            set
            {
                subNodes = value;
                OnPropertyChanged();
            }
        }
        private LoadLibCommand load;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public LoadLibCommand Load { 
            get 
            { return load?? (load = new LoadLibCommand((obj) =>
            {
                try
                {
                    if (dialogService.OpenFileDialog() == true)
                    {
                        SubNodes = Parse(dialogService.FilePath);
                    }
                }
                catch(Exception ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
            })); } set => load = value; }
    }

}
