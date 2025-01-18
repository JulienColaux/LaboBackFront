using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using System.Data.SqlClient;                         //Utilisé pour interagir avec la base de données SQL Server.
using System.Data;                                        //Contient des objets nécessaires pour les interactions avec les bases de données (par exemple, SqlCommand, SqlConnection, SqlDataReader).
using Microsoft.Extensions.Configuration;     //Permet de lire la configuration de l'application, comme la chaîne de connexion à la base de données.
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;


// La DAL(Data Access Layer) est responsable de l'interaction directe avec la base de données. Elle encapsule les requêtes SQL, l'ouverture des connexions,
// et la gestion des résultats sous forme d'objets de votre application. Elle permet à la logique métier (comme la BLL - Business Logic Layer) de ne pas être directement
// liée aux détails de la gestion de la base de données. Cela simplifie la maintenance et la lisibilité du code tout en suivant une architecture bien structurée.



namespace DAL.Repositories
{
    public  class ProductRepository
    {
        private readonly string _connectionString; // Propriété qui est utilisé pour se connecter a la DB

        public ProductRepository(IConfiguration configuration)//check mieu
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }




        //--------------------------------------------------------------------------




        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();// Creation d'une liste vide qui acceuillera tout les products de la DB

            using (var connection = new SqlConnection(_connectionString))// Using permet de creer un objet (SqlConnection) qui sera libéré a la fin du using, ca économise des ressources
            {
                connection.Open(); // La méthode Open() sert a ouvrir la connection avec la DB, verifie les parametres de conections (nom server, nom DB...) et si tout est ok, autorise l appli à effectuer des requette SQL

                using (var command = new SqlCommand("SELECT * FROM Product", connection))// On creer un objet SqlCommand qui prend 2 parametre, la requete SQL a effectuer dans la DB et une connection active lié a la DB
                {
                    using (var reader = command.ExecuteReader()) // On va utiliser ExecuteReader() pour renvoyer un objet de type SqlDataReader (ici le type c'est var c'est normal). l'objet SqlDataReader contient une lecture des donnée de la DB ligne par ligne.
                    {
                        while (reader.Read())// Read lit une ligne puit passe a la suivante, si pas de ligne apres, ca renvoie false
                        {
                            products.Add(new Product // A chaque ligne de donné on creer un nouvelle objet de type product et on l ajoute dans notre liste products creer au debut
                            {
                                id = (int)reader["Id"],
                                name = reader["Name"].ToString(),
                                description = reader["Description"].ToString(),
                                price = (int)reader["Price"],
                                stock = (int)reader["Stock"],
                                category = reader["Category"].ToString()
                            });
                        }
                    }
                }
            }

            return products; // La méthode retourn la liste products
        }


        //---------------------------------------------------------------------------------


        public List<Product>GetProductsByCategory(string category)
        {
            var productsSortByCategory = new List<Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM Product WHERE Category = @category ", connection))
                {
                    command.Parameters.AddWithValue("@category", category); //parameters cest un propriété qui contient une liste de parametre qui seront utilisé dans la commande. addwithvalue cest une méthode de la propriété parameters qui permet d ajouter un nvx parametre. en gros ca permet d associer la valeur de category a @category

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productsSortByCategory.Add(new Product
                        {
                            id = (int)reader["Id"],
                            name = reader["Name"].ToString(),
                            description = reader["Description"].ToString(),
                            price = (int)reader["Price"],
                            stock = (int)reader["Stock"],
                            category = reader["Category"].ToString()
                        });
                        }
                    }
                }
            }
            return productsSortByCategory;
        }
    }
}
