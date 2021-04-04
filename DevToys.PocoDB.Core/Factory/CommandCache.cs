using System;
using System.Collections.Generic;

namespace DevToys.PocoDB.Core.Factory
{
    internal class CommandCache
    {
        private static CommandCache _Instance;
        private Dictionary<Type, Object> _Cache = new Dictionary<Type, object>();

        private CommandCache()
        {
        }

        public static CommandCache Instance => _Instance ??= new CommandCache();

        public Object Get(Type type)
        {
            if (!_Cache.ContainsKey(type))
                return null;

            return _Cache[type];
        }

        public void Register(Type type, Object instance)
        {
            if (_Cache.ContainsKey(type))
                return;
            _Cache.Add(type, instance);
        }
    }
}