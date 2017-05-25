using System.Collections;


namespace GalaxyGen.Engine.Ai.Fsm
{
    public interface FSMState
    {

        void Update(FSM fsm, object obj);
    }

}