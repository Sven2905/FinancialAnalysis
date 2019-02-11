﻿using System;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.SalesManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ShipmentTypeViewModel : ViewModelBase
    {
        #region Constructor

        public ShipmentTypeViewModel()
        {
            if (IsInDesignMode) return;

            _ShipmentTypes = LoadAllShipmentTypes();
            NewShipmentTypeCommand = new DelegateCommand(NewShipmentType);
            SaveShipmentTypeCommand = new DelegateCommand(SaveShipmentType, () => Validation());
            DeleteShipmentTypeCommand = new DelegateCommand(DeleteShipmentType, () => SelectedShipmentType != null);
        }

        #endregion Constructor

        #region Fields

        private readonly ShipmentType _SelectedShipmentType;
        private readonly SvenTechCollection<ShipmentType> _ShipmentTypes = new SvenTechCollection<ShipmentType>();
        private string _FilterText;

        #endregion Fields

        #region Methods

        private SvenTechCollection<ShipmentType> LoadAllShipmentTypes()
        {
            var allShipmentTypes = new SvenTechCollection<ShipmentType>();
            try
            {
                allShipmentTypes = DataContext.Instance.ShipmentTypes.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
            }

            return allShipmentTypes;
        }

        private void NewShipmentType()
        {
            SelectedShipmentType = new ShipmentType();
            _ShipmentTypes.Add(SelectedShipmentType);
        }

        private void DeleteShipmentType()
        {
            if (SelectedShipmentType == null) return;

            if (SelectedShipmentType.ShipmentTypeId == 0)
            {
                _ShipmentTypes.Remove(SelectedShipmentType);
                SelectedShipmentType = null;
                return;
            }

            try
            {
                DataContext.Instance.ShipmentTypes.Delete(SelectedShipmentType.ShipmentTypeId);
                _ShipmentTypes.Remove(SelectedShipmentType);
                SelectedShipmentType = null;
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private void SaveShipmentType()
        {
            try
            {
                if (SelectedShipmentType.ShipmentTypeId != 0)
                    DataContext.Instance.ShipmentTypes.Update(SelectedShipmentType);
                else
                    DataContext.Instance.ShipmentTypes.Insert(SelectedShipmentType);
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private bool Validation()
        {
            if (SelectedShipmentType == null) return false;
            if (string.IsNullOrEmpty(SelectedShipmentType.Name)) return false;
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<ShipmentType> FilteredShipmentTypes { get; set; } =
            new SvenTechCollection<ShipmentType>();

        public DelegateCommand NewShipmentTypeCommand { get; set; }
        public DelegateCommand SaveShipmentTypeCommand { get; set; }
        public DelegateCommand DeleteShipmentTypeCommand { get; set; }

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredShipmentTypes = new SvenTechCollection<ShipmentType>();
                    foreach (var item in _ShipmentTypes)
                        if (item.Name.Contains(FilterText))
                            FilteredShipmentTypes.Add(item);
                }
                else
                {
                    FilteredShipmentTypes = _ShipmentTypes;
                }
            }
        }

        public ShipmentType SelectedShipmentType { get; set; }

        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}