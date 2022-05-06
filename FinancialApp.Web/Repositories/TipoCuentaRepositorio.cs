using FinancialApp.Web.DB;
using FinancialApp.Web.Models;

namespace FinancialApp.Web.Repositories;

public interface ITipoCuentaRepositorio
{
    void Guardar(TipoCuenta tipoCuenta);
    List<TipoCuenta> ObtenerTodos();
    List<TipoCuenta> ObtenerPoNombre(string nombre);
}

public class TipoCuentaRepositorio : ITipoCuentaRepositorio
{
    private DbEntities _dbEntities;

    public TipoCuentaRepositorio(DbEntities dbEntities)
    {
        _dbEntities = dbEntities;
    }

    public void Guardar(TipoCuenta tipoCuenta)
    {
        _dbEntities.TipoCuentas.Add(tipoCuenta);
        _dbEntities.SaveChanges();
    }

    public List<TipoCuenta> ObtenerTodos()
    {
        return _dbEntities.TipoCuentas.ToList();
    }

    public List<TipoCuenta> ObtenerPoNombre(string nombre)
    {
        return _dbEntities
            .TipoCuentas.Where(o => o.Nombre.Contains(nombre))
            .ToList();
    }
}