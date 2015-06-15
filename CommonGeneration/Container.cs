namespace ContextItems.CommonGeneration
{
    using System.Linq;
    using global::ContextItems.CommonGeneration.Parameters;

    public class Container
    {
        private readonly IoC container;

        public Container(object source)
        {
            container = new IoC();
            Register(source);
            Register(this);
            container.RegisterSingleton<ParametersService>();
        }

        private void Register(object source)
        {
            foreach (var type in source.GetType().Assembly.GetTypes().Where(x => !x.IsAbstract).Where(x => !x.IsValueType).Where(x => !x.Name.StartsWith("<")))
            {
                container.Register(type);
                var face = type.GetInterfaces().FirstOrDefault();
                if (face != null)
                {
                    container.Register(face, type);
                }
            }
        }

        public T Get<T>()
        {
            return container.Resolve<T>();
        }
    }
}