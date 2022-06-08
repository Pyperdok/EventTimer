using System;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Media;

namespace EventTimer
{
    using Timer = System.Timers.Timer;
    public class ModelEventTimer
    {
        private Timer _timer;
        private DateTime _endTime;
        private double _remainTime;
        private double _totalTime;

        public readonly double Interval = 10;
        public event Action<TimeSpan> Updated;
        public event Action Ended;

        public Keys Key { get; set; }
        public string Icon { get; set; }
        public string Desc { get; set; }
        public SolidColorBrush Color { get; set; }

        public double TotalTime { get => _totalTime; 
            set 
            {
                _totalTime = value;
                RemainTime = value; 
            } 
        }

        public double RemainTime 
        {
            get => _remainTime;
            private set
            {
                _endTime = DateTime.Now.AddSeconds(value);
                _remainTime = value;
            }
        }

        public bool IsEnabled { get => _timer.Enabled; }

        private void OnTimerUpdated(object sender, ElapsedEventArgs e)
        {
            TimeSpan remainTime = new TimeSpan(0);
            if (e.SignalTime <= _endTime)
            {
                remainTime = _endTime - e.SignalTime;
                _remainTime = remainTime.TotalSeconds;
            }
            else
            {
                OnTimerEnded();
            }
            Updated?.Invoke(remainTime);
        }

        private void OnTimerEnded()
        {
            Console.WriteLine("Timer Ended");
            _timer.Stop();            
            Ended?.Invoke();

            Ended = null;
            Updated = null;
        }

        public ModelEventTimer()
        {
            _timer = new Timer(Interval);
            _timer.Elapsed += OnTimerUpdated;
        }

        public void Refresh()
        {
            RemainTime = TotalTime;
        }

        public void Start()
        {
            Refresh();
            _timer.Start();
        }

        public void Reset()
        {
            RemainTime = 0;
        }
        
        public override string ToString()
        {
            return
            "{\n" +
                $"\t\"icon\": \"{Icon}\",\n" +
                $"\t\"seconds\": {TotalTime},\n" +
                $"\t\"key\": \"{Key}\",\n" +
                $"\t\"description\": \"{Desc}\",\n" +
                $"\t\"color\": \"{Color.Color.ToString().Replace("#FF", "#")}\"\n" +
            "}";
        }
    }
}
