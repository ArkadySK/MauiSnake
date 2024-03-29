﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MauiDemo.Snake;

namespace MauiDemo
{
    public class SnakeGame
    {
        public event GameOverEventHandler GameOverEvent;
        bool IsInfiniteGame { get; set; } = false;
        public Snake Snake { get; private set; }
        public Size AreaSize = new(10, 10);

        public List<Food> SpawnedFood { get; private set; } = new List<Food>();
        public IDispatcherTimer Timer { get; private set; } = Application.Current.Dispatcher.CreateTimer();
        public IDispatcherTimer FoodSpawnTimer { get; private set; } = Application.Current.Dispatcher.CreateTimer();

        public int Score
        {
            get
            {
                int resultScore = Snake.FoodEaten - Snake.Penalty;
                if (resultScore >= 0)
                    return resultScore;
                else

                    return 0;
            }
        }


        public SnakeGame(int snakeSize, int? goalSize)
        {
            Snake = new Snake(snakeSize, goalSize);
            if (goalSize is null)
                IsInfiniteGame = true;
            Timer.Start();
            Timer.Interval = TimeSpan.FromSeconds(0.3);

            FoodSpawnTimer.Start();
            FoodSpawnTimer.Interval = TimeSpan.FromSeconds(5);
            FoodSpawnTimer.Tick += FoodSpawnTimer_Tick;

            for(int i = 0; i < 5; i++)
            {
                SpawnBadFood();
            }
        }

        private void FoodSpawnTimer_Tick(object sender, EventArgs e)
        {
            var rand = new Random();
            int randomNb = rand.Next(1, 5);
            if (randomNb == 2)
                SpawnGoodFood();
            else
                SpawnBadFood();

            FoodSpawnTimer.Interval = TimeSpan.FromSeconds(rand.Next(2, 6));
        }

        #region Pause and resume
        public void PauseOrResume()
        {
            if (Timer.IsRunning)
            {
                this.Pause();
            }
            else
            {
                this.Resume();
            }
        }

        public void Pause()
        {
            Timer.Stop();
            FoodSpawnTimer.Stop();
        }

        public void Resume()
        {
            Timer.Start();
            FoodSpawnTimer.Start();
        }
        #endregion

        #region Movement
        public bool CanMove(Direction direction)
        { 
            Point HeadPosition = Snake.BodyPositions.First();
            switch (direction)
            {
                case Direction.Up:
                    if (HeadPosition.Y <= 0)
                        return false;
                    break;
                case Direction.Down:
                    if (HeadPosition.Y >= AreaSize.Height - 1)
                        return false;
                    break;
                case Direction.Left:
                    if (HeadPosition.X <= 0) 
                        return false;
                    break;
                case Direction.Right:
                    if (HeadPosition.X >= AreaSize.Width - 1)
                        return false;
                    break;
            }

            return true; 
        }

        public void MoveSnake()
        {
            
            if(!IsInfiniteGame && Snake.DisplaySize >= Snake.GoalSize)
            {
                GameOverEvent?.Invoke(this, new GameOverEventArgs() 
                { 
                    Score = Score, 
                    IsFinished = true,
                    MessageText = $"You have successfully completed this level! \n\nYour Score: {Score}" 
                }
                );
                return;
            }

            if (Snake.BodyPositions.Count <= 0 || Snake.DisplaySize <= 0)
            {
                GameOverEvent?.Invoke(this, new GameOverEventArgs()
                {
                    Score = Score,
                    IsFinished = false,
                    MessageText = $"You were destroyed! \n\nScore: {Score}"
                }
                );
                return;
            }
            Point headPosition = Snake.BodyPositions.First();

            if (!CanMove(Snake.MoveDirection))
            {
                bool isFinished = Snake.DisplaySize >= Snake.GoalSize;
                GameOverEvent?.Invoke(this, new GameOverEventArgs() { Score = Score, IsFinished = isFinished });
            }

            Point? newPos = null;
            switch (Snake.MoveDirection)
            {
                case Direction.Up:
                    newPos = new Point(headPosition.X, headPosition.Y - 1);
                    break;
                case Direction.Down:
                    newPos = new Point(headPosition.X, headPosition.Y + 1);
                    break;
                case Direction.Left:
                    newPos = new Point(headPosition.X - 1, headPosition.Y);
                    break;
                case Direction.Right:
                    newPos = new Point(headPosition.X + 1, headPosition.Y);
                    break;
            }

            // If the snake is eating itself
            if(Snake.BodyPositions.Contains(newPos.Value))
            {
                GameOverEvent.Invoke(this, new GameOverEventArgs() 
                { 
                    Score = Score, 
                    IsFinished = false , 
                    MessageText = $"You ate yourself!\n\nScore: {Score}"
                }
                );
                return;
            }

            Snake.BodyPositions.Insert(0, newPos.Value);

            // Make snake shorter if needed
            while (Snake.DisplaySize < Snake.BodyPositions.Count)
            {
                Point lastPiecePosition = Snake.BodyPositions.Last();
                Snake.BodyPositions.Remove(lastPiecePosition);
            }

            Point newHeadPosition = Snake.BodyPositions.First();
            Food foodToRemove = null;
            foreach(var f in SpawnedFood)
            {
                if (f.Position == newHeadPosition)
                {
                    Snake.FoodEaten += f.Value;
                    foodToRemove = f;
                }
            }

            if (foodToRemove is not null)
                SpawnedFood.Remove(foodToRemove);

        }
        #endregion

        #region Spawn Foods
        public void SpawnGoodFood()
        {
            int count = FoodLibrary.GoodFoods.Count();
            Random rand = new Random();
            int i = rand.Next(0, count);

            var foodToSpawn = FoodLibrary.GoodFoods[i].Clone();
            SetFoodPositionRandomly(foodToSpawn);
            SpawnedFood.Add(foodToSpawn);
        }

        public void SpawnBadFood()
        {
            if (Snake.BodyPositions.Count <= 0) return;

            int count = FoodLibrary.BadFoods.Count();
            Random rand = new Random();
            int i = rand.Next(0, count);

            var foodToSpawn = FoodLibrary.BadFoods[i].Clone();
            SetFoodPositionRandomly(foodToSpawn);
            SpawnedFood.Add(foodToSpawn);
        }

        private void SetFoodPositionRandomly(Food food)
        {
            Random rand = new Random();
            Point suitablePosition;
            while (true)
            {
                suitablePosition = new Point(rand.Next(0, (int)AreaSize.Width), rand.Next(0, (int)AreaSize.Height));

                // ak sa neprekryva s hadom
                if (Snake.BodyPositions.Contains(suitablePosition))
                    continue;

                // ak uz na danom mieste nie je dalsie jedlo
                foreach (var f in SpawnedFood)
                {
                    if (f?.Position == suitablePosition) continue;
                }

                // Dont spawn too close to head
                var snakeHead = Snake.BodyPositions.First();
                if (Math.Abs(snakeHead.X - suitablePosition.X) < 3 || Math.Abs(snakeHead.Y - suitablePosition.Y) < 3)
                    continue;
                break;
            }
            food.Position = suitablePosition;
        }
        #endregion
    }
}
