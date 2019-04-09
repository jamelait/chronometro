using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using ChronoMetro.Business;
using ChronoMetro.Data.Interval;
using ChronoMetro.Resources;
using ChronoMetro.UserControls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ChronoMetro.ViewModels
{
    public class IntervalPageViewModel : ViewModelBase
    {
        private IntervalBlockStore _intervalBlockStorage;
        private TimeSpan _remainingTime;
        private TimeSpan _orignalTime;
        private TimeSpan _elapsedTime;
        private readonly DispatcherTimer _dispatcherTimer;
        private string _buttonStartStopText;
        private SolidColorBrush _buttonStartStopBackground;

        public ObservableCollection<IntervalBlock> Blocks { get; set; }
        public IntervalBlock CurrentBlock { get { return Blocks.FirstOrDefault(); } }
        public bool HasStarted { get; set; }
        public bool HasFinished { get; private set; }
        
        public SolidColorBrush ButtonStartStopBackground
        {
            get { return _buttonStartStopBackground; }
            set { this.SetProperty(ref _buttonStartStopBackground, value); }
        }

        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set { this.SetProperty(ref _remainingTime, value); }
        }

        public String ButtonStartStopText
        {
            get { return _buttonStartStopText; }
            set { this.SetProperty(ref _buttonStartStopText, value); }
        }

        public IntervalPageViewModel()
        {
            HasStarted = false;
            RemainingTime = TimeSpan.FromSeconds(0);
            Blocks = new ObservableCollection<IntervalBlock>();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += Tick;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);

            _elapsedTime = TimeSpan.FromSeconds(0);

            ButtonStartStopText = AppResources.ButtonStart;
            ButtonStartStopBackground = Common.GreenColorBrush;

            _intervalBlockStorage = IntervalBlockStore.Prepare();
        }

        public void Tick(object sender, EventArgs e)
        {
            if (HasFinished)
                return;
            // update general Time
            if (CurrentBlock != null)
                //RemainingTime = RemainingTime.Subtract(currentBlock.OriginalTime.Subtract(currentBlock.Time));
                RemainingTime = _orignalTime.Subtract(_elapsedTime).Subtract(CurrentBlock.ElapsedTime);
            else
                RemainingTime = TimeSpan.FromSeconds(0);

            // check if current block is still  active, if not go to next block
            if (CurrentBlock == null)
                return;
            
            // démarrer le temps et la progress bar du nouveau block
        }

        public void Start()
        {
            if (HasFinished || !Blocks.Any())
                return;

            _dispatcherTimer.Start();
            StartNextBlock();

            ButtonStartStopText = AppResources.ButtonStop;
            ButtonStartStopBackground = Common.RedColorBrush;

            //if (!HasStarted)
            //    _orignalTime = RemainingTime; // keep the original time in memory so we can substract later
            //else
                HasStarted = true;

            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        public void Stop()
        {
            if (CurrentBlock != null)
                CurrentBlock.Stop();
            _dispatcherTimer.Stop();

            ButtonStartStopText = AppResources.ButtonStart;
            ButtonStartStopBackground = Common.GreenColorBrush;

            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        public void AddBlock(String label, TimeSpan time)
        {
            if (HasStarted)
                return;

            var interval = new IntervalBlock();
            interval.CenterText = label;
            interval.CurrentTime = time;
            interval.OnFinished += delegate(object o, EventArgs args)
            {
                _elapsedTime = _elapsedTime.Add(interval.OriginalTime);
                if (Blocks.Count == Blocks.Count(b => b.HasFinished))
                {
                    Stop();
                    HasFinished = true;
                    var oldBlock = CurrentBlock;
                    Blocks.RemoveAt(0);
                    Blocks.Add(oldBlock);
                }
                else
                {
                    StartNextBlock();
                }

                Common.PlaySound(Common.AlertSound1);
            };
            RemainingTime = RemainingTime.Add(interval.CurrentTime);
            _orignalTime = RemainingTime; // keep the original time in memory so we can substract when running
            Blocks.Add(interval);
        }



        private void StartNextBlock()
        {
            if (CurrentBlock == null) return;

            if (CurrentBlock.IsRunning)
            {
                CurrentBlock.Stop();
                var oldBlock = CurrentBlock;
                Blocks.RemoveAt(0);
                Blocks.Add(oldBlock);
            }
            CurrentBlock.Start();
        }
        
        public void Reset()
        {
            _elapsedTime = TimeSpan.FromSeconds(0);
            RemainingTime = _orignalTime;
            HasFinished = false;
            HasStarted = false;
            var orderBlocks = new ObservableCollection<IntervalBlock>(Blocks.OrderBy(b => b.Order));
            Blocks.Clear();
            foreach (var item in orderBlocks)
            {
                Blocks.Add(item);
            }
            
            foreach (var intervalBlock in Blocks)
            {
                intervalBlock.Reset();
            }
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        public bool Persist(string name)
        {
            return _intervalBlockStorage.SaveNew(name, Blocks);
        }

        public void Populate(Guid guid)
        {
            Blocks.Clear();
            var v = _intervalBlockStorage.Get(guid);
            foreach (var block in v.Blocks)
            {
                AddBlock(block.Key, block.Value);
            }
        }
    }
}
