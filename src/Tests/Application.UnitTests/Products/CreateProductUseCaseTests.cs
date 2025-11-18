using TemplateCleanArchi.Application.DTOs;
using TemplateCleanArchi.Application.UseCases.product;
using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;
using Moq;

namespace Application.UnitTests.Products
{
    [TestFixture]
    public class CreateProductUseCaseTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private CreateProductUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _useCase = new CreateProductUseCase(_productRepositoryMock.Object);
        }

        [Test]
        public async Task Execute_ShouldAddProductAndReturnId()
        {
            // Arrange
            var request = new CreateProductRequest("Test Product", 100m);
            var productId = Guid.NewGuid();

            _productRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(product => product.Id = productId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.Execute(request);

            // Assert
            Assert.That(result, Is.EqualTo(productId));
            _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
