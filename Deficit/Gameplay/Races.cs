using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Deficit.Gameplay
{
    class Races
    {
        Dictionary<string, Race> races = new Dictionary<string, Race>();

        private static Races _instance ;
        public static Races Instance
        {
            get
            {
                if (_instance == null) _instance = new Races();
                return _instance;
            }
        }

        private Races()
        {
            XDocument xml = XDocument.Load("XML\\races.xml");
            if (xml == null) throw new NullReferenceException("No races.xml found");

            foreach (var xElement in xml.Root.Elements())
            {
                var race = new Race();
                race.Name = xElement.Name.ToString();
                races.Add(race.Name, race);
            }
        }

        public static Race Get(string name)
        {
            return Instance.races.ContainsKey(name) ? Instance.races[name] : null;
        }

        public static Race Get(int num)
        {
            var races = Instance.races;
            if (races.Count <= num || num < 0) return null;
            int i = 0;
            foreach (var race in races)
            {
                if (i != num)
                {
                    i++;
                    continue;
                }
                return race.Value;
            }
            return null;
        }

        public static Race GetRandom()
        {
            var races = Instance.races;
            Random rnd = Program.Random;
            return Get(rnd.Next(0, races.Count));
        }
    }

    class Race
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
