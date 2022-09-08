using System.IO;
using System.Reflection;

using BehindTheWheel.GameMechanics;

using BlazorTestApp.Pages;

using static System.Net.Mime.MediaTypeNames;

using Index = BlazorTestApp.Pages.Index;

namespace BehindTheWheel.ViewModel
{
    /// <summary>
    /// The View Model
    /// </summary>
    internal class PlayerModel
    {
        public object Name { get; private set; }
        public object Points { get; private set; }
        public object Cash { get; private set; }
        public object CurrentSquare { get; private set; }
        public static PlayerModel playerModel = new BehindTheWheel.ViewModel.PlayerModel();

        public void Update(int player, int points, int cash, Square currentSquare)
        {

            string squareType = Path.GetExtension(currentSquare.GetType().ToString());
            string squareText = currentSquare.Text;
            if (squareType == ".BrooklandsSquare"
                || squareType == ".FirstBrooklandsSquare"
                || squareType == ".LastBrooklandsSquare")
            {
                squareText = "Brooklands Square\n\n" + squareText;
            }
            Index.players[player].Name = Player.players[player].Name;
            Index.players[player].Text = squareText;
            Index.players[player].Cash = $"Cash: {Player.players[player].Cash.ToString()}";
            Index.players[player].Points = $"Points: {Player.players[player].Points.ToString()}";

            Index.CardColour(squareType, player);
        }

        public void DisplayThrow(int player, int dice, int currentSquare)
        {
            string text = $"{Player.players[player].Name} throws {dice}\n";
            Index.commentary += text;
            Index.commentary2 = "";
            /*
            Program.MainWindow.textBoxCommentary.AppendText(text);
            AutoClosingMessageBox.Show($"Moves to square {currentSquare}:\n'{Squares.board[currentSquare].Text}'", 
                $"{text}",
                timeout: 2000,
                MessageBoxButtons.OK);
            */
        }

        public static void DisplayToolTime(ToolTimeCard toolTimeCard)
        {
            string points = toolTimeCard.Points > 1 ? "Points" : "Point";
            Index.commentary2 = $"TOOL TIME\n\n{toolTimeCard.Text}\n\n{toolTimeCard.Points} " + points + "\n\n";

            /*MessageBox.Show($"{toolTimeCard.Text}\n\n{toolTimeCard.Points} " + points,
                 "Maintenance To Be Done",
                MessageBoxButtons.OK
                );
            */
        }
        public static void DisplayTechTime(TechTimeCard techTimeCard)
        {
            string text = "TECH TIME\n\n" + techTimeCard.Text;
            for (var i = 0; i < techTimeCard.Answers.Length; i++)
            {
                text += $"\n {i+1}. {techTimeCard.Answers[i]}";
            }
            Index.commentary2 = text + "\n\n";
            /*MessageBox.Show(text,
                 $"{techTimeCard.Index}.",
                MessageBoxButtons.OK
                );
            */
        }
        public static void DisplaytheSpiritOfBrooklands(TheSpiritOfBrooklandsCard theSpiritOfBrooklandsCard, string title)
        {
            string points = theSpiritOfBrooklandsCard.Points > 1 ? "Points" : "Point";
            Index.commentary2 = $"{title}\n\n{theSpiritOfBrooklandsCard.Text}\n\n{theSpiritOfBrooklandsCard.Points} " + points + "\n\n";

            /*MessageBox.Show($"{theSpiritOfBrooklandsCard.Text}\n\n{theSpiritOfBrooklandsCard.Points} " + points,
                 $"{theSpiritOfBrooklandsCard.Index.ToString()}: {title}",
                MessageBoxButtons.OK
                );
            */
        }

        private static void showModal()
        {
            
        }
    }
}
