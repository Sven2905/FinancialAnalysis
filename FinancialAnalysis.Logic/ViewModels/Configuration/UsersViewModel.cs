using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using System.IO;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        public SvenTechCollection<User> Users { get; set; } = new SvenTechCollection<User>();
        public DelegateCommand NewUserCommand { get; set; }
        public DelegateCommand SaveUserCommand { get; set; }
        public DelegateCommand DeleteUserCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;

        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
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
                _Image = value; _SelectedUser.Picture = ConvertToByteArray(value);
            }
        }

        private User _SelectedUser;
        private BitmapImage _Image;

        public UsersViewModel()
        {
            Users = LoadAllUsers();
            SelectedUser = new User();
            NewUserCommand = new DelegateCommand(NewUser);
            SaveUserCommand = new DelegateCommand(SaveUser);
            DeleteUserCommand = new DelegateCommand(DeleteUser);
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
            Users.Add(SelectedUser);
        }

        private void DeleteUser()
        {
            if (SelectedUser == null)
            {
                return;
            }

            if (SelectedUser.UserId == 0)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.Users.Delete(SelectedUser.UserId);
                    Users.Remove(SelectedUser);
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
            if (SelectedUser == null || string.IsNullOrEmpty(SelectedUser.LoginUser))
            {
                return;
            }

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

        private bool IsPasswordIdentical() => (Password == PasswordRepeat);

        private bool IsPasswordSet() => (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PasswordRepeat));
    }
}
