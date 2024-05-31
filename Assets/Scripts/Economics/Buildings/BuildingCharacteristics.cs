using System;

namespace Scripts.Economics.Buildings
{
    public abstract class BuildingCharacteristics
    {
        public int Level;
        public float CurrentFullness;
        public bool IsOpen;
        public string TimeText;
        public float CurrentTimerTime;
    }
}