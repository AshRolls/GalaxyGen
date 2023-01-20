using System.Collections;


namespace GalaxyGenEngine.Engine.Ai.Fsm
{
    public interface FSMState
    {

        void Update(FSM fsm, object obj);
    }

}