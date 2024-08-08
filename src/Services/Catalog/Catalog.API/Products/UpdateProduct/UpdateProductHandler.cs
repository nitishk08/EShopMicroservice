
using Catalog.API.Products.CreateProduct;
using MediatR;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .Length(2,150).WithMessage("Name must be between ");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IDocumentSession _documentSession;
        public UpdateProductCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _documentSession.LoadAsync<Product>(command.Id, cancellationToken);
                if (product is null)
                { 
                    throw new ProductNotFoundException(command.Id);
                }

                product.Name = command.Name;
                product.Category = command.Category;
                product.Description = command.Description;
                product.ImageFile = command.ImageFile;
                product.Price = command.Price;

                _documentSession.Store(product);
                await _documentSession.SaveChangesAsync(cancellationToken);

                return new UpdateProductResult(true);
            }
            catch (ProductNotFoundException)
            {
                return new UpdateProductResult(false);
            }
            catch (System.Exception)
            {
                return new UpdateProductResult(false);
            }
        }
    }
}
