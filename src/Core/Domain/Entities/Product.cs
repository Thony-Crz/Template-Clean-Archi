namespace Core.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; private set; } = null!;
        public decimal Price { get; private set; }

        private Product() { }
        public Product(string name, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }
    }
}
