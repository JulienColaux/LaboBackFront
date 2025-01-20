using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCrypt.Net; // hachage et la vérification des mots de passe.
using DAL.Entities;
using BLL.DTO_s;


namespace BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        //-------------------------------------------------------------------------------------

        public void InscrireUtilisateur(string nom, string email, string motDePasse)
        {
            // Vérifier si l'utilisateur existe
            if (_repository.ObtenirParEmail(email) != null) 
                throw new Exception("Email déjà utilisé");

            // Hacher le mot de passe
            string motDePasseHash = BCrypt.Net.BCrypt.HashPassword(motDePasse);

            // On creer un User avec le mdp haché
            var utilisateur = new User
            {
                Nom = nom,
                Email = email,
                MotDePasseHash = motDePasseHash
            };

            _repository.AjouterUtilisateur(utilisateur); //ajout du User
        }

        //----------------------------------------------------------------------------------

        public UserDTO? ConnecterUtilisateur(string email, string motDePasse)
        {
            var utilisateur = _repository.ObtenirParEmail(email);
            if (utilisateur == null || !BCrypt.Net.BCrypt.Verify(motDePasse, utilisateur.MotDePasseHash))//si ya pas le mail ou que le mot de passe est faux
                throw new Exception("Email ou mot de passe incorrect");

            return new UserDTO
            {
                Id = utilisateur.Id,
                Nom = utilisateur.Nom,
                Email = utilisateur.Email
            };
        }
    }
}
