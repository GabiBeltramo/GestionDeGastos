using GestionDeGastos.DATA;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GestionDeGastos.Controllers
{
    public class PerfilController : Controller
    {
        private readonly GestionDeGastosWebContext _Context;

        public PerfilController(GestionDeGastosWebContext context)
        {
            _Context = context;
        }

        private Usuario usuarioGlobal;

        //
        public void InyectarUsuarioGlobal(Usuario usuario)
        {
            HttpContext.Session.SetString("UsuarioActual", JsonSerializer.Serialize(usuario));
        }

        [HttpGet]
        public ActionResult Perfil()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResumenGastos()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ModificarFondo()
        {
            var usuarioActual = HttpContext.Session.GetString("UsuarioActual");
            if (!string.IsNullOrEmpty(usuarioActual))
            {
                var usuario = JsonSerializer.Deserialize<Usuario>(usuarioActual);
                return View(usuario);
            }

            return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public async Task<IActionResult> ModificarFondo(decimal nuevoFondo)
        {
            var usuarioActual = HttpContext.Session.GetString("UsuarioActual");
            if (!string.IsNullOrEmpty(usuarioActual))
            {
                var usuarioGuardado = JsonSerializer.Deserialize<Usuario>(usuarioActual);
                usuarioGuardado.Fondo = nuevoFondo;

                _Context.Update(usuarioGuardado);
                await _Context.SaveChangesAsync();

                ViewBag.Fondo = nuevoFondo;
                return RedirectToAction("Perfil");
            }

            return RedirectToAction("Perfil", "Perfil");
        }

        [HttpPost]
        public  List<Object> VistaGastos()
        {
            List<Object> datos = new List<Object>();
            List<string> tags = _Context.Etiqueta.Select(x => x.Nombre).ToList();
            datos.Add(tags);
            List<decimal> gastos = _Context.Etiqueta.Select(x => x.GastoEtiqueta).ToList();
            datos.Add(gastos);
            return datos; 
        }

    }
}
