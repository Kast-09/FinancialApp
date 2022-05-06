using FinancialApp.Web.Models;
using FinancialApp.Web.DB;
using Microsoft.EntityFrameworkCore;
using FinancialApp.Web.Models;

namespace FinancialApp.Web.Repositories
{
    public interface ICuentaTransaccionRepositorio
    {
        List<Transaccion> ListaTransacciones(int cuentaId);
    }

    public class CuentaTransaccionRepositorio : ICuentaTransaccionRepositorio
    {
        private DbEntities _dbEntities;

        public CuentaTransaccionRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public List<Transaccion> ListaTransacciones(int cuentaId)
        {
            return DbEntities.Transacciones.Where(o => o.CuentaId == cuentaId).ToList();
        }
    }
}
