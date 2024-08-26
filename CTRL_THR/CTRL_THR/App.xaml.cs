namespace CTRL_THR
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cWWhAYVFpR2Nbe05xdl9EYlZQRGY/P1ZhSXxXdk1hX35ccnZURmVYWU0=");
            MainPage = new AppShell();
        }
    }
}
