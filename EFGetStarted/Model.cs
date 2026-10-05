using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class EstudiantesContext : DbContext
{
    public DbSet<Estudiante> Estudiantes { get; set; }

    public string DbPath { get; }

    public EstudiantesContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "estudiantes.db");
    }

    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Estudiante
{
    public int EstudianteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Carrera { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
}