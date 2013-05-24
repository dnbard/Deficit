using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deficit.Events
{
    /// <summary>
    /// One time event.
    /// </summary>
    class EventComponent : DrawableGameComponent
    {
        /// <summary>
        /// Time when event must fire
        /// </summary>
        public virtual TimeSpan FireTime { get; set; }
        public Action<object> Action { get; set; }
        public object Argument { get; set; }

        protected readonly List<DrawableGameComponent> _list;

        public EventComponent(List<DrawableGameComponent> list)
            : base(Program.Game)
        {
            _list = list;
        }

        public override void Update(GameTime gameTime)
        {
            var time = gameTime.TotalGameTime;
            if (time > FireTime)
            {
                if (Action != null) Action(Argument);
                _list.Remove(this);
            }
        }
    }
}
