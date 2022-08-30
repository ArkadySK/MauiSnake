namespace MauiDemo;

[XamlCompilation(XamlCompilationOptions.Skip)]
public partial class PauseMenuPage : ContentPage
{
	public PauseMenuPage()
	{
		InitializeComponent();
	}

	private async void ButtonRestart_Clicked(object sender, EventArgs e)
	{
        var w = this.Parent as Window;
        await (w.Page as PlayPage).Navigation.PopModalAsync();
        (w.Page as PlayPage).MenuNavigator.GoToPlayPage();
    }

	private async void ButtonBack_Clicked(object sender, EventArgs e)
	{
        var w = this.Parent as Window;
        await (w.Page as PlayPage).Navigation.PopModalAsync(); 
		(w.Page as PlayPage).MenuNavigator.GoToMainPage();
	}

	private async void ButtonReturn_Clicked(object sender, EventArgs e)
	{
		var w = this.Parent as Window;
		await (w.Page as PlayPage).Navigation.PopModalAsync(true);
		await (w.Page as PlayPage).ClosePauseMenu();
	}
}