using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EjemploFace
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new VerificacionPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
