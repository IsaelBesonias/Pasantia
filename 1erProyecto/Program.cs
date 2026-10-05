using Microsoft.EntityFrameworkCore;

using var db = new EstudiantesContext();
await db.Database.MigrateAsync();

Console.WriteLine($"Ruta de la base de datos: {db.DbPath}.");

while (true)
{
    Console.WriteLine("\n--- Registro de estudiantes ---");
    Console.WriteLine("1. Registrar estudiante");
    Console.WriteLine("2. Mostrar todos los estudiantes");
    Console.WriteLine("3. Modificar estudiante");
    Console.WriteLine("4. Eliminar estudiante");
    Console.WriteLine("0. Salir");
    Console.Write("Seleccione una opcion: ");

    switch (Console.ReadLine())
    {
        case "1":
            await CrearEstudianteAsync(db);
            break;
        case "2":
            await MostrarEstudiantesAsync(db);
            break;
        case "3":
            await ModificarEstudianteAsync(db);
            break;
        case "4":
            await EliminarEstudianteAsync(db);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Opcion no valida.");
            break;
    }
}

static async Task CrearEstudianteAsync(EstudiantesContext db)
{
    var estudiante = new Estudiante
    {
        Nombre = LeerTextoObligatorio("Nombre: "),
        Apellido = LeerTextoObligatorio("Apellido: "),
        Edad = LeerEnteroPositivo("Edad: "),
        Carrera = LeerTextoObligatorio("Carrera: "),
        Correo = LeerTextoObligatorio("Correo electronico: ")
    };

    db.Estudiantes.Add(estudiante);
    await db.SaveChangesAsync();
    Console.WriteLine("Estudiante registrado correctamente.");
}

static async Task MostrarEstudiantesAsync(EstudiantesContext db)
{
    var estudiantes = await db.Estudiantes
        .OrderBy(estudiante => estudiante.EstudianteId)
        .ToListAsync();

    if (estudiantes.Count == 0)
    {
        Console.WriteLine("No hay estudiantes registrados.");
        return;
    }

    Console.WriteLine("\nID\tNombre\tApellido\tEdad\tCarrera\tCorreo");
    foreach (var estudiante in estudiantes)
    {
        Console.WriteLine(
            $"{estudiante.EstudianteId}\t{estudiante.Nombre}\t{estudiante.Apellido}" +
            $"\t{estudiante.Edad}\t{estudiante.Carrera}\t{estudiante.Correo}");
    }
}

static async Task ModificarEstudianteAsync(EstudiantesContext db)
{
    var estudiante = await BuscarEstudianteAsync(db);
    if (estudiante is null)
    {
        return;
    }

    estudiante.Nombre = LeerTextoObligatorio("Nuevo nombre: ");
    estudiante.Apellido = LeerTextoObligatorio("Nuevo apellido: ");
    estudiante.Edad = LeerEnteroPositivo("Nueva edad: ");
    estudiante.Carrera = LeerTextoObligatorio("Nueva carrera: ");
    estudiante.Correo = LeerTextoObligatorio("Nuevo correo electronico: ");
    await db.SaveChangesAsync();
    Console.WriteLine("Estudiante modificado correctamente.");
}

static async Task EliminarEstudianteAsync(EstudiantesContext db)
{
    var estudiante = await BuscarEstudianteAsync(db);
    if (estudiante is null)
    {
        return;
    }

    db.Estudiantes.Remove(estudiante);
    await db.SaveChangesAsync();
    Console.WriteLine("Estudiante eliminado correctamente.");
}

static async Task<Estudiante?> BuscarEstudianteAsync(EstudiantesContext db)
{
    Console.Write("ID del estudiante: ");
    if (!int.TryParse(Console.ReadLine(), out var id) || id <= 0)
    {
        Console.WriteLine("El ID debe ser un numero positivo.");
        return null;
    }

    var estudiante = await db.Estudiantes.FindAsync(id);
    if (estudiante is null)
    {
        Console.WriteLine("No se encontro un estudiante con ese ID.");
    }

    return estudiante;
}

static string LeerTextoObligatorio(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        var valor = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(valor))
        {
            return valor;
        }

        Console.WriteLine("El valor no puede estar vacio.");
    }
}

static int LeerEnteroPositivo(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        if (int.TryParse(Console.ReadLine(), out var valor) && valor > 0)
        {
            return valor;
        }

        Console.WriteLine("Ingrese un numero entero positivo.");
    }
}