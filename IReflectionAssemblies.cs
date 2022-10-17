using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Julesabr.Wrappers {
    /// <summary>
    /// Accesses the .NET Reflection API on all referenced assemblies in solution.
    /// </summary>
    [PublicAPI]
    public interface IReflectionAssemblies {
        /// <returns>Instance of <see cref="IReflectionTypes" />
        /// containing all types in all referenced assemblies in solution</returns>
        IReflectionTypes Types { get; }
        
        /// <returns>List of all referenced assemblies in solution</returns>
        IEnumerable<Assembly> Get();

        /// <summary>
        /// Accesses the .NET Reflection API on all types in all referenced assemblies in solution.
        /// </summary>
        [PublicAPI]
        public interface IReflectionTypes {
            /// <returns>List of all types in all referenced assemblies in solution</returns>
            IEnumerable<Type> All();
            
            /// <summary>Used when you need the type instance and you know the exact name of the type.</summary>
            /// <param name="name">The name of a type in the referenced assemblies</param>
            /// <returns>The type with the given name</returns>
            Type WithName(string name);
            
            /// <summary>Used when you need all types whose instances are assignable to a given type.</summary>
            /// <typeparam name="T">The reference type to target</typeparam>
            /// <param name="abstracts">true to include abstract types in output; otherwise, false</param>
            /// <param name="interfaces">true to include interface types in output; otherwise, false</param>
            /// <returns>List of all non-abstract classes that are assignable to T</returns>
            IEnumerable<Type> AssignableTo<T>(bool abstracts = false, bool interfaces = false) where T : class;
            
            /// <summary>
            /// Used when you need all types whose instances are assignable to a given type. Does not work with
            /// generics whose type params are not known at compile time. For generic interfaces whose type params may
            /// not be known, see ImplementsGeneric.
            /// </summary>
            /// <param name="type">The reference type to target</param>
            /// <param name="abstracts">true to include abstract types in output; otherwise, false</param>
            /// <param name="interfaces">true to include interface types in output; otherwise, false</param>
            /// <returns>List of all types that are assignable to the given type</returns>
            IEnumerable<Type> AssignableTo(Type type, bool abstracts = false, bool interfaces = false);
            
            /// <summary>
            /// Used when you need all types that implement or inherit a generic interface. Also works when the type
            /// params are not known at compile time.
            /// </summary>
            /// <param name="type">The generic interface to target</param>
            /// <param name="abstracts">true to include abstract types in output; otherwise, false</param>
            /// <param name="interfaces">true to include interface types in output; otherwise, false</param>
            /// <returns>List of all types that implement or inherit the generic interface</returns>
            IEnumerable<Type> ImplementsGeneric(Type type, bool abstracts = false, bool interfaces = false);
            
            /// <summary>Used when you need all types that have a specific attribute.</summary>
            /// <typeparam name="T">The attribute type to target</typeparam>
            /// <param name="inherit">true to include type's that inherit the attribute; otherwise, false</param>
            /// <param name="abstracts">true to include abstract types in output; otherwise, false</param>
            /// <param name="interfaces">true to include interface types in output; otherwise, false</param>
            /// <returns>List of all types that have the attribute T</returns>
            IEnumerable<Type> HaveAttribute<T>(bool inherit = true, bool abstracts = false, bool interfaces = false) where T : Attribute;
            
            /// <summary>Used when you need all types that have a specific attribute.</summary>
            /// <param name="type">The attribute type to target</param>
            /// <param name="inherit">true to include type's that inherit the attribute; otherwise, false</param>
            /// <param name="abstracts">true to include abstract types in output; otherwise, false</param>
            /// <param name="interfaces">true to include interface types in output; otherwise, false</param>
            /// <returns>List of all types that have the given attribute type</returns>
            IEnumerable<Type> HaveAttribute(Type type, bool inherit = true, bool abstracts = false, bool interfaces = false);
            
            /// <summary>Used when you need instances of all types that are assignable to a given type.</summary>
            /// <typeparam name="T">The reference type to target</typeparam>
            /// <returns>List of new instances of all types that are assignable to T</returns>
            IEnumerable<T> InstanceOf<T>() where T : class;
            
            /// <summary>Used when you need instances of all types that are assignable to a given type.</summary>
            /// <param name="type">The reference type to target</param>
            /// <returns>List of new instances of all types that are assignable to the given type</returns>
            IEnumerable<object> InstanceOf(Type type);
        }
    }
}