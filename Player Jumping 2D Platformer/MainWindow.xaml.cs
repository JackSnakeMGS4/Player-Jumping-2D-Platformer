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

        private const double fullXAxisRotation = 0;//used to reset the player's x-axis rotation to 0
        private double rotateXAxis = 0;//will be used to rotate x-axis

        /*So you want to rotate the player as they go up and down!?
         * Used RotateTransform
         */
        private RotateTransform rotate = new RotateTransform();
        private RotateTransform resetRotation = new RotateTransform(fullXAxisRotation);

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

            /*note to self: I want to create two or three platforms so I can prototype the player 
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
            if (isPlayerJumping && !PlayerHitCeiling())
            {
                //if player pressed space bar then this will hold true until isPlayerJumping becomes false and player has not hit ceiling
                //next line adds playerSpeedY to playerY and set playerY to the new value. playerSpeedY is reset to 8 everytime the space bar is pressed
                playerY += playerSpeedY;

                //next line calls RotatePlayer() to calculate the angle for rotate
                rotate.Angle = RotatePlayer();

                //next line reflects change calculated from RotatePlayer()
                player.RenderTransform = rotate;

                //next substracts .75 from playerSpeedY and enables playerY to decrease
                playerSpeedY -= GRAVITY;           
            }            
            //next statement checks if player has hit the ceiling
            else if (PlayerHitCeiling())
            {
                /*since playerY is reset to 8 everytime space is pressed then substracting playerSpeedY wouldn't work 
                 * since it is far less than playerY because of the calculations performed in the previous if statement
                 * the solution is to instead substract by GRAVITY multiplied by whatever number you choose
                 * which works since it will always run when the playerY is greater than the canvas height - player height
                 * note that this if/else if causes what may appear to be a bug when the hitting the ceiling, but remember
                 * that physics are in effect. think of a human running into a pole! they wouldn't just stop immediately, 
                 * instead the collision will have this rebound effect. Don't believe me? Try it! Go run into a wall or a pole 
                 * as fast as you can and see if you actually stop instantenously
                 */
                playerY -= GRAVITY * 10;
            }

            //next statement checks if player hit the bottom on the canvas
            if (PlayerHitGround())
            {
                //if PlayerHitGround() return true then isPlayerJumping will be set to false and prevent playerY from being decreased
                isPlayerJumping = false;

                //next line sets playerY to 0 in order to prevent a bug where the player's bottom property is less than 0 and then shown on the game
                playerY = 0;

                //next line prevents another visual bug where the player landed on a angle other than 180 or 360 or a multiple of either those               
                player.RenderTransform = resetRotation;
            }

            //next two lines set the player's position according to the calculation's performed above
            Canvas.SetLeft(player, playerX);
            Canvas.SetBottom(player, playerY);
        }

        private bool PlayerHitCeiling()
        {
            if (playerY > gameCanvas.Height - player.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private double RotatePlayer()
        {
            //next line sets the amount to rotate the player by
            double rotateBy = 22.5;
            if (isPlayerJumping)//fires while isPlayerJumping is true
            {
                //adds 22.5 to rotateXAxis
                rotateXAxis += rotateBy;
            }
            //next line returns the end result of the calculation from above
            return rotateXAxis;
        }
    }
}
