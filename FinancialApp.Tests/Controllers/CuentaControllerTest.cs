using System.Collections.Generic;
using System.Security.Claims;
using FinancialApp.Web.Controllers;
using FinancialApp.Web.Models;
using FinancialApp.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FinancialApp.Tests.Controllers;

public class CuentaControllerTest
{
    private readonly ICuentaRepositorio _cuentaRepositorio;

    [Test]
    public void CreateViewCase01()
    {
        var mockTipoRepositorio = new Mock<ITipoCuentaRepositorio>();
        mockTipoRepositorio.Setup(o => o.ObtenerTodos()).Returns(new List<TipoCuenta>());
        
        var controller = new CuentaController(mockTipoRepositorio.Object, null, null);
        var view = controller.Create();
        
        Assert.IsNotNull(view);
    }
    
    [Test]
    public void EditViewCase01()
    { 
        var mockTipoRepositorio = new Mock<ITipoCuentaRepositorio>();
        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>();
        mockCuentaRepositorio.Setup(o => o.ObtenerCuentaPorId(2)).Returns(new Cuenta{Id = 1, Nombre = "Tarjeta de Credito", Monto = 25});
        var controller = new CuentaController(mockTipoRepositorio.Object,mockCuentaRepositorio.Object, null);
        var view = (ViewResult)controller.Edit(2);
        
        Assert.IsNotNull(view.Model);
        Assert.IsNotNull(view);

    }

    [Test]
    public void EditPostViewCase01()
    {
        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>();
        mockCuentaRepositorio.Setup(o => o.ObtenerCuentaPorId(1)).Returns(new Cuenta { Id = 1, Nombre = "Tarjeta de Credito", Monto = 25 });
        var controller = new CuentaController(null, mockCuentaRepositorio.Object, null);
        var EditC = (ViewResult)controller.Edit(1, new Cuenta { Id = 1, Nombre = "Debito", Monto = 25 });

        Assert.IsNotNull(EditC);
        Assert.IsNotNull(EditC.Model);
    }

    [Test]
    public void IndexViewCase01()
    {
        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { 
            new Claim(ClaimTypes.Name, "admin") 
        });

        var mockContext = new Mock<HttpContext>();   
        mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

        var mockCuentaRepo = new Mock<ICuentaRepositorio>();
        mockCuentaRepo.Setup(o => o.ObtenerCuentasDeUsuario(1)).Returns(new List<Cuenta> { 
            new Cuenta()    
        });

        var controller = new CuentaController(null, mockCuentaRepo.Object, null);
        controller.ControllerContext = new ControllerContext() { 
            HttpContext = mockContext.Object 
        };
        var view = (ViewResult)controller.Index();
        Assert.IsNotNull(view);
        Assert.AreEqual(1, ((List<Cuenta>)view.Model).Count);
    }

    [Test]
    public void postCreateok()
    {
        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> {
            new Claim(ClaimTypes.Name, "admin")
        });

        var mockContext = new Mock<HttpContext>();
        mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>();

        var controller = new CuentaController(null, mockCuentaRepositorio.Object, null);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockContext.Object
        };

        var ResultadoC = controller.Create(new Cuenta() { TipoCuentaId = 2 });
        Assert.IsNotNull(ResultadoC);
        Assert.IsInstanceOf<RedirectToActionResult>(ResultadoC);
    }

    [Test]
    public void postCreateCrash()
    {
        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> {
            new Claim(ClaimTypes.Name, "admin")
        });

        var mockContext = new Mock<HttpContext>();
        mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>();
        var mocktipoCuentaRepo = new Mock<ITipoCuentaRepositorio>();

        var controller = new CuentaController(mocktipoCuentaRepo.Object, mockCuentaRepositorio.Object, null);
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockContext.Object
        };

        var ResultadoC = controller.Create(new Cuenta() { TipoCuentaId = 7 });
        Assert.IsNotNull(ResultadoC);
        Assert.IsInstanceOf<ViewResult>(ResultadoC);
    }

    [Test]
    public void DeleteCase01()
    {
        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>();
        mockCuentaRepositorio.Setup(o => o.ObtenerCuentaPorId(1)).Returns(new Cuenta { Id = 1, Nombre = "Tarjeta de Credito", Monto = 25 });

        var controller = new CuentaController(null, mockCuentaRepositorio.Object, null);
        var Eliminar = controller.Delete(1);

        //Assert.IsTrue(_cuentaRepositorio.VerificarEliminacion());

        Assert.IsNotNull(Eliminar);
    }
}