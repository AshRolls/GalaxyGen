using System.Collections;


namespace GCEngine.Engine.Ai.Fsm
{
    public interface FSMState
    {

        void Update(FSM fsm, object obj);
    }

}