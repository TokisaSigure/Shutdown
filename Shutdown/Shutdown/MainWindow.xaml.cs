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
        ProcessStartInfo psi;
        Process p;
        DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();//初期化コンテンツ
            /*----------------shutdown.exe設定----------------------------*/
            psi = new ProcessStartInfo();
            psi.FileName = @"C:\Windows\System32\shutdown.exe";//呼び出すアプリケーションにshut(略)を設定
            //ウィンドウを表示しないようにする
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            /*----------------shutdwon.exe設定ここまで--------------------*/
            /*----------------------タイマー設定--------------------------*/
            dispatcherTimer = new DispatcherTimer();//タイマー宣言
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);//重複して呼び出されるコンテンツ
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);//呼び出されるタイミング指定、1秒ごとに
            dispatcherTimer.Start();//タイマー起動
            /*----------------------タイマー設定ここまで-------------------*/
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (start)
            {
                ++timer;//カウントダウン追加
                this.TimerTest.Content = timer;//タイマ―チェック用
                if (timer >= setTime * 60 * 60)
                {
                    dispatcherTimer.Stop();
                    p = Process.Start(psi);//シャットダウン
                    start = !start;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!start)
            {
                setTime = int.Parse(this.Setting.Text);
                psi.Arguments = @"/s"; //シャットダウンアクション、lでログオフ,rで再起動,sでシャットダウン,fで強制的に全アプリケーション終了
                this.Button.Content = "タイマーキャンセル";
                this.restart.Content = "タイマーキャンセル";
                start = !start;
            }
            else
            {
                start = !start;
                timer = 0;
                this.Button.Content = "シャットダウン";
                this.restart.Content = "再起動";
                MessageBox.Show("タイマーを停止しました。\n※シャットダウン1分前の場合は停止不可能です※");
            }
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            if (!start)
            {
                setTime = int.Parse(this.Setting.Text);
                psi.Arguments = @"/r"; //シャットダウンアクション、lでログオフ,rで再起動,sでシャットダウン,fで強制的に全アプリケーション終了
                this.Button.Content = "タイマーキャンセル";
                this.restart.Content = "タイマーキャンセル";
                start = !start;
            }
            else
            {
                start = !start;
                psi.Arguments = @"/a";//シャットダウン処理停止
                p = Process.Start(psi);
                timer = 0;
                this.Button.Content = "シャットダウン";
                this.restart.Content = "再起動";
                MessageBox.Show("タイマーを停止しました。\n※シャットダウン1分前の場合は停止不可能です※");
            }
        }
    }
}
