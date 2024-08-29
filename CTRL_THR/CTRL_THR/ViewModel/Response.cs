using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Core.Carousel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
//using CoreImage;  // Import to use LINQ functions like Min and Max

namespace CTRL_THR
{
    public class StepResponseViewModel : INotifyPropertyChanged
    {

        public ICommand ClearGraphCommand { get; }

        private Dictionary<string, double> parameters = new Dictionary<string, double> { { "Tlead", 0.0 },
            {"Tlag1", 0.0 },{"Tlag2", 0.0 }, {"Kp", 0.0 }, {"Theta", 0.0 }, {"Points", 500 }, {"TimeInterval", 15 }};


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

        private string _kp;
        public string Kp
        {
            get => _kp;
            set
            {
                _kp = value;
                OnPropertyChanged(nameof(Kp));
            }
        }

        private string _tlag1;
        public string Tlag1
        {
            get => _tlag1;
            set
            {
                _tlag1 = value;
                OnPropertyChanged(nameof(Tlag1));
            }
        }

        private string _tlag2;
        public string Tlag2
        {
            get => _tlag2;
            set
            {
                _tlag2 = value;
                OnPropertyChanged(nameof(Tlag2));
            }
        }

        private string _tlead;
        public string Tlead
        {
            get => _tlead;
            set
            {
                _tlead = value;
                OnPropertyChanged(nameof(Tlead));
            }
        }

        private string _theta;
        public string Theta
        {
            get => _theta;
            set
            {
                _theta = value;
                OnPropertyChanged(nameof(Theta));
            }
        }

