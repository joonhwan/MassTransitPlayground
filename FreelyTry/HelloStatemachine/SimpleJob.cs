using System;
using Automatonymous;
using Stateless;

namespace HelloStatemachine
{
    public enum JobState
    {
        Running,
        Stopped,
    }

    public class SimpleJob
    {
        public JobState State { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }

        public override string ToString()
        {
            return $"State: {State}, Name: {Name}, Start: {Start}";
        }

        public int IntState
        {
            get { return _intState; }
            set { _intState = value; }
        }

        public string StringState
        {
            get { return State.ToString(); }
            set { State = (JobState) Enum.Parse(typeof(JobState), value); }
        }

        private int _intState;
    };

    public static class StatelessStatemachineExtension
    {
        public static StateMachine<TState, TTrigger>.StateConfiguration Handle<TState, TTrigger>(this StateMachine<TState, TTrigger>.StateConfiguration config,
                                                                       TTrigger trigger, Action action)
        {
            config.IgnoreIf(trigger, () =>
            {
                action();
                return true;
            });
            return config;
        }
    }

    public interface IHaveStateOf<TState>
    {
        TState State { get; set; }
    }
    
    public interface ITriggerLifter<TState>
    {
        void Raise(IHaveStateOf<TState> instance);
    }

    public class TriggerLifter<TState, TTrigger> : ITriggerLifter<TState>
    {
        private readonly IStateOwner<TState> _stateOwner;
        private readonly TTrigger _trigger;
        private readonly StateMachine<TState, TTrigger> _sm;

        public TriggerLifter(IStateOwner<TState> stateOwner, StateMachine<TState, TTrigger> sm, TTrigger trigger)
        {
            _stateOwner = stateOwner;
            _sm = sm;
            _trigger = trigger;
        }

        public void Raise(IHaveStateOf<TState> instance)
        {
            _stateOwner.StateOwner = instance;
            _sm.Fire(_trigger);
        }
    }

    public interface IStateOwner<TState>
    {
        IHaveStateOf<TState> StateOwner { get; set; }
    }
    
    public class SharedStateMachine<TState, TTrigger> : IStateOwner<TState>
    {
        IHaveStateOf<TState> IStateOwner<TState>.StateOwner { get; set; }

        private StateMachine<TState, TTrigger> _machine;
        
        protected StateMachine<TState, TTrigger> StateMachine
        {
            get { return _machine; }
        }
        
        public SharedStateMachine()
        {
            _machine = new StateMachine<TState, TTrigger>(
                () => ((IStateOwner<TState>)this).StateOwner.State, 
                state => ((IStateOwner<TState>)this).StateOwner.State = state);

        }

        public ITriggerLifter<TState> CreateTriggerLifter(TTrigger trigger)
        {
            return null;
        }

        public void Fire(IHaveStateOf<TState> instance, TTrigger trigger)
        {
            ((IStateOwner<TState>)this).StateOwner = instance;
            _machine.Fire(trigger);
        }
    }

    public class Tester
    {
        enum Trigger { Start, Stop }

        public static void Test()
        {
            var sm = new SharedStateMachine<JobState, Trigger>();
        }
    }

    public class SimpleJobStateMachine
    {
        private SimpleJob _job;
        private StateMachine<JobState, Trigger> _machine;

        enum Trigger { Start, Stop }

        public Action<string> Logged;

        public SimpleJobStateMachine()
        {
            _machine = new Stateless.StateMachine<JobState, Trigger>(() => Job.State, state => Job.State = state);
            
            _machine.Configure(JobState.Stopped)
                    .OnEntry(() => Print($"Now Stopped {Job.Name}"))
                    .Permit(Trigger.Start, JobState.Running)
                    .Handle(Trigger.Stop, () => Print($"{Job.Name} : Ignore Stop while Stopped"))
                ;
            
            _machine.Configure(JobState.Running)
                    .OnEntry(() => Print($"{Job.Name} : Now Running"))
                    .Permit(Trigger.Stop, JobState.Stopped)
                    .Handle(Trigger.Start, () => Print($"{Job.Name} : Ignore Start while Running"))
                    ;
        }

        private void Print(string s)
        {
            Logged?.Invoke(s);
        }

        public SimpleJob Job
        {
            get { return _job; }
            set { _job = value; }
        }

        public void Start(SimpleJob job)
        {
            _job = job;
            _machine.Fire(Trigger.Start);
        }

        public void Stop(SimpleJob job)
        {
            _job = job;
            _machine.Fire(Trigger.Stop);
        }
    }

    public class RemixJobStateMachine : AutomatonymousStateMachine<SimpleJob>
    {
        public RemixJobStateMachine()
        {
            InstanceState(job => job.StringState);

            //DuringAny(
            //    When(Start).Then(context => Print($"{context.Instance.ToString()} : Received Start!")),
            //    When(Stop).Then(context => Print($"{context.Instance.ToString()} : Received Stop!"))
            //    );

            Initially(
                When(Start).Then(context => Print($"{context.Instance.Name} : Initially Start!")).TransitionTo(Running),
                When(Stop).Then(context => Print($"{context.Instance.Name} : Initially Stop!")).TransitionTo(Stopped));
            During(Running, When(Start).Then(context => Print($"{context.Instance.Name} : During Running / Start!")),
                When(Stop)
                    .Then(context => Print($"{context.Instance.Name} : During Running / Stop!"))
                    .TransitionTo(Stopped));
            During(Stopped,
                When(Start)
                    .Then(context => Print($"{context.Instance.Name} : During Stopped / Start!"))
                    .TransitionTo(Running),
                When(Stop).Then(context => Print($"{context.Instance.Name} : During Stopped / Stop!")));
        }

        private void Print(string s)
        {
            //Console.WriteLine(s);
            Logged?.Invoke(s);
        }

        public Action<string> Logged;

        //public State Registered { get; private set; }
        //public State Queued { get; private set; }
        public State Running { get; private set; }
        public State Stopped { get; private set; }
        //public State Cacelled { get; private set; }
        //public State Finished { get; private set; }

        public Event Start { get; private set; }
        public Event Stop { get; private set; }
    }
}