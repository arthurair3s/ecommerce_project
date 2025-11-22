using ecommerce_crud.DTO;
using ecommerce_crud.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ecommerce_crud.Controllers
{
    public class JsonPatchDocument<T>
    {
        internal void ApplyTo(Product productToUpdate, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }

        internal void ApplyTo(ProductPatchDto dto, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }
    }
}