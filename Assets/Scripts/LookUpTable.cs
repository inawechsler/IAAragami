using UnityEngine;
using System;
using System.Collections.Generic;

public class LookUpTable<Key, Value>
{
    Dictionary<Key, Value> _cache;
    Func<Key, Value> _func;
    
    public LookUpTable(Func<Key, Value> func)
    {
        _func = func;
        _cache = new Dictionary<Key, Value>();
    }
    public Value Get(Key key)
    {
        if (!_cache.ContainsKey(key))
        {
            _cache[key] = _func(key);
        }
        return _cache[key];
    }
}
