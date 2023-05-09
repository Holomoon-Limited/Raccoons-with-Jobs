using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string CardName;
    public int Power;
    public string CardDescription;
    public Sprite Image;
    public int ID;
    // public Effect Effect;
}
