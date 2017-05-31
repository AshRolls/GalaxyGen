using System.Collections;


namespace GalaxyGen.Engine.Goap.Fsm
{
    public interface FSMState
    {

        void Update(FSM fsm, object obj);
    }

}