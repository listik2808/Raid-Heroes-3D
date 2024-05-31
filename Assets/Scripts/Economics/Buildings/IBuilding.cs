using Scripts.StaticData;

namespace Scripts.Economics.Buildings
{
    public interface IBuilding
    {
        public int Level { get; set; }
        public float CurrentFullness { get; set; }
        public float CurrentTimerTime { get; set; }
        public string TimeText { get; set; }
        public bool IsOpen { get; set; }
    }
}