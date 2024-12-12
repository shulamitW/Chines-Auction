using AutoMapper;
using Chines_auction_project.BLL;
using Chines_auction_project.Modells;
using Chines_auction_project.Modells.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Chines_auction_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController:ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<Category>> GetCategory() 
        {
            var c = await categoryService.GetCategory();
            return c == null ? NotFound() : Ok(c);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("AddCategory")]
        public async Task<ActionResult<Category>> AddCategory(CategoryDto category)
        {
            var c= mapper.Map<Category>(category);
            return Created($"http://localhost:3000/category/{c.Id}", await categoryService.AddCategory(c));
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("RemoveCategory")]
        public async Task<ActionResult<Category>> RemoveCategory(int id)
        {
            var c = await categoryService.RemoveCategory(id);
            return c == null ? NotFound() : Ok(c);
        }
        
        [Authorize(Roles = "Manager")]
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<Category>> UpdateCategory(CategoryDto category, int id)
        {
            var c = mapper.Map<Category>(category);
            return c == null ? NotFound() : Ok(await categoryService.UpdateCategory(c, id));
        }
    }
}
