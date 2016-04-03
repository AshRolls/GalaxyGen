using GalaxyGen.Engine;
using GalaxyGen.Framework;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public class StoreViewModel : IStoreViewModel
    {
        IResourceQuantityViewModelFactory _resQVmFactory;

        public StoreViewModel(IResourceQuantityViewModelFactory initResQVmFactory)
        {
            _resQVmFactory = initResQVmFactory;
        }

        private Store model_Var;
        public Store Model
        {
            get { return model_Var; }
            set
            {
                model_Var = value;
                updateFromModel();
                OnPropertyChanged("Model");
            }
        }

        private void updateFromModel()
        {
            Name = model_Var.Name;
            Owner = model_Var.Owner;

            //foreach (ResourceQuantity resQ in model_Var.StoredResources) // TODO large inefficiencies with this method. Maybe better just to have storeVM add model objects directly?
            //{
            //    IResourceQuantityViewModel resQVm = _resQVmFactory.CreateResourceQuantityViewModel();
            //    resQVm.Type = resQ.Type;
            //    resQVm.Quantity = resQ.Quantity;
            //    storedResources_Var.Add(resQVm);
            //}
        }

        public String Name
        {
            get {
                if (model_Var != null)
                    return model_Var.Name;
                else
                    return null;
            }
            set {
                if (model_Var != null)
                {
                    model_Var.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public Agent Owner
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Owner;
                else
                    return null;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.Owner = value;
                    OnPropertyChanged("Owner");
                }
            }
        }

        private ObservableCollection<IResourceQuantityViewModel> storedResources_Var = new ObservableCollection<IResourceQuantityViewModel>();
        public ObservableCollection<IResourceQuantityViewModel> StoredResources
        {
            get
            {
                return storedResources_Var;
            }
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
