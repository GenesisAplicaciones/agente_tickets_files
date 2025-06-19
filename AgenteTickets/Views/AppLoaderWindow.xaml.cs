using ACTools.NetFramework.Controls;
using System;

namespace AgenteTickets.Views
{
    /// <summary>
    /// Lógica de interacción para AppLoaderWindow.xaml
    /// </summary>
    public partial class AppLoaderWindow : LoaderWindow
    {
        public AppLoaderWindow(Action<LoaderWindow> action) : base(action)
        {
            InitializeComponent();
        }
    }
}
