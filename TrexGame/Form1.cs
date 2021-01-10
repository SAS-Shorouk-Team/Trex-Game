using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrexGame
{
    public partial class Form1 : Form
    {
         /// Global variables for the game /// 
         

        Stopwatch sw = new Stopwatch(); // initializa stop watch
        bool jumping = false; // boolean to check  if the player is jumping or not
        int jumpSpeed; // int to set jump speed
        int force = 14; // int to set the force of the jump
        int score = 0; // int to set the default score to 0 
        int obstacleSpeed = 10; // int to set the default speed of the obstacles 
        Random rdn = new Random(); // initializa a random class
        int position; // int to set the position of the obstacle
        bool GameOver = false; // boolean to check if the game is over or not 

        public Form1()
        {
            InitializeComponent();
            GameReset(); // run the reset game function 
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            int m = sw.Elapsed.Minutes; // int to count minutes
            int s = sw.Elapsed.Seconds; // int to count seconds
            int mi = sw.Elapsed.Milliseconds; // int to count milliseconds
            string strM = (m < 10) ? "0" + m : m + " "; // convert from int to string
            string strS = (s < 10) ? "0" + s : s + " "; // convert from int to string
            txtmin.Text = strM; // display lable of minutes
            txtsec.Text = strS; // display lable of seconds
            txtmil.Text = mi.ToString(); 
            sw.Start(); // start the stop watch
            trex.Top += jumpSpeed; 
            gamescore.Text = "Score : " + score + "\n" ; // display the game score
            // Trex jumping
            if (jumping == true && force <0)
            {
                jumping = false;
            }
            if (jumping == true)
            {
                jumpSpeed = -10;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }
            if (trex.Top > 284 && jumping == false)
            {
                force = 14;
                trex.Top = 285;
                jumpSpeed = 0;

            }


            foreach(Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;
                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rdn.Next(5,80) + (x.Width * 15);
                        score++;
                    } 
                    // level 1 : easy
                    if (score>=0 && score <=10)
                    {
                        txtLevel.Text = "Level : Easy";
                    }
                    // level 2 : medium 
                    if (score>10 && score <=20)
                    {
                        txtLevel.Text = "Level : Medium";
                        obstacleSpeed = 13;
                    }
                     // level 3 : hard
                    if (score>20)
                    {
                        txtLevel.Text = "Level : Hard";
                        obstacleSpeed = 18;
                    }
                   
                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        GameTimer.Stop();
                        sw.Stop();
                        trex.Image = Properties.Resources.dead;
                        gamescore.Text += "\n" + "Press R Or Space to play again";
                        MessageBox.Show("Game Over" + "\n" + "Your score " + score);
                        GameOver = true;
                    }
                }

            }
        }
        // press space or up to jump 
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) && jumping == false)
            {
                jumping = true;
            }
        }
        // press R or space to play again
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (jumping == true)
            {
                jumping = false;
            }
            
            if ((e.KeyCode == Keys.R || e.KeyCode == Keys.Space)  && GameOver == true)
            {
                GameReset();
            }
        } 
        // function to reset the game
        private void GameReset()
        {
            sw.Restart();
            force = 14;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            gamescore.Text = "Score : " + score;
            trex.Image = Properties.Resources.running;
            GameOver = false;
            trex.Top = 285;
            

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    position = this.ClientSize.Width + rdn.Next(10,100) + (x.Width * 10);
                    x.Left = position;
                }
            }
            GameTimer.Start();
  
        }
    }
}
