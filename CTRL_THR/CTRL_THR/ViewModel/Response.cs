using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Core.Carousel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;  // Import to use LINQ functions like Min and Max

namespace CTRL_THR
{
    public class StepResponseViewModel : INotifyPropertyChanged
    {

        public ICommand ClearGraphCommand { get; }

        private ObservableCollection<ResponseData> _dataMV;
        public ObservableCollection<ResponseData> DataMV
        {
            get => _dataMV;
            set
            {
                if (_dataMV != value)
                {
                    _dataMV = value;
                    OnPropertyChanged(nameof(DataMV));
                }
            }
        }
        private ObservableCollection<ResponseData> _dataStep;
        public ObservableCollection<ResponseData> DataStep
        {
            get => _dataStep;
            set
            {
                if (_dataStep != value)
                {
                    _dataStep = value;
                    OnPropertyChanged(nameof(DataStep));
                }
            }
        }

        

        private double _maximum;
        public double Maximum
        {
            get => _maximum;
            set
            {
                if (_maximum != value)
                {
                    _maximum = value;
                    OnPropertyChanged(nameof(Maximum));
                }
            }
        }

        private double _minimum;
        public double Minimum
        {
            get => _minimum;
            set
            {
                if (_minimum != value)
                {
                    _minimum = value;
                    OnPropertyChanged(nameof(Minimum));
                }
            }
        }

        public ICommand NavigateToFoPageCommand { get; }

        private readonly INavigation _navigation;

        private async void NavigateToFoPage()
        {
            // Créez une nouvelle instance de la page FoPage
            var foPage = new FoPage(false);

            // Naviguez vers cette page
            await _navigation.PushAsync(foPage);
        }

        public StepResponseViewModel(bool scenario, INavigation navigation)
        {
            _navigation = navigation;
            NavigateToFoPageCommand = new Command(NavigateToFoPage);

            DataStep = new ObservableCollection<ResponseData>();
            DataMV = new ObservableCollection<ResponseData>();

            if (scenario)
            {
                GenerateStepResponse(1.0, 5.0, 100);
                GenerateMV(1.0, 100);
                AdjustYAxisLimits(1);
            }
            else
            {
                Debug.WriteLine("HELLLLLLLLLLLLLO");
                GenerateStepResponse(1.1, 20.0, 60);
            }
        }

        private void GenerateStepResponse(double Kp, double T, int numberOfPoints)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                double t = i; // Temps
                double y = Kp * (1 - Math.Exp(-t / T)); // Réponse à l'échelon
                DataStep.Add(new ResponseData { Time = t, Response = y });
            }
        }

        private void GenerateMV(double Kp, int numberOfPoints)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                double t = i; // Temps
                double y = Kp; // échelon
                DataMV.Add(new ResponseData { Time = t, Response = y });
            }
        }

        private void AdjustYAxisLimits(double margin)
        {
            // Combine the data from both collections
            var allData = DataStep.Concat(DataMV);
            if (!allData.Any()) return;

            // Find the minimum and maximum Y values
            double minY = allData.Min(item => item.Response);
            double maxY = allData.Max(item => item.Response);

            Minimum = minY - margin;
            Maximum = maxY + margin;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ResponseData
    {
        public double Time { get; set; }
        public double Response { get; set; }
    }
}
