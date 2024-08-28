namespace CTRL_THR
{
    public partial class StepPage : ContentPage
    {
        public StepPage(Dictionary<string, double> parameters)
        {
            InitializeComponent();
            BindingContext = new StepResponseViewModel(parameters, this.Navigation);
        }
    }
}
