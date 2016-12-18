using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FRILTimeMachine.Properties;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace FRILTimeMachine
{
    /// <summary>
    /// Interaction logic for PreWindow.xaml
    /// </summary>
    public partial class PreWindow : MetroWindow
    {

        private List<CPilot> Pilots = new List<CPilot>();
        private List<CRound> Rounds = new List<CRound>();

        public enum RaceTypeEnum
        {
            TotalTime,BestLap,None
        }
        public List<CPilot> PilotList { get { return Pilots; } }
        public List<CRound> RoundList { get { return Rounds; } }
        public int PenaltyCost { get; set; }

        public RaceTypeEnum RaceType { get; set; }

        public PreWindow()
        {
            InitializeComponent();
            RaceType = RaceTypeEnum.TotalTime;
        }

        private void SavePreButton_OnClick(object sender, RoutedEventArgs e)
        {
            int noRounds = 0;
            int noLaps = int.Parse(NoLapsBox.Text);
            PenaltyCost = int.Parse(PenaltyBox.Text);

            int minLapTime = int.Parse(TimeLimitBox.Text);
            Settings.Default.minLapTime = minLapTime;
            Settings.Default.Save();

            foreach (var pilot in Pilots)
            {
                noRounds = Math.Max(noRounds, pilot.AssignedRound);
            }

            for (int i = 0; i < noRounds; i++)
            {
                var round = new CRound()
                {
                    RoundNo = i + 1,
                    RoundPilots = (from t in Pilots where t.AssignedRound == (i + 1) select t).ToList(),
                    NoLaps = noLaps,
                };
                Rounds.Add(round);
            }
            this.Close();
        }

        private void AddPilotButton_OnClick(object sender, RoutedEventArgs e)
        {
            var newPilot = new CPilot(PilotNameBox.Text, int.Parse(AssignRoundNoBox.Text));
           
            Pilots.Add(newPilot);
            PilotsDataGrid.ItemsSource = null;
            PilotsDataGrid.ItemsSource = Pilots;
        }

        public IEnumerable<CPilot> ReadCSV(string fileName)
        {
            // We change file extension here to make sure it's a .csv file.
            // TODO: Error checking.
            string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv"));

            // lines.Select allows me to project each line as a Person. 
            // This will give me an IEnumerable<Person> back.
            return lines.Select(line =>
            {
                string[] data = line.Split(',');
                // We return a person with the data in order.
                return new CPilot(data[0], int.Parse(data[1]));
            });
        }

        private void LoadBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.ShowDialog();
            if (dlg.FileName == "")
                return;

            var list = ReadCSV(dlg.FileName);

            Pilots = list.ToList();
            PilotsDataGrid.ItemsSource = null;
            PilotsDataGrid.ItemsSource = Pilots;

        }

     

        private void RaceTypeCmb_DropDownClosed(object sender, EventArgs e)
        {
            switch (RaceTypeCmb.Text)
            {
                case "Best Lap":
                    RaceType = RaceTypeEnum.BestLap;
                    NoLapsBox.IsEnabled = false;
                    PenaltyBox.IsEnabled = false;
                    TimeLimitBox.IsEnabled = true;
                    break;
                case "Total Time":
                    RaceType = RaceTypeEnum.TotalTime;
                    NoLapsBox.IsEnabled = true;
                    TimeLimitBox.IsEnabled = false;
                    PenaltyBox.IsEnabled = true;
                    break;
            }
        }
    }
}
