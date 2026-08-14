using Marten.Schema;

namespace Catalog.Api.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync()) return;

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
        {
            new Product()
            {
                Id= Guid.NewGuid(),
                Name = "IPhone X",
                Category = ["category1"],
                Description = "Long Description",
                ImageFile = "ImageFile",
                Price = 500
            },
             new Product()
            {
                Id= Guid.NewGuid(),
                Name = "Samsung 10",
                Category = ["category1"],
                Description = "Long Description",
                ImageFile = "ImageFile",
                Price = 300
            },
             new Product()
            {
                Id= Guid.NewGuid(),
                Name = "Huawei plus",
                Category = ["category2"],
                Description = "Long Description",
                ImageFile = "ImageFile",
                Price = 700
            },
             new Product()
            {
                Id= Guid.NewGuid(),
                Name = "Xiaomi Mi",
                Category = ["category2"],
                Description = "Long Description",
                ImageFile = "ImageFile",
                Price = 900
            }
        };
    }
}
