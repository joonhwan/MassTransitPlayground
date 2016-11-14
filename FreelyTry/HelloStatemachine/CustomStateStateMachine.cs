using Automatonymous;

namespace HelloStatemachine
{
    public class CustomStateStateMachine<T> : AutomatonymousStateMachine<T>
        where T : class
    {
        public CustomStateStateMachine()
        {
        }
    }
    
}