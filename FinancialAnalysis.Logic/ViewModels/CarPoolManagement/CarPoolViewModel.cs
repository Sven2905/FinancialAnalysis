using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.CarPoolManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

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
            CarMakes = DataContext.Instance.CarMakes.GetAll().ToSvenTechCollection();
            CarModels.Clear();
            CarBodies.Clear();
            CarGenerations.Clear();
            CarTrims.Clear();
            CarEngine = null;
        }

        private void GetCarModels(int RefCarMakeId)
        {
            CarModels = DataContext.Instance.CarModels.GetByRefCarMakeId(RefCarMakeId).ToSvenTechCollection();
            CarBodies.Clear();
            CarGenerations.Clear();
            CarTrims.Clear();
            CarEngine = null;
        }

        private void GetCarBodies(int RefCarModelId)
        {
            //CarBodies = DataContext.Instance.CarBodies.GetByRefCarModelId(RefCarModelId).ToSvenTechCollection();
            CarGenerations.Clear();
            CarTrims.Clear();
            CarEngine = null;
        }

        private void GetCarGeneration(int RefCarBodyId)
        {
            //CarGenerations = DataContext.Instance.CarGenerations.GetByRefCarBodyId(RefCarBodyId).ToSvenTechCollection();
            CarTrims.Clear();
            CarEngine = null;
        }

        private void GetCarTrims(int RefCarGenerationId)
        {
            CarTrims = DataContext.Instance.CarTrims.GetByRefCarGenerationId(RefCarGenerationId).ToSvenTechCollection();
            CarEngine = null;
        }

        private void GetCarEngine(int RefCarTrimId)
        {
            CarEngine = DataContext.Instance.CarEngines.GetByRefCarTrimId(RefCarTrimId);
        }

        public SvenTechCollection<CarMake> CarMakes { get; set; } = new SvenTechCollection<CarMake>();
        public SvenTechCollection<CarModel> CarModels { get; set; } = new SvenTechCollection<CarModel>();
        public SvenTechCollection<CarBody> CarBodies { get; set; } = new SvenTechCollection<CarBody>();
        public SvenTechCollection<CarGeneration> CarGenerations { get; set; } = new SvenTechCollection<CarGeneration>();
        public SvenTechCollection<CarTrim> CarTrims { get; set; } = new SvenTechCollection<CarTrim>();
        public SvenTechCollection<int> Years { get; set; } = new SvenTechCollection<int>();
        public CarEngine CarEngine { get; set; }

        public CarMake SelectedCarMake
        {
            get { return _SelectedCarMake; }
            set { _SelectedCarMake = value; if (_SelectedCarMake != null) GetCarModels(value.CarMakeId); }
        }

        public CarModel SelectedCarModel
        {
            get { return _SelectedCarModel; }
            set { _SelectedCarModel = value; if (_SelectedCarModel != null) GetCarBodies(value.CarModelId); }
        }

        public CarBody SelectedCarBody
        {
            get { return _SelectedCarBody; }
            set { _SelectedCarBody = value; if (_SelectedCarBody != null) GetCarGeneration(value.CarBodyId); }
        }

        public CarGeneration SelectedCarGeneration
        {
            get { return _SelectedCarGeneration; }
            set { _SelectedCarGeneration = value; if (_SelectedCarGeneration != null) GetCarTrims(value.CarGenerationId); }
        }

        public CarTrim SelectedCarTrim
        {
            get { return _SelectedCarTrim; }
            set { _SelectedCarTrim = value; if (_SelectedCarTrim != null) GetCarEngine(value.CarTrimId); }
        }

    }
}
