using System;
using System.IO;
using System.Runtime.CompilerServices;

using BehindTheWheel.ViewModel;

[assembly: InternalsVisibleTo("UnitTests")]

namespace BehindTheWheel.GameMechanics
{
    internal static class Dice
    {
        public static int GetThrow()
        {
            return Utilities.GetRandom(1, 7);
        }
    }
    /// <summary>
    /// index keeps track of current square number
    /// which is used to remember special squares
    /// </summary>
    internal static class Index
    {
        internal static int index = -1; // (Indexed before using)
        internal static int lastSquare = 0;
        internal static int TechTimeSquare = 0;   // a Tech Time square (for dev mode)
        internal static int gotoBrooklandsSquare = 0;   // the square that branches off to the inner loop
        internal static int brooklandsSquare = 0;   // the square that branches off to the inner loop
        internal static int brooklandsRejoinSquare = 0;
        internal static int brooklandsFirstSquare = 0; // the first square of the inner loop
        internal static int brooklandsLastSquare = 0; // the last square of the inner loop

        internal static int throwEvenSquare = 0; // For testing
        internal static int throwOddSquare = 0; // For testing
        internal static int goBackThreeSquare = 0; // For testing
    }

    internal class Square
    {
        public string Text { get; }
        public int Points { get; private set; }
        public int Cash { get; private set; }
        public Square(string text, int points = 0, int cash = 0)
        {
            Text = text;
            Points = points;
            Cash = cash;
            Index.index++;
        }
        /// <summary>
        /// Any special action on landing
        /// </summary>
        /// <returns></returns>
        public virtual Tuple <int, int> OnLandingAction(int currentSquare)
        {
            return Tuple.Create(currentSquare, 0);
        }
        public virtual int NextSquare(int currentSquare, int steps)
        {
            int nextSquare = currentSquare + steps;
            if (nextSquare >= Index.lastSquare)
            {
                nextSquare -= Index.lastSquare;
            }
            return nextSquare;
        }
    }

    internal class GreenSquare : Square
    {
        public GreenSquare(string text, int points = 0, int cash = 0) : base(text, points, cash)
        { }
    }
    internal class AmberSquare : Square
    {
        public AmberSquare(string text, int points = 0, int cash = 0) : base(text, points, cash)
        { }
    }
    internal class RedSquare : Square
    {
        public RedSquare(string text, int points = 0, int cash = 0) : base(text, points, cash)
        { }
    }

    internal class ToolTimeSquare : AmberSquare
    {
        public ToolTimeSquare(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
        }
        /// <summary>
        /// Any special action on landing
        /// </summary>
        /// <returns></returns>
        public override Tuple <int, int> OnLandingAction(int currentSquare)
        {
            var card =ToolTimeCards.Card();
            PlayerModel.DisplayToolTime(card);
            return Tuple.Create(currentSquare, -card.Points);
        }
    }
    internal class TechTimeSquare : AmberSquare
    {
        public TechTimeSquare(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
            Index.TechTimeSquare = Index.index;
        }
        /// <summary>
        /// Any special action on landing
        /// </summary>
        /// <returns></returns>
        public override Tuple <int, int> OnLandingAction(int currentSquare)
        {
            try
            {
                var card = TechTimeCards.Card();
                var X = PlayerModel.DisplayTechTimeAsync(card);
            }
            catch (Exception ex)
            { 
                var err = ex.Message;
            }
            return Tuple.Create(currentSquare, 0);
        }
    }
    internal class ThrowEvenNumberSquare : Square
    {
        public ThrowEvenNumberSquare(string text, int points = 0, int cash = 0) : base(text, points, cash)
        {
            Index.throwEvenSquare = Index.index;
        }
        public override int NextSquare(int currentSquare, int steps)
        {
            if ((steps & 1) == 0)
            {
                currentSquare += steps;
            }
            return currentSquare;
        }
    }
    internal class ThrowOddNumberSquare : Square
    {
        public ThrowOddNumberSquare(string text, int points = 0, int cash = 0) : base(text, points, cash)
        {
            Index.throwOddSquare = Index.index;
        }
        public override int NextSquare(int currentSquare, int steps)
        {
            if ((steps & 1) == 1)
            {
                currentSquare += steps;
            }
            return currentSquare;
        }
    }
    internal class GoBackThreeSquare : Square
    {
        public GoBackThreeSquare(string text, int points = 0, int cash = 0) : base(text, points, cash)
        {
            Index.goBackThreeSquare = Index.index;
        }
        public override Tuple <int, int> OnLandingAction(int currentSquare)
        {
            return Tuple.Create(currentSquare - 3, 0);
        }
    }


