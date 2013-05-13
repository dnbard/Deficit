using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Deficit.Gameplay
{
    class ShipsManager
    {
        private static ShipsManager _instance;
        private static ShipsManager Instance
        {
            get { return _instance ?? (_instance = new ShipsManager()); }
        }

        private XDocument ships;

        private ShipsManager()
        {
            ships = XDocument.Load("XML//ships.xml");
            if (ships == null) throw new NullReferenceException("XML/ships.xml is corrupted");
        }

        public static XElement Get(string shipType)
        {
            if (string.IsNullOrEmpty(shipType)) throw new ArgumentNullException("shipType");

            return Instance.ships.Root.Elements().
                FirstOrDefault(element => element.Element("name").Value.ToString().
                    Equals(shipType, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
