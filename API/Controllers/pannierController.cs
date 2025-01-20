using BLL.DTO_s;
using BLL.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class pannierController : Controller
    {
        private readonly PannierService _pannierService;

        public pannierController(PannierService service)
        {
            _pannierService = service;
        }


        //--------------------------------------------------------------------------------------------------

        [HttpGet("{userId}")]
        public IActionResult GetCart(int userId)
        {
            try
            {
                var cart = _pannierService.GetCart(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{userId}/add")]
        public IActionResult AddToCart(int userId, [FromBody] productDTOpannier item)
        {
            try
            {
                Console.WriteLine($"Requête reçue : UserId={userId}, ProductId={item.ProductId}, Quantity={item.Quantity}");

                if (item.Quantity <= 0)
                    throw new ArgumentException("La quantité doit être supérieure à zéro.");

                _pannierService.AddToCart(userId, item.ProductId, item.Quantity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erreur de validation : {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur interne : {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("{userId}/remove")]
        public IActionResult RemoveFromCart(int userId, [FromBody] int productId)
        {
            try
            {
                _pannierService.RemoveFromCart(userId, productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
