using ACTools.NetFramework.Controls;
using System;

namespace AgenteTickets.Views
{
    /// <summary>
    /// Lógica de interacción para SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : LoaderWindow
    {
        public SplashScreenWindow(Action<LoaderWindow> action) : base(action)
        {
            InitializeComponent();
        }
    }
}
