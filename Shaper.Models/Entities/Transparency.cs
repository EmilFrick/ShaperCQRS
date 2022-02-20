using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shaper.Models.Models.TransparencyModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaper.Models.Entities
{
    public class Transparency
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(0,100)]
        public int Value { get; set; }
        [Required]
        [Range(1,100)]
        [Column(TypeName = "money")]
        public double AddedValue { get; set; }

        //Navigation
        [ValidateNever]
        public ICollection<Product> Products { get; set; }

        public Transparency()
        {

        }

        public Transparency(TransparencyCreateModel model)
        {
            Name = model.Name;
            if(model.Description != null)
                Description = model.Description;
            
            Value = model.Value;
            AddedValue = model.AddedValue;
        }
    }
}
