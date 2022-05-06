using FinancialApp.Web.Models;
using FinancialApp.Web.DB;
using Microsoft.EntityFrameworkCore;

namespace FinancialApp.Web.Repositories;

public interface ICuentaRepositorio
{
    Cuenta ObtenerCuentaPorId(int id);
    void ActualizarDatos();
    List<Cuenta> ObtenerCuentasDeUsuario(int UserId);
    void guardarCuenta(Cuenta cuenta);
    List<Cuenta> ObtenerTodos();
    void EliminarCuenta(Cuenta cuentaDb);
    bool VerificarEliminacion();
}

public class CuentaRepositorio: ICuentaRepositorio
{
    private DbEntities _dbEntities;
    private Cuenta _cuentaAux;
    private int IdAux;


    public CuentaRepositorio(DbEntities dbEntities)
    {
        _dbEntities = dbEntities;
    }
    
    public Cuenta ObtenerCuentaPorId(int id)
    {
        return _dbEntities.Cuentas.First(o => o.Id == id); // lambdas / LINQ       
    }

    public void ActualizarDatos()
    {
         _dbEntities.SaveChanges();
    }

    public List<Cuenta> ObtenerCuentasDeUsuario(int UserId)
    {
        return _dbEntities.Cuentas
            .Include(o => o.TipoCuenta)
            .Where(o => o.UsuarioId == UserId).ToList();
    }

    public void guardarCuenta(Cuenta cuenta)
    {
        _dbEntities.Cuentas.Add(cuenta);
        _dbEntities.SaveChanges();
    }

    public List<Cuenta> ObtenerTodos()
    {
        return _dbEntities.Cuentas.ToList();
    }

    public void EliminarCuenta(Cuenta cuentaDb)
    {
        IdAux = cuentaDb.Id;
        _dbEntities.Cuentas.Remove(cuentaDb);
        _dbEntities.SaveChanges();
    }

    public bool VerificarEliminacion()
    {
        _cuentaAux = _dbEntities.Cuentas.First(o => o.Id == IdAux);
        if(_cuentaAux == null)
        {
            return true;
        }
        return false;
    }
}