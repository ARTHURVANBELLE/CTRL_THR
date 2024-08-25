
namespace CTRL_THR
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnFOClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FO());
        }

    }

}
