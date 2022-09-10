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
            //Index.commentary2 = "";
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
            string points = toolTimeCard.Points > 1 ? " Points deducted" : " Point deducted";
            Index.ConfirmOpenDialog("TOOL TIME", new List<string>() { toolTimeCard.Text_Question, toolTimeCard.Points + points });

            /*MessageBox.Show($"{toolTimeCard.Text}\n\n{toolTimeCard.Points} " + points,
                 "Maintenance To Be Done",
                MessageBoxButtons.OK
                );
            */
        }
        public static int TechTimeResult;
        public static async Task DisplayTechTimeAsync(TechTimeCard techTimeCard)
        {
            //string text = "TECH TIME\n\n" + techTimeCard.Text_Question;
            List<string> answers = new List<string>() { techTimeCard.Text_Question };
            int answerCount = techTimeCard.Answers.Where(x => !x.Equals("")).Count();
            var columnOrder = Utilities.GetRandomOrder(0, answerCount);
            int ansNumber = 1;
            // Put the answers in random order
            for (int i = 0; i < answerCount; i++)
            {
                string answerText = techTimeCard.Answers[columnOrder[i]];
                answers.Add($"{ansNumber}. {answerText}");
                //text += $"\n {ansNumber}. {answerText}";
                ansNumber++;
            }
            Index.buttonsRequired = answerCount;
            Index.OpenDialog(answers);
            //Index.commentary2 = text + "\n\n";
            /*MessageBox.Show(text,
                 $"{techTimeCard.Index}.",
                MessageBoxButtons.OK
                );
            */
            while (Index.response == -1)
            {
                await Task.Delay(100);
            }
            var answer = answers[Index.response].Substring(3);
            if (techTimeCard.Answers[0] == answer)
            {
                TechTimeResult = 3; // points
                Index.ConfirmOpenDialog("Right answer!",new List<string>() { "3 points"});
            }
            else
            {
                TechTimeResult = 0; // points
                Index.ConfirmOpenDialog("Wrong answer",new List<string>() { "Correct answer was", techTimeCard.Answers[0], "not " + answer });
            }
            Index.mv.Score(TechTimeResult);
            Index.Instance.refreshTheScreen();
        }

        public static void DisplaytheSpiritOfBrooklands(TheSpiritOfBrooklandsCard theSpiritOfBrooklandsCard, string title)
        {
            string points = theSpiritOfBrooklandsCard.Points > 1 ? " Points" : " Point";
            Index.ConfirmOpenDialog(title, new List<string>() { theSpiritOfBrooklandsCard.Text_Question, theSpiritOfBrooklandsCard.Points +  points });
            /*MessageBox.Show($"{theSpiritOfBrooklandsCard.Text}\n\n{theSpiritOfBrooklandsCard.Points} " + points,
                 $"{theSpiritOfBrooklandsCard.Index.ToString()}: {title}",
                MessageBoxButtons.OK
                );
            */
        }
        public static void DisplaytheSpiritOfBrooklandsQuestion(TheSpiritOfBrooklandsCard theSpiritOfBrooklandsCard, string title)
        { }

        private static void showModal()
        {
            
        }
    }
}
