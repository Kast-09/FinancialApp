using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FinancialApp.Web.Controllers;
using FinancialApp.Web.Models;
using FinancialApp.Web.Repositories;

namespace FinancialApp.Tests.Controllers
{
    internal class CuentaTransaccionControllerTest
    {
        private readonly ICuentaTransaccionRepositorio _cuentaTransaccionRepositorio;

        [Test]
        public void IndexViewCase01()
        {
            var mockListaTransaccion = new Mock<ICuentaTransaccionRepositorio>();
            //mockListaTransaccion.Setup(o => o.ListaTransacciones(1)).Returns(new List<Transaccion> { new Transaccion { Id = 1, N} })
            var controller = new CuentaTransaccionController(null, null);
            var view = controller.Index(1);
        }
    }
}
