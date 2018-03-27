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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Player_Jumping_2D_Platformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double playerSpeedX = 7;
        private double playerX;
        private double playerY;
        private double playerSpeedY;
        private const double GRAVITY = .75;
        private bool isPlayerJumping = false;
        private const int framesPerSecond = 120;

        public MainWindow()
        {
            InitializeComponent();
            playerX = Canvas.GetLeft(player);//sets playerX to 0
            playerY = Canvas.GetBottom(player);//sets playerY to 0
            DispatcherTimer update = new DispatcherTimer();
            update.Tick += Update_Tick;
            update.Interval = TimeSpan.FromMilliseconds(1000 / framesPerSecond);
            update.Start();
        }

        private void Update_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Space))
            {
                PlayerJumping();
            }
            MovePlayer();                  
        }

        private bool PlayerJumping()
        {
            isPlayerJumping = true;
            playerSpeedY = 8;
            return isPlayerJumping;
        }

        private void MovePlayer()
        {
            playerX += playerSpeedX;
            if (playerX > gameCanvas.Width - player.Width)
            {
                playerSpeedX *= -1;
            }
            if (playerX < 0)
            {
                playerSpeedX *= -1;
            }
            if (isPlayerJumping)
            {
                playerY += playerSpeedY;
                playerSpeedY -= GRAVITY;
                if (PlayerHitGround())
                {
                    isPlayerJumping = false;
                    playerY = 0;
                }             
            }
          
            Canvas.SetLeft(player, playerX);
            Canvas.SetBottom(player, playerY);
        }

        private bool PlayerHitGround()
        {
            if (playerY < 0)
            {
                return true;
            }
            return false;
        }
    }
}
