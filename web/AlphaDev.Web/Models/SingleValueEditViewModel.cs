﻿using System.ComponentModel.DataAnnotations;

namespace AlphaDev.Web.Models
{
    public abstract class SingleValueEditViewModel
    {
        protected SingleValueEditViewModel(string value)
        {
            Value = value;
        }

        [Required]
        public string Value { get; set; }
    }
}