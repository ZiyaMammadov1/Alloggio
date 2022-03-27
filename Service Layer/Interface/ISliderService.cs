using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service_Layer.Interface
{
    public interface ISliderService
    {
        Slider Get(int id);
        List<Slider> GetAll();
        void Add(Slider slider);
        void Delete(int id);
        void Update(int id, Slider slider);
        void Commit();

    }
}
