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
        public string Text_Question { get; private set; }
        public int Points { get; private set; }
        public List<(string text, bool correct)> Answers { get; private set; }
        public Card(int index, string text, int points, List<(string, bool)> answers)
        {
            Index = index;
            Text_Question = text;
            Points = points;
            Answers = answers;
        }
    }
    public class ToolTimeCard : Card
    {
        public ToolTimeCard(int index, string text=null, int points=0, List<(string, bool)> answers = null) : base(index, text, points, answers)
        {
            // "Maintenance To Be Done";
        }
    }
    public class TechTimeCard : Card
    {
        public TechTimeCard(int index, string question, int points, List<(string, bool)> answers = null) : base(index, question, points, answers)
        {
        }
        public bool CheckAnswer(string answer)
        {
            bool result = false;
            foreach (var ans in this.Answers)
            {
                if (ans.text == answer)
                {
                    result = ans.correct;
                }
            }
            return result;
        }
    }
    public class TheSpiritOfBrooklandsCard : Card
    {
        public TheSpiritOfBrooklandsCard(int index, string text, int points, List<(string, bool)> answers=null) : base(index, text, points, answers)
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
            toolTimeIndex = Utilities.GetRandom(0, toolTimeCards.Count);
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
        internal static List<TechTimeCard> techTimeCards = new List<TechTimeCard>();
        public static void Load()
        {
            string[] lines =
@"Why port an engine?;It gives better fuel and air flow;It cures engine gout;It makes engine oil flow better;It cools the engine down;;;;3
What does an overdrive do?;It’s a higher ratio gear for relaxed cruising;It acts as a form of cruise control;It mutes back-seat drivers;It’s a form of turbo;;;;3
Why fit electronic ignition?;It generates electrical impulses to give a better spark which doesn’t degrade;✓It enables the car to go faster;✓It eliminates the need for a distributor;It adds 10 BHP;;;;3
Will fitting wider tyres make the steering lighter or heavier?;Heavier;Lighter;;;;;;3
Why cover battery terminals?;To avoid them shorting out on metal components;To keep them warm;To keep them cool in the engine bay;To prolong the life of the battery;;;;3
Is it better to draw in hot air or cold air for the petrol mix;Cold;Hot;;;;;;3
What is a ‘Cherry Bomb’;An exhaust silencer;A form of air freshener;An engine additive;A reclining passenger seat;;;;3
How many DVLA point to keep historic car status?;8;2;4;6;;;;3
Which of these are modification point categories?;All;✓Chassis;✓Suspension;✓Axles;✓Transmission;✓Steering;✓Engine;3
What are the colloquial terms for the stages of the 4 stroke cycle?;Suck, squeeze, bang, blow;Wheeze, squeeze, pop, drip;Bang, clang, stop, repair;On, hope, wish, mechanic;;;;3
Where does the power from the coil go to?;The distributor;The garage lightbulb;The engine;The carburettor;;;;3
What is the most common firing order of a 4 cylinder engine;1-3-4-2;1-2-3-4;1-4-2-3;4-3-2-1;;;;3
What is the most common firing order of a 6 cylinder engine;1-5-3-6-2-4;1-2-3-4-5-6;6-5-4-3-2-1;1-6-2-5-3-4;;;;3
What is the earth cable on the battery usually connected to?;The bodywork;The engine;The carburettor;The fuel tank;;;;3
On a bolt and nut specification, what does UNF mean?;Unified fine;You need friends;Unlucky, not fixed;Usually nut frayed;;;;3
Why fit a dry sump?;Better for cornering when driving hard;✓Holds more oil;✓Allows you to fit a shallower oil pan so that the car can be lower to the road;You don’t need to add oil to an engine;;;;3
Why use silicone hoses?;Greater operating temperature;✓More flexible;✓Last longer;They match the colour of the car;;;;3
Where is the reverse light activation switch usually mounted?;The gearbox;In the boot of the car;On the dashboard;The button on the handbrake;;;;3
How does a turbo increase the power of the engine?;It forces more air into the engine cylinders;It makes the engine louder;It enables you to add a turbo sticker to your car;It forces air into the carburettor;;;;3
How does a supercharger increase the power of the engine?;It increases the volume of air into the engine;It makes the engine louder;It enables you to add a turbo sticker to your car;It forces air into the carburettor;;;;3
Which of these is a car steering system?;Rack and pinion;✓Worm and sector;Joystick;;;;;3
What is the technique of operating the brake and accelerator simultaneously known as?;Heel and toe;Contortionism;Shoe and shuffle;Foot and mouth;;;;3
What are hazard warning lights known as in America?;Four ways;Flashers;Moonies;Out-the-ways;;;;3
What’s the most common antifreeze colour used in a classic car;Blue;Orange;Red;Clear;;;;3
What should you do if your radiator water is orange / brown?;Flush it to remove rust;Fill it with oil to coat the inside;Run the engine with the radiator cap off;Ignore it;;;;3
What is the function of the thermostat?;It enables the engine to warm up more quickly;It cools down the engine;It increases the fuel/air mixture;It decreases the fuel/air mixture;;;;3
What is the most common grade of oil that classic cars use?;20/50;10/60;50/50;20/20;;;;3
What does a throttle body do?;It regulates air into the fuel injection system;It feeds petrol into the carburettor;It increases the petrol flow to the engine;It makes the accelerator pedal easier to push down;;;;3
What effect does Ethanol have on a classic car?;It perishes fuel pipe rubber;✓It causes steel components to rust;It cleans the engine;It stops bodywork rust;;;;3
Which of these are used for car interiors?;All;✓Leather;✓Vinyl;✓Cloth;✓Wood;✓Plastic;✓Velour;3
How old does a car have to be to attain historic status;40 years old;21 years old;18 years old;45 years old;;;;3
Why wrap an exhaust?;It keeps the engine bay cooler and makes the engine run more efficiently;It stops exhaust fumes coming into the passenger area;It reduces emissions;It makes the car easier to start in winter;;;;3
What is the purpose of a carburettor?;It regulates fuel and air into the engine;It regulates petrol being fed into the engine;It regulates air being fed into the engine;It acts as a petrol filter to remove impurities;;;;3
Why are twin carburettors more efficient than one single unit?;They deliver fuel to specific cylinders rather than all of them;They deliver more fuel into the engine;They enable one to work if the other fails;You can shine them and impress your friends;;;;3
What’s the purpose of an engine fan?;It cools the water flowing around the engine;It stops leaves getting into the engine bay;It acts as a turbo to force air into the engine;It provides cool air for the heater;;;;3
What is commonly referred to as the ‘nut behind the wheel’?;The driver;The horn;The gearstick;The pistachios you eat when going on a long trip;;;;3
What is the purpose of a limited slip differential?;It allows one wheel to keep traction whilst the other wheel is spinning;It enables you to drive in deep snow;It’s a form of anti-lock braking;It’s a car seat support to protect the passenger’s back;;;;3
What is a torque wrench used for?;It’s used to tighten components to a value pre-determined in the vehicle technical manual;It increases the torque of a vehicle;It converts torque into brake horsepower;To hit a seized component;;;;3
Which was the first car manufacturer to fit seat belts as standard?;Volvo;BMW;Audi;Little Tikes;;;;3
What does a speedo cable usually fit between?;The gearbox and speedometer;The front axle and the wheel;The rear axle and the wheel;The gearbox and the clutch;;;;3
What brake fluid does a classic car usually use?;Dot 4;Dot 1;Dot 2;Dot Cotton;;;;3
What are remoulds?;A worn tyre that has been given a new tread;A worn tyre that is used on the race track;A tyre used for crash barriers at a race circuit;Cheap second-hand tyres;;;;3
What is the secret for installing new glass into a classic car?;Use a strong, thin cable in the window channel;Get someone else to do it;Complete a one month weight training course;Put oil around the edge of the glass;;;;3
Should a fuel pump be installed near the fuel tank or under the bonnet?;Near the fuel tank;✓Under the bonnet;Under the dashboard;;;;;3
What is the name of the clips used to attach car seat material to the car seat frame?;Hog rings;Omega rings;Staples;Bodgers;;;;3
Which of these are a form of welding?;All;✓MIG;✓TIG;✓Arc;✓Flux Cored, Gas;;;3
Which of these are a type of car paint?;All;✓Cellulose;✓2-pack;✓Acrylic;;;;3
What type of antifreeze should you use in a Type 2 VW Camper Van?;None, it’s air cooled;Blue;Orange;Clear;;;;3
Where would you find a gudgeon pin?;In the engine;In the radiator;In the gearbox;In a haystack;;;;3
What component did the alternator replace?;The dynamo;Distributor points;Horn relay;Advance and retard lever;;;;3".Split('\n');
            //string[] lines = System.IO.File.ReadAllLines(Utilities.getDefaultLocation(@"Resources\TechTime.tsv"));
            string[] columns;
            foreach (string line in lines)
            {
                try
                {
                    columns = line.Split(';');
                    if (columns.Length >= 5)
                    {
                        // Question is column[0]
                        List<(string, bool)> answers = new List<(string, bool)>(); 
                        for (int i = 1; i < columns.Length - 1; i++)
                        {
                            bool right = false;
                            if (i == 1)
                            {   // First answer is the right one
                                right = true;
                            }
                            if (columns[i].Length != 0 && columns[i][0].Equals('✓'))
                            {   // Answers prefixed with ✓ are also true
                                columns[i] = columns[i].Substring(1);
                                right = true;
                            }
                            answers.Add((columns[i], right));
                        }
                        techTimeCards.Add(new TechTimeCard(0, columns[0],
                            int.Parse(columns[columns.Length-1]),
                            answers));
                    }
                }
                catch (Exception ex)
                {

                }
            }
            techTimeIndex = Utilities.GetRandom(0, techTimeCards.Count);
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
