namespace membresias.be.Filters
{
    public class MiembroFilters
    {
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Curp { get; set; }
        public string? FechaNacimientoDesde { get; set; }
        public string? FechaNacimientoHasta { get; set; }
        public string? FechaReingresoDesde { get; set; }
        public string? FechaReingresoHasta { get; set; }
        public List<string>? MembresiaCodigo { get; set; }
        public List<string>? MiembroEstatusCodigo { get; set; }
        public bool? IsDeleted { get; set; }

        // Paginación
        public int Pages { get; set; } = 1;
        public int PageSize { get; set; } = 10; 

        // Ordenamiento
        public string? SortBy { get; set; }
        public bool SortDesc { get; set; } = false; 
    }
}
