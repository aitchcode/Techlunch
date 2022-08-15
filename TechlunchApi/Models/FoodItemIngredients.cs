﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TechlunchApi.Models
{
    public class FoodItemIngredients
    {
        public int Id { get; set; }

        [Required]
        public int FoodItemId { get; set; }


        [Required]
        public int IngredientId { get; set; }

        [ForeignKey("IngredientId")]
        public Ingredient Ingredient { get; set; }


        [ForeignKey("FoodItemId")]
        public FoodItem FoodItem { get; set; }

    }
}
