using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

using static System.Collections.Specialized.BitVector32;

using static System.Net.Mime.MediaTypeNames;

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
            string[] lines =
@"Change air filter;1
Top up brake and clutch fluid;1
Replace tyre;1
Adjust headlights;1
Fill up screen wash bottle;1
Change the high tension leads;1
Change the battery;1
Change oil filter and oil;1
Grease and lubricate;1
Adjust handbrake;1
Replace blown bulb;1
Change the spark plugs;1
Replace the door cards;1
Underseal the car;1
Rustproof the car;1
Change the coil;2
Fit a new fuel pump;2
Fit a new oil pump;2
Replace brake shoes / discs;2
Bleed brakes;2
Adjust / replace the points;2
Change the timing belt;2
Fit car stereo;2
Repair window winders;2
Replace shock absorbers;2
Tune carburettors;2
Fit electronic ignition;2
Replace the radiator;2
Wrap exhaust;2
Repair rust and bodywork;2
Give the car a full service;3
Change the cambelt;3
Fit vented discs;3
Replace body panels;3
Replace windscreen;3
Fit a new exhaust;3
Reupholster seats;3
Adjust valve timing;4
Fix electrical fault;4
Fit adjustable dampers;4
Weld rusted panels;4
Recondition the carburettors with a service kit;4
Respray the car;5
Repair dashboard gauges;5
Port and flow engine head;5
Bore out cylinders;5".Split('\n');
            //string[] lines = System.IO.File.ReadAllLines(Utilities.getDefaultLocation(@"Resources\ToolTime.tsv"));
            string[] columns;
            foreach (string line in lines)
            {
                try
                {
                    columns = line.Split(';');
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
            string[] lines =
@"Why port an engine?;It gives better fuel and air flow;3
What does an overdrive do?;It’s a higher ration gear for relaxed cruising;3
Why fit electronic ignition?;It generates electrical impulses to give a better spark which doesn’t degrade;3
Will fitting wider tyres make the steering lighter or heavier?;Lighter  Heavier;3
Why cover battery terminals?;To avoid them shorting out on metal components;3
Is it better to draw in hot air or cold air for the petrol mix;Hot  Cold;3
What is a ‘Cherry Bomb’;An exhaust silencer;3
How many DVLA point to keep historic car status?;8;3
Which of these are modification point categories? Chassis  Suspension  Axles  Transmission  Steering  Engine;All;3
What are the colloquial terms for the stages of the 4 stroke cycle?;Suck, squeeze, bang, blow;3
Where does the power from the coil go to?;The distributer;3
What is the most common firing order of a 4 cylinder engine;1-3-4-2;3
What is the most common firing order of a 6 cylinder engine;1-5-3-6-2-4;3
What is the earth cable on the battery usually connected to?;The bodywork;3
On a bolt and nut specification, what does UNF mean?;Unified fine;3
Why fit a dry sump?;Holds more oil  Better for cornering when driving hard  Allows you to fit a shallower oil pan so that the car can be lower to the road;3
Why use silicon hoses?;Greater operating temperature  More flexible  Last longer;3
Where is the reverse light activation switch usually mounted?;The gearbox;3
How does a turbo increase the power of the engine?;It forces more air into the engine cylinders;3
How does a supercharger increase the power of the engine?;It increases the volume of air into the engine;3
Which of these is a car steering system?;Steering box  Rack and pinion;3
What is the technique of operating the brake and accelerator simultaneously known as?;Heal and toe;3
What are hazard warning lights known as in America?;Four ways;3
What’s the most common antifreeze colour used in a classic car;Blue;3
What should you do if your radiator water is orange / brown?;Flush it to remove rust;3
What is the function of the thermostat?;It enables the engine to warm up more quickly;3
What is the most common grade of oil that classic cars use?;20/50;3
What does a throttle body do?;It regulates air into the fuel injection system;3
What effect does Ethanol have on a classic car?;It perishes fuel pipe rubber  It causes steel components to rust;3
Which of these are used for car interiors? Leather  Vinyl  Cloth  Wood  Plastic  Velour;All;3
How old does a car have to be to attain historic status;40 years old;3
Why wrap an exhaust?;It keeps the engine bay cooler and makes the engine run more efficiently;3
What is the purpose of a carburettor?;It regulates fuel and air into the engine;3
Why are twin carburettors more efficient than one single unit?;Twin carbs deliver fuel to specific cylinders rather than all of them;3
What’s the purpose of an engine fan?;It cools the water flowing around the engine;3
What is commonly referred to as the ‘nut behind the wheel’?;The driver;3
What is the purpose of a limited slip differential?;It allows one wheel to keep traction whilst the other wheel is spinning;3
What is a torque wrench used for?;It’s used to tighten components to a value pre-determined in the vehicle technical manual;3
Which was the first car manufacturer to fit seat belts as standard?;Volvo;3
What does a speedo cable usually fit between?;The gearbox and speedometer;3
What brake fluid does a classic car usually use?;Dot 4 or Dot 5;3
What are remoulds?;A worn tyre that has been given a new tread;3
What is the secret for installing new glass into a classic car?;Use a strong, thin cable in the window channel;3
Should a fuel pump be installed near the fuel tank or under the bonnet?;Either  Near the fuel tank  Under the bonnet;3
What is the name of the clips used to attach car seat material to the car seat frame?;Hog rings;3
Which of these are a form of welding? MIG  TIG  Arc  Flux Cored, Gas;All;3
Which of these are a type of car paint? Cellulose  2-pack  Acryllic;All;3
What type of antifreeze should you use in a Type 2 VW Camper Van?;None, it’s air cooled;3
Where would you find a gudgeon pin?;In the engine;3
What component did the alternator replace?;The dynamo;3".Split('\n');
            //string[] lines = System.IO.File.ReadAllLines(Utilities.getDefaultLocation(@"Resources\TechTime.tsv"));
            string[] columns;
            foreach (string line in lines)
            {
                try
                {
                    columns = line.Split(';');
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
            string[] lines =
@"1;The visitors’ entrance to Brooklands. The start of a magical journey of discovery. The Shop has unique souvenir and gift items.;2
2;Brooklands was the ‘Ascot of Motorsport’. Visitors can walk on the Members’ Banking and the Finishing Straight.;4
3;Explore the immersive Brooklands Aircraft Factory, originally a manufacturing building.;4
4;Test Hill was built in 1909. It comprises three sections of increasing gradient, 1 in 8, 1 in 5 and 1, in 4. Built to test climbing capability and brakes, it became a challenge to set fastest climb records.;3
5;The Shell, BP and Pratts Petrol pagodas were built in 1922 and supplied drivers with fuel. They have all been fully restored to their 1920’s or ‘30’s external appearance.;2
6;Celebrate two historic Transatlantic ait races: Alcock and Brown’s non-stop crossing of the Atlantic in a Brooklands-built Vickers Vimy in June 1919 and the Daily Mail Transatlantic Air Race of 1969.;2
7;Widely used for event hosting, the Paddock is a great place to display your classic car. The scoreboard is a faithful recreation of this iconic information hub.;3
8;Relax and enjoy the ambience of Brooklands Museum within the iconic Clubhouse at the Sunbeam Café.;3
9;The Press Hut was built in 1930 for journalists reporting on Brooklands events. Directly behind it lies a line of Race Bays where vehicles would be checked and waiting before going out onto the track.;3
10;Here David McDonald supervised tyre fitting in the 1920’s and ‘30’s. It is now a vehicle workshop.;2
11;Built in 1930-31 by Robin Jackson to maintain and tune racing cars, the Jackson Shed now houses the Grand Prix Exhibition, a mechanic’s workshop and the McLaren Formula 1 simulator.;5
12;Housed inside the two original sheds, this exhibition recalls the countless speed records achieved at Brooklands and pays tribute to the Brooklands drivers, riders, mechanics and engineers.;5
13;The Barnes Wallace designed Stratosphere Chamber was built in 1946 to investigate high speed flight at very high altitudes.;2
14;The exhibition area houses the BAC 1-11 and the collection of Vickers aircraft built after 1945, including the Varsity military trainer, Viscount, Vanguard ‘Merchantman’, VC 10 airliners and Viking.;3
15;The initial British production Concorde, G-BBDG was the first aircraft ever to carry 100 passengers at twice the speed of sound. Much of Concorde’s design and manufacture took place at Brooklands.;5
16;This is a replica of Alliot Verdon Roe’s shed, built on the Finishing Straight to construct his Roe 1 Biplane. It now houses the Museum’s replica.;3
17;Enjoy the period atmosphere and the evocative room settings. See the displays and exhibits throughout the ground floor of the 1907 Clubhouse.;5
18;The World’s first flight ticket office was built in 1911 for London booking agents Keith Prowse to sell sightseeing flights around the track.;2
19;Classic car entrance to Brooklands. Relive the excitement of arriving at one of the world’s most historic race venues.;2
20;1907;5
21;1926 (7th August);5
22;9 Months;5
23;2.75 Miles;5".Split('\n');

            /*System.IO.File.ReadAllLines(Utilities.getDefaultLocation(@"Resources\SpiritOfBrooklands.tsv"));*/
            string[] columns;
            foreach (string line in lines)
            {
                try
                {
                    columns = line.Split(';');
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
            }
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
