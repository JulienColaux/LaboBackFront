using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories
{
    public class PannierRepository
    {
        private readonly string _connectionString;

        public PannierRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //-----------------------------------------------------------------------------------------

        public DataTable GetCartByUserId(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Ajout de la requête validée
                var command = new SqlCommand(
                    "SELECT c.CartId, ci.ProductId AS Id, ci.Quantity, p.Name, p.Price " +
                    "FROM Cart c " +
                    "JOIN CartItem ci ON c.CartId = ci.CartId " +
                    "JOIN Product p ON ci.ProductId = p.Id " +
                    "WHERE c.UserId = @UserId",
                    connection
                );

                // Ajout du paramètre @UserId
                command.Parameters.AddWithValue("@UserId", userId);

                // Exécution de la requête
                var adapter = new SqlDataAdapter(command);
                var cartTable = new DataTable();
                adapter.Fill(cartTable);
                return cartTable;
            }
        }



        public int CreateCart(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Cart (UserId) OUTPUT INSERTED.CartId VALUES (@UserId)", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                return (int)command.ExecuteScalar();
            }
        }

        public void AddCartItem(int cartId, int productId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Vérifiez si le CartId existe
                var checkCartCommand = new SqlCommand("SELECT COUNT(1) FROM Cart WHERE CartId = @CartId", connection);
                checkCartCommand.Parameters.AddWithValue("@CartId", cartId);
                if ((int)checkCartCommand.ExecuteScalar() == 0)
                {
                    throw new InvalidOperationException($"CartId {cartId} does not exist.");
                }

                // Vérifiez si le ProductId existe
                var checkProductCommand = new SqlCommand("SELECT COUNT(1) FROM Product WHERE Id = @ProductId", connection); // Remplacement ici
                checkProductCommand.Parameters.AddWithValue("@ProductId", productId);
                if ((int)checkProductCommand.ExecuteScalar() == 0)
                {
                    throw new InvalidOperationException($"ProductId {productId} does not exist.");
                }

                // Insérer l'élément dans le panier
                var command = new SqlCommand("INSERT INTO CartItem (CartId, ProductId, Quantity) VALUES (@CartId, @ProductId, @Quantity)", connection);
                command.Parameters.AddWithValue("@CartId", cartId);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateCartItem(int cartId, int productId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE CartItem SET Quantity = Quantity + @Quantity WHERE CartId = @CartId AND ProductId = @ProductId", connection);
                command.Parameters.AddWithValue("@CartId", cartId);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.ExecuteNonQuery();
            }
        }

        public void RemoveCartItem(int cartId, int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM CartItem WHERE CartId = @CartId AND ProductId = @ProductId", connection);
                command.Parameters.AddWithValue("@CartId", cartId);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.ExecuteNonQuery();
            }
        }
    }
}
