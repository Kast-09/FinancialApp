using System.Collections.Generic;
using System.Linq;
using FinancialApp.Tests.Helpers;
using FinancialApp.Web.DB;
using FinancialApp.Web.Models;
using FinancialApp.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using NUnit.Framework;

namespace FinancialApp.Tests.Repositories;

public class CuentaRepositorioTest
{
    private IQueryable<Cuenta> data;

    [SetUp]
    public void SetUp()
    {
        data = new List<Cuenta>
        {
            new() { Id = 1, Nombre = "Cuenta 01"},
            new() { Id = 2, Nombre = "Cuenta 02"},
            new() { Id = 3, Nombre = "Cuenta 03"},
        }.AsQueryable();
    }

    [Test]
    public void ObtenerTodosTestCaso01()
    {
        // var mockDbSetCuenta = new Mock<DbSet<Cuenta>>();
        // mockDbSetCuenta.As<IQueryable<Cuenta>>().Setup(o => o.Provider).Returns(data.Provider);
        // mockDbSetCuenta.As<IQueryable<Cuenta>>().Setup(o => o.Expression).Returns(data.Expression);
        // mockDbSetCuenta.As<IQueryable<Cuenta>>().Setup(o => o.ElementType).Returns(data.ElementType);
        // mockDbSetCuenta.As<IQueryable<Cuenta>>().Setup(o => o.GetEnumerator()).Returns(data.GetEnumerator());

        var mockDbSetCuenta = new MockDBSet<Cuenta>(data);
        var mockDB = new Mock<DbEntities>();
        mockDB.Setup(o => o.Cuentas).Returns(mockDbSetCuenta.Object);

        var repo = new CuentaRepositorio(mockDB.Object);
        var result = repo.ObtenerTodos();

        Assert.AreEqual(3, result.Count);
    }
}