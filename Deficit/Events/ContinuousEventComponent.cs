using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deficit.Events
{
    class ContinuousEventComponent: EventComponent
    {
        /// <summary>
        /// Time interval between events.
        /// </summary>
        public override TimeSpan FireTime
        {
            get { return base.FireTime; }
            set { base.FireTime = value; }
        }

        public int FireCount { get; set; }
        private int _eventsFired = 0;

        private TimeSpan _lastEvent;

        public Action<object> EventEnd { get; set; }

        public ContinuousEventComponent(List<DrawableGameComponent> list) : base(list)
        {
            _lastEvent = Program.Game.GameTime;
            FireCount = 1;
        }

        public override void Update(GameTime gameTime)
        {
            var time = gameTime.TotalGameTime;
            if (time > FireTime + _lastEvent)
            {
                _lastEvent = time;
                _eventsFired++;

                if (Action != null) Action(Argument);

                if (_eventsFired >= FireCount)
                {
                    if (EventEnd != null) EventEnd(Argument);
                    _list.Remove(this);
                }
            }
        }

    }
}
