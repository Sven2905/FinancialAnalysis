﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.General;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Linq;
using Utilities;
using WebApiWrapper.Administration;
using WebApiWrapper.ProjectManagement;
using WebApiWrapper.TimeManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeHolidayRequestViewModel : ViewModelBase
    {
        public TimeHolidayRequestViewModel()
        {
            GetData();
            HolidayInfoBoxViewModel.SetIconData("M391.32 128H24.68c-21.95 0-32.94 26.53-17.42 42.05L192 354.79V480h-53.33c-14.73 0-26.67 11.94-26.67 26.67 0 2.95 2.39 5.33 5.33 5.33h181.33c2.95 0 5.33-2.39 5.33-5.33 0-14.73-11.94-26.67-26.67-26.67H224V354.79l107.7-107.7 22.66-22.66 54.37-54.37c15.52-15.53 4.53-42.06-17.41-42.06zM208 325.53L42.47 160h331.06L208 325.53zM432 0c-62.55 0-114.89 40.23-134.61 96h34.31c17.95-37.68 55.83-64 100.3-64 61.76 0 112 50.24 112 112s-50.24 112-112 112c-18.49 0-35.68-4.93-51.06-12.9l-23.52 23.52C379.23 279.92 404.59 288 432 288c79.53 0 144-64.47 144-144S511.53 0 432 0zm0 192c-.04 0-.08-.01-.13-.01-.2.21-.3.48-.51.69L405.04 219c8.46 3.05 17.46 5 26.96 5 44.12 0 80-35.89 80-80s-35.88-80-80-80c-26.05 0-49.01 12.68-63.62 32H432c26.47 0 48 21.53 48 48s-21.53 48-48 48z");
            RequestCommand = new DelegateCommand(CreateNewRequest, () => Validate());
            NewTimeHolidayUser.RefUserId = Globals.ActiveUser.UserId;
            if (UserManager.Instance.IsUserRightGranted(Globals.ActiveUser.UserId, Models.Administration.Permission.TimeHolidayRequestForOthers))
                ShowTimeHolidayRequestForOthers = true;

            NewTimeHolidayUser.ValueChanged += NewTimeHolidayUser_ValueChanged;
        }

        private void NewTimeHolidayUser_ValueChanged(object sender, EventArgs e)
        {
            CalculateDays();
        }

        public decimal RemainingDays { get; set; }
        public decimal RemainingDaysLastYear { get; set; }
        public decimal RemainingDaysAfterRequest { get; set; }
        public bool ShowTimeHolidayRequestForOthers { get; set; }
        public TimeHolidayUser NewTimeHolidayUser { get; set; } = new TimeHolidayUser();
        public DelegateCommand RequestCommand { get; set; }
        public SvenTechCollection<TimeHolidayUser> HolidayUserList { get; private set; }
        public SvenTechCollection<User> UserList { get; private set; }
        public SvenTechCollection<TimeHolidayType> TimeHolidayTypeList { get; set; }
        public User SelectedUser => Users.GetById(NewTimeHolidayUser.RefUserId);

        public InfoBoxViewModel HolidayInfoBoxViewModel { get; set; } = new InfoBoxViewModel()
        {
            Color = SvenTechColors.BrushBlue,
            Description = "Verf. Urlaubstage",
            Unit = "d",
            Value = 20,
        };

        private void GetData()
        {
            HolidayUserList = TimeHolidayUsers.GetAll().ToSvenTechCollection();
            UserList = Users.GetAll().ToSvenTechCollection();
            TimeHolidayTypeList = TimeHolidayTypes.GetAll().ToSvenTechCollection();
        }

        private void CreateNewRequest()
        {
            TimeHolidayUsers.Insert(NewTimeHolidayUser);
            int UserId = NewTimeHolidayUser.RefUserId;
            NewTimeHolidayUser.ValueChanged -= NewTimeHolidayUser_ValueChanged;
            NewTimeHolidayUser = new TimeHolidayUser
            {
                RefUserId = UserId
            };
            NewTimeHolidayUser.ValueChanged += NewTimeHolidayUser_ValueChanged;
            CalculateDays();
        }

        private bool Validate()
        {
            if (NewTimeHolidayUser.FirstDay > NewTimeHolidayUser.LastDay)
                return false;

            return true;
        }

        private void CalculateDays()
        {
            if (DateTime.Now < new DateTime(DateTime.Now.Year, 4, 1))
                RemainingDaysLastYear = 0; // ToDo

            RemainingDays = SelectedUser.VacationDays - HolidayUserList.Where(x => !x.IsSpecialLeave).Sum(x => x.Days) + RemainingDaysLastYear;
            if (NewTimeHolidayUser.IsSpecialLeave)
                RemainingDaysAfterRequest = RemainingDays;
            else
                RemainingDaysAfterRequest = RemainingDays - NewTimeHolidayUser.Days;

            HolidayInfoBoxViewModel.Value = RemainingDaysAfterRequest;
        }
    }
}