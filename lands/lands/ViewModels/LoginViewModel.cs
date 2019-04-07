namespace lands.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using lands.Views;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Services;
    using lands.Helpers;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiservice;
        #endregion


        #region Events

        #endregion

        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnable;
        #endregion

        #region properties

        public string Email {
            get
            {
                return this.email;
            }
            set
            {
                SetValue(ref this.email, value);
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                SetValue(ref this.password, value);
            }
        }

        public bool IsRunning {
            get
            {
                return this.isRunning;
            }
            set
            {
                SetValue(ref this.isRunning, value);
            }
        }

        public bool IsRemenbered { get; set; }

        public bool IsEnable {
            get
            {
                return this.isEnable;
            }
            set
            {
                SetValue(ref this.isEnable, value);
            }
        }

        #endregion

        #region Commands

        public ICommand LoginCommand {

            get {
                return new RelayCommand(Login);
            }
        }


        private async void Login()
        {
            this.IsRunning = true;
            this.IsEnable = false;
            
            if (String.IsNullOrEmpty(this.Email))
            {
                this.IsRunning = false;
                this.IsEnable = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (String.IsNullOrEmpty(this.Password))
            {
                this.IsRunning = false;
                this.IsEnable = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            var connection = await this.apiservice.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var token = await this.apiservice.GetToken("https://landsapirafael.azurewebsites.net/Token", this.Email, this.Password);
            if (token == null )
            {
                this.IsRunning = false;
                this.IsEnable = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SomethingWrong,
                    Languages.Accept);
                return;
            }

            if ( string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnable = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    token.ErrorDescription,
                    Languages.Accept);

                this.Password = string.Empty;
                return;
            }

            /*
            if (this.Email != "hreine@gmail.com" || this.Password != "123456")
            {
                this.IsEnable = !this.IsEnable;
                this.IsRunning = !this.IsRunning;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Email or password incorrect",
                    "Accept");

                this.Password = string.Empty;
                return;
            }
            */
        
            var mainViewmodel = MainViewModel.GetInstance();
            mainViewmodel.Token = token;
            mainViewmodel.Lands = new LandsViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());

            this.IsRunning = false;
            this.IsEnable = true;

            this.Email = string.Empty;
            this.Password = string.Empty;
        }

        #endregion

        #region Constructors

        public LoginViewModel(){
            this.apiservice = new ApiService();
            this.IsRemenbered = true;
            this.IsRunning = false;
            this.IsEnable = true;
            this.Email = "hreine@gmail.com";
            this.Password = "Samuel31*";
        }
        #endregion
    }
}
