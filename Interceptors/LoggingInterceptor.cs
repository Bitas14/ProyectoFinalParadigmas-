using Castle.DynamicProxy;

namespace ProyectoFinal.Interceptors
{
    // PARADIGMA DE ASPECTOS (AOP) - Interceptor 1: Logging automático
    // Funciones transversales: registrar entrada/salida de métodos sin modificar la lógica de negocio
    // Implementa IInterceptor de Castle DynamicProxy
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var metodo = invocation.Method.Name;
            var args = string.Join(", ", invocation.Arguments.Select(a => a?.ToString() ?? "null"));

            // ANTES de ejecutar el método real
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n  [LOG] → {metodo}({args})");
            Console.ResetColor();

            var inicio = DateTime.Now;

            try
            {
                // Ejecuta el método real interceptado
                invocation.Proceed();

                var duracion = (DateTime.Now - inicio).TotalMilliseconds;

                // DESPUÉS de ejecutar exitosamente
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  [LOG] ← {metodo} completado en {duracion:F1}ms");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"  [LOG] ✗ {metodo} falló: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
    }
}
