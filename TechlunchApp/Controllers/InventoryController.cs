﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechlunchApp.Common;
using TechlunchApp.ViewModels;

namespace TechlunchApp.Controllers
{
    public class InventoryController : Controller
    {
        private static List<IngredientViewModel> Ingredients = new List<IngredientViewModel>();

        public async Task<IActionResult> Index()
        {
            List<GeneralInventoryViewModel> inventoryList = new List<GeneralInventoryViewModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Constants.ApiUrl}generalinventories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    inventoryList = JsonConvert.DeserializeObject<List<GeneralInventoryViewModel>>(apiResponse);
                }
            }
            return View(inventoryList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using (var httpClient = new HttpClient())
            {
                using (var IngredientsResponse = await httpClient.GetAsync($"{Constants.ApiUrl}Ingredients"))
                {
                    string IngredientApiResponse = await IngredientsResponse.Content.ReadAsStringAsync();
                    Ingredients = JsonConvert.DeserializeObject<List<IngredientViewModel>>(IngredientApiResponse);

                }
            }

            ViewBag.Ingredients = Ingredients;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryViewModel inventoryObj)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(inventoryObj), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{Constants.ApiUrl}inventories", content);

                }
                return RedirectToAction("Index");
            }

            ViewBag.Ingredients = Ingredients;
            return View(inventoryObj);
        }

        [HttpGet]
        public async Task<ActionResult> IngredientHistory(int id)
        {
            List<IngredientHistoryViewModel> IngredientHistories = new List<IngredientHistoryViewModel>();
            IngredientViewModel ingredientObj;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Constants.ApiUrl}Inventories/Ingredient/{id}"))
                {
                    string content = await response.Content.ReadAsStringAsync();
                    IngredientHistories = JsonConvert.DeserializeObject<List<IngredientHistoryViewModel>>(content);
                }

                using (var response = await httpClient.GetAsync($"{Constants.ApiUrl}Ingredients/{id}"))
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ingredientObj = JsonConvert.DeserializeObject<IngredientViewModel>(content);
                }
            }

            ViewData["name"] = ingredientObj.Name;
            return View(IngredientHistories);
        }

    }
}
