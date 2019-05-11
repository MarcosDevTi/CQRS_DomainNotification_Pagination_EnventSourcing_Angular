﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Arch.Cqrs.Client.Command.Product
{
    public abstract class ProductCommand : Infra.Shared.Cqrs.Command.Command
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The Name is Required")]
        [MinLength(2)]
        [MaxLength(40)]
        [DisplayName("First Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Description is Required")]
        [MinLength(2)]
        [MaxLength(250)]
        [DisplayName("Description")]
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
