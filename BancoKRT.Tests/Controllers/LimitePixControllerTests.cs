using BancoKRT.Controllers;
using BancoKRT.Models;
using BancoKRT.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BancoKRT.Tests.Controllers
{
    public class LimitePixControllerTests
    {
        private readonly Mock<ILimitePixRepository> _mockRepository;
        private readonly LimitePixController _controller;

        public LimitePixControllerTests()
        {
            _mockRepository = new Mock<ILimitePixRepository>();
            _controller = new LimitePixController(null, _mockRepository.Object);
        }

        [Fact]
        public async Task ConsultarLimite_Post_ValidData_ReturnsViewWithMessage()
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

            var result = await _controller.ConsultarLimite(viewModel.Cpf, viewModel.Agencia, viewModel.Conta, viewModel.LimitePix) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("ConsultarLimite", result.ViewName);
            Assert.Equal($"O LimitePix informado ({viewModel.LimitePix}) é maior que o limite já registrado ({existingCliente.LimitePix}).", result.ViewData["Message"]);
        }

        [Fact]
        public async Task ConsultarLimite_Post_InvalidCpf_ReturnsViewWithErrorMessage()
        {
            var viewModel = new LimitePixModel
            {
                Cpf = "12345678900",
                Conta = "1234",
                Agencia = 5678,
                LimitePix = 1500
            };

            _mockRepository.Setup(repo => repo.Buscar(viewModel.Cpf)).ReturnsAsync((IEnumerable<LimitePixModel>)null);

            var result = await _controller.ConsultarLimite(viewModel.Cpf, viewModel.Agencia, viewModel.Conta, viewModel.LimitePix) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("ConsultarLimite", result.ViewName);
            Assert.Equal("CPF não encontrado.", result.ViewData["Message"]);
        }

        [Fact]
        public async Task ConsultarLimite_Post_LowerOrEqualLimit_ReturnsViewWithErrorMessage()
        {
            var viewModel = new LimitePixModel
            {
                Cpf = "12345678900",
                Conta = "1234",
                Agencia = 5678,
                LimitePix = 800
            };

            var existingCliente = new LimitePixModel
            {
                Cpf = viewModel.Cpf,
                Conta = viewModel.Conta,
                Agencia = viewModel.Agencia,
                LimitePix = 1000
            };

            _mockRepository.Setup(repo => repo.Buscar(viewModel.Cpf)).ReturnsAsync(new List<LimitePixModel> { existingCliente });

            var result = await _controller.ConsultarLimite(viewModel.Cpf, viewModel.Agencia, viewModel.Conta, viewModel.LimitePix) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("ConsultarLimite", result.ViewName);
            Assert.Equal($"O LimitePix informado ({viewModel.LimitePix}) é menor ou igual ao limite já registrado ({existingCliente.LimitePix}).", result.ViewData["Message"]);
        }
    }
}
