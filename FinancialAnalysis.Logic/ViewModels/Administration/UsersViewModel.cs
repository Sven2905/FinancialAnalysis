using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        #region Constructor

        public UsersViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _Users = LoadAllUsers();
            NewUserCommand = new DelegateCommand(NewUser);
            SaveUserCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteUserCommand = new DelegateCommand(DeleteUser,
                () => SelectedUser != null && SelectedUser.UserId != Globals.ActiveUser.UserId);
        }

        #endregion Constructor

        #region Fields

        private User _SelectedUser;
        private BitmapImage _Image;
        private SvenTechCollection<User> _Users = new SvenTechCollection<User>();
        private string _FilterText;

        #endregion Fields

        #region Methods

        private void UserRightUserMappingFlatStructure_OnItemPropertyChanged(object sender, object item,
            PropertyChangedEventArgs e)
        {
            UserRightUserMappingFlatStructure.OnItemPropertyChanged -=
                UserRightUserMappingFlatStructure_OnItemPropertyChanged;
            UserRightUserMappingFlatStructure right = (UserRightUserMappingFlatStructure)item;
            foreach (UserRightUserMappingFlatStructure UserRightUserMapping in UserRightUserMappingFlatStructure)
            {
                if (UserRightUserMapping.ParentCategory == right.HierachicalId && right.IsGranted)
                {
                    UserRightUserMapping.IsGranted = true;
                }
                else if (UserRightUserMapping.ParentCategory == right.HierachicalId && right.IsGranted == false)
                {
                    UserRightUserMapping.IsGranted = false;
                }

                if (UserRightUserMapping.RefUserRightId == right.HierachicalId && right.IsGranted)
                {
                    UserRightUserMapping.IsGranted = true;
                }
            }

            if (right.IsGranted)
            {
                int parentId = right.ParentCategory;
                do
                {
                    UserRightUserMappingFlatStructure parentRight =
                        UserRightUserMappingFlatStructure.SingleOrDefault(x => x.HierachicalId == parentId);
                    if (parentRight != null)
                    {
                        parentRight.IsGranted = true;
                        parentId = parentRight.ParentCategory;
                    }
                    else
                    {
                        parentId = 0;
                    }
                } while (parentId != 0);
            }

            UserRightUserMappingFlatStructure.OnItemPropertyChanged +=
                UserRightUserMappingFlatStructure_OnItemPropertyChanged;
            RaisePropertyChanged("UserRightUserMappingFlatStructure");
        }

        private SvenTechCollection<User> LoadAllUsers()
        {
            return UserManager.Instance.UserList;
        }

        private void NewUser()
        {
            SelectedUser = UserManager.Instance.NewUser();
            UserRightUserMappingFlatStructure = UserManager.Instance.GetUserRightUserMappingFlatStructure(SelectedUser);
            UserRightUserMappingFlatStructure.OnItemPropertyChanged +=
                UserRightUserMappingFlatStructure_OnItemPropertyChanged;
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

            UserManager.Instance.DeleteUser(SelectedUser);
            _Users.Remove(SelectedUser);
            SelectedUser = null;
        }

        private void SaveUser()
        {
            SelectedUser.SetPassword(Password);
            SelectedUser.UserRightUserMappings =
                UserManager.Instance
                    .ConvertUserRightUserMappingFlatStructureToNormal(UserRightUserMappingFlatStructure);
            SelectedUser = UserManager.Instance.InsertOrUpdateUser(SelectedUser);
            int selectedUserId = SelectedUser.UserId;
            FilteredUsers = _Users = UserManager.Instance.UserList;

            SelectedUser = _Users.Single(x => x.UserId == selectedUserId);

            if (selectedUserId == Globals.ActiveUser.UserId)
            {
                Globals.ActiveUser = SelectedUser;
            }
        }

        public BitmapImage ConvertToImage(byte[] array)
        {
            if (array == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream(array))
            {
                BitmapImage image = new BitmapImage();
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

            if (string.IsNullOrEmpty(SelectedUser.Firstname) || string.IsNullOrEmpty(SelectedUser.Lastname) ||
                string.IsNullOrEmpty(SelectedUser.LoginUser))
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

            if (!string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(PasswordRepeat))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PasswordRepeat))
            {
                return false;
            }

            if (IsPasswordSet())
            {
                if (!IsPasswordIdentical())
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsPasswordIdentical()
        {
            return Password == PasswordRepeat;
        }

        private bool IsPasswordSet()
        {
            return !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PasswordRepeat);
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<UserRightUserMappingFlatStructure> UserRightUserMappingFlatStructure { get; set; } =
            new SvenTechCollection<UserRightUserMappingFlatStructure>();

        public SvenTechCollection<User> FilteredUsers { get; set; } = new SvenTechCollection<User>();
        public DelegateCommand NewUserCommand { get; set; }
        public DelegateCommand SaveUserCommand { get; set; }
        public DelegateCommand DeleteUserCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredUsers = new SvenTechCollection<User>();
                    foreach (User item in _Users)
                    {
                        if (item.LoginUser.Contains(FilterText) || item.Firstname.Contains(FilterText) ||
                            item.Lastname.Contains(FilterText))
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
            get => _SelectedUser;
            set
            {
                _SelectedUser = value;
                if (value != null)
                {
                    UserRightUserMappingFlatStructure.OnItemPropertyChanged -=
                        UserRightUserMappingFlatStructure_OnItemPropertyChanged;
                    UserRightUserMappingFlatStructure =
                        UserManager.Instance.GetUserRightUserMappingFlatStructure(_SelectedUser);
                    UserRightUserMappingFlatStructure.OnItemPropertyChanged +=
                        UserRightUserMappingFlatStructure_OnItemPropertyChanged;
                }

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
            get => _Image;
            set
            {
                _Image = value;
                if (SelectedUser != null)
                {
                    _SelectedUser.Picture = ConvertToByteArray(value);
                }
            }
        }

        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}