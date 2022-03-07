using System.Windows;
using Jot.ViewModels;

namespace Jot.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            MainViewModel instanceOfMainViewModel = new();
            DataContext = instanceOfMainViewModel;
        }
    }
}
