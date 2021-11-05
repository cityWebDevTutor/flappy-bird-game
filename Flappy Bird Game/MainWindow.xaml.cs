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

using System.Windows.Threading; // add this for the timer

namespace Flappy_Bird_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // create a new instance of the timer class called gameTimer
        DispatcherTimer gameTimer = new DispatcherTimer();
        // declare variables
        double score;
        int gravity = 8;
        bool gameOver;
        // declare rect to use for collision detection between bird and pipes
        Rect FlappyBirdHitBox;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer; // link the timer tick to the event
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // set the interval to 20 miliseconds
            StartGame(); // run the start game function
        }
        //mainEventTimer will execute function every 20 milliseconds
        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score; // write out score to label
            // determines where flappybird is in the game ## reduce wisth by -5px to make flappy closer to pipe for collision
            FlappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width, flappyBird.Height + 5);
            // move flappy up or down dependant on value of gravity
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);

            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 458)
            {
                EndGame();
            }
            // loop through each image in canvas
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                // ### conditional statement to detect pipes ### 
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    // if we found an image with the tag obs1,2 or 3 (pipes) then we will animate towards left of the screen
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5); 
                    // if pipes go past left of the canvas (off the screen)
                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800); // reset pipe to 800px from left (on screen)                                  
                        score += .5; // add .5 to the score for each pipe flappy passes
                    }
                    // every iteration of loop x will get coordinates in the canvas of each pipe graphic and store in pipeHitBox
                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); 
                    // Collision Detection: if flappybird collides with pipe
                    if (FlappyBirdHitBox.IntersectsWith(pipeHitBox))
                    {
                        EndGame(); // call the function to end the game
                    }
                }

                // ### conditional statement to move clouds
                if ((string)x.Tag == "cloud") // if image is a cloud
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2); // move cloud slowly -2 pixel to left every 20 milliseconds
                    // if cloud -250 pixels to left of screen (off the screen)
                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 500); //reset could back on screen to 500 pixels from left
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // if the space key is pressed
            if (e.Key == Key.Space)
            {
                // rotate the bird image to -20 degrees from the center position () rotates slightly in direction bird is heading
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);
                gravity = -8;  // change gravity so it will move upwards
            }
            if (e.Key == Key.R && gameOver == true)
            {
                // if the r key is pressed AND game over boolean is set to true
                StartGame(); // run the start game function
            }
        }

        private void KeyIsUP(object sender, KeyEventArgs e)
        {
            // rotate flappy bird in direction going up
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            // change gravity back to positive value so bird moves down 8 pixels on the canvas every 20 milliseconds
            gravity = 8;
        }

        private void StartGame()
        {
            MyCanvas.Focus();
            int temp = 300;
            score = 0; // set to 0 as start of game
            gameOver = false; // set to false as start of game
            Canvas.SetTop(flappyBird, 190); // reset flappy to 190 pixels from top of screen

            // loop through every image (pipes and cloud) and set to to default position/coordinates on the canvas
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                // set obs1 pipes to its default position
                if ((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                // set obs2 pipes to its default position
                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                // set obs3 pipes to its default position
                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }
                // set the clouds to its default position
                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, (300 + temp));
                    temp = 800;
                }
            }
           
            gameTimer.Start();  // start the main game timer
        }

        private void EndGame()
        {
            gameTimer.Stop(); // stop the timer
            gameOver = true; // set variable to true
            txtScore.Content += " Game Over !! Press R to try again"; // display message to player in label
        }
    }
}
