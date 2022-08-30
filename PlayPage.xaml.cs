using Microsoft.Maui.Graphics;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;

namespace MauiDemo;

[XamlCompilation(XamlCompilationOptions.Skip)]
public partial class PlayPage : ContentPage
{
	
	public MenuNavigator MenuNavigator { get; set; }
	PauseMenuPage PauseMenu = new PauseMenuPage();

    SnakeGame Game = new SnakeGame();

    List<Rectangle> SnakeRectangles = new List<Rectangle>();
    List<Image> FoodImages = new List<Image>();


    public PlayPage(MenuNavigator menuNavigator)
	{
		MenuNavigator = menuNavigator;
		InitializeComponent();
		GameUIGrid.BindingContext = Game;

        Game.Timer.Tick += Timer_Tick;
        Game.GameOverEvent += Snake_GameOverEvent;
		AddCoulmnsAndRows();

    }
    private async void Snake_GameOverEvent(object sender, GameOverEventArgs e)
    {
        Game.Pause();
        string descText = "\nCongratulations! \nYou passed this level!";
        if (!e.IsFinished)
            descText = "You failed!";
        await DisplayAlert(
            "Game over!",
            "Score: " + e.Score + Environment.NewLine + descText,
            "Go to main menu");
        MenuNavigator.GoToMainPage();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        Game.MoveSnake();
        DrawSnake();
    }

    #region Update and Draw to UI stuff
    private void AddCoulmnsAndRows()
	{
        for(int w = 0; w < Game.AreaSize.Width; w++)
		{
			SnakeGrid.AddColumnDefinition(new ColumnDefinition());
		}
        for (int h = 0; h < Game.AreaSize.Height; h++)
        {
            SnakeGrid.AddRowDefinition(new RowDefinition());
        }
    }

    byte col = 220;
    private void DrawSnake()
    {
        // Check amount of positons and create / remove rectangles
        while (SnakeRectangles.Count != Game.Snake.BodyPositions.Count)
        {
            if (SnakeRectangles.Count > Game.Snake.BodyPositions.Count)
            {
                SnakeGrid.Remove(SnakeRectangles.Last());
                SnakeRectangles.Remove(SnakeRectangles.Last());
            }
            else if (SnakeRectangles.Count < Game.Snake.BodyPositions.Count)
            {
                var rect = new Rectangle();
                if(SnakeRectangles.Count != 0)
                    rect.Fill = new SolidColorBrush(new Color(col, (byte)20, (byte)20));
                else // If it is the head of snake
                    rect.Fill = new SolidColorBrush(Colors.Red);
                SnakeRectangles.Add(rect);
                SnakeGrid.Children.Add(rect);
                col -= 15;
                
            }
        }

        if (SnakeRectangles.Count != Game.Snake.BodyPositions.Count)
            throw new Exception("UI is not synced");

        // Change positions of the rectangles
        int i = 0;
        foreach (Point pos in Game.Snake.BodyPositions) {
            if((int)pos.X < 0 || (int)pos.Y < 0) continue;
            if (i < SnakeRectangles.Count)
            {
                
                SnakeGrid.SetColumn(SnakeRectangles[i], (int)pos.X);
                SnakeGrid.SetRow(SnakeRectangles[i], (int)pos.Y);
            }
            i++;
        }


        // Update food locations
        FoodImages.ForEach(f => SnakeGrid.Remove(f));
        FoodImages.Clear();

		foreach (Food f in Game.SpawnedFood)
		{
            try {
                Image image = new();
			    image.Source = f.ImageSource;
                SnakeGrid.SetColumn(image, (int)f.Position.X);
			    SnakeGrid.SetRow(image, (int)f.Position.Y);
                FoodImages.Add(image);
                SnakeGrid.Children.Add(image);
            }
            catch { }
        }
	}

    #endregion

    #region Move buttons
    private void LeftButton_Clicked(object sender, EventArgs e)
	{
		Game.Snake.ChangeDirection(Snake.Direction.Left);
	}

	private void RightButton_Clicked(object sender, EventArgs e)
	{
        Game.Snake.ChangeDirection(Snake.Direction.Right);
	}

	private void UpButton_Clicked(object sender, EventArgs e)
	{
        Game.Snake.ChangeDirection(Snake.Direction.Up);
    }

    private void DownButton_Clicked(object sender, EventArgs e)
	{
        Game.Snake.ChangeDirection(Snake.Direction.Down);
    }
    #endregion

    #region Pause and PauseMenu
    private void PauseButton_Clicked(object sender, EventArgs e)
    {
        if (Game.Timer.IsRunning)
        {
            Game.Pause();
            PauseButton.Text = "Resume";
        }
        else
        {
            Game.Resume();
            PauseButton.Text = "Pause";
        }

    }

    private async Task ShowPauseMenu()
    {
        Game.Pause();
        PauseMenuFrame.IsVisible = true;
        await Navigation.PushModalAsync(PauseMenu);
    }

    public async Task ClosePauseMenu()
    {
        PauseMenuFrame.IsVisible = false;
        Game.Resume();
        await Task.CompletedTask;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await ShowPauseMenu();
    }
    #endregion

    #region Page events
    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        Game.Timer.Stop();
        Game.Timer.Tick -= Timer_Tick;
        Game.GameOverEvent -= Snake_GameOverEvent;
    }

    private void ContentPage_SizeChanged(object sender, EventArgs e)
	{
		if(Width > Height)
		{
			SnakeBorder.WidthRequest = SnakeBorder.Height;
			SnakeBorder.HeightRequest = -1;
		}		 
		else	 
		{		 
			SnakeBorder.HeightRequest = SnakeBorder.Width;
			SnakeBorder.WidthRequest = -1;
        }
	}
    #endregion
}