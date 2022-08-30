using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDemo
{
    public static class FoodLibrary
    {
        public static Food[] GoodFoods { get; } = new Food[]
            {
                new Food("Apple", 1),
                new Food("Banana", 1) 
            };
        public static Food[] BadFoods { get; } = new Food[]
            {
                new Food("Bomb", -20),
                new Food("Brick", -3),
                new Food("Junk", -1) 
            };
    }
}
