using Microsoft.EntityFrameworkCore;

namespace GestionDeGastos.DATA;
public class GestionDeGastosWebContext : DbContext
{
    public GestionDeGastosWebContext(DbContextOptions<GestionDeGastosWebContext> options) : base(options)
    {
    }

    public DbSet<GestionDeGastos.Models.Usuario> Usuario { get; set; } = default!;
    public DbSet<GestionDeGastos.Models.Consumo> Consumo { get; set; } = default!;
    public DbSet<GestionDeGastos.Models.Etiqueta> Etiqueta { get; set; } = default!;

}
