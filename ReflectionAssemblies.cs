using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace NaughtyBiker.Wrappers {
    [PublicAPI]
    public class ReflectionAssemblies<TEntryPoint> : IReflectionAssemblies {
        private readonly IEnumerable<Assembly> assemblies;

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

        [PublicAPI]
        public class ReflectionTypes : IReflectionAssemblies.IReflectionTypes {
            private readonly IEnumerable<Type> types;

            public ReflectionTypes(IEnumerable<Assembly> assemblies) {
                types = assemblies.SelectMany(assembly => assembly.GetTypes());
            }

            public IEnumerable<Type> All() {
                return types;
            }

            public Type WithName(string name) {
                return types.FirstOrDefault(type => type.Name == name);
            }

            public IEnumerable<Type> From<T>() where T : class {
                return From(typeof(T));
            }

            public IEnumerable<Type> From(Type type) {
                return types.Where(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            }

            public IEnumerable<Type> ImplementsGeneric(Type type, Predicate<Type> predicate = null) {
                return types.Where(t =>
                    t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)
                    && (predicate == null || predicate!(t))
                );
            }

            public IEnumerable<Type> HaveAttribute<T>() where T : Attribute {
                return types.Where(type => type.GetCustomAttributes(typeof(T), true).Length > 0);
            }

            public IEnumerable<T> InstanceOf<T>() where T : class {
                return From<T>()
                    .Select(type => Activator.CreateInstance(type) as T)
                    .ToList();
            }
        }
    }
}