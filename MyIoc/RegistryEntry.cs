using System;

namespace MyIoc.Core
{
    internal class RegistryEntry
    {
        public RegistryEntry(Type classType, LifeCycleTypes lifeCycleType)
        {
            Class = classType;
            LifeCycleType = lifeCycleType;
        }

        public Type Class { get; private set; }
        public LifeCycleTypes LifeCycleType { get; private set; }
    }
}
