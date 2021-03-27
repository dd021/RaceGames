using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceGames
{
    /// <summary>
    /// Player abstract class for sharing common details
    /// </summary>
    public abstract class Player
    {
        // Amount in Hand of Player
        public int Amount;

        // Current Bet class object reference
        public Bet PlayerBet;

        // Radio Button of Player
        public RadioButton PlayerRadioButton;

        // Name of the Player
        public string Name;

        // Current Game Winning Status
        public bool Winner;

        // To Display Current Game Player Status 
        public TextBox PlayerTextBox;

        // Status to present All Amount Lose on Games
        public bool Busted;
    }
}
