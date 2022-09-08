using System;
using System.Collections.Generic;

namespace BehindTheWheel.GameMechanics
{
    public class Card
    {
        public int Index { get; internal set; }
        public string Text { get; private set; }
        public int Points { get; private set; }
        public string[] Answers { get; private set; }
        public Card(int index, string text, int points, string[] answers)
        {
            Index = index;
            Text = text;
            Points = points;
            Answers = answers;
        }
    }
    public class ToolTimeCard : Card
    {
        public ToolTimeCard(int index, string text=null, int points=0, string[] answers=null) : base(index, text, points, answers)
        {
            // "Maintenance To Be Done";
        }
    }
    public class TechTimeCard : Card
    {
        public TechTimeCard(int index, string text, int points, string[] answers) : base(index, text, points, answers)
        {
        }
    }
    public class TheSpiritOfBrooklandsCard : Card
    {
        public TheSpiritOfBrooklandsCard(int index, string text, int points, string[] answers=null) : base(index, text, points, answers)
        {
        }
    }
    public static class ToolTimeCards
    {
        //internal static ToolTimeCard[] toolTimeCards = new ToolTimeCard[] {
        //    new ToolTimeCard(1, "Change the battery",1),
        //    new ToolTimeCard(2, "Reupholster seats",3),
        //    new ToolTimeCard(3, "Bore out cylinders",5),
        //};
        internal static List<ToolTimeCard> toolTimeCards = new List<ToolTimeCard>();
        public static void Load()
        {
            return;
            string[] lines = System.IO.File.ReadAllLines(Utilities.getDefaultLocation(@"Resources\ToolTime.tsv"));
            string[] columns;
            foreach (string line in lines)
            {
                try
                {
                    columns = line.Split('\t');
                    if (columns.Length >= 2)
                    {
                        toolTimeCards.Add(new ToolTimeCard(0, columns[0],
                            int.Parse(columns[1])));
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        internal static int toolTimeIndex = 0;
        public static ToolTimeCard Card()
        {
            if (++toolTimeIndex >= toolTimeCards.Count)
            {
                toolTimeIndex = 0;
            }
            return toolTimeCards[toolTimeIndex];
        }
    }
    public static class TechTimeCards
    {
        //internal static TechTimeCard[] techTimeCards = new TechTimeCard[] {
        //    new TechTimeCard(1,"Why port an engine?", 3, new string[] {
        //        "Change the battery",
        //        "It makes the engine give more power",
        //        "A giraffe with floppy ears"
        //    }),
        //    new TechTimeCard(2,"What does an 'overdrive' do?", 3, new string[] {
        //        "It gives more relaxed cruising",
        //        "It makes the car faster",
        //        "It's used for overtaking"
        //    }),
        //};
        internal static List<TechTimeCard> techTimeCards = new List<TechTimeCard>();
        public static void Load()
        {
            return;
            string[] lines = System.IO.File.ReadAllLines(Utilities.getDefaultLocation(@"Resources\TechTime.tsv"));
            string[] columns;
            foreach (string line in lines)
            {
                try
                {
                    columns = line.Split('\t');
                    if (columns.Length >= 3)
                    {
                        string[] answers = new string[3] { columns[1], columns[1], columns[1] };
                        techTimeCards.Add(new TechTimeCard(0, columns[0],
                            int.Parse(columns[2]),
                            answers));
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        internal static int techTimeIndex = 0;
        public static TechTimeCard Card()
        {
            if (++techTimeIndex >= techTimeCards.Count)
            {
                techTimeIndex = 0;
            }
            return techTimeCards[techTimeIndex];
        }
    }
    public static class TheSpiritOfBrooklandsCards
    {
        internal static List<TheSpiritOfBrooklandsCard> theSpiritOfBrooklandsCards = new List<TheSpiritOfBrooklandsCard>();
        public static void Load()
        {
            /*System.IO.File.ReadAllLines(Utilities.getDefaultLocation(@"Resources\SpiritOfBrooklands.tsv"));
            string[] columns;
            foreach (string line in lines)
            {
                try
                {
                    columns = line.Split('\t');
                    if (columns.Length >= 3)
                    {
                        theSpiritOfBrooklandsCards.Add(new TheSpiritOfBrooklandsCard(int.Parse(columns[0]),
                            columns[1],
                            int.Parse(columns[2])));
                    }
                }
                catch (Exception ex)
                {

                }
            }*/
        }
        public static TheSpiritOfBrooklandsCard Card(int index)
        {
            if (index >= theSpiritOfBrooklandsCards.Count)
            {
                index = 0;
            }
            return theSpiritOfBrooklandsCards[index];
        }
    }
}
