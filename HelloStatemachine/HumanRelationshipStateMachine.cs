using System;
using Automatonymous;

namespace HelloStatemachine
{
    public class HumanRelationshipStateMachine : AutomatonymousStateMachine<Person>
    {
        public HumanRelationshipStateMachine()
        {
            Event(() => Hello);
            Event(() => PissOff);
            Event(() => Introduce);

            State(() => Friendly);
            State(() => Enemy);

            Initially(
                When(Hello)
                    .TransitionTo(Friendly),
                When(PissOff)
                    .TransitionTo(Enemy),
                When(Introduce)
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
                    .TransitionTo(Enemy)
                );
            During(Enemy,
                When(Hello)
                    .Then(context => Print("...")),
                When(PissOff)
                    .Then(context => Print($"Fuck you {context.Instance.Name}"))
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