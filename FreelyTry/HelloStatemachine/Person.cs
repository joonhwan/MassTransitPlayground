using System;
using Automatonymous;
using Automatonymous.States;

namespace HelloStatemachine
{
    public class Person
    {
        public enum Sangte
        {
            Chingu = 0,
            Jeok,
        }
        public Sangte SangteValue { get; set; }
        public string Name { get; set; }
        
        public Person()
        {
            SangteValue = Sangte.Chingu;
        }

        public int StateInInteger
        {
            get
            {
                switch (SangteValue)
                {
                case Sangte.Chingu:
                    return 0;
                case Sangte.Jeok:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (value)
                {
                case 0: 
                    SangteValue = Sangte.Chingu;
                    break;
                case 1:
                    SangteValue = Sangte.Jeok;
                    break;
                }
            }
        }
    }

    public class PersonSmInstance
    {
        private HumanRelationshipStateMachine _sm;

        public PersonSmInstance(HumanRelationshipStateMachine sm, Person data)
        {
            _sm = sm;
            Data = data;
        }

        public Person Data { get; private set; }

        public State CurrentState
        {
            get { return GetSmState(); }
            set { SetSmState(value); }
        }

        private State GetSmState()
        {
            switch (Data.SangteValue)
            {
            case Person.Sangte.Chingu:
                return _sm.Friendly;
            case Person.Sangte.Jeok:
                return _sm.Enemy;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private void SetSmState(State state)
        {
            Person.Sangte sangte = Person.Sangte.Chingu;
            if (state == _sm.Enemy)
                sangte = Person.Sangte.Jeok;
            else if (state == _sm.Friendly)
                sangte = Person.Sangte.Chingu;
            Data.SangteValue = sangte;
        }
    }

    public class StateMachineInstance<T>
    {
        private IKnowState _stateOwner;
        public T Source { get; private set; }

        private State GetState()
        {
            IKnowState translator = null;
            return translator.State;
        }

        public StateMachineInstance(IKnowState stateOwner)
        {
            _stateOwner = stateOwner;
        }
    }

    public interface IKnowState
    {
        Automatonymous.State State { get; set; }
    }
}