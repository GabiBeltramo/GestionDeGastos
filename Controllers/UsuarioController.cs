using GestionDeGastos.DATA;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GestionDeGastos.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly GestionDeGastosWebContext _Context;

        public static Usuario usuarioGlobal;
        
        public UsuarioController(GestionDeGastosWebContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AcercaDe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearPerfil(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.EncriptarContraseña();

                usuario.Fondo = 0;
                _Context.Add(usuario);
                await _Context.SaveChangesAsync();
                usuarioGlobal = usuario;

                return RedirectToAction("Perfil", "Perfil");
            }
            return View(usuario);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string correo, string password)
        {
            Usuario usuario = await ObtenerUsuarioPorCorreo(correo);

            if (usuario != null && usuario.VerificarContraseña(password))
            {
                HttpContext.Session.SetString("UsuarioActual", JsonSerializer.Serialize(usuario));

                return RedirectToAction("Perfil", "Perfil");
            }

            ModelState.AddModelError("", "Correo o contraseña incorrectos");
            return View();
        }

        private async Task<Usuario> ObtenerUsuarioPorCorreo(string correo)
        {
            var usuario = await _Context.Usuario.FirstOrDefaultAsync(u => u.Correo == correo);
            return usuario;
        }



    }
}
