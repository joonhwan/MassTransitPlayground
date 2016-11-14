using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Automatonymous;

namespace HelloStatemachine
{
    public partial class StatelessJobTestForm : Form
    {
        public StatelessJobTestForm()
        {
            InitializeComponent();

            InitUi();
            InitSm();
        }

        private SimpleJob Current
        {
            get { return jobComboBox.SelectedItem as SimpleJob; }
        }

        private int instanceIndex = 0;
        private void InitUi()
        {
            jobComboBox.DisplayMember = "Name";
            addButton.Click += (sender, args) =>
            {
                var initState = instanceIndex%2 > 0 ? JobState.Running : JobState.Stopped;
                var job = new SimpleJob()
                             {
                                 Name = $"Job{instanceIndex++}",
                                 State = initState
                                 //State = "Stopped",
                             };
                jobComboBox.Items.Add(job);
                Log("Created New Job : {0}", job);
                jobComboBox.SelectedItem = job;
            };
        }

        private void InitSm()
        {
            var sm = new SimpleJobStateMachine();
            sm.Logged += s => Log(s);
            startButton.Click += (s, e) => WithCurrent(job => sm.Start(job));
            stopButton.Click += (s, e) => WithCurrent(job => sm.Stop(job));
        }

        private void WithCurrent(Action<SimpleJob> action)
        {
            if (Current != null)
            {
                action(Current);
                //Log($"Person[{CurrentPerson.Name}] => {CurrentPerson.SangteValue}");
                Log($"Job[{Current.Name}] => {Current.State}");
            }
        }

        private void Log(string format, params object[] args)
        {
            var line = string.Format(format, args) + Environment.NewLine;
            logTextBox.AppendText(line);
        }
    }
}
