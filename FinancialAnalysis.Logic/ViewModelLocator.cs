/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:FinancialAnalysis.Logic"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using FinancialAnalysis.Logic.Model.ViewModel;
using FinancialAnalysis.Logic.ViewModel;

namespace FinancialAnalysis.Logic
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<KontenrahmenViewModel>();
            SimpleIoc.Default.Register<BookingViewModel>();
            SimpleIoc.Default.Register<DialogViewModel>();
            SimpleIoc.Default.Register<TaxTypeViewModel>();
            SimpleIoc.Default.Register<CostAccountViewModel>();
            SimpleIoc.Default.Register<CreditorDebitorViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public KontenrahmenViewModel Kontenrahmen
        {
            get
            {
                return ServiceLocator.Current.GetInstance<KontenrahmenViewModel>();
            }
        }

        public BookingViewModel Bookings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BookingViewModel>();
            }
        }

        public DialogViewModel Dialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DialogViewModel>();
            }
        }

        public TaxTypeViewModel TaxType
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TaxTypeViewModel>();
            }
        }

        public CostAccountViewModel CostAccount
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CostAccountViewModel>();
            }
        }

        public CreditorDebitorViewModel CreditorDebitor
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreditorDebitorViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}