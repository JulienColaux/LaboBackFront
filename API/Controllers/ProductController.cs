using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Entities;
using BLL.DTO_s;

namespace API.Controllers
{
    [Route("api/[controller]")] //On definit l'URL à laquelle ce controleur répondra
    [ApiController] //Ca indique qui'il s'agit d'un controleur API


    public class ProductController : Controller
    {

    private readonly ProductService _productService; // On creer un objet pour stocker la dependance au sevice de la BLL

        public ProductController(ProductService productService) //On inject le service de la BLL via le constructeur
        {
            _productService = productService;
        }



        //-------------------------------------------------------------------------



        [HttpGet("all")]
        public ActionResult<List<ProductDTOs>> GetAllProducts()
        {
            var products = _productService.GetAllProductsDTO();
            return Ok(products); // Retourne les DTOs mappés au frontend
        }

        //-----------------------------------------------------------------------------

        [HttpGet("sortByCategory")]
        public ActionResult<List<ProductDTOs>> GetProductSortByCategory(string category)
        {
            var productsSortByCategory = _productService.GetProductDTOSortByCategory(category);
            return Ok(productsSortByCategory);
        }

    }
}
