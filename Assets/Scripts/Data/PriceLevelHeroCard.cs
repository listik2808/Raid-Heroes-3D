using System;

namespace Scripts.Data
{
    [Serializable]
    public class PriceLevelHeroCard
    {
        private int _levelZero = 1;
        private int _levelOne = 1;
        private int _levelTwo = 27;
        private int _levelThree = 729;
        private int _levelFour = 19683;
        private int _levelFive = 531441;

        public int GetPrice(int levelRang)
        {
            if(levelRang == -1)
                return _levelZero;
            if(levelRang == 0)
                return _levelOne;
            if(levelRang == 1)
                return _levelTwo;
            if(levelRang == 2)
                return _levelThree;
            if(levelRang ==3)
                return _levelFour;
            if(levelRang == 4)
                return _levelFive;
            return 0;
        }
    }
}