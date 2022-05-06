using FinancialApp.Web.DB;
using FinancialApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancialApp.Web.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace FinancialApp.Web.Controllers;

[Authorize]
public class CuentaTransaccionController : Controller
{
    //private readonly ICuentaTransaccionRepositorio _cuentaTransaccionRepositorio;
    //private DbEntities _DbEntities;

    //public CuentaTransaccionController(ICuentaTransaccionRepositorio cuentaTransaccionRepositorio, DbEntities dbEntities)
    //{
    //    _cuentaTransaccionRepositorio = cuentaTransaccionRepositorio;
    //    _DbEntities = dbEntities;
    //}

    [HttpGet]
    public IActionResult Index(int cuentaId)
    {
        var items = DbEntities.Transacciones.Where(o => o.CuentaId == cuentaId).ToList();
        //var items = _cuentaTransaccionRepositorio.ListaTransacciones(cuentaId);
        ViewBag.CuentaId = cuentaId;
        ViewBag.Total = items.Any() ? items.Sum(x => x.Monto) : 0;

        return View(items);
    }
    
    [HttpGet]
    public IActionResult Create(int cuentaId)
    {
        ViewBag.CuentaId = cuentaId;
        return View(new Transaccion());
    }
    
    [HttpPost]
    public IActionResult Create(int cuentaId, Transaccion transaccion)
    {
        transaccion.Id = GetNextId();
        transaccion.CuentaId = cuentaId;
        if (transaccion.Tipo == "GASTO")
            transaccion.Monto *= -1;
        
        DbEntities.Transacciones.Add(transaccion);

        return RedirectToAction("Index", new { cuentaId = cuentaId});
    }
    
    public int GetNextId()  {
        if (DbEntities.Transacciones.Count == 0)
            return 1;
        return DbEntities.Transacciones.Max(o => o.Id) + 1;
    }
}