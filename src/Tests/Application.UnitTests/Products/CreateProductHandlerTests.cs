using TemplateCleanArchi.Application.UseCases;
using TemplateCleanArchi.Application.UseCases.product;
using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;
using Moq;

namespace Application.UnitTests.Products
{
    [TestFixture]
    public class CreateProductHandlerTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private CreateProductHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new CreateProductHandler(_productRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldAddProductAndReturnId()
        {
            // Arrange
            var productName = "Test Product";
            var productPrice = 100m;
            var productId = Guid.NewGuid();

            _productRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(product => product.Id = productId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(productName, productPrice);

            // Assert
            Assert.That(result, Is.EqualTo(productId));
            _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
