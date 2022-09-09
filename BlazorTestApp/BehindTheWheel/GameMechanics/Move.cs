﻿using System;
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
            if (System.Diagnostics.Debugger.IsAttached ||
                BlazorIndexPage.Password == "TVR. Blackpool's finest")
            {
                var currentSquare = Squares.NextSquare(Player.players[player].CurrentSquare, dice);
                PlayerModel.playerModel.DisplayThrow(player, dice, currentSquare);
                // Display the new square
                PlayerModel.playerModel.Update(player,
                    Player.players[player].Points,
                    Player.players[player].Cash,
                    Squares.board[currentSquare]);

                Tuple<int, int> tuple = Squares.board[currentSquare].OnLandingAction(currentSquare);

                // Display status after any actions
                Player.players[player].CurrentSquare = tuple.Item1;
                Player.players[player].Points += tuple.Item2;
                Player.players[player].Cash += Squares.board[currentSquare].Cash;
                Player.players[player].Points += Squares.board[currentSquare].Points;
                PlayerModel.playerModel.Update(player,
                    Player.players[player].Points,
                    Player.players[player].Cash,
                    Squares.board[Player.players[player].CurrentSquare]);
                player = (player + 1) % 2;
            }
        }
    }
}