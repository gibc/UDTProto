using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpoTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {

        //public dynamic exObj;
        public MainWindow()
        {

            ex = new Expo();
            ex.View = this as IView;

            DataContext = ex;
 
            InitializeComponent();
        }

        Expo ex;
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ex.SetText("change the text");
        }

        public void AddTextBoxToGrid(Control control)
        {
            ControlTemplate template = (ControlTemplate)FindResource("validationErrorTemplate");
            Validation.SetErrorTemplate(control, template);
            StackPanel.Children.Add(control);
        }
    }
}
