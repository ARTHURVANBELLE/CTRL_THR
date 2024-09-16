namespace CTRL_THR
{
    public partial class SoPage : ContentPage
    {
        public SoPage(Dictionary<string, double> parameters)
        {
            InitializeComponent();
            BindingContext = new StepResponseViewModel(parameters, this.Navigation);
        }

        private void CategoryAxis_LabelCreated(object sender, Syncfusion.Maui.Charts.ChartAxisLabelEventArgs e)
        {

        }
    }
}
