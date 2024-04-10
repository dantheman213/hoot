using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hoot
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            generateTimePickerItems(cbStartTime);
            cbStartTime.SelectedIndex = 18; // 9am

            generateTimePickerItems(cbEndTime);
            cbEndTime.SelectedIndex = 34; // 5pm

            cbAlertFrequency.SelectedIndex = 0;

            generateSpeechVoices(cbVoiceType); 
            cbVoiceType.SelectedIndex = 0;
        }

        private void generateTimePickerItems(ComboBox cb)
        {
            var now = DateTime.Now;
            var timeFrame = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var tomorrow = timeFrame.AddDays(1);
            while (timeFrame < tomorrow) 
            {
                cb.Items.Add(timeFrame.ToString("h:mm tt"));
                timeFrame = timeFrame.AddMinutes(30);
            }
        } 

        private void generateSpeechVoices(ComboBox cb)
        {
            var synth = new SpeechSynthesizer();
            var voices = synth.GetInstalledVoices();
            foreach ( var voice in voices )
            {
                cb.Items.Add(voice.VoiceInfo.Name);
            }
        }
    }
}
