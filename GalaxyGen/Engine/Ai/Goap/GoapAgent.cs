using System.Collections;
using System.Collections.Generic;
using System;
using GalaxyGen.Engine.Ai.Fsm;
using GalaxyGen.Engine.Controllers;

namespace GalaxyGen.Engine.Ai.Goap
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
            _stateMachine.pushState(_idleState);
            loadActions();
        }


        public void Tick()
        {
            _stateMachine.Update(this);
        }


        public void AddAction(GoapAction a)
        {
            _availableActions.Add(a);
        }

        public GoapAction GetAction(Type action)
        {
            foreach (GoapAction g in _availableActions)
            {
                if (g.GetType().Equals(action))
                    return g;
            }
            return null;
        }

        public void RemoveAction(GoapAction action)
        {
            _availableActions.Remove(action);
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

                // get the world state and the goal we want to plan for
                HashSet<KeyValuePair<string, object>> worldState = _dataProvider.getWorldState();
                HashSet<KeyValuePair<string, object>> goal = _dataProvider.createGoalState();

                // Plan
                Queue<GoapAction> plan = _planner.plan(this, _availableActions, worldState, goal);
                if (plan != null)
                {
                    // we have a plan, hooray!
                    _currentActions = plan;
                    _dataProvider.planFound(goal, plan);

                    fsm.popState(); // move to PerformAction state
                    fsm.pushState(_performActionState);

                }
                else
                {
                    // ugh, we couldn't get a plan
                    // Console.WriteLine("<color=orange>Failed Plan:</color>" + PrettyPrint(goal));
                    _dataProvider.planFailed(goal);
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

                GoapAction action = _currentActions.Peek();
                if (action.requiresInRange() && action.target == null)
                {
                    // Console.WriteLine("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                    fsm.popState(); // move
                    fsm.popState(); // perform
                    fsm.pushState(_idleState);
                    return;
                }

                // get the agent to move itself
                if (_dataProvider.moveAgent(action))
                {
                    fsm.popState();
                }

                /*MovableComponent movable = (MovableComponent) gameObj.GetComponent(typeof(MovableComponent));
                if (movable == null) {
                    // Console.WriteLine("<color=red>Fatal error:</color> Trying to move an Agent that doesn't have a MovableComponent. Please give it one.");
                    fsm.popState(); // move
                    fsm.popState(); // perform
                    fsm.pushState(idleState);
                    return;
                }

                float step = movable.moveSpeed * Time.deltaTime;
                gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, action.target.transform.position, step);

                if (gameObj.transform.position.Equals(action.target.transform.position) ) {
                    // we are at the target location, we are done
                    action.setInRange(true);
                    fsm.popState();
                }*/
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
                    _dataProvider.actionsFinished();
                    return;
                }

                GoapAction action = _currentActions.Peek();
                if (action.isDone())
                {
                    // the action is done. Remove it so we can perform the next one
                    _currentActions.Dequeue();
                }

                if (hasActionPlan())
                {
                    // perform the next action
                    action = _currentActions.Peek();
                    bool inRange = action.requiresInRange() ? action.isInRange() : true;

                    if (inRange)
                    {
                        // we are in range, so perform the action
                        bool success = action.perform(gameObj);

                        if (!success)
                        {
                            // action failed, we need to plan again
                            fsm.popState();
                            fsm.pushState(_idleState);
                            _dataProvider.planAborted(action);
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
                    _dataProvider.actionsFinished();
                }

            };
        }

        private void loadActions()
        {
            GoapAction[] actions = _dataProvider.GetActions();
            foreach (GoapAction a in actions)
            {
                _availableActions.Add(a);
            }
            //// Console.WriteLine("Found actions: " + prettyPrint(actions));
        }

        public static string PrettyPrint(HashSet<KeyValuePair<string, object>> state)
        {
            String s = "";
            foreach (KeyValuePair<string, object> kvp in state)
            {
                s += kvp.Key + ":" + kvp.Value.ToString();
                s += ", ";
            }
            return s;
        }

        public static string PrettyPrint(Queue<GoapAction> actions)
        {
            String s = "";
            foreach (GoapAction a in actions)
            {
                s += a.GetType().Name;
                s += "-> ";
            }
            s += "GOAL";
            return s;
        }

        public static string prettyPrint(GoapAction[] actions)
        {
            String s = "";
            foreach (GoapAction a in actions)
            {
                s += a.GetType().Name;
                s += ", ";
            }
            return s;
        }

        public static string prettyPrint(GoapAction action)
        {
            String s = "" + action.GetType().Name;
            return s;
        }
    }
}