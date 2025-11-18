using TemplateCleanArchi.Application.Exceptions;
using TemplateCleanArchi.Application.UseCases.product;
using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;
using Moq;

namespace Application.UnitTests.Products
{
    [TestFixture]
    public class GetProductUseCaseTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private GetProductUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _useCase = new GetProductUseCase(_productRepositoryMock.Object);
        }

        [Test]
        public async Task Execute_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Test Product", 100m) { Id = productId };

            _productRepositoryMock?
                .Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync((Product?)product);

            // Act
            var result = await _useCase.Execute(productId);

            // Assert
            Assert.That(result, Is.EqualTo(product));
            _productRepositoryMock.Verify(repo => repo.GetAsync(productId), Times.Once);
        }

        [Test]
        public void Execute_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock
                .Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(productId));
            _productRepositoryMock.Verify(repo => repo.GetAsync(productId), Times.Once);
        }
    }
}
