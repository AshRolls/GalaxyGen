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
        private Dictionary<GoapStateKey, object> values = new Dictionary<GoapStateKey, object>();        
        private readonly Dictionary<GoapStateKey, object> bufferA = new Dictionary<GoapStateKey, object>(DefaultSize);
        private readonly Dictionary<GoapStateKey, object> bufferB = new Dictionary<GoapStateKey, object>(DefaultSize);
        public static int DefaultSize = 20;

        public GoapState()
        {
        }

        public override int GetHashCode()
        {
            int hash = 397;
            foreach (KeyValuePair<GoapStateKey, object> kvp in values)
            {
                hash *= 37 + kvp.Key.GetHashCode() + kvp.Value.GetHashCode();
            }
            return hash;
        }
        public override bool Equals(object obj)
        {
            GoapState otherState = (GoapState)obj;
            lock (values) lock (otherState.values)
            {
                foreach (var pair in otherState.values)
                {
                    object thisValue;
                    values.TryGetValue(pair.Key, out thisValue);
                    if (!Equals(thisValue, pair.Value))
                        return false;
                }
                foreach (var pair in values)
                {
                    object thisValue;
                    otherState.values.TryGetValue(pair.Key, out thisValue);
                    if (!Equals(thisValue, pair.Value))
                        return false;
                }
                return true;
            }
        }

        public GoapState(GoapState state)
        {
            foreach (KeyValuePair<GoapStateKey, object> kvp in state.GetValues())
            {
                values.Add(kvp.Key, kvp.Value);
            }
        }

        public static GoapState operator +(GoapState a, GoapState b)
        {
            GoapState result;
            lock (a.values)
            {
                result = new GoapState(a);
            }
            lock (b.values)
            {
                foreach (var pair in b.values)
                {
                    if (pair.Key.Type == GoapStateKeyTypeEnum.StateName) result.values[pair.Key] = pair.Value;
                    if (pair.Key.Type == GoapStateKeyTypeEnum.Resource && !result.values.ContainsKey(pair.Key)) result.values[pair.Key] = pair.Value;
                    else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource) result.values[pair.Key] = (long)pair.Value + (long)result.values[pair.Key];
                }
                return result;
            }
        }

        public int Count
        {
            get { return values.Count; }
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
            lock (values)
            {
                if (!values.ContainsKey(key))
                    return default(object);
                return values[key];
            }
        }

        public Dictionary<GoapStateKey, object> GetValues()
        {
            lock (values)
                return values;
        }

        public void Set(GoapStateKey key, object value)
        {
            lock (values)
            {
                if (values.ContainsKey(key))
                {
                    if (key.Type == GoapStateKeyTypeEnum.StateName) values[key] = value;
                    else if (key.Type == GoapStateKeyTypeEnum.Resource) values[key] = (long)values[key] + (long)value;
                }
                else values.Add(key, value);
            }
        }

        public GoapState Clone()
        {
            GoapState newState = new GoapState(this);
            return newState;
        }

        public bool HasKey(GoapStateKey key)
        {
            lock (values)
                return values.ContainsKey(key);
        }

        public void Remove(GoapStateKey key)
        {
            lock (values)
            {
                values.Remove(key);
            }
        }
        public void AddFromState(GoapState b)
        {
            lock (values) lock (b.values)
                {
                    foreach (var pair in b.values)
                        values[pair.Key] = pair.Value;
                }
        }

        // keep only missing differences in values
        public int ReplaceWithMissingDifference(GoapState other, int stopAt = int.MaxValue)
        {
            lock (values)
            {
                var count = 0;
                var buffer = values;
                values = values == bufferA ? bufferB : bufferA;
                values.Clear();
                foreach (var pair in buffer)
                {
                    object otherValue;
                    other.values.TryGetValue(pair.Key, out otherValue);
                    if (!Equals(pair.Value, otherValue))
                    {
                        count++;
                        if (pair.Key.Type == GoapStateKeyTypeEnum.StateName) values[pair.Key] = pair.Value;
                        else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource && otherValue != null) values[pair.Key] = (long)pair.Value + (long)otherValue;
                        else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource) values[pair.Key] = pair.Value;
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
            lock (values)
            {
                var count = 0;
                foreach (var pair in values)
                {
                    object otherValue;
                    other.values.TryGetValue(pair.Key, out otherValue);
                    if (!Equals(pair.Value, otherValue))
                    {
                        count++;
                        if (difference != null)
                        {
                            if (pair.Key.Type == GoapStateKeyTypeEnum.StateName) difference.values[pair.Key] = pair.Value;
                            else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource && otherValue != null) difference.values[pair.Key] = (long)pair.Value + (long)otherValue;
                            else if (pair.Key.Type == GoapStateKeyTypeEnum.Resource) difference.values[pair.Key] = pair.Value;
                        }
                        if (count >= stopAt)
                            break;
                    }
                }
                return count;
            }
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
