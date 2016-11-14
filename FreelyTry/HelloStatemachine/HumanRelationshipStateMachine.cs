using System;
using System.Linq.Expressions;
using Automatonymous;

namespace HelloStatemachine
{
    public class HumanRelationshipStateMachine : AutomatonymousStateMachine<Person>//<PersonSmInst>
    {
        public HumanRelationshipStateMachine()
        {
            //InstanceState(person => (new PersonStateTranlator(this, person)).CurrentState);
            InstanceState(person => person.StateInInteger, Friendly, Enemy);

            Event(() => Hello);
            Event(() => PissOff);
            Event(() => Introduce);

            State(() => Friendly);
            State(() => Enemy);
            
            Initially(
                When(Hello)
                    .Then(_ => Print($"{_.Instance.Name} got Hello initially"))
                    .TransitionTo(Friendly),
                When(PissOff)
                    .Then(_ => Print($"{_.Instance.Name} got PissOff initially"))
                    .TransitionTo(Enemy),
                When(Introduce)
                    .Then(_ => Print($"{_.Instance.Name} got Introduce initially"))
                    .Then(context =>
                    {
                        Print("Context Info: {0}", context.Instance);
                    })
                    .TransitionTo(Friendly)
                );
            During(Friendly,
                When(Hello)
                    .Then(context =>
                    {
                        Print($"Hello {context.Instance.Name}! Good to see you");
                    }),
                When(PissOff)
                    .Then(context =>
                    {
                        Print("What's wrong?");
                    })
                    .TransitionTo(Enemy),
                When(Introduce)
                    .Then(_ => Print("I know you hehe!"))
                );
            During(Enemy,
                When(Hello)
                    .TransitionTo(Friendly),
                When(PissOff)
                    .Then(context => Print($"Fuck you {context.Instance.Name}!")),
                When(Introduce)
                    .Then(context => Print($"Get away!"))
                );
        }

        public Event Hello { get; private set; }
        public Event PissOff { get; private set; }
        public Event Introduce { get; private set; }

        public State Friendly { get; private set; }
        public State Enemy { get; private set; }

        public Action<string> Logged;

        public void Print(string format, params object[] args)
        {
            Logged?.Invoke(string.Format(format, args));
        }
    }
}