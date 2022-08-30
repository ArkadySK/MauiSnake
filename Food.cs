namespace MauiDemo
{
    public class Food
    {
        public string Name { get; }
        //public string Description { get; }
        public int Value { get; }
        public Point Position { get; set; }

        public ImageSource ImageSource { get; }


        public Food(string name, int value)
        {
            Name = name;
            Value = value;
            ImageSource = ImageSource.FromFile(name + ".png");
        }

        public Food Clone()
        {
            return this.MemberwiseClone() as Food;
        }
    }
}