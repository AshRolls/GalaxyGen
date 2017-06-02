using System.Collections;
using System.Collections.Generic;
using System;
using GalaxyGen.Engine.Ai.Fsm;
using GalaxyGen.Engine.Controllers;
using GalaxyGen.Engine.Goap.Planner;
using GalaxyGen.Engine.Goap.Core;

namespace GalaxyGen.Engine.Ai.Goap
{
    public sealed class GoapAgent<T, W> : IReGoapAgent<T,W>
    {

        private FSM _stateMachine;

        private FSM.FSMState _idleState; // finds something to do
        private FSM.FSMState _moveToState; // moves to a target
        private FSM.FSMState _performActionState; // performs an action

        private IReGoapGoal<T,W> _currentPlan;

        private IGoap<T,W> _dataProvider; // this is the implementing class that provides our world data and listens to feedback on planning
        private IAgentActions _actionProvider; // this is the class that will perform actions from the goap
        private IAgentControllerState _stateProvider;

        private IGoapPlanner<T, W> _planner;
        private List<IReGoapGoal<T, W>> _goals;
        private List<IReGoapAction<T, W>> _actions;
        private IReGoapMemory<T, W> _memory;

        public GoapAgent(IGoap<T,W> provider, IAgentActions actions, IAgentControllerState state)
        {
            _dataProvider = provider;
            _actionProvider = actions;
            _stateProvider = state;
            _stateMachine = new FSM();
            _planner = new ReGoapPlanner<T, W>();        
            createIdleState();
            createMoveToState();
            createPerformActionState();
            _stateMachine.pushState(_idleState);
        }


        public void Tick()
        {
            _stateMachine.Update(this);
        }        

        private bool hasActionPlan()
        {
            return _currentPlan.GetPlan().Count > 0;
        }

        private void createIdleState()
        {
            _idleState = (fsm, gameObj) =>
            {
                // GOAP planning
                _goals = _dataProvider.GetGoals();
                _actions = _dataProvider.GetActions();
                _memory = _dataProvider.GetMemory();

                // Plan
                IReGoapGoal<T,W> plan = _planner.Plan(this, null, null);
                if (plan != null)
                {
                    // we have a plan, hooray!
                    _currentPlan = plan;
                    _dataProvider.PlanFound(plan);

                    fsm.popState(); // move to PerformAction state
                    fsm.pushState(_performActionState);

                }
                else
                {
                    // ugh, we couldn't get a plan
                    // Console.WriteLine("<color=orange>Failed Plan:</color>" + PrettyPrint(goal));
                    _dataProvider.PlanFailed(plan);
                    fsm.popState(); // move back to IdleAction state
                    fsm.pushState(_idleState);
                }

            };
        }

        private void createMoveToState()
        {
            _moveToState = (fsm, gameObj) =>
            {
                // move the game object               

                ReGoapActionState<T,W> action = _currentPlan.GetPlan().Peek();
                if (action.Action.RequiresInRange() && action.Action.TargetScId == 0)
                {
                    // Console.WriteLine("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                    fsm.popState(); // move
                    fsm.popState(); // perform
                    fsm.pushState(_idleState);
                    return;
                }

                // get the agent to move itself
                if (_dataProvider.MoveAgent(action))
                {
                    fsm.popState();
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
                    fsm.popState();
                    fsm.pushState(_idleState);
                    _dataProvider.ActionsFinished();
                    return;
                }


                ReGoapActionState<T, W> action = _currentPlan.GetPlan().Peek();
                if (action.Action.IsDone())
                {
                    // the action is done. Remove it so we can perform the next one
                    _currentPlan.GetPlan().Dequeue();
                }

                if (hasActionPlan())
                {
                    // perform the next action
                    action = _currentPlan.GetPlan().Peek();
                    bool inRange = action.Action.RequiresInRange() ? action.Action.IsInRange() : true;

                    if (inRange)
                    {
                        // we are in range, so perform the action
                        bool success = action.Action.Perform(action.Settings,_currentPlan.GetGoalState());

                        if (!success)
                        {
                            // action failed, we need to plan again
                            fsm.popState();
                            fsm.pushState(_idleState);
                            _dataProvider.PlanAborted(action);
                        }
                    }
                    else
                    {
                        // we need to move there first
                        // push moveTo state
                        fsm.pushState(_moveToState);
                    }

                }
                else
                {
                    // no actions left, move to Plan state
                    fsm.popState();
                    fsm.pushState(_idleState);
                    _dataProvider.ActionsFinished();
                }

            };
        }

        public List<IReGoapGoal<T, W>> GetGoalsSet()
        {
            return _goals;
        }

        public List<IReGoapAction<T, W>> GetActionsSet()
        {
            return _actions;
        }

        public IReGoapMemory<T, W> GetMemory()
        {
            return _memory;
        }

        public IAgentActions ActionProvider
        {
            get
            {
                return _actionProvider;
            }
        }

        public IAgentControllerState StateProvider
        {
            get
            {
                return _stateProvider;
            }
        }
    }
}