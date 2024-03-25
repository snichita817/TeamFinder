using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;
using TeamFinder.Models.DTO;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            // from dto to domain
            var categoryDomain = new Category
            {
                Name = request.Name,
            };

            var createdCategory = await _categoryRepository.CreateAsync(categoryDomain);

            var response = new CategoryDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
            };

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var response = new List<CategoryDto>();

            foreach (var category in categories)
            {
                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                };

                response.Add(categoryDto);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);

            if(category is null)
                return NotFound();

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var categoryToDelete = await _categoryRepository.DeleteAsync(id);

            if(categoryToDelete is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = categoryToDelete.Id,
                Name = categoryToDelete.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, [FromBody] EditCateogryRequestDto request)
        {
            var category = new Category
            {
                Id = id,
                Name = request.Name,
            };

            var update = await _categoryRepository.EditCategory(category);

            if(update is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = update.Id,
                Name = update.Name,
            };

            return Ok(response);  

        }
    }
}
