using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace GalaxyGen.ViewModel
{
    public class TextOutputViewModel : ITextOutputViewModel
    {
        private ObservableCollection<String> consoleLines_Var = new ObservableCollection<string>();
        public ObservableCollection<String> ConsoleLines
        {
            get
            {
                return consoleLines_Var;
            }
            private set
            {
                consoleLines_Var = value;
            }
        }

        public void AddLine(string line)
        {            
            consoleLines_Var.Add(line);
            while (consoleLines_Var.Count > 1000) consoleLines_Var.RemoveAt(0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
