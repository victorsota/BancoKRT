using BancoKRT.Controllers;
using BancoKRT.Models;
using BancoKRT.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BancoKRT.Tests.Controllers
{
    public class LimitePixControllerTests
    {
        private readonly Mock<ILimitePixRepository> _mockRepository;
        private readonly Mock<ILogger<LimitePixController>> _mockLogger;
        private readonly LimitePixController _controller;

        public LimitePixControllerTests()
        {
            _mockRepository = new Mock<ILimitePixRepository>();
            _mockLogger = new Mock<ILogger<LimitePixController>>();
            _controller = new LimitePixController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void Add_Get_ReturnsView()
        {
            var result = _controller.Add() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Add_Post_ValidData_RedirectsToBuscar()
        {
            var viewModel = new LimitePixModel
            {
                Cpf = "12345678900",
                Conta = "1234",
                Agencia = 5678,
                LimitePix = 1500
            };

            _mockRepository.Setup(repo => repo.Buscar(viewModel.Cpf)).ReturnsAsync(new List<LimitePixModel>());
            _mockRepository.Setup(repo => repo.ListarTodos()).ReturnsAsync(new List<LimitePixModel>());

            var result = await _controller.Add(viewModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Buscar", result.ActionName);
        }

        [Fact]
        public async Task Add_Post_ExistingCpf_ReturnsViewWithModelError()
        {
            var viewModel = new LimitePixModel
            {
                Cpf = "12345678900",
                Conta = "1234",
                Agencia = 5678,
                LimitePix = 1500
            };

            var existingCliente = new LimitePixModel
            {
                Cpf = viewModel.Cpf,
                Conta = viewModel.Conta,
                Agencia = viewModel.Agencia,
                LimitePix = 1000
            };

            _mockRepository.Setup(repo => repo.Buscar(viewModel.Cpf)).ReturnsAsync(new List<LimitePixModel> { existingCliente });

            var result = await _controller.Add(viewModel) as ViewResult;

            Assert.NotNull(result);
            Assert.True(result.ViewData.ModelState.ContainsKey("Cpf"));
            Assert.Equal("Este CPF já foi cadastrado.", result.ViewData.ModelState["Cpf"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Buscar_Get_NoParameter_ReturnsViewWithAllRecords()
        {
            var mockData = new List<LimitePixModel>
            {
                new LimitePixModel { Id = 1, Cpf = "12345678900", Conta = "1234", Agencia = 5678, LimitePix = 1500 },
                new LimitePixModel { Id = 2, Cpf = "98765432100", Conta = "5678", Agencia = 9012, LimitePix = 2000 }
            };

            _mockRepository.Setup(repo => repo.ListarTodos()).ReturnsAsync(mockData);

            var result = await _controller.Buscar(null) as ViewResult;
            var model = result.Model as IEnumerable<LimitePixModel>;

            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(mockData.Count, model.Count());
        }

        [Fact]
        public async Task Buscar_Get_WithParameter_ReturnsViewWithFilteredRecords()
        {
            var cpf = "12345678900";
            var mockData = new List<LimitePixModel>
            {
                new LimitePixModel { Id = 1, Cpf = cpf, Conta = "1234", Agencia = 5678, LimitePix = 1500 }
            };

            _mockRepository.Setup(repo => repo.Buscar(cpf)).ReturnsAsync(mockData);

            var result = await _controller.Buscar(cpf) as ViewResult;
            var model = result.Model as IEnumerable<LimitePixModel>;

            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(mockData.Count, model.Count());
        }

        [Fact]
        public async Task Editar_Get_ValidId_ReturnsView()
        {
            var id = 1;
            var mockData = new LimitePixModel { Id = id, Cpf = "12345678900", Conta = "1234", Agencia = 5678, LimitePix = 1500 };

            _mockRepository.Setup(repo => repo.BuscarPorId(id)).ReturnsAsync(mockData);

            var result = await _controller.Editar(id) as ViewResult;
            var model = result.Model as LimitePixModel;

            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task Editar_Get_InvalidId_ReturnsNotFound()
        {
            var id = 999; // Id inválido que não existe nos dados de mock

            _mockRepository.Setup(repo => repo.BuscarPorId(id)).ReturnsAsync((LimitePixModel)null);

            var result = await _controller.Editar(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Editar_Post_ValidData_RedirectsToBuscar()
        {
            var viewModel = new LimitePixModel
            {
                Id = 1,
                Cpf = "12345678900",
                Conta = "1234",
                Agencia = 5678,
                LimitePix = 1500
            };

            var result = await _controller.Editar(viewModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Buscar", result.ActionName);
        }

        [Fact]
        public async Task Editar_Post_InvalidModel_ReturnsViewWithModelError()
        {
            var viewModel = new LimitePixModel
            {
                Id = 1,
                Cpf = "12345678900",
                Conta = "1234",
                Agencia = 5678,
                LimitePix = 0 // Valor inválido que falhará na validação do modelo
            };

            _controller.ModelState.AddModelError("LimitePix", "O LimitePix deve ser maior que zero.");

            var result = await _controller.Editar(viewModel) as ViewResult;

            Assert.NotNull(result);
            Assert.True(result.ViewData.ModelState.ContainsKey("LimitePix"));
            Assert.Equal("O LimitePix deve ser maior que zero.", result.ViewData.ModelState["LimitePix"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Deletar_Post_ValidId_RedirectsToBuscar()
        {
            var id = 1;

            var result = await _controller.Deletar(id.ToString()) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Buscar", result.ActionName);
        }

        [Fact]
        public async Task Deletar_Post_InvalidId_ReturnsBadRequest()
        {
            var id = ""; // Id inválido (string vazia)

            var result = await _controller.Deletar(id) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Id é obrigatório.", result.Value);
        }

        [Fact]
        public void ConsultarLimite_Get_ReturnsView()
        {
            var result = _controller.ConsultarLimite() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ConsultarLimite_Post_CpfNotFound_ReturnsViewWithMessage()
        {
            var cpf = "12345678900";
            _mockRepository.Setup(repo => repo.Buscar(cpf)).ReturnsAsync((IEnumerable<LimitePixModel>)null);

            var result = await _controller.ConsultarLimite(cpf, 5678, "1234", 1500) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("ConsultarLimite", result.ViewName);
            Assert.Equal("CPF não encontrado.", result.ViewData["Message"]);
        }

        [Fact]
        public async Task ConsultarLimite_Post_InvalidData_ReturnsViewWithMessage()
        {
            var cpf = "12345678900";
            var cliente = new LimitePixModel { Cpf = cpf, Conta = "1234", Agencia = 5678, LimitePix = 1000 };
            _mockRepository.Setup(repo => repo.Buscar(cpf)).ReturnsAsync(new List<LimitePixModel> { cliente });

            var result = await _controller.ConsultarLimite(cpf, 9999, "5678", 1500) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("ConsultarLimite", result.ViewName);
            Assert.Equal("Os dados informados não correspondem ao CPF informado.", result.ViewData["Message"]);
        }

    }
}
