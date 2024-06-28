﻿using ShopOnline.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBase.Contract.Entities
{
    public class User : EntityBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Address { get; set; }
    }
}
