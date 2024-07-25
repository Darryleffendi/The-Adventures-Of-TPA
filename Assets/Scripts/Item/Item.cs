using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int id;
    public Sprite itemImg;
    public int price;
    public BuffType[] buff;
    public float buffAmount;
    public string description;
}

public enum BuffType
{
    Health = 0,
    Mana = 1,
    Speed = 2
}