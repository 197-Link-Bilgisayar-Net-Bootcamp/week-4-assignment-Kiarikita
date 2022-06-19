using Cache.API.Models;

namespace Cache.API.Services
{
    public interface ICategoryService
    {
        List<CategoryModel> GetAllCategory();
    }
}
