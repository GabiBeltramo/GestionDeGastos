using GestionDeGastos.DATA;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GestionDeGastos.Controllers
{
    public class EtiquetaController : Controller
    {
        private readonly GestionDeGastosWebContext _Context;
        
        public EtiquetaController(GestionDeGastosWebContext context)
        {
            _Context = context;
        }
        public static Etiqueta etiquetaGlobal;
       
        private Usuario usuarioGlobal;

        public void InyectarUsuarioGlobal(Usuario usuario)
        {
            HttpContext.Session.SetString("UsuarioActual", JsonSerializer.Serialize(usuario));
        }

        [HttpGet]
        public ActionResult NuevaEtiqueta()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearEtiqueta(Etiqueta etiqueta)
        {
            var usuarioGlobal = UsuarioController.usuarioGlobal;
            Usuario usuario = usuarioGlobal;

            if (usuarioGlobal != null)
            {
                etiquetaGlobal = etiqueta;
                etiquetaGlobal.GastoEtiqueta = 0;
                etiqueta.IdUsuario = usuarioGlobal.Id;
                _Context.Add(etiqueta);
                await _Context.SaveChangesAsync();

                return RedirectToAction("NuevaEtiqueta");
            }

            return RedirectToAction("NuevaEtiqueta");
        }
    }
}
