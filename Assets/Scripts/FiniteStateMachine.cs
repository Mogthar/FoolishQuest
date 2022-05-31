using System.Collections.Generic;

namespace Patterns
{
    public class State
    {
        public virtual void Enter() { }
        public virtual void Exit() { }

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }

    public class FSM
    {
        protected Dictionary<int, State> _states;
        protected State _currentState;

        public FSM()
        {
            _states = new Dictionary<int, State>();
        }

        public void Update()
        {
            if(_currentState != null)
            {
                _currentState.Update();
            }
        }

        public void FixedUpdate()
        {
            if(_currentState != null)
            {
                _currentState.FixedUpdate();
            }
        }

        public void AddState(int key, State state)
        {
            _states.Add(key, state);
        }

        public State GetState(int key)
        {
            return _states[key];
        }

        public void SetCurrentState(State state)
        {
            if(_currentState != null)
            {
                // in case we need to do somehting to the current state before changing it
                _currentState.Exit();
            }
            _currentState = state;

            if(_currentState != null)
            {
                _currentState.Enter();
            }

        }

        public State GetCurrentState()
        {
            return _currentState;
        }

    }

    public class Command
    {
        public virtual void Execute() { }
    }
}
