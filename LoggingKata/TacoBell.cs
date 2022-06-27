using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingKata
{
    public class TacoBell : ITrackable
    {
        public string Name { get; set; }
        public Point Location { get; set; }

        public TacoBell()
        {
            Console.WriteLine("live mas");
        }

        public TacoBell(string name, Point location)
        {
            Name = name;
            Location = location;
        }   
    }
}
