using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceGames
{
    public partial class Form1 : Form
    {
        // represent all scooters 
        private Scooter[] scooters;

        // represent all players
        private Player[] players;

        // represent all Timer for movement of pictures
        private Timer[] timers;

        // represent Winner Scooter
        Scooter winner;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeGameComponents();
        }

        /// <summary>
        /// Initialize All Game Comonents
        /// </summary>
        private void InitializeGameComponents()
        {
            // Hold All picture box reference to an Array
            PictureBox[] pictures = { picture1, picture2, picture3, picture4 };
            
            // Initialize the scooters array
            scooters = new Scooter[pictures.Length];
            // Prepare Scooter Object for storing details of Scooter
            for(int index = 0; index < scooters.Length; index++)
            {
                scooters[index] = new Scooter();
                scooters[index].Name = "Scooter " + (index + 1);
                scooters[index].ScooterPictureBox = pictures[index];
                scooters[index].TrackLength = 710;
            }

            // Hold All Text Box reference to an Arrays
            TextBox[] textBoxes = { text1, text2, text3 };
            // Hold All Radio Button reference to an Arrays
            RadioButton[] radioButtons = { radio1, radio2, radio3 };

            // Initialize the players array
            players = new Player[textBoxes.Length];
            for( int index = 0; index < players.Length; index++)
            {
                players[index] = PlayerFactory.GetPlayer(index + 1);
                players[index].PlayerRadioButton = radioButtons[index];
                players[index].PlayerTextBox = textBoxes[index];
                players[index].Amount = 50;
                players[index].PlayerRadioButton.Text = players[index].Name;
            }

            // Set the Number for Scooter
            npScooterNo.Minimum = 1;
            npScooterNo.Maximum = scooters.Length;
            npScooterNo.Value = 1;

            // Disable the Begin Race Button...
            btnBegin.Enabled = false;
        }

        private void radio1_CheckedChanged(object sender, EventArgs e)
        {
            SetupPlayerDetails();
        }

        private void radio2_CheckedChanged(object sender, EventArgs e)
        {
            SetupPlayerDetails();
        }

        private void radio3_CheckedChanged(object sender, EventArgs e)
        {
            SetupPlayerDetails();
        }

        /// <summary>
        /// Setup Player Details of Bet     
        /// </summary>
        private void SetupPlayerDetails()
        {
            // Iterate All Player holded by players Arary
            for(int index = 0; index < players.Length; index++)
            {
                Player player = players[index];
                // Assure Player is Busted his/her all amount
                if(player.Busted)
                {
                    player.PlayerTextBox.Text = "Player Lost all Amount So BUSTED";
                }
                else
                {
                    // player Placed a Bet or Not
                    if(player.PlayerBet == null )
                    {
                        player.PlayerTextBox.Text = string.Format("{0} hasn't placed a Bet", player.Name);
                    }
                    else
                    {
                        player.PlayerTextBox.Text = string.Format("{0} placed Bet Amount ${1} on {2}", player.Name, player.PlayerBet.Amount, player.PlayerBet.Scooter.Name);
                    }
                    // Is Player Radio Button is Checked or Not
                    if(player.PlayerRadioButton.Checked)
                    {
                        // Set All Control value according to selected Player
                        lblMax.Text = string.Format("{0} Max Bet Amount Limit is ${1}", player.Name, player.Amount);
                        btnPlace.Text = string.Format("Place A BET For Player {0}", player.Name);
                        lblBet.Text = string.Format("Bet Amount of {0} is $", player.Name);
                        lblScooter.Text = string.Format("{0} Place Bet on Scooter No", player.Name);
                        npBetAmount.Minimum = 1;
                        npBetAmount.Maximum = player.Amount;
                        npBetAmount.Value = 1;
                    }
                }
            }
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            int active_player = 0;
            int bet_count = 0;
            // Traverse all Player in players Array
            for(int index = 0; index < players.Length; index++)
            {
                // Check  Player is Still in Game or it has money for Bet
                if( !players[index].Busted)
                {
                    active_player++;
                    // Check Currently Player Radio Button is Selected
                    if( players[index].PlayerRadioButton.Checked)
                    {
                        string message = "";
                        // Check Player Already place a bet or  not
                        if( players[index].PlayerBet != null )
                        {
                            message = string.Format(" {0} is Already Placed Bet For Race Game...", players[index].Name);
                        }
                        else
                        {
                            // Capture value of Bet Amount and Scooter No
                            int scooter_no = (int)npScooterNo.Value;
                            int bet_amount = (int)npBetAmount.Value;

                            // Check Current Scooter No is Already pick by other player
                            bool picked = false;
                            for(int i = 0; i < players.Length; i++)
                            {
                                // Check Player Bet on Scooter
                                if (players[i].PlayerBet != null && players[i].PlayerBet.Scooter == scooters[scooter_no - 1])
                                {
                                    picked = true;
                                    break;
                                }
                            }
                            if(picked)
                            {
                                message = string.Format("Scooter No {0} is Picked By Another Player", scooter_no);
                            }
                            else
                            {
                                players[index].PlayerBet = new Bet();
                                players[index].PlayerBet.Amount = bet_amount;
                                players[index].PlayerBet.Scooter = scooters[scooter_no-1];
                            }
                        }
                        // If there is any message to display
                        if(message.Length!=0)
                        {
                            MessageBox.Show(message);
                        }                        
                    }
                    
                    if(players[index].PlayerBet!=null)
                    {
                        bet_count++;
                    }
                }
            }
            SetupPlayerDetails();
            if(bet_count == active_player)
            {
                btnPlace.Enabled = false;
                btnBegin.Enabled = true;
                panelGame.Enabled = false;
            }
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            timers = new Timer[scooters.Length];
            for( int index = 0; index < timers.Length; index++)
            {
                timers[index] = new Timer();
                timers[index].Interval = 16;
                timers[index].Tick += Timer_Tick;
            }
            foreach(Timer timer in timers)
            {
                timer.Start();
            }
            btnBegin.Enabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Event Sender is Timer 
            if(sender is Timer)
            {
                Timer timer = (Timer)sender;
                int index = -1;
                for(int i = 0; i < timers.Length; i++)
                {
                    if(timer == timers[i])
                    {
                        index = i;
                        break;
                    }
                }
                if(index != -1)
                {
                    PictureBox picture = scooters[index].ScooterPictureBox;
                    if (picture.Location.X + picture.Width > scooters[index].TrackLength)
                    {
                        if (winner == null)
                        {
                            winner = scooters[index];
                        }
                        foreach(Timer tim in timers)
                        {
                            tim.Stop();
                        }
                    }
                    else
                    {
                        int step = new Random().Next(1, 25);
                        picture.Location = new Point(picture.Location.X + step, picture.Location.Y);
                    }
                }
            }
        
            // Check is There any Winner
            if(winner != null )
            {
                MessageBox.Show(string.Format("{0} is won the Race!!!", winner.Name));
                SetupPlayerDetails();
                // Update The Winner Status of Player
                for(int index = 0; index < players.Length; index++)
                {
                    // Check Player is involved in Betting
                    if( players[index].PlayerBet != null )
                    {
                        int amount = players[index].PlayerBet.Amount;
                        // Check Player Scooter win
                        if(players[index].PlayerBet.Scooter == winner)
                        {
                            players[index].Amount += amount;
                            players[index].PlayerTextBox.Text = string.Format("{0} won the Race and Now, has ${1} Amount in Hand", players[index].Name, players[index].Amount);
                            players[index].Winner = true;
                        }
                        else
                        {
                            players[index].Amount -= amount;
                            if(players[index].Amount == 0 )
                            {
                                players[index].PlayerTextBox.Text = "Player Lost all Amount So BUSTED";
                                players[index].Busted = true;
                                players[index].PlayerRadioButton.Enabled = false;
                            }
                            else
                            {
                                players[index].PlayerTextBox.Text = string.Format("{0} Lost ${1} Amount in the Race and Now, has ${1} Amount in Hand", players[index].Name,amount, players[index].Amount);
                            }
                        }
                    }
                }

                // Reset the Game components
                winner = null;
                timers = null;
                int inactive_count = 0;
                for (int index = 0; index < players.Length; index++)
                {
                    // Check Player is Busted
                    if (players[index].Busted)
                    {
                        inactive_count++;
                    }
                    else
                    { 
                        // Check Radio of Player is Selected or Not
                        RadioButton radioButton = players[index].PlayerRadioButton;
                        if (radioButton.Enabled && radioButton.Checked)
                        {
                            lblMax.Text = string.Format("{0} Max Bet Amount Limit is ${1}", players[index].Name, players[index].Amount);
                            btnPlace.Text = string.Format("Place A BET For Player {0}", players[index].Name);
                            lblBet.Text = string.Format("Bet Amount of {0} is $", players[index].Name);
                            lblScooter.Text = string.Format("{0} Place Bet on Scooter No", players[index].Name);
                            npBetAmount.Maximum = players[index].Amount;
                            npBetAmount.Minimum = 1;
                        }
                    }
                    players[index].PlayerBet = null;
                    players[index].Winner = false;
                }
                // Check All Player are Busted
                if(inactive_count == players.Length)
                {
                    MessageBox.Show("GAME OVER!!!!");
                    Application.Exit();
                }
                else // Enable the Game for Restart
                {
                    panelGame.Enabled = true;
                    btnPlace.Enabled = true;
                    MessageBox.Show("You Can Place More Bet...");
                    SetupPlayerDetails();
                }
                // Reset the Scooter Picture at Original Position
                for(int index = 0; index < scooters.Length; index++)
                {
                    PictureBox picture = scooters[index].ScooterPictureBox;
                    picture.Location = new Point(2, picture.Location.Y);
                }
            }
        }
    }
}
