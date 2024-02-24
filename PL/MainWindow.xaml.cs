using BlApi;
using DalTest;
using PL.Engineer;

using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly IBl s_bl = Factory.Get;

    public MainWindow()
    {
        InitializeComponent();
    }
    private void ResetDB(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbResult = MessageBox.Show(
        "Are you sure?", "reset",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question,
        MessageBoxResult.Yes,
        MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        if (mbResult == MessageBoxResult.Yes)
            s_bl.Reset();
    }

    private void InitDB(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbResult = MessageBox.Show(
       "Are you sure?", "reset",
       MessageBoxButton.YesNo,
       MessageBoxImage.Question,
       MessageBoxResult.Yes,
       MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        if (mbResult == MessageBoxResult.Yes)
            Initialization.createEngineers();
        new EngineerListWindow().Show();

    }

    private void Admin(object sender, RoutedEventArgs e)
    {
        new AdminWindow().Show();
        new AddUpdateEngineer().ShowDialog();
    }

    private void Engineer(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().Show();
    }

    /*private void Date(object sender, RoutedEventArgs e)
    {
        new DateWindow().Show();
    }*/
}