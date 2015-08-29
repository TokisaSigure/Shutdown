using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;//shutdown.exeを呼び出すための布石

namespace Shutdown
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        int timer = 0 , setTime;//タイマー,設定時間
        Boolean start = false;//タイマー起動の有無
        ProcessStartInfo psi;//Shutdown.exeを起動するための布石,外部アプリを起動するための宣言

        public MainWindow()
        {
            InitializeComponent();//初期化コンテンツ
            psi.FileName = "shutdown.exe";//呼び出すアプリケーションにshut(略)を追加
            psi.Arguments = "/r";//シャットダウンアクション、lでログオフ,rで再起動,sでシャットダウン,fで強制的に全アプリケーション終了
            DispatcherTimer dispatcherTimer = new DispatcherTimer();//タイマー宣言
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);//重複して呼び出されるコンテンツ
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);//呼び出されるタイミング指定、1秒ごとに
            dispatcherTimer.Start();//タイマー起動
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (start)
            {
                ++timer;//カウントダウン追加
                this.TimerTest.Content = timer;//タイマ―チェック用
                if (timer >= setTime)
                {
                    Process p = Process.Start(psi);//シャットダウン
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            setTime = int.Parse(this.Setting.Text);
            this.Button.Visibility = Visibility.Hidden; 
            start = !start; 
        }
    }
}
