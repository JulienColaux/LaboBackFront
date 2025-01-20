using DAL.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)//check mieu
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //--------------------------------------------------------------------------------

        public void AjouterUtilisateur(User utilisateur)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (Nom, Email, MotDePasseHash) VALUES (@Nom, @Email, @MotDePasseHash)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nom", utilisateur.Nom);//AddWithValue lie les valeurs des propriétés de l'objet utilisateur aux paramètres de la requête.
                command.Parameters.AddWithValue("@Email", utilisateur.Email);
                command.Parameters.AddWithValue("@MotDePasseHash", utilisateur.MotDePasseHash);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //-----------------------------------------------------------------------------------

        public User? ObtenirParEmail(string email) // ? = soit user soit null
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE Email = @Email";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email); //associe la valeur de l'argument email au paramètre @Email

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = (int)reader["Id"],
                            Nom = reader["Nom"].ToString(),
                            Email = reader["Email"].ToString(),
                            MotDePasseHash = reader["MotDePasseHash"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}

