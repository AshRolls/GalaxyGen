using GCEngine.Engine;
using GCEngine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.ViewModel
{
    //public class ResourceQuantityViewModel : IResourceQuantityViewModel
    //{
     
    //    private ResourceQuantity model_Var;
    //    public ResourceQuantity Model
    //    {
    //        get { return model_Var; }
    //        set
    //        {                
    //            model_Var = value;
    //            updateFromModel();
    //            OnPropertyChanged("Model");
    //        }
    //    }

    //    private void updateFromModel()
    //    {
    //        Quantity = model_Var.Quantity;
    //        Type = model_Var.Type;
    //    }

    //    public ResourceTypeEnum Type
    //    {
    //        get
    //        {
    //            if (model_Var != null)
    //                return model_Var.Type;
    //            else
    //                return ResourceTypeEnum.NotSet;
    //        }
    //        set
    //        {
    //            if (model_Var != null)
    //            {
    //                model_Var.Type = value;
    //                OnPropertyChanged("Type");
    //            }
    //        }
    //    }

    //    public Int64 Quantity
    //    {
    //        get
    //        {
    //            if (model_Var != null)
    //                return model_Var.Quantity;
    //            else
    //                return 0;
    //        }
    //        set
    //        {
    //            if (model_Var != null)
    //            { 
    //                model_Var.Quantity = value;
    //                OnPropertyChanged("Quantity");
    //            }
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected virtual void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //    protected bool SetField<T>(ref T field, T value, string propertyName)
    //    {
    //        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
    //        field = value;
    //        OnPropertyChanged(propertyName);
    //        return true;
    //    }

    //}
}
