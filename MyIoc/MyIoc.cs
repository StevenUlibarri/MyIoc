using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MyIoc
{
    public class MyIoc
    {
        private ConcurrentDictionary<Type, RegistryEntry> Registry;
        private ConcurrentDictionary<Type, object> SingletonMap;
        private Dictionary<LifeCycleTypes, Func<Type, object>> LifeCycleMapping;

        public MyIoc()
        {
            Registry = new ConcurrentDictionary<Type, RegistryEntry>();
            SingletonMap = new ConcurrentDictionary<Type, object>();
            LifeCycleMapping = new Dictionary<LifeCycleTypes, Func<Type, object>>();
            LifeCycleMapping[LifeCycleTypes.TRANSIENT] = ResolveTransient;
            LifeCycleMapping[LifeCycleTypes.SINGLETON] = ResolveSingleton;
        }

        public void Register<TInterface, TClass>(LifeCycleTypes lifeCycleType = LifeCycleTypes.TRANSIENT) where TClass : TInterface
        {
            RegistryEntry entry;
            Registry.TryGetValue(typeof(TInterface), out entry);
            if (entry != null)
                throw new Exception("Interface: " + typeof(TInterface) + " is already registered.");
            else
            {
                entry = new RegistryEntry(typeof(TClass), lifeCycleType);
                Registry[typeof(TInterface)] = entry;
            }
        }

        public object Resolve<TInterface>()
        {
            return Resolve(typeof(TInterface));
        }

        private object Resolve(Type type)
        {
            RegistryEntry entry;
            Registry.TryGetValue(type, out entry);
            if (entry == null)
                throw new Exception("Cannot resolve unregistered interface: " + type);
            else
            {
                return LifeCycleMapping[entry.LifeCycleType](entry.Class);
            }
        }

        private object ResolveTransient(Type type)
        {
            var constructor = type.GetConstructors().Single();
            var args = constructor.GetParameters()
                                  .Select(p => Resolve(p.ParameterType)) // potential infinte recursion
                                  .ToArray();
            return constructor.Invoke(args);
        }

        private object ResolveSingleton(Type type)
        {
            object o;
            SingletonMap.TryGetValue(type, out o);
            if (o != null)
                return o;
            else
            {
                o = ResolveTransient(type);
                SingletonMap[type] = o;
                return o;
            }
        }
    }
}
