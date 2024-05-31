using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CheatPanel
{
    public abstract class CheatPanelDropDownComponent : MonoBehaviour
    {
        public TMP_InputField Input;
        public TMP_Dropdown DropDown;

        public abstract void Start();
        public abstract void Change();

        public virtual bool IsCorrect(TMP_InputField input, out int amount)
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
