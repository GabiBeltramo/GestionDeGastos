using GestionDeGastos.DATA;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GestionDeGastos.Controllers
{
    public class ConsumoController : Controller
    {
        private readonly GestionDeGastosWebContext _Context;
        public ConsumoController(GestionDeGastosWebContext context)
        {
            _Context = context;
        }

        public static Consumo consumoGlobal;

        private Usuario usuarioGlobal;
        private Etiqueta etiquetaGlobal;

        public void InyectarUsuarioGlobal(Usuario usuario)
        {
            HttpContext.Session.SetString("UsuarioActual", JsonSerializer.Serialize(usuario));
        }

        public void InyectarEtiquetaGlobal(Etiqueta etiqueta)
        {
            HttpContext.Session.SetString("EtiquetaActual", JsonSerializer.Serialize(etiqueta));
        }

        [HttpGet]
        public async Task<IActionResult> NuevoConsumo()
        {
            if (usuarioGlobal != null)
            {
              var etiquetas = await _Context.Etiqueta.ToListAsync();
              ViewBag.Etiquetas = etiquetas;

                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NuevoConsumo(Consumo consumo)
        {
            if (usuarioGlobal != null)
            {

                consumoGlobal = consumo;
                consumo.IdUsuario = usuarioGlobal.Id;
                consumo.IdEtiqueta = etiquetaGlobal.Id;
                consumo.Fecha = consumo.Fecha;
                _Context.Add(consumo);
                await _Context.SaveChangesAsync();

                var etiquetas = await _Context.Etiqueta.ToListAsync();
                ViewBag.Etiquetas = etiquetas;

                var usuario = await _Context.Usuario.FindAsync(usuarioGlobal.Id);
                usuario.Fondo -= consumo.Monto;
                etiquetaGlobal.GastoEtiqueta = etiquetaGlobal.GastoEtiqueta = consumo.Monto;
                _Context.Update(usuario);
                await _Context.SaveChangesAsync();
                

                return RedirectToAction("NuevoConsumo");
            }

            return RedirectToAction("NuevoConsumo");
        }

    }
}
