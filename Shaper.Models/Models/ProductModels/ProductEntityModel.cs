using Newtonsoft.Json;
using Shaper.Models.Entities;
using Shaper.Models.Models.ColorModels;
using Shaper.Models.Models.ShapeModels;
using Shaper.Models.Models.TransparencyModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Shaper.Models.Entities.Color;

namespace Shaper.Models.Models.ProductModels
{
    public class ProductEntityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Artist { get; set; }
        public DateTime Created { get; set; }
        public int ShapeId { get; set; }
        public Shape Shape { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int TransparencyId { get; set; }
        public Transparency Transparency { get; set; }

        public ProductEntityModel()
        {

        }

        public ProductEntityModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Artist = product.Artist;
            Created = product.Created;
            ColorId = product.ColorId;
            Color = product.Color;
            ShapeId = product.ShapeId;
            Shape = product.Shape;
            TransparencyId = product.TransparencyId;
            Transparency = product.Transparency;
        }
    }
}
