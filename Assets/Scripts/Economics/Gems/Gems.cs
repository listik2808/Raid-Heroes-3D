[System.Serializable]
public class Gems
{
    public GreenGems Green;
    public OrangeGems Orange;
    public PurpleGems Purple;
    public Gems()
    {
        Green = new GreenGems();
        Orange = new OrangeGems();
        Purple = new PurpleGems();
    }
}