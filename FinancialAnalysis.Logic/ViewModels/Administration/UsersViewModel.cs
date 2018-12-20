using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        #region Fields

        private User _SelectedUser;
        private BitmapImage _Image;
        private SvenTechCollection<User> _Users = new SvenTechCollection<User>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public UsersViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            UserRightUserMappingFlatStructure.OnItemPropertyChanged += UserRightUserMappingFlatStructure_OnItemPropertyChanged;

            _Users = LoadAllUsers();
            NewUserCommand = new DelegateCommand(NewUser);
            SaveUserCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteUserCommand = new DelegateCommand(DeleteUser, () => (SelectedUser != null));
        }

        #endregion Constructor

        #region Methods

        private void UserRightUserMappingFlatStructure_OnItemPropertyChanged(object sender, object item, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UserRightUserMappingFlatStructure.OnItemPropertyChanged -= UserRightUserMappingFlatStructure_OnItemPropertyChanged;
            foreach (var UserRightUserMapping in UserRightUserMappingFlatStructure)
            {
                var right = (UserRightUserMappingFlatStructure)item;
                if (UserRightUserMapping.ParentCategory == right.RefUserRightId && right.IsGranted == true)
                {
                    UserRightUserMapping.IsGranted = true;
                }
                else if (UserRightUserMapping.ParentCategory == right.RefUserRightId && right.IsGranted == false)
                {
                    UserRightUserMapping.IsGranted = false;
                }
                if (UserRightUserMapping.RefUserRightId == right.ParentCategory && right.IsGranted == true)
                {
                    UserRightUserMapping.IsGranted = true;
                }
            }
            UserRightUserMappingFlatStructure.OnItemPropertyChanged += UserRightUserMappingFlatStructure_OnItemPropertyChanged;
            RaisePropertyChanged("UserRightUserMappingFlatStructure");
        }

        private SvenTechCollection<User> LoadAllUsers()
        {
            SvenTechCollection<User> allUsers = new SvenTechCollection<User>();
            try
            {
                using (var db = new DataLayer())
                {
                    allUsers = db.Users.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allUsers;
        }

        private void NewUser()
        {
            SelectedUser = new User();
            foreach (var item in Globals.UserRights())
            {
                SelectedUser.UserRightUserMappings.Add(new UserRightUserMapping(0, item.UserRightId, false) { User = SelectedUser, UserRight = item });
            }
            FillUserRightUserMappingFlatStructure();
            _Users.Add(SelectedUser);
        }

        private void DeleteUser()
        {
            if (SelectedUser == null)
            {
                return;
            }

            if (SelectedUser.UserId == 0)
            {
                _Users.Remove(SelectedUser);
                SelectedUser = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.Users.Delete(SelectedUser.UserId);
                    _Users.Remove(SelectedUser);
                    SelectedUser = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveUser()
        {
            try
            {
                if (SelectedUser.UserId != 0)
                {
                    if (IsPasswordSet())
                    {
                        if (IsPasswordIdentical())
                        {
                            SelectedUser.Password = Password;
                            using (var db = new DataLayer())
                            {
                                db.Users.UpdatePassword(SelectedUser);
                            }
                        }
                        else
                        {
                            return; // Passwords are not identical
                        }
                    }

                    using (var db = new DataLayer())
                    {
                        db.Users.Update(SelectedUser);
                        foreach (var item in UserRightUserMappingFlatStructure)
                        {
                            SelectedUser.UserRightUserMappings.SingleOrDefault(x => x.UserRightUserMappingId == item.UserRightUserMappingId).IsGranted = item.IsGranted;
                        }
                        db.UserRightUserMappings.UpdateOrInsert(SelectedUser.UserRightUserMappings);
                    }
                    if (SelectedUser.UserId == Globals.ActualUser.UserId)
                    {
                        Globals.ActualUser = SelectedUser;
                    }
                }
                else
                {
                    if (IsPasswordSet())
                    {
                        if (IsPasswordIdentical())
                        {
                            SelectedUser.Password = Password;
                            using (var db = new DataLayer())
                            {
                                SelectedUser.UserId = db.Users.Insert(SelectedUser);
                                foreach (var item in SelectedUser.UserRightUserMappings)
                                {
                                    item.RefUserId = SelectedUser.UserId;
                                }
                                foreach (var item in UserRightUserMappingFlatStructure)
                                {
                                    SelectedUser.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == item.RefUserRightId).IsGranted = item.IsGranted;
                                }
                                db.UserRightUserMappings.UpdateOrInsert(SelectedUser.UserRightUserMappings);
                                SelectedUser = db.Users.GetById(SelectedUser.UserId);
                            }
                        }
                        else
                        {
                            // not identical
                        }
                    }
                    else
                    {
                        // not set
                    }
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        public BitmapImage ConvertToImage(byte[] array)
        {
            if (array == null)
            {
                return null;
            }

            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private byte[] ConvertToByteArray(BitmapImage bitmapImage)
        {
            if (bitmapImage == null)
            {
                return null;
            }

            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        private bool Validation()
        {
            if (SelectedUser == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedUser.Firstname) || string.IsNullOrEmpty(SelectedUser.Lastname) || string.IsNullOrEmpty(SelectedUser.LoginUser))
            {
                return false;
            }
            if (SelectedUser.UserId == 0)
            {
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(PasswordRepeat))
                {
                    return false;
                }
            }
            return true;
        }

        public void FillUserRightUserMappingFlatStructure()
        {
            if (SelectedUser == null)
            {
                return;
            }

            UserRightUserMappingFlatStructure.Clear();
            foreach (var item in SelectedUser.UserRightUserMappings)
            {
                UserRightUserMappingFlatStructure.Add(new UserRightUserMappingFlatStructure(item));
            }
        }

        private bool IsPasswordIdentical() => (Password == PasswordRepeat);

        private bool IsPasswordSet() => (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PasswordRepeat));

        #endregion Methods

        #region Properties

        public SvenTechCollection<UserRightUserMappingFlatStructure> UserRightUserMappingFlatStructure { get; set; } = new SvenTechCollection<UserRightUserMappingFlatStructure>();
        public SvenTechCollection<User> FilteredUsers { get; set; } = new SvenTechCollection<User>();
        public DelegateCommand NewUserCommand { get; set; }
        public DelegateCommand SaveUserCommand { get; set; }
        public DelegateCommand DeleteUserCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredUsers = new SvenTechCollection<User>();
                    foreach (var item in _Users)
                    {
                        if (item.LoginUser.Contains(FilterText) || item.Firstname.Contains(FilterText) || item.Lastname.Contains(FilterText))
                        {
                            FilteredUsers.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredUsers = _Users;
                }
            }
        }
        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                FillUserRightUserMappingFlatStructure();
                if (_SelectedUser != null && _SelectedUser.Picture != null)
                {
                    Image = ConvertToImage(SelectedUser.Picture);
                }
                else
                {
                    Image = null;
                }
            }
        }
        public BitmapImage Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                if (SelectedUser != null)
                {
                    _SelectedUser.Picture = ConvertToByteArray(value);
                }
            }
        }
        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
