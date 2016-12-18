using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using FRILTimeMachine.Properties;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Application = System.Windows.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Timers.Timer;

namespace FRILTimeMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    ///
    


    public partial class MainWindow : MetroWindow
    {
        private Stopwatch sp;
        private DispatcherTimer timer;
        private List<CPilot> Pilots = new List<CPilot>(); 
        private List<CRound> Rounds = new List<CRound>();
        private List<CRound> TotalRounds = new List<CRound>();
        private TimeSpan lapMinTime;
        private CRound currRound;
        private CRound nextRound;
        private int totalLaps = 0;
        private int penalty;
        private Timer nextTimer;
        private int m=0, s=0;
        public enum RaceTypeEnum
        {
            TotalTime, BestLap, None
        }

        public RaceTypeEnum RaceType { get; set; }
        
            

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);

        public MainWindow()
        {
            InitializeComponent();

            nextTimer = new Timer();

            //var googleSearchText = File.ReadAllText("c:\\fril.json");
          

            //JObject googleSearch = JObject.Parse(googleSearchText);

            // get JSON result objects into a list
            //IList<JToken> results = googleSearch["data"].Children().ToList();

            // serialize JSON results into .NET objects
            //IList<CRaceJSONData> searchResults = new List<CRaceJSONData>();
            //foreach (JToken result in results)
            //{
             //   CRaceJSONData searchResult = JsonConvert.DeserializeObject<CRaceJSONData>(result.ToString());
             //   searchResults.Add(searchResult);
            //}
         
            var pre = new PreWindow();
            pre.ShowDialog();

            int minTime = Settings.Default.minLapTime;
            lapMinTime = new TimeSpan(0, 0, minTime);

            RaceType = (RaceTypeEnum) pre.RaceType;

            if (pre.PilotList.Count == 0)
                Close();

            if(pre.PilotList!=null)
                Pilots = pre.PilotList;
            
            if(pre.RoundList!=null)
                Rounds = pre.RoundList;

            penalty = pre.PenaltyCost;

            totalLaps = int.Parse(pre.NoLapsBox.Text);

            foreach (var round in Rounds)
            {
                foreach (CPilot pilotInRound in round.RoundPilots)
                {
                    var pilotRound = new CPilotRound();
                    pilotRound.Pilot = pilotInRound;
                   
                    round.PilotsLaps.Add(pilotRound);
                    pilotRound.Active = true;
                }
            }

            if (Pilots != null && Rounds != null && Rounds.Count>0)
            {
                currRound = Rounds[0];
                nextRound = Rounds[1];
                SetupRound(currRound);
                
            }

            switch(RaceType)
            {
             case RaceTypeEnum.BestLap:
                    DescLabel.Content = "Race Type: Best Lap";
                    penalty = 0;
                    totalTimeCol.Visibility = Visibility.Collapsed;
                    TopPilotLabel.Content = "Top Pilots - By Best Lap";
                   
                    break;
                case RaceTypeEnum.TotalTime:
                    DescLabel.Content = "Race Type: Total Time, In " + pre.NoLapsBox.Text + " Laps";
                    TopPilotLabel.Content = "Top Pilots - By Best Total Time";
                    
                    break;
            }

            

        }


        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();

            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "HornSiren.wav";
            player.Play();

            /*
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "//airhorneffect.mp3"));
            mediaPlayer.Play();
            //mediaPlayer.Stop();

            //mciSendString("play " + System.AppDomain.CurrentDomain.BaseDirectory + "//airhorneffect.mp3", null, 0, 0);
             
            
            /*
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory+"//airhorneffect.mp3"))
                mciSendString("play " + System.AppDomain.CurrentDomain.BaseDirectory + "//airhorneffect.mp3", null, 0, 0);
            else
                MessageBox.Show("Cant find file");*/
            //player.Play();
            set1stLap();
            sp = new Stopwatch();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_Tick;
            timer.Start();
           
            sp.Start();

            startBtn.IsEnabled = false;
            stopBtn.IsEnabled = true;

        }

        void timer_Tick(object sender, EventArgs e)
        {
            MinTimeLabel.Content = sp.Elapsed.Minutes.ToString("D2");
            SecTimeLabel.Content = sp.Elapsed.Seconds.ToString("D2");
            MiliTimeLabel.Content = sp.Elapsed.Milliseconds.ToString("D");
        }

        private void set1stLap()
        {
            foreach (var pilot in currRound.PilotsLaps)
            {
                pilot.Laps = new List<CLap>();
                if(RaceType == RaceTypeEnum.BestLap)
                    pilot.Laps.Add(new CLap(1,false));
                else
                    pilot.Laps.Add(new CLap(1, true));
               
            }

            RaceDataGrid.ItemsSource = null;
            RaceDataGrid.ItemsSource = currRound.PilotsLaps;
        }

        private void SetKeyPress(int pilotSlot,bool lap)
        {
            if (currRound.RoundPilots.Count < pilotSlot+1)
                return;
            if (lap)
            {
                var keyTime = sp.Elapsed;

                if (keyTime < lapMinTime)
                    return;
                
                var pilot = currRound.PilotsLaps[pilotSlot];
               
                var last = pilot.Laps.Last();
               
                last.LapTime = keyTime;

                if (!pilot.Active)
                    return;
                

                    if (pilot.Laps.Count > 1)
                    {
                        var before = pilot.Laps[pilot.Laps.Count - 2];
                        if ((keyTime - before.LapTime) < lapMinTime)
                            return;
                        last.LapTime = keyTime;
                        pilot.LastLap = last.LapTime - before.LapTime;

                        if (pilot.LastLap < pilot.BestLap)
                        {
                            pilot.BestLap = pilot.LastLap;
                            pilot.BestLapNo = last.LapNo;

                            if (last.PenaltiesList.Count == 0)
                            {
                                if (pilot.LastLap < pilot.ClearBestLap || pilot.ClearBestLapNo == 0)
                                {
                                    pilot.ClearBestLap = pilot.LastLap;
                                    pilot.ClearBestLapNo = last.LapNo;
                                }
                            }
                        }
                    }
                    else
                    {
                        //for lap no.1
                        pilot.BestLap = last.LapTime;
                        pilot.BestLapNo = 1;
                        pilot.LastLap = last.LapTime;

                        if (RaceType == RaceTypeEnum.BestLap)
                        {
                            pilot.BestLap = new TimeSpan(9,59,59);
                            pilot.ClearBestLap = new TimeSpan(9, 59, 59);
                            pilot.LastLap = new TimeSpan(9, 59, 59);
                        }
                       

                        if (last.PenaltiesList.Count == 0)
                        {
                            if (pilot.LastLap < pilot.ClearBestLap || pilot.ClearBestLapNo == 0)
                            {
                                pilot.ClearBestLap = pilot.LastLap;
                                pilot.ClearBestLapNo = last.LapNo;
                            }
                        }

                    }

                    if (RaceType == RaceTypeEnum.TotalTime)
                    {
                        if (last.LapNo == totalLaps)
                        {
                            pilot.Active = false;
                           
                            pilot.CalculateRound();
                        }
                    }
                
               
                    pilot.Laps.Add(new CLap(pilot.Laps.Count + 1, pilot.Active));
             
            }
            else
            {
              
                var pilot = currRound.PilotsLaps[pilotSlot];
                var last = pilot.Laps.Last();
                last.PenaltiesList.Add(new CPenalty(sp.Elapsed,penalty));
                last.legit = false;
                pilot.PenaltyCount++;
            }

            
        }

        private void MetroWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (sp == null)
                return;
            if (currRound == null || !sp.IsRunning)
                return;

           
            switch (e.Key)
            {
                //pilots lap
                case Key.D1:
                    if (currRound.RoundPilots.Count < 1)
                        return;
                    SetKeyPress(0,true);

                   
                    break;
                case Key.D2:
                    if (currRound.RoundPilots.Count < 2)
                        return;
                    SetKeyPress(1, true);
                   
                    break;
                case Key.D3:
                    if (currRound.RoundPilots.Count < 3)
                        return;
                    SetKeyPress(2, true);
                   
                    break;
                case Key.D4:
                    if (currRound.RoundPilots.Count < 4)
                        return;
                    SetKeyPress(3, true);
                  
                    break;
                case Key.D5:
                    if (currRound.RoundPilots.Count < 5)
                        return;
                    SetKeyPress(4, true);
                   
                    break;
                case Key.D6:
                    if (currRound.RoundPilots.Count < 6)
                        return;
                    SetKeyPress(5, true);
                   
                    break;
                case Key.D7:
                    if (currRound.RoundPilots.Count < 7)
                        return;
                    SetKeyPress(6, true);
                   
                    break;
                case Key.D8:
                    if (currRound.RoundPilots.Count < 8)
                        return;
                    SetKeyPress(7, true);
                   
                    break;

                //pilot penalty
                case Key.A:
                    if (currRound.RoundPilots.Count < 1)
                        return;
                    SetKeyPress(0, false);
                   
                    break;
                case Key.S:
                    if (currRound.RoundPilots.Count < 2)
                        return;
                    SetKeyPress(1, false);
                   
                    break;
                case Key.D:
                    if (currRound.RoundPilots.Count < 3)
                        return;
                    SetKeyPress(2, false);
                   
                    break;
                case Key.F:
                    if (currRound.RoundPilots.Count < 4)
                        return;
                    SetKeyPress(3, false);
                   
                    break;
                case Key.J:
                    if (currRound.RoundPilots.Count < 5)
                        return;
                    SetKeyPress(4, false);
                   
                    break;
                case Key.K:
                    if (currRound.RoundPilots.Count < 6)
                        return;
                    SetKeyPress(5, false);
                  
                    break;
                case Key.L:
                    if (currRound.RoundPilots.Count < 7)
                        return;
                    SetKeyPress(6, false);
                  
                    break;
               case Key.Oem1:
                    if (currRound.RoundPilots.Count < 8)
                        return;
                    SetKeyPress(7, false);
                  
                    break;
            }
            RaceDataGrid.ItemsSource = null;
           
            RaceDataGrid.ItemsSource = currRound.PilotsLaps;
        }

        private void SetupRound(CRound rnd)
        {
            RoundNoLabel.Content = rnd.RoundNo;

            RaceDataGrid.ItemsSource = currRound.PilotsLaps;

            if (Rounds.Count > currRound.RoundNo)
                nextPilotDataGrid.ItemsSource = nextRound.PilotsLaps;
            else
            {
                nextPilotDataGrid.ItemsSource = Rounds[0].PilotsLaps;
            }
        }

        private bool FilterZero(object o)
        {
            if (((CPilot)o).Visible)
                return true;
            return false;
        }

        private void StopBtn_OnClick(object sender, RoutedEventArgs e)
        {
            startBtn.IsEnabled = true;
            stopBtn.IsEnabled = false;

            if(sp.IsRunning == false)
                return;
           
            sp.Stop();
            timer.Stop();
            foreach (var pilots in currRound.PilotsLaps)
            {
                pilots.RemovteDummy();
            }

            if (RaceType == RaceTypeEnum.BestLap)
            {
                foreach (var pilots in currRound.PilotsLaps)
                {
                   
                    pilots.CalculateRound();
                }
            }

            var counts = TotalRounds.Count(i => i.RoundNo == currRound.RoundNo);
            TotalRounds.Add(new CRound(currRound,DateTime.Now,counts+1));
            
            RaceRoundsDataGrid.ItemsSource = null;
            RaceRoundsDataGrid.ItemsSource = TotalRounds;

            ///update best lap for pilots

            foreach (var pilots in TotalRounds[TotalRounds.Count-1].PilotsLaps)
            {
                var p = Pilots.Find(item => item.Name == pilots.Pilot.Name);

                if (RaceType == RaceTypeEnum.BestLap)
                {
                    if (p.PilotBestRound == null || ((p.PilotBestRound.ClearBestLap > pilots.ClearBestLap) && (pilots.ClearBestLap != TimeSpan.Zero)))
                    {
                        p.PilotBestRound = pilots;
                        p.PilotBestRound.HeatNo = counts + 1;
                        p.Visible = true;
                    }
                }
                else
                {
                    if (p.PilotBestRound == null || ((p.PilotBestRound.TotalTime > pilots.TotalTime) && (pilots.TotalTime != TimeSpan.Zero) ))
                    {
                        p.PilotBestRound = pilots;
                        p.PilotBestRound.HeatNo = counts + 1;
                        p.Visible = true;
                    }
                }

            }

            if (RaceType == RaceTypeEnum.BestLap)
            {
                Pilots = Pilots.OrderBy(a => a.PilotBestRound.ClearBestLap != TimeSpan.Zero).ThenBy(a => a.PilotBestRound.ClearBestLap).ToList();
            }
            else
            {
                Pilots = Pilots.OrderBy(a => a.PilotBestRound.TotalTime != TimeSpan.Zero).ThenBy(a => a.PilotBestRound.TotalTime).ToList();
            }

            
            TopDataGrid.ItemsSource = Pilots;
            TopDataGrid.Items.Filter = FilterZero;

            ExportCSV();

            foreach (var pilots in currRound.PilotsLaps)
            {
                pilots.Laps.Clear();
                pilots.PenaltyCount = 0;
                pilots.ClearBestLap = TimeSpan.Zero;
                pilots.LastLap = TimeSpan.Zero;
                pilots.TotalTime = TimeSpan.Zero;
                pilots.BestLap = TimeSpan.Zero;
                pilots.ClearBestLapNo = 0;
                pilots.BestLapNo = 0;
                pilots.Active = true;
            }


           
        }

        private void ExportCSV()
        {
            try
            {

                string foldername = System.AppDomain.CurrentDomain.BaseDirectory + "\\Round" + currRound.RoundNo + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute+"_"+DateTime.Now.Second;
                Directory.CreateDirectory(foldername);
                TextWriter tw = new StreamWriter(foldername + "\\RoundResults.csv");
             
                tw.Write("Racer Name, Lap Count, Best Lap No, Best Lap Time,Best Clear Lap No,Best Clear Lap Time,Penalty Count, Total Time");
                tw.WriteLine();
                foreach (var pilot in currRound.PilotsLaps)
                {
                    tw.Write(pilot.Pilot + "," + pilot.CompletedLaps + "," + pilot.BestLapNo + "," + pilot.BestLap + "," +pilot.ClearBestLapNo+","+pilot.ClearBestLap+","+ pilot.PenaltyCount + "," + pilot.TotalTime);

                    tw.WriteLine();
                }

                tw.Close();

                TextWriter w = new StreamWriter(foldername + "\\TopPilots.csv");

                w.Write("Racer Name,Round No,Heat No, Lap Count, Best Lap No, Best Lap Time,Best Clear Lap No,Best Clear Lap Time,Penalty Count, Total Time");
                w.WriteLine();
                foreach (var pilot in Pilots)
                {
                    w.Write(pilot.Name + "," +pilot.AssignedRound+","+pilot.PilotBestRound.HeatNo+","+pilot.PilotBestRound.CompletedLaps + "," + pilot.PilotBestRound.BestLapNo + "," + pilot.PilotBestRound.BestLap + "," + pilot.PilotBestRound.ClearBestLapNo + "," + pilot.PilotBestRound.ClearBestLap + "," + pilot.PilotBestRound.PenaltyCount + "," + pilot.PilotBestRound.TotalTime);

                    w.WriteLine();
                }

                w.Close();


                foreach (var laps in currRound.PilotsLaps)
                {
                    TextWriter tw2 = new StreamWriter(foldername + "\\"+laps.Pilot+".csv");
                    tw2.Write("Lap No, Lap Time, Legit");
                    tw2.WriteLine();
                    foreach (var lap in laps.Laps)
                    {

                        tw2.Write(lap.LapNo + "," + lap.LapDuration + "," + lap.legit);
                        tw2.WriteLine();
                    }
                    tw2.Close();
                }




              



            }
            catch (Exception)
            {
                
                
            }
           
          
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Rounds.Count > currRound.RoundNo)
            {
                currRound = Rounds[currRound.RoundNo];

                if (Rounds.Count > nextRound.RoundNo)
                    nextRound = Rounds[currRound.RoundNo];
                
                SetupRound(currRound);
            } 
        }

        private void PrevButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Rounds.Count >= currRound.RoundNo && currRound.RoundNo>1)
            {
                currRound = Rounds[currRound.RoundNo-2];
                if (Rounds.Count > currRound.RoundNo)
                    nextRound = Rounds[currRound.RoundNo];
               
                SetupRound(currRound);
            }
        }

        private void RaceRoundsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(RaceRoundsDataGrid.SelectedItem == null)
                return;
            RoundsDataGrid.ItemsSource = ((CRound)RaceRoundsDataGrid.SelectedItem).PilotsLaps;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var res = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question,MessageBoxResult.No);
            if (res == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void RoundsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RoundsDataGrid.SelectedItem == null)
                return;

            var win = new PilotLapsWindow( ((CPilotRound)RoundsDataGrid.SelectedItem));
            win.ShowDialog();
        }

        private void startTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            nextTimer.Interval = 1000;
            nextTimer.Start();
            nextTimer.Elapsed += nextTimer_Elapsed;
        }

        void nextTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (s == 0 && m>0)
            {
                s = 59;
                m--;
          
            }
            else if (s == 0 && m == 0)
            {
                nextTimer.Stop();
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();

                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "timesup.wav";
                player.Play();
            }
            else
                s--;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                setupMinTimeLabel.Content = m;
                setupSecTimeLabel.Content = s;
            }));
        }

        private void set2TimerBtn_Click(object sender, RoutedEventArgs e)
        {
            s = 0;
            m = 2;
            setupMinTimeLabel.Content = m;
            setupSecTimeLabel.Content = s;

        }

        private void stopTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            nextTimer.Stop();
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();

            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "timesup.wav";
            player.Play();
            
        }

        private void set3TimerBtn_Click(object sender, RoutedEventArgs e)
        {
            s = 0;
            m = 3;
            setupMinTimeLabel.Content = m;
            setupSecTimeLabel.Content = s;
        }
    }
}