        private string _points;
        public string Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged(nameof(Points));
            }
        }
        private string _timeInterval;
        public string TimeInterval
        {
            get => _timeInterval;
            set
            {
                _timeInterval = value;
                OnPropertyChanged(nameof(TimeInterval));
            }
        }

        private string _processName;
        public string ProcessName
        {
            get => _processName;
            set
            {
                _processName = value;
                OnPropertyChanged(nameof(ProcessName));
            }
        }

        public ICommand NavigateToStepPageCommand { get; }

        private readonly INavigation _navigation;

        public StepResponseViewModel(Dictionary<string, double> parameters, INavigation navigation)
        {
            _navigation = navigation;
            NavigateToStepPageCommand = new Command(NavigateToStepPage);

            DataStep = new ObservableCollection<ResponseData>();
            DataMV = new ObservableCollection<ResponseData>();

            this.parameters = parameters;

            PrefillEntries();
            GenerateStepResponse();
            GenerateMV(1, parameters["Points"]);
            AdjustYAxisLimits(1);
        }
        private async void NavigateToStepPage()
        {
            GetEntryValues();

            // Créez une nouvelle instance de la page FoPage
            var stepPage = new StepPage(parameters);

            // Naviguez vers cette page
            await _navigation.PushAsync(stepPage);
        }
        private void GenerateStepResponse()
        {
            double numberPoints = parameters["Points"];
            double timeInterval = parameters["TimeInterval"];
            double y = 0; //response
            double t = 0; //time
            double pas = timeInterval / numberPoints;
            Debug.WriteLine("This is le PAS : " + pas.ToString());

            foreach (var item in parameters.Keys)
            {
                Debug.WriteLine("--" + item.ToString() + " : " + parameters[item].ToString());
            }

            for (int i = 0; i < numberPoints; i++)
            {
                t = i * pas; //time
                y = vectorGenerator(t);
               
                DataStep.Add(new ResponseData { Time = t, Response = y });
            }
        }

        private double vectorGenerator(double t)
        {
            double Tlag1 = parameters["Tlag1"];
            double Tlag2 = parameters["Tlag2"];
            double Tlead = parameters["Tlead"];
            double Theta = parameters["Theta"];
            double Kp = parameters["Kp"];
            
            double y = 0; //response

            if (Tlag1 != 0 && Tlag2 != 0 && Tlead != 0 && Kp != 0)
            {
                y = GenerateStepLLANDFO(t, Tlag1, Tlag2, Tlead, Kp);
            }
            else if (Tlag1 != 0 && Tlag2 != 0 && Kp != 0)
            {
                y = GenerateStepSO(t, Tlag1, Tlag2, Kp);
            }
            else if (Tlag1 != 0 && Tlead != 0 && Kp != 0)
            {
                y = GenerateStepLeadLag(t, Tlag1, Tlead, Kp);
            }
            else if (Tlag1 != 0 && Kp != 0)
            {
                y = GenerateStepFO(t, Tlag1, Kp);
            }
            return y;
        }

        private void GetEntryValues()
        {
            double tlead = 0;
            double tlag1 = 0;
            double tlag2 = 0;
            double kp = 0;
            double theta = 0;
            double points = 500;
            double timeInterval = 0.5;

            // Culture info with '.' as decimal separator
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            double.TryParse(_tlead?.ToString(), System.Globalization.NumberStyles.Any, culture, out tlead);
            double.TryParse(_tlag1?.ToString(), System.Globalization.NumberStyles.Any, culture, out tlag1);
            double.TryParse(_tlag2?.ToString(), System.Globalization.NumberStyles.Any, culture, out tlag2);
            double.TryParse(_kp?.ToString(), System.Globalization.NumberStyles.Any, culture, out kp);
            double.TryParse(_theta?.ToString(), System.Globalization.NumberStyles.Any, culture, out theta);
            double.TryParse(_points?.ToString(), System.Globalization.NumberStyles.Any, culture, out points);
            double.TryParse(_timeInterval?.ToString(), System.Globalization.NumberStyles.Any, culture, out timeInterval);
            // You can similarly parse points if needed

            parameters["Tlead"] = tlead;
            parameters["Tlag1"] = tlag1;
            parameters["Tlag2"] = tlag2;
            parameters["Kp"] = kp;
            parameters["Theta"] = theta;
            parameters["Points"] = points;
            parameters["TimeInterval"] = timeInterval;
        }
        private double GenerateStepFO(double time, double Tlag, double Kp)
        {
            double y = Kp * (1 - Math.Exp(-time / Tlag)); // Fo
            _processName = "-- First Order --";
            return y;
        }

        private double GenerateStepLeadLag(double time, double Tlag, double Tlead, double Kp)
        {
            double y = Kp * (1 - Math.Exp(-time / Tlag)) * ((Tlag - Tlead) / Tlag) + Kp * (Tlead/Tlag);
            _processName = "-- Lead-Lag --";
            return y;
        }

        private double GenerateStepSO(double time, double Tlag1, double Tlag2, double Kp)
        {
            double y = Kp * (1 + (Tlag1 / (Tlag2 - Tlag1)) * Math.Exp(-time / Tlag1) - (Tlag2 / (Tlag2 - Tlag1)) * Math.Exp(-time / Tlag2));
            _processName = "-- Second Order --";
            return y;
        }

        private double GenerateStepLLANDFO(double time, double Tlag1, double Tlag2, double Tlead, double Kp)
        {
            double y = Kp * (1 + ((Tlag1 - Tlead) / (Tlag2 - Tlag1)) * Math.Exp(-time / Tlag1) - ((Tlag2 - Tlead) / (Tlag2 - Tlag1)) * Math.Exp(-time / Tlag2));
            _processName = "-- Lead-Lag & FO in Series --";
            return y;
        }

        private void GenerateMV(double Kc, double numberOfPoints)
        {
            double numberPoints = parameters["Points"];
            double timeInterval = parameters["TimeInterval"];
            double pas = timeInterval / numberPoints;

            for (int i = 0; i < numberOfPoints; i++)
            {
                double t = i*pas; // Temps
                double y = Kc; // échelon
                DataMV.Add(new ResponseData { Time = t, Response = y });
            }
        }

        private void AdjustYAxisLimits(double margin)
        {
            // Combine the data from both collections
            var allData = DataStep.Concat(DataMV);
            double minMargin = 0;
            if (!allData.Any()) return;

            // Find the minimum and maximum Y values
            double minY = allData.Min(item => item.Response);
            double maxY = allData.Max(item => item.Response);

            if((minY) < 0) { minMargin = minY - margin; }

            Minimum = minMargin;
            Maximum = maxY + margin;
        }

        private void PrefillEntries()
        {
            _kp = parameters["Kp"].ToString();
            _tlag1 = parameters["Tlag1"].ToString();
            _tlag2 = parameters["Tlag2"].ToString();
            _tlead = parameters["Tlead"].ToString();
            _theta = parameters["Theta"].ToString();
            _timeInterval = parameters["TimeInterval"].ToString();
            _points = parameters["Points"].ToString();
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
