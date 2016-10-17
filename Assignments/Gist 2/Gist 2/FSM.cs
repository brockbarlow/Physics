using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSM
{
    public struct Transition<T> //generic
    {
        public T from; //current trans
        public T to; //next trans
    }

    public class FSM<T> //generic
    {
        public T currentState;
        public List<T> states;
        public List<Transition<T>> _transition;

        public FSM() { }

        public FSM(T state)
        {
            states = new List<T>();
            _transition = new List<Transition<T>>();
            AddState(state);
            currentState = state;
        }

        public bool AddState(T state)
        {
            if (states.Contains(state))
            {
                return false;
            }
            states.Add(state);
            return true;
        }

        public bool AddTransition(T from, T to)
        {
            Transition<T> temp = new Transition<T>();
            temp.from = from;
            temp.to = to;
            if (_transition.Contains(temp))
            {
                return false;
            }
            _transition.Add(temp);
            return true;
        }

        public T changeStates(T to)
        {
            Transition<T> temp = new Transition<T>();
            temp.from = this.currentState;
            temp.to = to;
            foreach (Transition<T> t in _transition)
            {
                if (t.Equals(temp))
                {
                    this.currentState = t.to;
                    return currentState;
                }
            }
            return currentState;
        }
    }
}