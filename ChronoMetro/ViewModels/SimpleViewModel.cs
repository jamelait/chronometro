using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoMetro.ViewModels
{
    class SimpleViewModel : ViewModelBase
    {
        private string _valeur;

        #region Valeur Property

        public string Valeur
        {
            get { return _valeur; }
            set
            {
                this.SetProperty(ref this._valeur, value);
            }
        }

        #endregion


        public SimpleViewModel()
        {
            Valeur = "jamel";
        }
    }
}
