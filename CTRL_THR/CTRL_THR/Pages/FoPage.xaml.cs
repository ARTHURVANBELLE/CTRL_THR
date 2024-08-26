namespace CTRL_THR
{
    public partial class FoPage : ContentPage
    {
        public FoPage(bool scenario)
        {
            InitializeComponent();
            BindingContext = new StepResponseViewModel(scenario, this.Navigation);
        }
    }
}
