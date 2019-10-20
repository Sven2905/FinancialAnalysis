using DevExpress.Mvvm;

using FinancialAnalysis.Models.CarPoolManagement;
using System.Collections.Generic;
using System.Linq;
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

        private CarMake selectedCarMake;
        private CarModel selectedCarModel;
        private CarBody selectedCarBody;
        private CarGeneration selectedCarGeneration;
        private CarTrim selectedCarTrim;
        private int selectedYear;
        private List<CarGeneration> tmpCarGenerationList { get; set; } = new List<CarGeneration>();
        private List<CarTrim> tmpCarTrimList { get; set; } = new List<CarTrim>();

        private void GetCarMakes()
        {
            CarMakeList = CarMakes.GetAll().ToSvenTechCollection();
            CarModelList.Clear();
            CarBodyList.Clear();
            CarGenerationList.Clear();
            CarTrimList.Clear();
            CarEngine = null;
        }

        private void GetCarModels(int RefCarMakeId)
        {
            CarModelList = CarModels.GetByRefCarMakeId(RefCarMakeId).ToSvenTechCollection();
            CarBodyList.Clear();
            CarGenerationList.Clear();
            CarTrimList.Clear();
            Years.Clear();
            CarEngine = null;
        }

        private void GetCarBodies(int RefCarModelId)
        {
            CarBodyList = CarBodies.GetByRefCarModelId(RefCarModelId).ToSvenTechCollection();
            CarGenerationList.Clear();
            CarTrimList.Clear();
            Years.Clear();
            CarEngine = null;

            if (CarBodyList?.Count == 1)
                SelectedCarBody = CarBodyList.First();
        }

        private void GetCarGeneration()
        {
            var generationIds = tmpCarTrimList.Where(x => x.Year == selectedYear).Select(x => x.RefCarGenerationId);

            CarGenerationList = tmpCarGenerationList.Where(x => generationIds.Contains(x.CarGenerationId)).ToSvenTechCollection();
            CarTrimList.Clear();
            if (CarGenerationList?.Count == 1)
                SelectedCarGeneration = CarGenerationList.FirstOrDefault();

            CarEngine = null;
        }

        private void GetYears(int RefCarModelId)
        {
            tmpCarGenerationList = CarGenerations.GetByRefCarModelId(RefCarModelId);
            CarTrimList.Clear();
            foreach (var item in tmpCarGenerationList)
            {
                var tmp = CarTrims.GetByRefCarGenerationId(item.CarGenerationId);
                foreach (var trim in tmp)
                    tmpCarTrimList.Add(trim);
            }
            var tmpYears = tmpCarTrimList.Select(x => x.Year).ToList();
            tmpYears.Sort();
            Years = tmpYears.Distinct().ToSvenTechCollection();
            CarGenerationList.Clear();
        }

        private void GetCarTrims()
        {
            CarTrimList = CarTrims.GetByRefCarGenerationId(selectedCarGeneration.CarGenerationId).Where(x => x.Year == selectedYear && x.RefCarBodyId == selectedCarBody.CarBodyId).ToSvenTechCollection();
            CarEngine = null;
        }

        private void GetCarEngine(int RefCarTrimId)
        {
            CarEngine = CarEngines.GetByRefCarTrimId(RefCarTrimId);
        }

        public SvenTechCollection<CarMake> CarMakeList { get; set; } = new SvenTechCollection<CarMake>();
        public SvenTechCollection<CarModel> CarModelList { get; set; } = new SvenTechCollection<CarModel>();
        public SvenTechCollection<CarBody> CarBodyList { get; set; } = new SvenTechCollection<CarBody>();
        public SvenTechCollection<CarGeneration> CarGenerationList { get; set; } = new SvenTechCollection<CarGeneration>();
        public SvenTechCollection<CarTrim> CarTrimList { get; set; } = new SvenTechCollection<CarTrim>();
        public SvenTechCollection<int> Years { get; set; } = new SvenTechCollection<int>();
        public CarEngine CarEngine { get; set; }

        public CarMake SelectedCarMake
        {
            get => selectedCarMake;
            set
            {
                if (selectedCarMake == value)
                    return;

                selectedCarMake = value; 
                if (selectedCarMake != null)
                    GetCarModels(value.CarMakeId);
            }
        }

        public CarModel SelectedCarModel
        {
            get => selectedCarModel;
            set
            {
                if (selectedCarModel == value)
                    return;

                selectedCarModel = value; 
                if (selectedCarModel != null)
                    GetCarBodies(selectedCarModel.CarModelId);
            }
        }

        public CarBody SelectedCarBody
        {
            get => selectedCarBody;
            set
            {
                if (selectedCarBody == value)
                    return;

                selectedCarBody = value; 
                if (selectedCarModel != null)
                    GetYears(selectedCarModel.CarModelId);
            }
        }

        public CarGeneration SelectedCarGeneration
        {
            get => selectedCarGeneration;
            set
            {
                if (selectedCarGeneration == value)
                    return;

                selectedCarGeneration = value; 
                if (selectedCarGeneration != null)
                    GetCarTrims();
            }
        }

        public CarTrim SelectedCarTrim
        {
            get => selectedCarTrim;
            set
            {
                if (selectedCarTrim == value)
                    return;

                selectedCarTrim = value; 
                if (selectedCarTrim != null)
                    GetCarEngine(value.CarTrimId);
            }
        }

        public int SelectedYear
        {
            get { return selectedYear; }
            set 
            {
                if (selectedYear == value)
                    return;

                selectedYear = value;
                if (selectedYear != 0)
                GetCarGeneration();
            }
        }

    }
}