﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using lands.Views;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace lands
{
    public partial class App : Application
    {
        #region Contructores
        public App()
        {
            InitializeComponent();
            MainPage =  new NavigationPage(new LoginPage());
        }
        #endregion

        #region Metodos
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        } 
        #endregion
    }
}
