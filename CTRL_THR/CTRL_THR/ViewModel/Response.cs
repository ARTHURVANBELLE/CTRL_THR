using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CTRL_THR
{
    public class StepResponseViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<StepResponseData> Data { get; set; }

        public StepResponseViewModel()
        {
            Data = new ObservableCollection<StepResponseData>();
            GenerateStepResponse(1.0, 5.0, 200); // Exemple avec Kp = 1.0, T = 5.0
        }

        private void GenerateStepResponse(double Kp, double T, int numberOfPoints)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                double t = i; // Temps
                double y = Kp * (1 - Math.Exp(-t / T)); // Réponse à l'échelon
                Data.Add(new StepResponseData { Time = t, Response = y });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StepResponseData
    {
        public double Time { get; set; }
        public double Response { get; set; }
    }

}
