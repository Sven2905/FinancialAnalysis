using DevExpress.Mvvm;

using FinancialAnalysis.Models.CarPoolManagement;
using Utilities;
using WebApiWrapper.CarPoolManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CarPoolViewModel : ViewModelBase
    {
        public CarPoolViewModel()
        {
            GetCarMakes();
        }

        private CarMake _SelectedCarMake;
        private CarModel _SelectedCarModel;
        private CarBody _SelectedCarBody;
        private CarGeneration _SelectedCarGeneration;
        private CarTrim _SelectedCarTrim;

        private void GetCarMakes()
        {
            CarMakeList = CarMakes.GetAll().ToSvenTechCollection();
            CarModelList.Clear();
            CarBodies.Clear();
            CarGenerations.Clear();
            CarTrimList.Clear();
            CarEngine = null;
        }

        private void GetCarModels(int RefCarMakeId)
        {
            CarModelList = CarModels.GetByRefCarMakeId(RefCarMakeId).ToSvenTechCollection();
            CarBodies.Clear();
            CarGenerations.Clear();
            CarTrimList.Clear();
            CarEngine = null;
        }

        private void GetCarBodies(int RefCarModelId)
        {
            //CarBodies = CarBodies.GetByRefCarModelId(RefCarModelId).ToSvenTechCollection();
            CarGenerations.Clear();
            CarTrimList.Clear();
            CarEngine = null;
        }

        private void GetCarGeneration(int RefCarBodyId)
        {
            //CarGenerations = CarGenerations.GetByRefCarBodyId(RefCarBodyId).ToSvenTechCollection();
            CarTrimList.Clear();
            CarEngine = null;
        }

        private void GetCarTrims(int RefCarGenerationId)
        {
            CarTrimList = CarTrims.GetByRefCarGenerationId(RefCarGenerationId).ToSvenTechCollection();
            CarEngine = null;
        }

        private void GetCarEngine(int RefCarTrimId)
        {
            //CarEngine = CarEngines.GetByRefCarTrimId(RefCarTrimId);
        }

        public SvenTechCollection<CarMake> CarMakeList { get; set; } = new SvenTechCollection<CarMake>();
        public SvenTechCollection<CarModel> CarModelList { get; set; } = new SvenTechCollection<CarModel>();
        public SvenTechCollection<CarBody> CarBodies { get; set; } = new SvenTechCollection<CarBody>();
        public SvenTechCollection<CarGeneration> CarGenerations { get; set; } = new SvenTechCollection<CarGeneration>();
        public SvenTechCollection<CarTrim> CarTrimList { get; set; } = new SvenTechCollection<CarTrim>();
        public SvenTechCollection<int> Years { get; set; } = new SvenTechCollection<int>();
        public CarEngine CarEngine { get; set; }

        public CarMake SelectedCarMake
        {
            get => _SelectedCarMake;
            set
            {
                _SelectedCarMake = value; if (_SelectedCarMake != null)
                {
                    GetCarModels(value.CarMakeId);
                }
            }
        }

        public CarModel SelectedCarModel
        {
            get => _SelectedCarModel;
            set
            {
                _SelectedCarModel = value; if (_SelectedCarModel != null)
                {
                    GetCarBodies(value.CarModelId);
                }
            }
        }

        public CarBody SelectedCarBody
        {
            get => _SelectedCarBody;
            set
            {
                _SelectedCarBody = value; if (_SelectedCarBody != null)
                {
                    GetCarGeneration(value.CarBodyId);
                }
            }
        }

        public CarGeneration SelectedCarGeneration
        {
            get => _SelectedCarGeneration;
            set
            {
                _SelectedCarGeneration = value; if (_SelectedCarGeneration != null)
                {
                    GetCarTrims(value.CarGenerationId);
                }
            }
        }

        public CarTrim SelectedCarTrim
        {
            get => _SelectedCarTrim;
            set
            {
                _SelectedCarTrim = value; if (_SelectedCarTrim != null)
                {
                    GetCarEngine(value.CarTrimId);
                }
            }
        }
    }
}