using System.Collections.Generic;
using System.Collections;

/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FSM.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */
using System;

namespace GalaxyGen.Engine.Goap.Fsm
{
    public class FSM
    {

        private Stack<FSMState> stateStack = new Stack<FSMState>();

        public delegate void FSMState(FSM fsm, object target);


        public void Update(object target)
        {
            if (stateStack.Peek() != null)
                stateStack.Peek().Invoke(this, target);
        }

        public void pushState(FSMState state)
        {
            stateStack.Push(state);
        }

        public void popState()
        {
            stateStack.Pop();
        }
    }
}
