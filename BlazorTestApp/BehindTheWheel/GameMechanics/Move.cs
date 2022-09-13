using System;
using System.Linq;

using BehindTheWheel.ViewModel;

using BlazorIndexPage = BlazorTestApp.Pages.Index;

namespace BehindTheWheel.GameMechanics
{
    public class Move
    {
        internal static int player = 0;
        internal void Init()
        {
            for (int player = 0; player < Player.players.Count(); player++)
            {
                PlayerModel.playerModel.Update(player, Player.players[player].Points, Player.players[player].Cash, Squares.board[0]);
            }
        }

        internal void throwDice()
        {
            fixedThrow(Dice.GetThrow());
        }
        internal void fixedThrow(int dice)
        {
            if (passwordCheck())
            {
                var currentSquare = Squares.NextSquare(Player.players[player].CurrentSquare, dice);
                PlayerModel.playerModel.DisplayThrow(player, dice, currentSquare);
                // Display the new square
                Tuple<int, int> tuple = DisplaySquare(currentSquare);

                // Display status after any actions
                Player.players[player].CurrentSquare = tuple.Item1;
                Score(tuple.Item2);
                player = (player + 1) % 2;
            }
            else
            {
            }
        }

        private static Tuple<int, int> DisplaySquare(int currentSquare)
        {
            Tuple<int, int> tuple = new Tuple<int, int>(currentSquare, 0);
            if (passwordCheck())
            {
                PlayerModel.playerModel.Update(player,
                Player.players[player].Points,
                Player.players[player].Cash,
                Squares.board[currentSquare]);

                tuple = Squares.board[currentSquare].OnLandingAction(currentSquare);
                return tuple;
            }
            return tuple;
        }

        private static bool passwordCheck()
        {
            if (BlazorIndexPage.Password != null &&
                    BlazorIndexPage.Password.Trim() == "TVR. Blackpool's finest")
            {
                BlazorIndexPage.commentary += $"\nPassword '{BlazorIndexPage.Password}' accepted";
                return true;
            }
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                BlazorIndexPage.commentary += $"\nPassword '{BlazorIndexPage.Password}' not accepted";
                return false;
            }
            return true;
        }
        public void Score(int points)
        {
            Player.players[player].Cash += Squares.board[Player.players[player].CurrentSquare].Cash;
            Player.players[player].Points += Squares.board[Player.players[player].CurrentSquare].Points + points;
            PlayerModel.playerModel.Update(player,
                Player.players[player].Points,
                Player.players[player].Cash,
                Squares.board[Player.players[player].CurrentSquare]);
        }
        internal void TechTime()
        {
            Player.players[player].CurrentSquare = Index.TechTimeSquare;
            DisplaySquare(Player.players[player].CurrentSquare);
        }
        internal void Brooklands()
        {
            Player.players[player].CurrentSquare = Index.brooklandsFirstSquare;
            DisplaySquare(Player.players[player].CurrentSquare);
        }
    }
}
