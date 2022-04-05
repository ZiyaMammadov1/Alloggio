using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Abstract
{
    public interface ISettingRepository : IRepository<Setting>
    {
        string GetValue(string key);

    }
}
