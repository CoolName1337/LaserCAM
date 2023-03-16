using LaserCAM.CAM;
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

namespace LaserCAM.Windows
{
    /// <summary>
    /// Логика взаимодействия для ParamsWindow.xaml
    /// </summary>
    public partial class ParamsWindow : Window
    {
        public ParamsWindow()
        {
            Resources.Add("useDots", GParams.UseDots);
            Resources.Add("useSpaces", GParams.UseSpacesInResult);
            Resources.Add("fullCode", GParams.FullCode);
            InitializeComponent();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is CheckBox checkBox)
            {
                switch (checkBox.Tag)
                {
                    case "spaces":
                        GParams.UseSpacesInResult = checkBox.IsChecked ?? false;
                        break;
                    case "sep":
                        GParams.UseDots = checkBox.IsChecked ?? false;
                        break;
                    case "full":
                        GParams.FullCode = checkBox.IsChecked ?? false;
                        break;
                }
            }
        }
    }
}
