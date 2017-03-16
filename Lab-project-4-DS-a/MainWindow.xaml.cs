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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_project_4_DS_a
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Automat AutomatWorker;
        public MainWindow()
        {
            InitializeComponent();
            AutomatWorker = new Automat();
        }

        private void RunStepButton_Click(object sender, RoutedEventArgs e)
        {
            int var = 0;
            if (InputChainlet.Text.IndexOf('~') < 0 || InputChainlet.Text.Length < 1)
            {
                MessageBox.Show("You must enter chainlet, and input '~' symbol at end.", "Warning!");
                return;
            }
            while (AutomatWorker.ChainletWorker.Count != InputChainlet.Text.Length)
                AutomatWorker.ChainletWorker.Add(InputChainlet.Text[var++].ToString());
            RunStartButton.IsEnabled = false;
			StackViewLog.Visibility = Visibility.Visible;
			ChainletViewLog.Visibility = Visibility.Visible;
			CheckComp.Visibility = Visibility.Visible;
        }

        private void RunStep_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (!AutomatWorker.RunStep())
                    if(AutomatWorker.ChainletWorker.Count == AutomatWorker.CurrentIndex)
                    {
                        MessageBox.Show("Processing Complete", "Attention!");
                        RunStep.IsEnabled = false;
                    }
                    else
                        AutomatWorker.Error();
            }
            catch
            {
                MessageBox.Show("Incorrect chainlet", "Error");
            }
            
            StackViewLog.Items.Clear();
            foreach (var x in AutomatWorker.StackWorker)
                StackViewLog.Items.Add(x);

            CheckComp.Items.Clear();
            foreach (var x in AutomatWorker.StackComputor)
                CheckComp.Items.Add(x);


            ChainletViewLog.Items.Clear();
            for (int i = AutomatWorker.CurrentIndex; i < AutomatWorker.ChainletWorker.Count; i++)
                ChainletViewLog.Items.Add(AutomatWorker.ChainletWorker[i]);


        }

        private void EnterStringButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
			EnterStringButton.Effect=new DropShadowEffect();
        }

        private void EnterStringButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			OpacityGrid.Visibility = Visibility.Visible;
        	EnterSpace.Visibility = Visibility.Visible;
			
        }

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			EnterSpace.Visibility = Visibility.Hidden;
			OpacityGrid.Visibility = Visibility.Hidden;
        }

        private void HeaderDock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	DragMove();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	Close();
        }
    }
}
