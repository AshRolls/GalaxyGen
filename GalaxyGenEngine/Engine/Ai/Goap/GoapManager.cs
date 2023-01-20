using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    //TODO Use NodeManager for object pooling
    //public class NodeManager
    //{
    //    static Stack<GoapNode> _usedNodes = new Stack<GoapNode>();
    //    static Stack<GoapNode> _freeNodes = new Stack<GoapNode>();
    //    public static GoapNode GetFreeNode(GoapNode parent, float runningCost, float weight, Dictionary<string, object> state, GoapAction action)
    //    {
    //        GoapNode free = null;
    //        if (_freeNodes.Count <= 0)
    //            free = new GoapNode(parent, runningCost, weight, state, action);
    //        else
    //        {
    //            free = _freeNodes.Pop();
    //            free.ReInit(parent, runningCost, weight, state, action);
    //        }

    //        _usedNodes.Push(free);
    //        return free;
    //    }

    //    public static void ReleaseNode()
    //    {
    //        while (_usedNodes.Count > 0)
    //        {
    //            _freeNodes.Push(_usedNodes.Pop());
    //        }
    //    }
    //    static Stack<Dictionary<string, object>> _usedState = new Stack<Dictionary<string, object>>();
    //    static Stack<Dictionary<string, object>> _freeState = new Stack<Dictionary<string, object>>();

    //    public static Dictionary<string, object> GetFreeState()
    //    {
    //        Dictionary<string, object> free = null;
    //        if (_freeState.Count > 0)
    //            free = _freeState.Pop();
    //        else
    //            free = new Dictionary<string, object>();

    //        _usedState.Push(free);
    //        return free;
    //    }

    //    private static void ReleaseState()
    //    {
    //        while (_usedState.Count > 0)
    //        {
    //            _freeState.Push(_usedState.Pop());
    //        }
    //    }

    //    static Stack<HashSet<GoapAction>> _usedSubset = new Stack<HashSet<GoapAction>>();
    //    static Stack<HashSet<GoapAction>> _freeSubset = new Stack<HashSet<GoapAction>>();

    //    public static HashSet<GoapAction> GetFreeActionSet()
    //    {
    //        HashSet<GoapAction> free = null;
    //        if (_freeSubset.Count > 0)
    //        {
    //            free = _freeSubset.Pop();
    //            free.Clear();
    //        }
    //        else
    //            free = new HashSet<GoapAction>();

    //        _usedSubset.Push(free);
    //        return free;
    //    }

    //    private static void ReleaseSubset()
    //    {
    //        while (_usedSubset.Count > 0)
    //        {
    //            _freeSubset.Push(_usedSubset.Pop());
    //        }
    //    }

    //    public static void Release()
    //    {
    //        ReleaseNode();
    //        ReleaseState();
    //        ReleaseSubset();
    //    }
    //}
}
