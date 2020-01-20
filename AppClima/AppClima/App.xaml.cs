using Xamarin.Forms;

namespace AppClima
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new View.InicialPage();
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
