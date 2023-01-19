using System.Collections;
using System.Collections.Generic;
using System;
using GalaxyGenEngine.Engine.Ai.Fsm;
using GalaxyGenEngine.Engine.Controllers;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    public sealed class GoapAgent
    {
        private FSM _stateMachine;

        private FSM.FSMState _idleState; // finds something to do
        private FSM.FSMState _moveToState; // moves to a target
        private FSM.FSMState _performActionState; // performs an action

        private HashSet<GoapAction> _availableActions;
        private Queue<GoapAction> _currentActions;

        private IGoap _dataProvider; // this is the implementing class that provides our world data and listens to feedback on planning
        public IAgentActions ActionProvider; // this is the class that will perform actions from the goap
        public IAgentControllerState StateProvider;

        private GoapPlanner _planner;

        public GoapAgent(IGoap provider, IAgentActions actions, IAgentControllerState state)
        {
            _dataProvider = provider;
            ActionProvider = actions;
            StateProvider = state;
            _stateMachine = new FSM();
            _availableActions = new HashSet<GoapAction>();
            _currentActions = new Queue<GoapAction>();
            _planner = new GoapPlanner();            
            createIdleState();
            createMoveToState();
            createPerformActionState();
            _stateMachine.PushState(_idleState);
            loadActions();
        }

        public void Tick()
        {
            _stateMachine.Update(this);
        }

        public void ResetPlan()
        {
            _stateMachine.ClearState();
            _stateMachine.PushState(_idleState);
        }

        private bool hasActionPlan()
        {
            return _currentActions.Count > 0;
        }

        private void createIdleState()
        {
            _idleState = (fsm, gameObj) =>
            {
                // GOAP planning
                _planner.Reset();
                // get the world state and the goal we want to plan for
                GoapStateBit worldState = _dataProvider.GetWorldState(_planner);
                (bool goalSuccess, GoapStateBit goal, GoapStateBit allowed) = _dataProvider.CreateGoalState(_planner);
                if (goalSuccess)
                {
                    loadActions();
                    // Plan
                    (Queue<GoapAction> plan, (int, long) stats) = _planner.PlanBit(this, _availableActions, worldState, goal, allowed);
                    if (plan != null)
                    {
                        // we have a plan
                        _currentActions = plan;
                        _dataProvider.PlanFound(goal, plan, stats);

                        fsm.PopState(); // move to PerformAction state
                        fsm.PushState(_performActionState);
                    }
                    else
                    {
                        // we couldn't get a plan
                        _dataProvider.PlanFailed(goal);
                        fsm.PopState(); // move back to IdleAction state
                        fsm.PushState(_idleState);
                    }
                }
                else
                {
                    fsm.PopState(); // move back to IdleAction state
                    fsm.PushState(_idleState);
                }

            };
        }

        private void createMoveToState()
        {
            _moveToState = (fsm, gameObj) =>
            {
                // move the game object

                GoapAction action = _currentActions.Peek();
                if (action.RequiresInRange() && action.target == null)
                {
                    // Console.WriteLine("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                    fsm.PopState(); // move
                    fsm.PopState(); // perform
                    fsm.PushState(_idleState);
                    return;
                }

                // get the agent to move itself
                if (_dataProvider.MoveAgent(action))
                {
                    fsm.PopState();
                }
            };
        }

        private void createPerformActionState()
        {
            _performActionState = (fsm, gameObj) =>
            {
                // perform the action

                if (!hasActionPlan())
                {
                    // no actions to perform
                    // Console.WriteLine("<color=red>Done actions</color>");
                    fsm.PopState();
                    fsm.PushState(_idleState);
                    _dataProvider.ActionsFinished();
                    return;
                }

                GoapAction action = _currentActions.Peek();
                if (action.IsDone(this))
                {
                    // the action is done. Remove it so we can perform the next one
                    _currentActions.Dequeue();
                }

                if (hasActionPlan())
                {
                    // perform the next action
                    action = _currentActions.Peek();
                    bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

                    if (inRange)
                    {
                        // we are in range, so perform the action
                        bool success = action.Perform(gameObj);

                        if (!success)
                        {
                            // action failed, we need to plan again
                            fsm.PopState();
                            fsm.PushState(_idleState);
                            _dataProvider.PlanAborted(action);
                        }
                    }
                    else
                    {
                        // we need to move there first
                        // push moveTo state
                        fsm.PushState(_moveToState);
                    }

                }
                else
                {
                    // no actions left, move to Plan state
                    fsm.PopState();
                    fsm.PushState(_idleState);
                    _dataProvider.ActionsFinished();
                }

            };
        }

        private void loadActions()
        {
            List<GoapAction> actions = _dataProvider.GetActions();
            _availableActions.Clear();
            foreach (GoapAction a in actions)
            {
                _availableActions.Add(a);
            }
            //// Console.WriteLine("Found actions: " + prettyPrint(actions));
        }

        public static string PrettyPrint(Dictionary<string, object> state)
        {
            String s = "";
            foreach (KeyValuePair<string, object> kvp in state)
            {
                s += kvp.Key + ":" + kvp.Value.ToString();
                s += ", ";
            }
            return s;
        }       
    }
}