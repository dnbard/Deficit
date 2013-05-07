using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deficit.Gameplay
{
    class GameFact<T>
    {
        public T FactYouKnow { get; set; }
        public T Fact { get; set; }

        public bool IsLie
        {
            get { return IsKnown && FactYouKnow.Equals(Fact); }
        }

        public bool IsKnown { get; set; }

        public GameFact(T truth)
        {
            FactYouKnow = Fact = truth;
            IsKnown = true;
        }

        public GameFact(T truth, T lie)
        {
            FactYouKnow = lie;
            Fact = truth;
            IsKnown = false;
        }
    }
}
