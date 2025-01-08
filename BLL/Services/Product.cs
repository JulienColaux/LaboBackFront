using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mapping;
using DAL.Entities;
using DAL.Repositories;
using BLL.DTO_s;

namespace BLL.Services
{

    public class ProductService
    {
        private readonly ProductRepository _productRepository; // On creer un objet de type ProductRepository

        public ProductService(ProductRepository productRepository)// On inject la dependance au repository via le constructeur. Ca permetra d'acceder a la DAL pour recuperer les données
        {
            _productRepository = productRepository;
        }



        //-------------------------------------------------------------------------------



        public List<ProductDTOs> GetAllProductsDTO() //On recupère simplement les données via la méthode de la DAL. Ici on ne cherche pas a la modifier.
        {
            var productsDTO = _productRepository.GetAllProducts();//On choppe tout les produit 
            return ProductMapper.ToDTOList(productsDTO); //On les fait passé par notre mapper puis on les return
        }


        //------------------------------------------------------------------------------


        public List<ProductDTOs> GetProductSortByCategory(string category)
        {
            var productsDTO = _productRepository.GetProductsByCategory(category);
            return ProductMapper.ToDTOList(productsDTO);
        }
    }

}
