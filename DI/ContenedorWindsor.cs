using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.DynamicProxy;
using ProyectoFinal.Interceptors;
using ProyectoFinal.Services;

namespace ProyectoFinal.DI
{
    // PARADIGMA DE ASPECTOS - Castle Windsor como contenedor de Inyección de Dependencias
    // SOLID - DIP: los servicios se registran y resuelven como interfaces, no como implementaciones
    public static class ContenedorWindsor
    {
        private static IWindsorContainer? _contenedor;

        public static IWindsorContainer Inicializar()
        {
            _contenedor = new WindsorContainer();

            // Registrar interceptores
            _contenedor.Register(Component.For<LoggingInterceptor>().LifestyleSingleton());
            _contenedor.Register(Component.For<ValidationInterceptor>().LifestyleSingleton());

            // Registrar servicios COMO SU INTERFAZ con interceptores aplicados
            // Castle Windsor los resuelve a través de interfaces = intercepta sin métodos virtual
            _contenedor.Register(
                Component.For<IEstudianteServicio>()
                    .ImplementedBy<EstudianteServicio>()
                    .Interceptors<LoggingInterceptor, ValidationInterceptor>()
                    .LifestyleTransient()
            );

            _contenedor.Register(
                Component.For<IMateriaServicio>()
                    .ImplementedBy<MateriaServicio>()
                    .Interceptors<LoggingInterceptor, ValidationInterceptor>()
                    .LifestyleTransient()
            );

            _contenedor.Register(
                Component.For<INotaServicio>()
                    .ImplementedBy<NotaServicio>()
                    .Interceptors<LoggingInterceptor, ValidationInterceptor>()
                    .LifestyleTransient()
            );

            return _contenedor;
        }

        public static T Resolver<T>()
        {
            if (_contenedor == null)
                throw new InvalidOperationException("Contenedor no inicializado. Llama a Inicializar() primero.");
            return _contenedor.Resolve<T>();
        }
    }
}
