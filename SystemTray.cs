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
        private DateTime lastAlert;

        public SystemTray()
        {
            tray = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Settings", Settings),
                    new MenuItem("Exit", Exit)
                }),
                Text = "Hoot",
                Visible = true
            };

            Start();
        }

        private void Start()
        {
            lastAlert = DateTime.Now.AddMinutes(-6);
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            timer = new Timer();
            timer.Interval = (5000); // run every 5 seconds
            timer.Tick += new EventHandler(Tick);
            timer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            if ((now.Minute == 0 || now.Minute == 30) && (now - lastAlert).TotalMinutes > 5)
            {
                lastAlert = now;
                var p = new Popup();
                p.Show();

                synth.Speak(String.Format("The time is {0}", DateTime.Now.ToString("h:mm tt")));
            }
        }

        private void Settings(object sender, EventArgs e)
        {
            new FormSettings().Show();
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            tray.Visible = false;
            Application.Exit();
        }
    }
}
