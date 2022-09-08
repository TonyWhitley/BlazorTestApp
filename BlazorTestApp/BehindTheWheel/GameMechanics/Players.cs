namespace BehindTheWheel.GameMechanics
{
    internal class Player
    {
        public string Name { get; private set; }
        public int Points { get; set; }
        public int Cash { get; set; }
        public int CurrentSquare { get; set; }
        public int WinTotal { get; private set; }
        public Player(string name, int points, int cash, int currentSquare, int winTotal)
        {
            Name = name;
            Points = points;
            Cash = cash;
            CurrentSquare = currentSquare;
            WinTotal = winTotal;
        }
        internal static Player[] players = new Player[]
        {
            new Player ("Ray", 10, 0, 0, 100 ),
            new Player ("Tony", 10, 0, 0, 100 ),
        };


    }
}
