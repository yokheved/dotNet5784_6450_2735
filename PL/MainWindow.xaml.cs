using BlApi;
using PL.Manager;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            EngineerList = s_bl?.EngineerImplementation.ReadAll()!;
        }
        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        { new EngineerListWindow().Show(); }

        private void btnResetDB_Click(object sender, RoutedEventArgs e)
        {

            //BL.BlApi.IBl.Reset();
        }
        static readonly BlApi.IBl s_bl = BlApi.IEngineer.Get();

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