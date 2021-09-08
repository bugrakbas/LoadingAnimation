using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace LoadingAnimation
{
    public partial class MainWindow : Window
    {
        BackgroundWorker worker;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            long sum = 0;
            long total = 100000;
            for (long i = 1; i <= total; i++)
            {
                sum += i;
                int percentage = Convert.ToInt32(((double)i / total) * 100);

                Dispatcher.Invoke(new System.Action(() =>
                {
                    worker.ReportProgress(percentage);
                }));
            }
            MessageBox.Show("İşlem Tamamlandı: " + sum);
        }

        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MyProgressBar.Value = e.ProgressPercentage;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MyProgressBar.Visibility = Visibility.Collapsed;
            MyProgressLabel.Visibility = Visibility.Collapsed;
            PerformTask.IsEnabled = true;
        }

        private void PerformTask_Click(object sender, RoutedEventArgs e)
        {
            MyProgressBar.Visibility = Visibility.Visible; //ProgressBarı görünür yapar.
            MyProgressLabel.Visibility = Visibility.Visible; //TextBlock'u görünür yapar.
            PerformTask.IsEnabled = false; //Düğmeyi devre dışı bırakır.
            worker = new BackgroundWorker(); //Yeni nesne oluşturma.
            worker.ProgressChanged += Worker_ProgressChanged; //Worker_ProgressChanged metodunu bağlama
            worker.DoWork += Worker_DoWork; //Worker_DoWork metodunu bağlama
            worker.WorkerReportsProgress = true;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted; //worker_RunWorkerCompleted methounu bağlama
            worker.RunWorkerAsync();
        }
    }
}
