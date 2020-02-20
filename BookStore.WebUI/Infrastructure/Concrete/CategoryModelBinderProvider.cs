using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Infrastructure.Concrete
{
    public class CategoryModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (typeof(Category) != modelType)
                return null;
            return new CategoryModelBinder();
        }
    }

    public class CategoryModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Category categoory = null;

            //Сначала попробуем найти найти название категории из данных маршрута
            //ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ValueProviderResult value = bindingContext.ValueProvider.GetValue("category");
            ICategoryRepository repository = DependencyResolver.Current.GetService<ICategoryRepository>();

            if (value != null)
            {
                if (string.IsNullOrEmpty(value.AttemptedValue))
                    return null;

                string NameCategory = value.AttemptedValue;                
                categoory = repository.GetByName(NameCategory);
            }

            //Попробуем получить категорию из данных формы      
            else
            {
                ValueProviderResult valueName        = bindingContext.ValueProvider.GetValue("Name");
                ValueProviderResult valueDescription = bindingContext.ValueProvider.GetValue("Description");

                if (valueName == null)
                    return null;

                string Name        = valueName.AttemptedValue;
                string Description = valueDescription.AttemptedValue;

                categoory = repository.GetByName(Name);
                if (categoory == null)
                    categoory = new Category();

                categoory.Name = Name;
                categoory.Description = Description;
            }

            return categoory;


            //if (value == null)
            //    return null;
            //if (string.IsNullOrEmpty(value.AttemptedValue))
            //    return null;

            //string NameCategory = value.AttemptedValue;
            //ICategoryRepository repository = DependencyResolver.Current.GetService<ICategoryRepository>();
            //Category categoory = repository.GetByName(NameCategory);

            //return categoory;

        }
    }
}