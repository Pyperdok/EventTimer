using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTimer.Source.Model
{
    class EventTimerConfig
    {
        private ModelEventTimer[] _timers;
        public EventTimerConfig(ModelEventTimer[] timers)
        {
            _timers = timers;
        }

        public void Save()
        {

        }

        public void Load()
        {

        }
    }
}
