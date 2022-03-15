using Data_Layer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Helpers.LayoutService
{
    public class LayoutService
    {
        private readonly DataContext _context;

        public LayoutService(DataContext context)
        {
            _context = context;
        }

        public Dictionary<string,string> GetSettings()
        {
            return _context.Settings.ToDictionary(x=>x.Key, x=>x.Value);
        }
    }
}
