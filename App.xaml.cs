namespace MauiDemo;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MenuNavigator menuNavigator = new MenuNavigator();
		menuNavigator.GoToMainPage();
    }
}
