using BlApi;
using PL.Manager;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }
        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        { new EngineerListWindow().Show(); }

        private void btnResetDB_Click(object sender, RoutedEventArgs e)
        {

            IBl.Reset();
        }
        static readonly IEngineer s_bl = IBl.Engineer;

        public IEnumerable<BO.EngineerInTask> EngineerList
        {
            get { return (IEnumerable<BO.EngineerInTask>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }
        public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.EngineerInTask>),
        typeof(EngineerListWindow), new PropertyMetadata(null));

    }
}