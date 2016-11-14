using System;
using Automatonymous;

namespace HelloStatemachine
{
    public class PersonStateTranlator
    {
        private readonly Person _person;
        private readonly HumanRelationshipStateMachine _sm;

        public PersonStateTranlator(HumanRelationshipStateMachine sm, Person person)
        {
            _sm = sm;
            _person = person;
        }

        public State CurrentState
        {
            get { return GetSmState(); }
            set { SetSmState(value); }
        }

        private State GetSmState()
        {
            switch (_person.SangteValue)
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
            if(state == _sm.Enemy)
                sangte = Person.Sangte.Jeok;
            else if (state == _sm.Friendly)
                sangte = Person.Sangte.Chingu;
            _person.SangteValue = sangte;
        }
    }
}