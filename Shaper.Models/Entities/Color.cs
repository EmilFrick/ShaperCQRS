using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shaper.Models.Models.ColorModels;
using Shaper.Utility.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Shaper.Models.Entities
{
    public class Color
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [HexValidation]
        public string Hex { get; set; }
        [Required]
        [Range(1, 100)]
        [Column(TypeName = "money")]
        public double AddedValue { get; set; }
        //Navigation
        [ValidateNever]
        public ICollection<Product> Products { get; set; }


        public Color()
        {

        }

        public Color(ColorCreateModel model)
        {
            Name = model.Name;
            Hex = model.Hex;
            AddedValue = model.AddedValue;
        }

    }
}
