using PL.Engineer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlApi;
using DalTest;

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {


        static readonly IBl s_bl = Factory.Get;


        public AdminWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// click to show the engineer list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

       
      


        private void Reset(object sender, RoutedEventArgs e)
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

        //private void InitDB(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult mbResult = MessageBox.Show(
        //   "Are you sure?", "reset",
        //   MessageBoxButton.YesNo,
        //   MessageBoxImage.Question,
        //   MessageBoxResult.Yes,
        //   MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        //    if (mbResult == MessageBoxResult.Yes)
        //        s_bl.in

        //}
    }
}
