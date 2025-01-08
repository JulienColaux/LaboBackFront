using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO_s;
using DAL.Entities;

namespace BLL.Mapping
{
    public static class ProductMapper
    {
        public static ProductDTOs ToDTO(Product product) //Static pour quelle ne necessite pas d instanciation, la méthode prend en entré une entité de la DAL et en sortie un DTO de la BLL
        {
            return new ProductDTOs
            {
                Name = product.name,
                Description = product.description,
                Price = product.price,
                Category = product.category
            };
        }

        public static List<ProductDTOs> ToDTOList(List<Product> products)//Meme idée mais pour une liste de product
        {
            return products.Select(ToDTO).ToList();// .Select applique ToDTO a chaque element de la list et .ToList convertit le resultat en une liste
        }
    }
}
