using Android.App;
using Android.OS;

namespace Xperimen.Droid
{
    [Activity(Theme = "@style/Theme.splash", MainLauncher = true, NoHistory = true)]
    public class SplashLoading : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
        }
    }
}