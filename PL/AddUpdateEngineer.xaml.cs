using BlApi;
using DalTest;
using System.Windows;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for AddUpdateEngineer.xaml
    /// </summary>
    public partial class AddUpdateEngineer : Window
    {
        static readonly IBl s_bl = Factory.Get;

        static string? addOrUpdate;

        public event EventHandler? CloseWindow;

        public AddUpdateEngineer(int currentEngineerId = 0)
        {
            //update engineer, that exists already
            InitializeComponent();
                


            try
            {
                CurrentEngineer = currentEngineerId == 0 ? new BO.Engineer()
                : s_bl.Engineer!.GetEngineerList(e => e.Id == currentEngineerId).First();
                addOrUpdate = currentEngineerId == 0 ? "add"
                : "update";
            }
            catch
            {
                MessageBox.Show($"Engineer with id {currentEngineerId} doesn't exist");
            }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(AddUpdateEngineer), new PropertyMetadata(null));

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        /// <summary>
        /// submit add or update engineet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUpdateEngineerSubmit(object sender, RoutedEventArgs e)
        {


            try
            {
                if (addOrUpdate == "add")
                {
                    try
                    {
                        s_bl.Engineer!.AddEngineer(CurrentEngineer);
                        MessageBox.Show("Engineer created successfully", "add engineer",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                        if (CloseWindow is not null)
                            CloseWindow(this, e);
                        this.Close();
                    }

                    catch (BO.BlAlreadyExistsException)
                    {
                        MessageBox.Show("Engineer already exist", "exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch
                    {
                        MessageBox.Show("Unexpected error", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    try
                    {
                        s_bl.Engineer!.UpdateEngineer(CurrentEngineer);

                        MessageBox.Show("Engineer updated successfully", "add engineer",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                        if (CloseWindow is not null)
                            CloseWindow(this, e);
                        this.Close();
                    }

                    catch
                    {
                        MessageBox.Show("Unexpected error", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("can't save the changes");
            }
        }

    }
}
