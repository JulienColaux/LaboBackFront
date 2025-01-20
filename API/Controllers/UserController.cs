using BLL.DTO_s;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user")] //route de base
    public class UserController : Controller
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        //-------------------------------------------------------------------------------------------

        [HttpPost("inscription")]
        public IActionResult InscrireUtilisateur([FromBody] UserDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    Console.WriteLine("DTO est null");
                    return BadRequest(new { message = "Les données envoyées sont invalides." });
                }

                Console.WriteLine($"Données reçues : Nom={dto.Nom}, Email={dto.Email}, MotDePasse={dto.MotDePasse}");

                _service.InscrireUtilisateur(dto.Nom, dto.Email, dto.MotDePasse);

                // Retourner un objet JSON
                return Ok(new { message = "Utilisateur inscrit avec succès" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'inscription : {ex.Message}");
                return BadRequest(new { message = "Une erreur est survenue lors de l'inscription.", details = ex.Message });
            }
        }

        //------------------------------------------------------------------------------------------------

        [HttpPost("connexion")]
        public IActionResult ConnecterUtilisateur([FromBody] LoginDTO dto)
        {
            Console.WriteLine($"Email: {dto?.Email}, MotDePasse: {dto?.MotDePasse}");
            try
            {
                var utilisateur = _service.ConnecterUtilisateur(dto.Email, dto.MotDePasse);

                // Retourner l'objet utilisateur au format JSON
                return Ok(new { message = "Connexion réussie", utilisateur });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion : {ex.Message}");

                // Retourner une réponse JSON en cas d'erreur
                return Unauthorized(new { message = "Connexion échouée", details = ex.Message });
            }
        }
    }
}
