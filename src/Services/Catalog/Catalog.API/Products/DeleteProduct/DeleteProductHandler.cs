
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    public class DeleteProductCommandHandler
        (IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("DeleteProductCommandHandler.Handler called with {@Command}", command);
                session.Delete<Product>(command.Id);
                await session.SaveChangesAsync(cancellationToken);

                return new DeleteProductResult(true);
            }
            catch(System.Exception ex)
            {
                logger.LogError(ex, "Product is not deleted");
                return new DeleteProductResult(false);
            }
        }
    }
}
