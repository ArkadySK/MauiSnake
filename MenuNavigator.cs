using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiDemo
{
    public class MenuNavigator
    {

        public MainPage mainPage;
        public PlayPage playPage;


        public void GoToMainPage()
        {
            //playPage.Unfocus();
            playPage = null;
            mainPage = new MainPage(this);
            App.Current.MainPage = mainPage;
        }

        internal void GoToPlayPage()
        {
            mainPage = null;
            playPage = new PlayPage(this);
            App.Current.MainPage = playPage;
        }
    }
}
