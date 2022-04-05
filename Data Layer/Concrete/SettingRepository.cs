using Core_Layer.Abstract;
using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data_Layer.Concrete
{
    public class SettingRepository : Repository<Setting>, ISettingRepository
    {
        private readonly DataContext _context;

        public SettingRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public string GetValue(string key)
        {
            Setting setting =  _context.Settings.FirstOrDefault(x => x.Key == key);

            return setting.Value;
        }
    }
}
