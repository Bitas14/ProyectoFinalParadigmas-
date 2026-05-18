using Castle.DynamicProxy;
using ProyectoFinal.Models;

namespace ProyectoFinal.Interceptors
{
    // PARADIGMA DE ASPECTOS (AOP) - Interceptor 2: Validación centralizada
    // Captura y valida parámetros antes de que lleguen al método real.
    // Sin este interceptor, cada servicio tendría que validar por separado (violación de SRP).
    public class ValidationInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var metodo = invocation.Method.Name;

            foreach (var arg in invocation.Arguments)
            {
                // Validar que los objetos de dominio no sean nulos
                if (arg == null && invocation.Arguments.Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"  [VALIDACIÓN] ⚠ Argumento nulo en {metodo}. Operación cancelada.");
                    Console.ResetColor();
                    return;
                }

                // Validar notas en rango válido (0.0 - 5.0 sistema colombiano)
                if (arg is Nota nota)
                {
                    if (nota.ValorNota < 0 || nota.ValorNota > 5.0m)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"  [VALIDACIÓN] ⚠ Nota {nota.ValorNota} fuera del rango [0.0 - 5.0]. Operación cancelada.");
                        Console.ResetColor();
                        return;
                    }
                }

                // Validar que Estudiantes y Profesores tengan nombre
                if (arg is Persona persona && string.IsNullOrWhiteSpace(persona.Nombres))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"  [VALIDACIÓN] ⚠ Persona sin nombre en {metodo}. Operación cancelada.");
                    Console.ResetColor();
                    return;
                }
            }

            // Si pasó todas las validaciones, ejecutar el método real
            invocation.Proceed();
        }
    }
}
