using Hoot.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private TimeSpan startWorkingHours;
        private TimeSpan endWorkingHours;

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

            // TODO: make this configurable
            startWorkingHours = new TimeSpan(9, 0, 0);
            endWorkingHours = new TimeSpan(17, 0, 0);

            // TODO: make this configurable
            // Check if the application is already set to run on startup
            if (!Debugger.IsAttached && !Startup.IsAppSetToRunOnStartup())
            {
                Startup.SetAppToRunOnStartup();
                Console.WriteLine("Application set to run on startup");
            }

            Start();

            if (Debugger.IsAttached)
            {
                DebugModeAlert();
            }
        }

        private void DebugModeAlert()
        {
            Task.Delay(5000).Wait();
            alert();
        }

        private void Start()
        {
            lastAlert = DateTime.Now.AddMinutes(-6);
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            timer = new Timer();
            timer.Interval = (5000);
            timer.Tick += new EventHandler(Tick);
            timer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var day = now.DayOfWeek;

            // only run between Mon-Fri 9am-5pm
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Friday)
            {
                // TODO: make this configurable
                if (now.TimeOfDay >= startWorkingHours && now.TimeOfDay <= endWorkingHours)
                {
                    if ((now.Minute == 0 || now.Minute == 30) && (now - lastAlert).TotalMinutes > 5)
                    {
                        alert();
                    }
                }
            }
        }

        private void alert()
        {
            lastAlert = DateTime.Now;

            if(!DetectFullscreen.IsExclusiveFullscreen())
            {
                var p = new Popup();
                p.Show();
            }

            synth.Speak(String.Format("The time is {0}", DateTime.Now.ToString("h:mm tt")));
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
