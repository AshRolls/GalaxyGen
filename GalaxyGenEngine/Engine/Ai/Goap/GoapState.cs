using Castle.Core;
using GalaxyGenCore.Resources;
using GalaxyGenEngine.Engine.Ai.Goap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Akka.Actor.FSMBase;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    public class GoapState
    {
        private Dictionary<GoapStateKey, object> _states = new Dictionary<GoapStateKey, object>();        
        private readonly Dictionary<GoapStateKey, object> bufferA = new Dictionary<GoapStateKey, object>(DefaultSize);
        private readonly Dictionary<GoapStateKey, object> bufferB = new Dictionary<GoapStateKey, object>(DefaultSize);
        public static int DefaultSize = 20;

        public GoapState()
        {
        }

        public override int GetHashCode()
        {
            int hash = 397;
            foreach (KeyValuePair<GoapStateKey, object> kvp in _states)
            {
                hash *= 37 + kvp.Key.GetHashCode() + kvp.Value.GetHashCode();
            }
            return hash;
            //return values.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            GoapState otherState = (GoapState)obj;
            return _states.Count == otherState._states.Count && !_states.Except(otherState._states).Any(); // https://stackoverflow.com/questions/3804367/testing-for-equality-between-dictionaries-in-c-sharp

            //GoapState otherState = (GoapState)obj;
            //lock (values) lock (otherState.values)
            //{
            //    if (values.Count != otherState.values.Count) return false;
            //    foreach (var pair in otherState.values)
            //    {
            //        object thisValue;
            //        values.TryGetValue(pair.Key, out thisValue);
            //        if (!Equals(thisValue, pair.Value))
            //            return false;
            //    }
            //    foreach (var pair in values)
            //    {
            //        object thisValue;
            //        otherState.values.TryGetValue(pair.Key, out thisValue);
            //        if (!Equals(thisValue, pair.Value))
            //            return false;
            //    }
            //    return true;
            //}
            //return values.Equals(((GoapState)obj).values);
        }

        public GoapState(GoapState state)
        {
            foreach (KeyValuePair<GoapStateKey, object> kvp in state.GetValues())
            {
                _states.Add(kvp.Key, kvp.Value);
            }
        }

        public static GoapState operator +(GoapState a, GoapState b)
        {
            GoapState result;
            lock (a._states)
            {
                result = new GoapState(a);
            }
            lock (b._states)
            {
                foreach (var pair in b._states)
                {
                    if (pair.Key.Type == GoapStateKeyTypeEnum.StateName) result._states[pair.Key] = pair.Value;
                    if (pair.Key.Type == GoapStateKeyTypeEnum.Resource && !result._states.ContainsKey(pair.Key)) result._states[pair.Key] = pair.Value;
                    else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource) result._states[pair.Key] = (long)pair.Value + (long)result._states[pair.Key];
                }
                return result;
            }
        }

        public int Count
        {
            get { return _states.Count; }
        }

        //public bool HasAny(GoapState other)
        //{
        //    lock (values) lock (other.values)
        //    {
        //        foreach (var pair in other.values)
        //        {
        //            object thisValue;
        //            values.TryGetValue(pair.Key, out thisValue);
        //            if (Equals(thisValue, pair.Value))
        //                return true;
        //        }
        //        return false;
        //    }
        //}

        public object Get(GoapStateKey key)
        {
            lock (_states)
            {
                if (!_states.ContainsKey(key))
                    return default(object);
                return _states[key];
            }
        }

        public Dictionary<GoapStateKey, object> GetValues()
        {
            lock (_states)
                return _states;
        }

        public void Set(GoapStateKey key, object value)
        {
            lock (_states)
            {
                if (_states.ContainsKey(key))
                {
                    if (key.Type == GoapStateKeyTypeEnum.StateName) _states[key] = value;
                    else if (key.Type == GoapStateKeyTypeEnum.Resource) _states[key] = (long)_states[key] + (long)value;
                }
                else _states.Add(key, value);
            }
        }

        public GoapState Clone()
        {
            GoapState newState = new GoapState(this);
            return newState;
        }

        public bool HasKey(GoapStateKey key)
        {
            lock (_states)
                return _states.ContainsKey(key);
        }

        public void Remove(GoapStateKey key)
        {
            lock (_states)
            {
                _states.Remove(key);
            }
        }
        public void AddFromState(GoapState b)
        {
            lock (_states) lock (b._states)
            {
                foreach (var pair in b._states)
                    _states[pair.Key] = pair.Value;
            }
        }

        // keep only missing differences in values
        public int ReplaceWithMissingDifference(GoapState other, int stopAt = int.MaxValue)
        {
            lock (_states)
            {
                var count = 0;
                var buffer = _states;
                _states = _states == bufferA ? bufferB : bufferA;
                _states.Clear();
                foreach (var pair in buffer)
                {
                    object otherValue;
                    other._states.TryGetValue(pair.Key, out otherValue);
                    if (!Equals(pair.Value, otherValue))
                    {
                        count++;
                        if (pair.Key.Type == GoapStateKeyTypeEnum.StateName) _states[pair.Key] = pair.Value;
                        else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource && otherValue != null) _states[pair.Key] = (long)pair.Value + (long)otherValue;
                        else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource) _states[pair.Key] = pair.Value;
                        if (count >= stopAt)
                            break;
                    }
                }
                return count;
            }
        }

        // write differences in "difference"
        public int MissingDifference(GoapState other, ref GoapState difference, int stopAt = int.MaxValue)
        {
            lock (_states)
            {
                var count = 0;
                foreach (var pair in _states)
                {
                    object otherValue;
                    other._states.TryGetValue(pair.Key, out otherValue);
                    if (!Equals(pair.Value, otherValue))
                    {
                        count++;
                        if (difference != null)
                        {
                            if (pair.Key.Type == GoapStateKeyTypeEnum.StateName) difference._states[pair.Key] = pair.Value;
                            else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource && otherValue != null) difference._states[pair.Key] = (long)pair.Value + (long)otherValue;
                            else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource) difference._states[pair.Key] = pair.Value;
                        }
                        if (count >= stopAt)
                            break;
                    }
                }
                return count;
            }
        }

        internal static string PrettyPrint(GoapState state)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<GoapStateKey,object> kvp in state._states)
            {
                if (kvp.Key.Type == GoapStateKeyTypeEnum.StateName)
                {
                    sb.Append("STA:(");
                    sb.Append(kvp.Key.StateName);
                    sb.Append(",");
                    sb.Append(kvp.Value);
                    sb.Append(") ");
                }
                else
                {
                    sb.Append("RES:(");
                    sb.Append(kvp.Key.ResourceLocation.ResType);
                    sb.Append("|");
                    sb.Append(kvp.Key.ResourceLocation.StoreId);
                    sb.Append(",");
                    sb.Append(kvp.Value);
                    sb.Append(") ");
                }                
            }
            return sb.ToString();
        }

        //public bool HasAnyConflict(GoapState other) // used only in backward for now
        //{
        //    lock (values) lock (other.values)
        //    {
        //        foreach (var pair in other.values)
        //        {
        //            object thisValue;
        //            values.TryGetValue(pair.Key, out thisValue);
        //            var otherValue = pair.Value;
        //            if (otherValue == null || Equals(otherValue, false))
        //                continue;
        //            if (thisValue != null && !Equals(otherValue, thisValue))
        //                return true;
        //        }
        //        return false;
        //    }
        //}
    }
}
