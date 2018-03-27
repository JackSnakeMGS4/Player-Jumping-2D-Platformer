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
        private double playerSpeedX = 7;//sets player's left and right speed
        private double playerX;//is used to set the player's position on canvas x-axis
        private double playerY;//is used to set the player's position on canvas y-axis
        private double playerSpeedY;//used to determine player's jumping capability
        private const double GRAVITY = .75;//used to bring the the player back to ground level at a constant rate
        private bool isPlayerJumping = false;//used to determine is the player is jumping
        private const int framesPerSecond = 120;//used to determine the rate at which the game updates (makes the game loop possible)

        public MainWindow()
        {
            InitializeComponent();
            playerX = Canvas.GetLeft(player);//sets playerX to 0 based on my choice
            playerY = Canvas.GetBottom(player);//sets playerY to 0 based on my choice
            DispatcherTimer update = new DispatcherTimer();
            update.Tick += Update_Tick;
            update.Interval = TimeSpan.FromMilliseconds(1000 / framesPerSecond);
            update.Start();
        }

        private void Update_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Space))//only fires if the space bar is pressed or held down
            {
                PlayerJumping();
            }
            //next line moves the player in accordance with variables
            MovePlayer();   
            /*note to self: I want to create two or three so I can prototype the player 
             * jumping onto a platform and let them land on it and be able to fall off it 
             * by moving past the platform's left or right boundaries
             */
        }

        private bool PlayerJumping()
        {
            /* if the player is pressing or holding down the space bar then isPlayerJumping is set to true
             * playerSpeedY is set to 8 and return the value of isPlayerJumping
             */
            isPlayerJumping = true;
            playerSpeedY = 8;
            return isPlayerJumping;
        }

        private void MovePlayer()
        {
            /* next three statements move the player left or right depending on the situation
             * playerX = playerX + playerSpeedX where playerSpeedX is 7 and playerX is the current value of the player's
             * left property
             */
            playerX += playerSpeedX;
            if (playerX > gameCanvas.Width - player.Width)
            {
                playerSpeedX *= -1;
            }
            if (playerX < 0)
            {
                playerSpeedX *= -1;
            }
            //next if statement changes the player's y position according to y-axis variable and GRAVITY
            //need to find a way to prevent the player from escaping the canvas by jumping 
            if (isPlayerJumping)
            {
                //if player pressed space bar then this will hold true until isPlayerJumping becomes false
                //next line adds playerSpeedY to playerY and set playerY to the new value. playerSpeedY is reset to 8 everytime the space bar is pressed
                playerY += playerSpeedY;
                //next substracts .75 from playerSpeedY and enables playerY to decrease
                playerSpeedY -= GRAVITY;
                //next statement checks if player hit the bottom on the canvas
                if (PlayerHitGround())
                {
                    //if PlayerHitGround() return true then isPlayerJumping will be set to false and prevent playerY from being decreased
                    isPlayerJumping = false;
                    //next line sets playerY to 0 in order to prevent a bug where the player's bottom property is less than 0 and then shown on the game
                    playerY = 0;
                }             
            }
          
            //next two lines set the player's position according to the calculation's performed above
            Canvas.SetLeft(player, playerX);
            Canvas.SetBottom(player, playerY);
        }

        private bool PlayerHitGround()
        {
            //if statement will fire when playerY is less than 0 and returns true
            if (playerY < 0)
            {
                return true;
            }
            
            return false;
        }
    }
}
