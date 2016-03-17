using System;
using System.Collections.Generic;
using System.Threading;

namespace FallingSymbols
{
    class Program
    {

        static List<Symbol> FireworkList   = new List<Symbol>(); // Create once symbol list Instance 
        static Random       RandomInstanse = new Random();       // Create once Random Instance 


        static void Main(string[] args)
        {
            Console.BufferHeight = 55;       //* Set Console Height
            Console.WindowHeight = 55;       //*

            Thread  firework_show    = null; // Head Thread of Falling Show
            Thread  replacement_Item = null; // Replace removed Symbol
            int     itemNumber       = 0;    // Lenght of Symbols List
            int     oldSize          = 0;    // Old Symbols List Size

            Console.Write("Enter number of item: ");

            itemNumber = Convert.ToInt32(Console.ReadLine());

            while (itemNumber > 127001 || itemNumber < 1)
            {
                Console.WriteLine("Size invalid!");
                Console.Write("Enter number of item: ");

                itemNumber = Convert.ToInt32(Console.ReadLine());
            }

            Console.Clear();

            while (FireworkList.Count != itemNumber)
            {
                FireworkList.Add(new Symbol());
            }

            firework_show = new Thread(Program.StartShow);
            firework_show.Start(FireworkList);

            oldSize = Program.FireworkList.Count;

            while (true)
            {
                if (Program.FireworkList.Count != oldSize)
                {
                    FireworkList.Add(new Symbol());

                    replacement_Item = new Thread(FireworkMoove);
                    replacement_Item.Start(FireworkList[FireworkList.Count - 1]);
                }
            }

        }


        static void FireworkMoove(Object obj)
        {
            Symbol item = obj as Symbol;

            while (item.life)
            {
                lock (typeof(Console))
                {
                    Console.SetCursorPosition(item.coord_x, item.coord_y);
                    Console.Write(" ");

                    if (Console.CursorTop == Console.WindowHeight - 3)
                    {
                        item.life = false;

                        //item.coord_y = 1;
                    }
                    else
                    {
                        item.coord_y++;

                        Console.SetCursorPosition(item.coord_x, item.coord_y);
                        Console.ForegroundColor = item.color;
                        Console.WriteLine(item.content);
                        Console.ResetColor();
                    }
                }

                Thread.Sleep(item.speed);

            }

            if (!item.life)
            {
                Program.FireworkList.Remove(item);
            }

        }


        static void StartShow(Object list)
        {
            List<Symbol> firework_list = list as List<Symbol>;
            foreach (Symbol item in firework_list)
            {
                Thread forOne = new Thread(Program.FireworkMoove);
                forOne.Start(item);
            }
        }


        public static ConsoleColor GetRandomConsoleColor()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)consoleColors.GetValue(RandomInstanse.Next(consoleColors.Length));
        }


        public class Symbol
        {

            public int speed { set; get; }

            public bool life { set; get; }

            public int coord_x { set; get; }

            public int coord_y { set; get; }

            public char content { set; get; }

            public ConsoleColor color { set; get; }

            public Symbol()
            {
                this.color = GetRandomConsoleColor();
                this.life = true;
                this.coord_x = RandomInstanse.Next(2, 60);
                this.coord_y = 1;
                this.speed = RandomInstanse.Next(200, 350);
                this.content = (char)RandomInstanse.Next(1, 255);
            }

        }

    }
}
