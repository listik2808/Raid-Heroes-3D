using Scripts.Army.TypesSoldiers;
using Scripts.Logic;
using System;

namespace Scripts.Data
{
    [Serializable]
    public class PositionOnLevel
    {
        public string Level;
        public int NumberRaid;
        public PositionOnLevel( string initialLevel, int numberRaid) 
        {
            Level = initialLevel;
            NumberRaid = numberRaid;
        }
    }
}
