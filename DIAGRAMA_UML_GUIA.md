# Guía para crear el Diagrama UML en Draw.io

## Clases a incluir en el diagrama:

### Clases Base (POO - Jerarquía)
- **Persona** (ABSTRACTA)
  - Propiedades: id, nombre, email
  - Método abstracto: ObtenerRol()
  - Interfaces: IIdentifiable, IReportable

- **Estudiante** (extiende Persona)
  - Propiedad adicional: direccion (Direccion)
  - Propiedades: materias (List<int>), notas (List<Nota>)

- **Profesor** (extiende Persona)
  - Propiedades: titulo, materias (List<int>)

### Clases de Dominio
- **Materia**
  - Propiedades: id, nombre, profesor_id (referencia a Profesor)
  - Interfaz: IIdentifiable, IReportable

- **Nota**
  - Propiedades: id, estudiante_id, materia_id, valor
  - Relación: Asociación a Estudiante y Materia

- **Direccion**
  - Propiedades: calle, numero, ciudad, pais
  - Relación: Composición con Estudiante (mismo ciclo de vida)

### Interfaces
- **IIdentifiable**
  - Propiedad: Id
  - Método: ObtenerRol()

- **IReportable**
  - Método: ObtenerResumen()

## Relaciones a mostrar:

1. **Herencia**: Estudiante y Profesor heredan de Persona
2. **Composición**: Estudiante contiene Direccion (la línea debe ser sólida con rombo lleno)
3. **Agregación**: Materia referencia Profesor por Id (línea con rombo vacío)
4. **Asociación**: Nota se asocia con Estudiante y Materia (línea sencilla)
5. **Implementación de Interfaz**: Persona, Materia, Nota implementan IIdentifiable e IReportable (línea punteada)


[UML_ProyectoFinal_SistemNotasPDF.pdf](https://github.com/user-attachments/files/28034012/UML_ProyectoFinal_SistemNotasPDF.pdf)
