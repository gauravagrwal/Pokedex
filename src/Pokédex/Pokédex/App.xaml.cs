using Xamarin.Forms;

namespace Pokédex
{
    public partial class App : Application
    {
        public App() => InitializeComponent();

        protected override void OnStart()
        {
            MainPage = new NavigationPage(new Pages.MainPage());
        }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
