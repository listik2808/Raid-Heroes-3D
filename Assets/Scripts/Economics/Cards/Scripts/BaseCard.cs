using UnityEngine;

[CreateAssetMenu(fileName = "HeroCard", menuName = "Card/BaseCard")]
public class BaseCard : ScriptableObject
{
    public Sprite[] TypeIcons;
    public Color[] TypeColors;
    public Sprite[] TypeSprites;
}
