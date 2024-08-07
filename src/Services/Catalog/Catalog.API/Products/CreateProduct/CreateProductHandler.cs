
namespace Catalog.API.Products.CreateProduct
{
    /// <summary>
    /// Create Record
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Category"></param>
    /// <param name="Description"></param>
    /// <param name="ImageUrl"></param>
    /// <param name="Price"></param>
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageUrl, decimal Price)
        : ICommand<CreateProductResult>;

    /// <summary>
    /// Return Response
    /// </summary>
    /// <param name="Id"></param>
    public record CreateProductResult(Guid Id);
    public class CreateProductHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //Create Product entity from command object

            // Save to database
            // return CreateProductResult result

            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageUrl = command.ImageUrl,
                Price = command.Price,
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}
