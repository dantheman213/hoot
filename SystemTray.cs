using Hoot.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hoot
{
    internal class SystemTray : ApplicationContext
    {
        private NotifyIcon tray;
        private Timer timer;
        private SpeechSynthesizer synth;

        public SystemTray()
        {
            tray = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Options", Options),
                    new MenuItem("Exit", Exit)
                }),
                Text = "Hoot",
                Visible = true
            };

            Start();
        }

        private void Start()
        {
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            //// temp
            //var p = new Popup();
            //p.Show();
            //synth.Speak(String.Format("The time is {0}", DateTime.Now.ToString("h:mm tt")));

            timer = new Timer();
            timer.Interval = (calculateNextInterval() * 1000); // 1 minute
            timer.Tick += new EventHandler(Tick);
            timer.Start();
        }

        private int calculateNextInterval()
        {
            var now = DateTime.Now;
            var minutesLeftTilInterval = 0;

            if (now.Minute >= 30)
            {
                minutesLeftTilInterval = 60 - now.Minute;
            }
            else
            {
                minutesLeftTilInterval = 30 - now.Minute;
            }

            var nextIntervalDateTime = now.AddMinutes(minutesLeftTilInterval);
            nextIntervalDateTime = new DateTime(nextIntervalDateTime.Year, nextIntervalDateTime.Month, nextIntervalDateTime.Day, nextIntervalDateTime.Hour, nextIntervalDateTime.Minute, 0);
            return Convert.ToInt32((nextIntervalDateTime - now).TotalSeconds);
        }

        private void Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Minute == 30 || DateTime.Now.Minute == 0)
            {
                var p = new Popup();
                p.Show();
                synth.Speak(String.Format("The time is {0}", DateTime.Now.ToString("h:mm tt")));
            }

            // set timer to 30 min intervals after initial timer fires off which will likely not have been on a 30 min even interval
            if (timer.Interval != (1800 * 1000))
            {
                timer.Interval = (1800 * 1000);
            }
        }

        private void Options(object sender, EventArgs e)
        {
            tray.Visible = false;
            Application.Exit();
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            tray.Visible = false;
            Application.Exit();
        }
    }
}