    internal class GotoBrooklands : AmberSquare
    {
        public GotoBrooklands(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
            Index.gotoBrooklandsSquare = Index.index;
        }
        /// <summary>
        /// Any special action on landing
        /// </summary>
        /// <returns></returns>
        public override Tuple <int, int> OnLandingAction(int currentSquare)
        {
            Squares.board[Index.brooklandsFirstSquare].OnLandingAction(Index.brooklandsFirstSquare);
            return Tuple.Create(Index.brooklandsFirstSquare, 0);
        }
    }

    internal class WelcomeToBrooklands : Square
    {
        public WelcomeToBrooklands(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
            Index.brooklandsSquare = Index.index;
        }
        /// <summary>
        /// Any special action on landing
        /// </summary>
        /// <returns></returns>
        public override Tuple <int, int> OnLandingAction(int currentSquare)
        {
            return Tuple.Create(Index.brooklandsFirstSquare, 0);
        }
    }

    internal class ThankYouForVisitingBrooklands : GreenSquare
    {
        public ThankYouForVisitingBrooklands(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
            Index.brooklandsRejoinSquare = Index.index;
        }
        /// <summary>
        /// Any special action on landing
        /// </summary>
        /// <returns></returns>
        public override Tuple <int, int> OnLandingAction(int currentSquare)
        {
            return Tuple.Create(currentSquare, 0);
        }
    }

    internal class LastSquare : Square
    {
        public LastSquare(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
            Index.lastSquare = Index.index;
        }
    }

    /// <summary>
    /// A square on the inner loop
    /// </summary>
    internal class BrooklandsSquare : Square
    {
        public BrooklandsSquare(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
        }
        /// <summary>
        /// Any special action on landing
        /// </summary>
        /// <returns></returns>
        public override Tuple <int, int> OnLandingAction(int currentSquare)
        {
            var card = TheSpiritOfBrooklandsCards.Card(currentSquare - Index.brooklandsFirstSquare);
            PlayerModel.DisplaytheSpiritOfBrooklands(card, Squares.board[currentSquare].Text);
            return Tuple.Create(currentSquare, card.Points);
        }
        public override int NextSquare(int currentSquare, int steps)
        {
            int nextSquare = currentSquare + steps;
            if (nextSquare > Index.brooklandsLastSquare)
            {
                nextSquare = nextSquare - Index.brooklandsLastSquare + Index.brooklandsRejoinSquare;
            }
            return nextSquare;
        }
    }
    internal class FirstBrooklandsSquare : BrooklandsSquare
    {
        public FirstBrooklandsSquare(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
            Index.brooklandsFirstSquare = Index.index;
        }
    }
    internal class LastBrooklandsSquare : BrooklandsSquare
    {
        public LastBrooklandsSquare(string text, int points = 0, int cash = 0, int nextSquare = 0) : base(text, points, cash)
        {
            Index.brooklandsLastSquare = Index.index;
        }
    }


