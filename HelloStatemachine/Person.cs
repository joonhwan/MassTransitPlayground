using Automatonymous;

namespace HelloStatemachine
{
    public class Person
    {
        public State CurrentState { get; set; }
        public string Name { get; set; }
    }
}