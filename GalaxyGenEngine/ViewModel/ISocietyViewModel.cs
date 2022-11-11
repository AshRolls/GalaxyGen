using GCEngine.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.ViewModel
{
    public interface ISocietyViewModel : INotifyPropertyChanged
    {
        Society Model { get; set; }
        string Name { get; set; }
    }

    public interface ISocietyViewModelFactory
    {
        ISocietyViewModel CreateSocietyViewModel();
    }
}