    public static class Squares
    {
        internal static Square[] board = new Square[] {
            new GreenSquare ( "START", 0 ),
            new GreenSquare ( "Win turbo in auction", 2 ),
            new RedSquare ( "Need new exhaust", -2 ),
            new Square ( "£2000 profit from sale of car", 0, 2000 ),
            new GreenSquare ( "Best in show prize", 4 ),
            new ToolTimeSquare ( "Tool time"),
            new GreenSquare ( "Inherit new classic", 5 ),
            new Square ( "Team talk\nOne minute bodywork"),
            new GreenSquare ( "Octane boost in car", 3 ),
            new TechTimeSquare ( "Tech time"),
            new RedSquare ( "Partner finds car receipts", -4 ),
            new ThrowEvenNumberSquare ( "Throw even number to move" ),
            new GreenSquare ( "Tow friend home", 3 ),

            new GotoBrooklands ( "Go to Brooklands", 0 ),
            new GreenSquare ( "Return parts for refund", 3 ),
            new ThankYouForVisitingBrooklands("Thank you for visiting Brooklands"),
            new Square ( "£2000 profit from sale of car", 0, 2000 ),
            new ToolTimeSquare ( "Tool time"),
            new GreenSquare ( "Favourite road\nSunny day", 4 ),
            new TechTimeSquare ( "Tech time"),
            new RedSquare ( "Convert garage to home office", -3 ),
            new ThrowOddNumberSquare ( "Throw odd number to move" ),
            new GreenSquare ( "Excellent respray!", 3 ),
            new GoBackThreeSquare("Go back three spaces"),

            new GreenSquare ( "Win pub quiz", 2 ),
            new Square ( "£2000 profit from sale of car", 0, 2000 ),
            new GreenSquare ( "Degrease parts in dishwasher", 2 ),
            new ToolTimeSquare ( "Tool time"),
            new RedSquare ( "Major repair cost", -5 ),
            new Square ( "Team talk\n1 minute E10 fuel", 0),
            new GreenSquare ( "Road trip with mates", 3 ),
            new TechTimeSquare ( "Tech time"),
            new GreenSquare ( "Insurance Refund", 3 ),
            new ThrowEvenNumberSquare ( "Throw even number to move" ),
            new GreenSquare ( "Fastest time on track day", 4 ),
            new AmberSquare ( "Double points next go"),

            new RedSquare ( "Traffic jam on M25", -2 ),
            new Square ( "£2000 profit from sale of car", 0, 2000 ),
            new ToolTimeSquare ( "Tool time"),
            new RedSquare ( "Pay insurance", -2 ),
            new WelcomeToBrooklands("Welcome to Brooklands\nGo to No.1"),
            new TechTimeSquare ( "Tech time"),
            new RedSquare ( "Caught speeding", -3 ),
            new Square ( "Team talk\n1 minute Tyres", 0),
            new GreenSquare ( "Take mechanic course", 3 ),
            new LastSquare ( "Convert car to electric"),

            new FirstBrooklandsSquare ( "Gift shop/\nStart line"),
            new BrooklandsSquare ( "Historic race track"),
            new BrooklandsSquare ( "Flight shed"),
            new BrooklandsSquare ( "Test hill"),
            new BrooklandsSquare ( "Shell, BP and Pratt's Pagoda"),
            new BrooklandsSquare ( "First to Fastest"),
            new BrooklandsSquare ( "Paddock & Scoreboard"),
            new BrooklandsSquare ( "Sunbeam Cafe"),
            new BrooklandsSquare ( "Press Hut"),
            new BrooklandsSquare ( "Dunlop Mac's Bungalow"),
            new BrooklandsSquare ( "Jackson Shed"),
            new BrooklandsSquare ( "ERA and Campbell Shed"),
            new BrooklandsSquare ( "Stratosphere Chamber"),
            new BrooklandsSquare ( "Aircraft Park"),
            new BrooklandsSquare ( "Concorde"),
            new BrooklandsSquare ( "Avro Shed"),
            new BrooklandsSquare ( "Club House"),
            new BrooklandsSquare ( "World's First Ticket Office"),
            new BrooklandsSquare ( "Campbell Gate"),
            new BrooklandsSquare ( "When was the track officially opened?"),
            new BrooklandsSquare ( "What year did it host the first British Grand Prix?"),
            new BrooklandsSquare ( "How long did the track take to build?"),
            new LastBrooklandsSquare("What is the length of the circuit?")
        };

        /// <summary>
        /// Fill in the Index values for the special squares
        /// </summary>
        public static void GetIndices(object[] board)
        {
            for (var i = 0; i < board.Length; i++)
            {
                switch (Path.GetExtension(board[i].GetType().ToString()))
                {
                    case ".LastSquare":
                        Index.lastSquare = i;
                        break;
                    case ".GotoBrooklands":
                        Index.gotoBrooklandsSquare = i;
                        break;
                    case ".BrooklandsSquare":
                        Index.brooklandsSquare = i;
                        break;
                    case ".ThankYouForVisitingBrooklands":
                        Index.brooklandsRejoinSquare = i;
                        break;
                    case ".FirstBrooklandsSquare":
                        Index.brooklandsFirstSquare = i;
                        break;
                    case ".LastBrooklandsSquare":
                        Index.brooklandsLastSquare = i;
                        break;
                    case ".ThrowEvenNumberSquare":
                        Index.throwEvenSquare = i;
                        break;
                    case ".ThrowOddNumberSquare":
                        Index.throwOddSquare = i;
                        break;
                    case ".GoBackThreeSquare":
                        Index.goBackThreeSquare = i;
                        break;
                }
            }
        }
        public static int NextSquare(int currentSquare, int moves)
        {
            return board[currentSquare].NextSquare(currentSquare, moves);
        }
    }
}

