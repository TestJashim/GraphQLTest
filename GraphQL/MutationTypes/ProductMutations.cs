using GraphQLTest.Entities;
using GraphQLTest.Repositories;

namespace GraphQLTest.GraphQL.MutationTypes
{
    public class ProductMutations
    {
        public async Task<bool> AddProductAsync([Service] IProductService productService,
    ProductDetails productDetails)
        {
            return await productService.AddProductAsync(productDetails);
        }

        public async Task<bool> UpdateProductAsync([Service] IProductService productService,
    ProductDetails productDetails)
        {
            return await productService.UpdateProductAsync(productDetails);
        }

        public async Task<bool> DeleteProductAsync([Service] IProductService productService,
   Guid productId)
        {
            return await productService.DeleteProductAsync(productId);
        }
    }
}
