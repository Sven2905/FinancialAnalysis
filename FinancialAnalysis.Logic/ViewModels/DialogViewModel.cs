using System;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        private string _ImageVector;
        private string _Message;
        private MessageBoxImage _MessageBoxImage;

        public DialogViewModel()
        {
            CloseCommand = new DelegateCommand(() => CloseAction());
        }

        public string _Title { get; set; }

        public Action CloseAction { get; set; }
        public DelegateCommand CloseCommand { get; set; }

        public string Message
        {
            get => _Message;
            set
            {
                _Message = value;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                RaisePropertyChanged();
            }
        }

        public string ImageVector
        {
            get => _ImageVector;
            set
            {
                _ImageVector = value;
                RaisePropertyChanged();
            }
        }

        public MessageBoxImage MessageBoxImage
        {
            get => _MessageBoxImage;
            set
            {
                _MessageBoxImage = value;
                RaisePropertyChanged();
                SetImage();
            }
        }

        public Brush Brush { get; set; }

        private void SetImage()
        {
            switch (MessageBoxImage)
            {
                case MessageBoxImage.None:
                    ImageVector = string.Empty;
                    break;
                case MessageBoxImage.Question:
                    ImageVector =
                        "M256 8C119.043 8 8 119.083 8 256c0 136.997 111.043 248 248 248s248-111.003 248-248C504 119.083 392.957 8 256 8zm0 448c-110.532 0-200-89.431-200-200 0-110.495 89.472-200 200-200 110.491 0 200 89.471 200 200 0 110.53-89.431 200-200 200zm107.244-255.2c0 67.052-72.421 68.084-72.421 92.863V300c0 6.627-5.373 12-12 12h-45.647c-6.627 0-12-5.373-12-12v-8.659c0-35.745 27.1-50.034 47.579-61.516 17.561-9.845 28.324-16.541 28.324-29.579 0-17.246-21.999-28.693-39.784-28.693-23.189 0-33.894 10.977-48.942 29.969-4.057 5.12-11.46 6.071-16.666 2.124l-27.824-21.098c-5.107-3.872-6.251-11.066-2.644-16.363C184.846 131.491 214.94 112 261.794 112c49.071 0 101.45 38.304 101.45 88.8zM298 368c0 23.159-18.841 42-42 42s-42-18.841-42-42 18.841-42 42-42 42 18.841 42 42z";
                    Brush = new SolidColorBrush(Color.FromRgb(51, 181, 229));
                    break;
                case MessageBoxImage.Error:
                    ImageVector =
                        "M207.6 256l107.72-107.72c6.23-6.23 6.23-16.34 0-22.58l-25.03-25.03c-6.23-6.23-16.34-6.23-22.58 0L160 208.4 52.28 100.68c-6.23-6.23-16.34-6.23-22.58 0L4.68 125.7c-6.23 6.23-6.23 16.34 0 22.58L112.4 256 4.68 363.72c-6.23 6.23-6.23 16.34 0 22.58l25.03 25.03c6.23 6.23 16.34 6.23 22.58 0L160 303.6l107.72 107.72c6.23 6.23 16.34 6.23 22.58 0l25.03-25.03c6.23-6.23 6.23-16.34 0-22.58L207.6 256z";
                    Brush = new SolidColorBrush(Color.FromRgb(255, 68, 68));
                    break;
                case MessageBoxImage.Warning:
                    ImageVector =
                        "M248.747 204.705l6.588 112c.373 6.343 5.626 11.295 11.979 11.295h41.37a12 12 0 0 0 11.979-11.295l6.588-112c.405-6.893-5.075-12.705-11.979-12.705h-54.547c-6.903 0-12.383 5.812-11.978 12.705zM330 384c0 23.196-18.804 42-42 42s-42-18.804-42-42 18.804-42 42-42 42 18.804 42 42zm-.423-360.015c-18.433-31.951-64.687-32.009-83.154 0L6.477 440.013C-11.945 471.946 11.118 512 48.054 512H527.94c36.865 0 60.035-39.993 41.577-71.987L329.577 23.985zM53.191 455.002L282.803 57.008c2.309-4.002 8.085-4.002 10.394 0l229.612 397.993c2.308 4-.579 8.998-5.197 8.998H58.388c-4.617.001-7.504-4.997-5.197-8.997z";
                    Brush = new SolidColorBrush(Color.FromRgb(255, 187, 51));
                    break;
                case MessageBoxImage.Information:
                    ImageVector =
                        "M256 8C119.043 8 8 119.083 8 256c0 136.997 111.043 248 248 248s248-111.003 248-248C504 119.083 392.957 8 256 8zm0 448c-110.532 0-200-89.431-200-200 0-110.495 89.472-200 200-200 110.491 0 200 89.471 200 200 0 110.53-89.431 200-200 200zm0-338c23.196 0 42 18.804 42 42s-18.804 42-42 42-42-18.804-42-42 18.804-42 42-42zm56 254c0 6.627-5.373 12-12 12h-88c-6.627 0-12-5.373-12-12v-24c0-6.627 5.373-12 12-12h12v-64h-12c-6.627 0-12-5.373-12-12v-24c0-6.627 5.373-12 12-12h64c6.627 0 12 5.373 12 12v100h12c6.627 0 12 5.373 12 12v24z";
                    Brush = new SolidColorBrush(Color.FromRgb(51, 181, 229));
                    break;
            }
        }
    }
}