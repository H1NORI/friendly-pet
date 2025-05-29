using UnityEngine;

[System.Serializable]
public class FoodItem
{
    public int id;
    public string name;
    public string descripion;
    public int price;
    public Sprite icon;
    public GameObject prefab;
    public int hungerBonus;
}