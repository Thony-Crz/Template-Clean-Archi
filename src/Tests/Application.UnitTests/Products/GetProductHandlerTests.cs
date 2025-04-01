using Core.Application.Exceptions;
using Core.Application.UseCases.product;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Moq;

namespace Application.UnitTests.Products
{
    [TestFixture]
    public class GetProductHandlerTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private GetProductHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetProductHandler(_productRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Test Product", 100m) { Id = productId };

            _productRepositoryMock?
                .Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync((Product?)product);

            // Act
            var result = await _handler.Handle(productId);

            // Assert
            Assert.That(result, Is.EqualTo(product));
            _productRepositoryMock.Verify(repo => repo.GetAsync(productId), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock
                .Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(productId));
            _productRepositoryMock.Verify(repo => repo.GetAsync(productId), Times.Once);
        }
    }
}
