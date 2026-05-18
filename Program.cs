using ProyectoFinal.DI;

// PARADIGMA DE ASPECTOS - Castle Windsor inicializa el contenedor DI
// Todos los servicios se resuelven como interfaces, con interceptores ya aplicados
var contenedor = ContenedorWindsor.Inicializar();

var estudianteServicio = ContenedorWindsor.Resolver<ProyectoFinal.Services.IEstudianteServicio>();
var materiaServicio    = ContenedorWindsor.Resolver<ProyectoFinal.Services.IMateriaServicio>();
var notaServicio       = ContenedorWindsor.Resolver<ProyectoFinal.Services.INotaServicio>();

// SOLID - DIP: Menu recibe abstracciones (interfaces), no implementaciones concretas
var menu = new ProyectoFinal.Menu(estudianteServicio, materiaServicio, notaServicio);
menu.Ejecutar();
