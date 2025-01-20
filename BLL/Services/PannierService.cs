using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PannierService
    {
        private readonly PannierRepository _pannierRepository;

        public PannierService(PannierRepository repository)
        {
            _pannierRepository = repository;
        }

        public DataTable GetCart(int userId)
        {
            return _pannierRepository.GetCartByUserId(userId);
        }

        public void AddToCart(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La quantité doit être supérieure à zéro.");
            }

            var cart = _pannierRepository.GetCartByUserId(userId);
            if (cart.Rows.Count == 0)
            {
                var cartId = _pannierRepository.CreateCart(userId);
                _pannierRepository.AddCartItem(cartId, productId, quantity);
            }
            else
            {
                var cartId = (int)cart.Rows[0]["CartId"];
                var existingItem = cart.Select($"ProductId = {productId}");
                if (existingItem.Length > 0)
                {
                    _pannierRepository.UpdateCartItem(cartId, productId, quantity);
                }
                else
                {
                    _pannierRepository.AddCartItem(cartId, productId, quantity);
                }
            }
        }

        public void RemoveFromCart(int userId, int productId)
        {
            var cart = _pannierRepository.GetCartByUserId(userId);
            if (cart.Rows.Count > 0)
            {
                var cartId = (int)cart.Rows[0]["CartId"];
                _pannierRepository.RemoveCartItem(cartId, productId);
            }
        }

    }
}
