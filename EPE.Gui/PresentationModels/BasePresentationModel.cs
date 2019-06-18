using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Configuration;

namespace EPE.Gui.PresentationModels
{
    public abstract class BasePresentationModel : INotifyPropertyChanged
    {
        protected readonly string connectionString = ConfigurationManager.ConnectionStrings["EPEValidation"].ConnectionString;

        protected BasePresentationModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
