﻿
namespace CTRL_THR
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private Dictionary<string, double> parameters = new Dictionary<string, double> { { "Tlead", 0.0 },
            {"Tlag1", 2.0 },{"Tlag2", 0.0 }, {"Kp", 1.2 }, {"Theta", 0.0 }, {"Points", 500 }, {"TimeInterval", 15 },
        {"OmegaD", 0.0}, {"Sigma", 0.0 }};

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnFOClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StepPage(parameters));
        }
        private async void OnSoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SoPage(parameters));
        }

    }

}
