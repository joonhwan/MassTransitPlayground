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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            InitUi();
            InitSm();
        }

        private Person CurrentPerson
        {
            get { return personsComboBox.SelectedItem as Person; }
        }

        private int personIndex = 0;
        private void InitUi()
        {
            personsComboBox.DisplayMember = "Name";
            addButton.Click += (sender, args) =>
            {
                var person = new Person()
                             {
                                 Name = $"Person{personIndex++}",
                             };
                personsComboBox.Items.Add(person);
            };
        }

        private void InitSm()
        {
            var sm = new HumanRelationshipStateMachine();
            sm.Logged += s => Log(s);
            var helloEvent = sm.CreateEventLift(sm.Hello);
            var pissOffEvent = sm.CreateEventLift(sm.PissOff);
            var introduceEvent = sm.CreateEventLift(sm.Introduce);
            helloButton.Click += (s, e) => WithCurrentPerson(p => helloEvent.Raise(p));
            pissOffButton.Click += (s, e) => WithCurrentPerson(p => pissOffEvent.Raise(p));
            introduceButton.Click += (s, e) => WithCurrentPerson(p => introduceEvent.Raise(p));
        }

        private void WithCurrentPerson(Action<Person> action)
        {
            if (CurrentPerson != null)
            {
                action(CurrentPerson);
                Log($"Person[{CurrentPerson.Name}] => {CurrentPerson.CurrentState}");
            }
        }

        private void Log(string format, params object[] args)
        {
            var line = string.Format(format, args) + Environment.NewLine;
            logTextBox.AppendText(line);
        }
    }
}
