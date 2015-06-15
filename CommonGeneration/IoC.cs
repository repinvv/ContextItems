namespace ContextItems.CommonGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class IoC
    {
        private readonly Dictionary<Type, RegisteredObject> registeredObjects = new Dictionary<Type, RegisteredObject>();

        public void Register<TType>() where TType : class
        {
            Register(typeof(TType), typeof(TType));
        }

        public void Register<TType, TConcrete>() where TConcrete : class, TType
        {
            Register(typeof(TType), typeof(TConcrete));
        }

        public void RegisterSingleton<TType>() where TType : class
        {
            RegisterSingleton<TType, TType>();
        }

        public void RegisterSingleton<TType, TConcrete>() where TConcrete : class, TType
        {
            RegisterInstance<TType, TConcrete>(null);
        }

        public void RegisterInstance<TType>(TType instance) where TType : class
        {
            RegisterInstance<TType, TType>(instance);
        }

        public void RegisterInstance<TType, TConcrete>(TConcrete instance) where TConcrete : class, TType
        {
            registeredObjects[typeof(TType)] = new Singleton(this, typeof(TConcrete), instance);
        }

        public TTypeToResolve Resolve<TTypeToResolve>()
        {
            return (TTypeToResolve)Resolve(typeof(TTypeToResolve));
        }

        public object Resolve(Type type)
        {
            RegisteredObject registeredObject;
            if (!registeredObjects.TryGetValue(type, out registeredObject))
            {
                throw new ArgumentOutOfRangeException(string.Format("The type {0} has not been registered", type.Name));
            }

            return registeredObject.CreateInstance();
        }

        public void Register(Type type, Type concrete)
        {
            registeredObjects[type] = new RegisteredObject(this, concrete);
        }

        public void Register(Type type)
        {
            Register(type, type);
        }

        private class RegisteredObject
        {
            private readonly IoC ioc;
            private readonly Type concreteType;
            private readonly ParameterInfo[] constructorParams;

            public RegisteredObject(IoC ioc, Type concreteType)
            {
                this.ioc = ioc;
                this.concreteType = concreteType;
                var constructorInfo = concreteType.GetConstructors().FirstOrDefault();
                constructorParams = constructorInfo == null ? new ParameterInfo[0] : constructorInfo.GetParameters();
            }

            public virtual object CreateInstance()
            {
                return Activator.CreateInstance(concreteType, constructorParams.Select(parameter => ioc.Resolve(parameter.ParameterType)).ToArray());
            }
        }

        private class Singleton : RegisteredObject
        {
            private object instance;

            public Singleton(IoC ioc, Type concreteType, object instance)
                : base(ioc, concreteType)
            {
                this.instance = instance;
            }

            public override object CreateInstance()
            {
                return instance = instance ?? base.CreateInstance();
            }
        }
    }
}
