using Core_Layer.Abstract;
using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Layer.Concrete
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        public SliderRepository(DataContext context) : base(context)
        {

        }
    }
}
