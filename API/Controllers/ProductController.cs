using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using DAL.Entities;

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



        [HttpGet("all")] //Ca indique que cette méthode sera appelé avec une requete HTTP GET
        public ActionResult<List<Product>> GetAllProducts() //Le type ActionResult permet de gerer plusieur scenario de reponse. ( OK(les données), NotFound et BadRequest et bien d autre)
        {
            var products = _productService.GetAllProductsDTO();
            return Ok(products); // On renvoi les données recupéré via la methode de la BLL sous format json
        }

        //-----------------------------------------------------------------------------

        [HttpGet("sortByCategory")]
        public ActionResult<List<Product>>GetProductSortByCategory(string category)
        {
            var productsSortByCategory = _productService.GetProductDTOSortByCategory(category);
            return Ok(productsSortByCategory);
        }

    }
}
