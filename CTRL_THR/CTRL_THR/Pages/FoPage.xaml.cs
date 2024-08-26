namespace CTRL_THR
{
    public partial class FoPage : ContentPage
    {
        public FoPage()
        {
            InitializeComponent();
            BindingContext = new StepResponseViewModel();
        }
    }
}
