using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiDemo
{
    public class Snake : INotifyPropertyChanged
    {
        private int foodEaten = 0;
        private int penalty = 0;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        internal void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Direction MoveDirection { get; private set; } = Direction.Down;
        public int InitialSize { get; private set; } = 3;
        public int? GoalSize { get; private set; } = 10;

        // Prva v arrayi je hlava, potom ide telo
        public List<Point> BodyPositions { get; private set; } = new List<Point>();
        public int DisplaySize
        {
            get
            {
                return InitialSize + FoodEaten - Penalty;
            }
        }

        public int FoodEaten 
        { 
            get => foodEaten; set
            {
                foodEaten = value;
                NotifyPropertyChanged("DisplaySize");
            }
        }
        public int Penalty 
        { 
            get => penalty; 
            internal set
            {
                penalty = value;
                NotifyPropertyChanged("DisplaySize");
            }
        }
        public Snake(int size, int? goalSize)
        {
            this.InitialSize = size;
            for (int i = 0; i < size; i++)
                BodyPositions.Add(new Point(5 + i, 5 + 0));
            GoalSize = goalSize;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ChangeDirection(Direction dir)
        {
            if (dir == Direction.Up && MoveDirection == Direction.Down)
                return;
            else if (dir == Direction.Down && MoveDirection == Direction.Up)
                return;
            else if (dir == Direction.Left && MoveDirection == Direction.Right)
                return;
            else if (dir == Direction.Right && MoveDirection == Direction.Left)
                return;

            MoveDirection = dir;

        }
    }
}
