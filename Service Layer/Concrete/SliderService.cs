using Core_Layer.Entities;
using Data_Layer.Concrete;
using Service_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service_Layer.Concrete
{
    public class SliderService : ISliderService
    {
        private readonly DataContext _context;

        public SliderService(DataContext context)
        {
            _context = context;
        }

        public void Add(Slider slider) 
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Slider Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Slider> GetAll()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            return sliders;
        }

        public void Update(int id, Slider slider)
        {
            throw new NotImplementedException();
        }
    }
}
