using BlApi;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace PL.Engineer;
/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly IBl s_bl = Factory.Get;
    public BO.EngineerExperience SearchLevelSelectedValue { get; set; } = BO.EngineerExperience.None;



    public static readonly DependencyProperty EngineerListProperty =
     DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    /// <summary>
    /// initialize the window
    /// </summary>
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer!.GetEngineerList();
    }

    /// <summary>
    /// level selection changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LevelSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (SearchLevelSelectedValue == BO.EngineerExperience.None) ?
            s_bl?.Engineer.GetEngineerList()! : s_bl?.Engineer.GetEngineerList(item => item.Level == (BO.EngineerExperience)SearchLevelSelectedValue)!;
    }


    /// <summary>
    /// create new engineer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddEngineer(object sender, RoutedEventArgs e)
    {

        AddUpdateEngineer addUpdateEngineer = new();
        addUpdateEngineer.CloseWindow += (s, e) =>
        {
            UpdateEngineersList();
        };
        addUpdateEngineer.ShowDialog();
    }

    /// <summary>
    /// select engineer to update
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SelectEngineer(object sender, MouseButtonEventArgs e)
    {
        BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
        AddUpdateEngineer addUpdateEngineer = new(engineer!.Id);
        addUpdateEngineer.CloseWindow += (s, e) =>
        {
            UpdateEngineersList();
        };
        addUpdateEngineer.ShowDialog();
    }

    /// <summary>
    /// load the engineer list after adding or updating
    /// </summary>
    private void UpdateEngineersList()
    {
        EngineerList = (SearchLevelSelectedValue == BO.EngineerExperience.None) ?
           s_bl?.Engineer!.GetEngineerList()! : s_bl?.Engineer!.GetEngineerList(item => item.Level == (BO.EngineerExperience)SearchLevelSelectedValue)!;
    }
}

