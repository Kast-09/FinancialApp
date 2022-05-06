using System.Security.Claims;
using FinancialApp.Web.DB;
using FinancialApp.Web.Models;
using FinancialApp.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialApp.Web.Controllers;

[Authorize]
public class CuentaController : Controller
{
    private readonly ITipoCuentaRepositorio _tipoCuentaRepositorio;
    private readonly ICuentaRepositorio _cuentaRepositorio;
    private DbEntities _dbEntities;

    public CuentaController(ITipoCuentaRepositorio tipoCuentaRepositorio, ICuentaRepositorio cuentaRepositorio, DbEntities dbEntities)
    {
        _tipoCuentaRepositorio = tipoCuentaRepositorio;
        _cuentaRepositorio = cuentaRepositorio;
        _dbEntities = dbEntities;
    }


    [HttpGet]
    public IActionResult Index()
    {
        var usuario = GetLoggedUser();
        var cuentas = _cuentaRepositorio.ObtenerCuentasDeUsuario(usuario.Id);
        ViewBag.Total = cuentas.Any() ? cuentas.Sum(o => o.Monto) : 0; 
        return View(cuentas);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.TipoDeCuentas = _tipoCuentaRepositorio.ObtenerTodos();
        return View(new Cuenta());
    }

    [HttpPost]
    public IActionResult Create(Cuenta cuenta)
    {
        cuenta.UsuarioId = GetLoggedUser().Id;
        
        if (cuenta.TipoCuentaId > 6 || cuenta.TipoCuentaId < 1)
        {
            ModelState.AddModelError("TipoCuentaId", "Tipo de cuenta no exite");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.TipoDeCuentas = _tipoCuentaRepositorio.ObtenerTodos();
            return View("Create", cuenta);
        }

        _cuentaRepositorio.guardarCuenta(cuenta);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        //var cuenta = _dbEntities.Cuentas.First(o => o.Id == id); // lambdas / LINQ
        var cuenta = _cuentaRepositorio.ObtenerCuentaPorId(id);
        //ViewBag.TipoDeCuentas = _dbEntities.TipoCuentas.ToList();
        ViewBag.TipoDeCuentas = _tipoCuentaRepositorio.ObtenerTodos();
        return View(cuenta);
    }
    
    [HttpPost]
    public IActionResult Edit(int id, Cuenta cuenta)
    {
        if (!ModelState.IsValid) {
            ViewBag.TipoDeCuentas = _dbEntities.TipoCuentas.ToList();
            return View("Edit", cuenta);
        }
        
        //var cuentaDb = _dbEntities.Cuentas.First(o => o.Id == id);
        var cuentaDb = _cuentaRepositorio.ObtenerCuentaPorId(id);
        cuentaDb.Nombre = cuenta.Nombre;
        //cuentaDb.Nombre = _cuentaRepositorio.cuentaActualizada(cuenta);
        //_dbEntities.SaveChanges();
        _cuentaRepositorio.ActualizarDatos();
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Delete(int id)
    {
        //var cuentaDb = _dbEntities.Cuentas.First(o => o.Id == id);
        var cuentaDb = _cuentaRepositorio.ObtenerCuentaPorId(id);
        //_dbEntities.Cuentas.Remove(cuentaDb);
        //_dbEntities.SaveChanges();
        _cuentaRepositorio.EliminarCuenta(cuentaDb);

        return RedirectToAction("Index");
    }

    private Usuario GetLoggedUser()
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        var username = claim.Value;
        return DbEntities.Usuarios.First(o => o.Username == username);
    }
}