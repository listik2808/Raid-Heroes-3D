using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CheatPanel
{
    public abstract class CheatPanelComponent: MonoBehaviour
    {
        public InputField Input;
        public Text Text;

        public abstract void Start();
        public abstract void Change();

        public virtual bool IsCorrect(InputField input, out int amount)
        {
            amount = 0;
            if (string.IsNullOrEmpty(input.text) == false && int.TryParse(input.text, out int value))
            {
                amount = value;
                return true;
            }

            return false;
        }
    }
}
