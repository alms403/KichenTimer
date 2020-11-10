using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Threading;
using System.Runtime.InteropServices;
using Android.Media;

namespace KichenTimer
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private int _remainingMilliSec = 0;
        private bool _isStart = false;
        private Button _startButton;
        private Button _add10MinButton;
        private Button _add1MinButton;
        private Button _add10SecButton;
        private Button _add1SecButton;
        private Timer _timer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // [+10分]ボタン
            _add10MinButton = FindViewById<Button>(Resource.Id.Add10MinButton);
            _add10MinButton.Click += Add10MinButton_Click;

            // [+1分]ボタン
            _add1MinButton = FindViewById<Button>(Resource.Id.Add1MinButton);
            _add1MinButton.Click += Add1MinButton_Click;

            // [+10秒]ボタン
            _add10SecButton = FindViewById<Button>(Resource.Id.Add10SecButton);
            _add10SecButton.Click += Add10SecButton_Click;

            // [+1秒]ボタン
            _add1SecButton = FindViewById<Button>(Resource.Id.Add1SecButton);
            _add1SecButton.Click += Add1SecButton_Click;

            // [クリア]ボタン
            var ClearButton = FindViewById<Button>(Resource.Id.ClearButton);
            ClearButton.Click += ClearButton_Click;

            // [スタート]ボタン
            _startButton = FindViewById<Button>(Resource.Id.StartButton);
            _startButton.Click += _startButton_Click;

            // タイマー
            _timer = new Timer(Timer_OnTick, null, 0, 100);
        }
        private void Timer_OnTick(object state)
        {
            if(!_isStart)
            {
                return;
            }

            RunOnUiThread(() =>
            {
                _remainingMilliSec -= 100;
                if (_remainingMilliSec <= 0)
                {
                    // 0ミリ秒になった
                    _isStart = false;
                    _remainingMilliSec = 0;
                    _startButton.Text = "スタート";
                    _add10MinButton.Enabled = true;
                    _add1MinButton.Enabled = true;
                    _add10SecButton.Enabled = true;
                    _add1SecButton.Enabled = true;

                    // アラームを鳴らす
                    var toneGenerator = new ToneGenerator(Stream.System, 100);
                    toneGenerator.StartTone(Tone.PropBeep);
                }
                ShowRemainingTime();
            });
        }
        private void _startButton_Click(object sender, System.EventArgs e)
        {
            if(!_isStart && _remainingMilliSec == 0)
            {
                return;
            }
            _isStart = !_isStart;
            if(_isStart)
            {
                _startButton.Text = "ストップ";
                _add10MinButton.Enabled = false;
                _add1MinButton.Enabled = false;
                _add10SecButton.Enabled = false;
                _add1SecButton.Enabled = false;
            }
            else
            {
                _startButton.Text = "スタート";
                _add10MinButton.Enabled = true;
                _add1MinButton.Enabled = true;
                _add10SecButton.Enabled = true;
                _add1SecButton.Enabled = true;
            }
        }

        private void ClearButton_Click(object sender, System.EventArgs e)
        {
            _remainingMilliSec = 0;
            ShowRemainingTime();
        }

        private void Add10MinButton_Click(object sender, System.EventArgs e)
        {
            _remainingMilliSec += 600 * 1000;
            ShowRemainingTime();
        }
        private void Add1MinButton_Click(object sender, System.EventArgs e)
        {
            _remainingMilliSec += 60 * 1000;
            ShowRemainingTime();
        }
        private void Add10SecButton_Click(object sender, System.EventArgs e)
        {
            _remainingMilliSec += 10 * 1000;
            ShowRemainingTime();
        }
        private void Add1SecButton_Click(object sender, System.EventArgs e)
        {
            _remainingMilliSec += 1 * 1000;
            ShowRemainingTime();
        }

        private void ShowRemainingTime()
        {
            var sec = _remainingMilliSec / 1000;
            FindViewById<TextView>(Resource.Id.RemainingTimeTextView).Text = string.Format("{0:f0}:{1:d2}", sec / 60, sec % 60);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}