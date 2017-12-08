using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Ai.Goap
{
    public class GoapState
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();
        private readonly Dictionary<string, object> bufferA = new Dictionary<string, object>(DefaultSize);
        private readonly Dictionary<string, object> bufferB = new Dictionary<string, object>(DefaultSize);
        public static int DefaultSize = 20;

        public GoapState()
        {
        }

        public GoapState(GoapState state)
        {
            foreach (KeyValuePair<string, object> kvp in state.GetValues())
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
                    result.values[pair.Key] = pair.Value;
                return result;
            }
        }

        public int Count
        {
            get { return values.Count; }
        }

        public bool HasAny(GoapState other)
        {
            lock (values) lock (other.values)
            {
                foreach (var pair in other.values)
                {
                    object thisValue;
                    values.TryGetValue(pair.Key, out thisValue);
                    if (Equals(thisValue, pair.Value))
                        return true;
                }
                return false;
            }
        }

        public object Get(string key)
        {
            lock (values)
            {
                if (!values.ContainsKey(key))
                    return default(object);
                return values[key];
            }
        }

        public Dictionary<string, object> GetValues()
        {
            lock (values)
                return values;
        }

        public void Set(string key, object value)
        {
            lock (values)
            {
                values[key] = value;
            }
        }

        public GoapState Clone()
        {
            GoapState newState = new GoapState(this);
            return newState;
        }

        public bool HasKey(string key)
        {
            lock (values)
                return values.ContainsKey(key);
        }

        public void Remove(string key)
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
                        values[pair.Key] = pair.Value;
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
                            difference.values[pair.Key] = pair.Value;
                        if (count >= stopAt)
                            break;
                    }
                }
                return count;
            }
        }

        public bool HasAnyConflict(GoapState other) // used only in backward for now
        {
            lock (values) lock (other.values)
                {
                    foreach (var pair in other.values)
                    {
                        object thisValue;
                        values.TryGetValue(pair.Key, out thisValue);
                        var otherValue = pair.Value;
                        if (otherValue == null || Equals(otherValue, false))
                            continue;
                        if (thisValue != null && !Equals(otherValue, thisValue))
                            return true;
                    }
                    return false;
                }
        }
    }
}
