using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.ViewModel
{
    public interface ITextOutputViewModel : INotifyPropertyChanged
    {
        ObservableCollection<String> ConsoleLines { get; }
        void AddLine(string line);
    }
}
