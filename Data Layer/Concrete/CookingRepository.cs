using Core_Layer.Abstract;
using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Layer.Concrete
{
    public class CookingRepository : Repository<CookingMenus>, ICookingRepository
    {
        public CookingRepository(DataContext context) : base(context)
        {

        }
    }
}
