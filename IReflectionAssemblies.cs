using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace NaughtyBiker.Wrappers {
    [PublicAPI]
    public interface IReflectionAssemblies {
        IReflectionTypes Types { get; }

        IEnumerable<Assembly> Get();

        [PublicAPI]
        public interface IReflectionTypes {
            IEnumerable<Type> All();
            Type WithName(string name);
            IEnumerable<Type> From<T>() where T : class;
            IEnumerable<Type> From(Type type);
            IEnumerable<Type> ImplementsGeneric(Type type, Predicate<Type> predicate = null);
            IEnumerable<Type> HaveAttribute<T>() where T : Attribute;
            IEnumerable<T> InstanceOf<T>() where T : class;
        }
    }
}