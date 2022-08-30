namespace MauiDemo;

[XamlCompilation(XamlCompilationOptions.Skip)]
public partial class MainPage : ContentPage
{

	MenuNavigator MenuNavigator;
	public MainPage(MenuNavigator menuNavigator)
	{
		InitializeComponent();
		MenuNavigator = menuNavigator;
    }

    private async void ButtonLeave_Clicked(object sender, EventArgs e)
	{
		var result = await DisplayAlert("Question", "Are you sure you want to leave Snake?", "Yes", "No");
		if (result == true)
            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
    }

	private void ButtonPlay_Clicked(object sender, EventArgs e)
	{
        MenuNavigator.GoToPlayPage();
	}
}

