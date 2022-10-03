using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace NaughtyBiker.Wrappers {
    /// <summary>Implementation of <see cref="NaughtyBiker.Wrappers.IReflectionAssemblies" />.</summary>
    /// <typeparam name="TEntryPoint">Type used to get the startup assembly for solution. The startup assembly
    /// will reference either directly or indirectly all other assemblies in solution and typically is the assembly
    /// with the Main method. The type can be any type as long as it is defined in the startup assembly.</typeparam>
    [PublicAPI]
    public class ReflectionAssemblies<TEntryPoint> : IReflectionAssemblies {
        private readonly IEnumerable<Assembly> assemblies;

        /// <summary>
        /// Initializes a new instance of <see cref="T:NaughtyBiker.Wrappers.ReflectionAssemblies`1" /> that uses
        /// reflection to collect all assemblies in solution and initializes
        /// <see cref="T:NaughtyBiker.Wrappers.ReflectionAssemblies`1.ReflectionTypes" />.
        /// </summary>
        public ReflectionAssemblies() {
            assemblies = GetAllAssemblies();
            Types = new ReflectionTypes(assemblies);
        }

        public IReflectionAssemblies.IReflectionTypes Types { get; }

        public IEnumerable<Assembly> Get() {
            return assemblies;
        }

        private static IEnumerable<Assembly> GetAllAssemblies() {
            ISet<string> assemblySet = new HashSet<string>();
            Queue<Assembly> next = new Queue<Assembly>();

            next.Enqueue(Assembly.GetAssembly(typeof(TEntryPoint)));

            do {
                Assembly asm = next.Dequeue();

                yield return asm;

                foreach (AssemblyName reference in asm.GetReferencedAssemblies())
                    if (assemblySet.Add(reference.FullName))
                        next.Enqueue(Assembly.Load(reference));
            } while (next.Count > 0);
        }

        /// <summary>
        /// Implementation of <see cref="NaughtyBiker.Wrappers.IReflectionAssemblies.IReflectionTypes" />.
        /// </summary>
        [PublicAPI]
        public class ReflectionTypes : IReflectionAssemblies.IReflectionTypes {
            private readonly IEnumerable<Type> types;

            /// <summary>
            /// Initializes a new instance of
            /// <see cref="T:NaughtyBiker.Wrappers.ReflectionAssemblies`1.ReflectionTypes" /> that uses reflection to
            /// collect all types in all given assemblies.
            /// </summary>
            /// <param name="assemblies">List of assemblies to collect types from</param>
            public ReflectionTypes(IEnumerable<Assembly> assemblies) {
                types = assemblies.SelectMany(assembly => assembly.GetTypes());
            }

            public IEnumerable<Type> All() {
                return types;
            }

            public Type WithName(string name) {
                return types.FirstOrDefault(type => type.Name == name);
            }

            public IEnumerable<Type> AssignableTo<T>(bool abstracts = false, bool interfaces = false) where T : class {
                return AssignableTo(typeof(T), abstracts, interfaces);
            }

            public IEnumerable<Type> AssignableTo(Type type, bool abstracts = false, bool interfaces = false) {
                return types.Where(t => 
                    type.IsAssignableFrom(t)
                    && (abstracts || !t.IsAbstract)
                    && (interfaces || !t.IsInterface)
                );
            }

            public IEnumerable<Type> ImplementsGeneric(Type type, bool abstracts = false, bool interfaces = false) {
                return types.Where(t =>
                    t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)
                    && (abstracts || !t.IsAbstract)
                    && (interfaces || !t.IsInterface)
                );
            }

            public IEnumerable<Type> HaveAttribute<T>(bool inherit = true, bool abstracts = false, bool interfaces = false) where T : Attribute {
                return HaveAttribute(typeof(T), inherit, abstracts, interfaces);
            }

            public IEnumerable<Type> HaveAttribute(Type type, bool inherit = true,  bool abstracts = false, bool interfaces = false) {
                return types.Where(t => 
                    t.GetCustomAttributes(type, inherit).Length > 0
                    && (abstracts || !t.IsAbstract)
                    && (interfaces || !t.IsInterface)
                );
            }

            public IEnumerable<T> InstanceOf<T>() where T : class {
                return AssignableTo<T>()
                    .Select(type => Activator.CreateInstance(type) as T)
                    .ToList();
            }

            public IEnumerable<object> InstanceOf(Type type) {
                return AssignableTo(type)
                    .Select(Activator.CreateInstance)
                    .ToList();
            }
        }
    }
}