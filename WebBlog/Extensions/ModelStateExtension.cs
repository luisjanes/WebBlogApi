using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace WebBlog.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelStateExtension)
        {
            var result = new List<string>();
            foreach (var item in modelStateExtension.Values)
                result.AddRange(from error in item.Errors select error.ErrorMessage);

            return result;
        }
    }
}
