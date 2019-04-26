using System.Collections.Generic;
using UnityEngine;

namespace Nox.SimpleHSM
{
    public enum TransitionType
    {
        None,
        Sibling,
        Inner,
    }

    public struct Transition
    {
        public TransitionType Type;
        public State FromState;
        public State ToState;

        public static Transition None()
        {
            return new Transition
            {
                Type = TransitionType.None,
                FromState = null,
                ToState = null,
            };
        }
    }

    public abstract class State
    {
        public object _owner;
        public object _context;

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract Transition EvaluateTransition();

        public abstract void Update(float timeDelta);

        public abstract Transition GetTransition<T>(TransitionType transitionType) where T : State, new();
    }

    public class State<TOwner, TContext> : State
        where TOwner : class
        where TContext : class
    {
        public TOwner Owner => _owner as TOwner;
        public TContext Context => _context as TContext;

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override Transition EvaluateTransition()
        {
            return SimpleHSM.Transition.None();
        }

        public override void Update(float timeDelta)
        {

        }

        public override Transition GetTransition<T>(TransitionType transitionType)
        {
            var toState = new T();
            toState._owner = Owner;
            toState._context = Context;

            return new Transition { Type = transitionType, FromState = this, ToState = toState };
        }
    }

    public class HSM
    {
        public int LogLevel = 0;
        private Stack<State> _stateStack = new Stack<State>();

        public void Initialize<T>() where T : State, new()
        {
            PushState(new T());
        }

        public void Update(float deltaTime)
        {
            if (_stateStack.Count == 0)
            {
                return;
            }

            foreach (var state in _stateStack)
            {
                state.Update(deltaTime);
            }

            foreach (var state in _stateStack)
            {
                var transition = state.EvaluateTransition();
                switch (transition.Type)
                {
                    case TransitionType.Sibling:
                    {
                        State poppedState = null;
                        while (poppedState != transition.FromState)
                        {
                            poppedState = PopState();
                        }

                        PushState(transition.ToState);
                        Log(transition);
                        return;
                    }

                    case TransitionType.Inner:
                    {
                        PushState(transition.ToState);
                        Log(transition);
                        return;
                    }

                    case TransitionType.None:
                    default:
                    {
                        Log(transition);
                        break;
                    }
                }
            }
        }

        private State PopState()
        {
            var poppedState = _stateStack.Pop();
            poppedState?.OnExit();
            return poppedState;
        }

        private void PushState(State state)
        {
            state?.OnEnter();
            _stateStack.Push(state);
        }

        private void Log(Transition transition)
        {
            if (LogLevel > 0 && transition.Type != TransitionType.None)
            {
                Debug.Log($"[{GetType()}]: {transition.Type} transition from {transition.FromState} to {transition.ToState.GetType()}");
            }
        }
    }
}