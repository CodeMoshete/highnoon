using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Entity/List", order = 1)]
public class MapEntry : ScriptableObject
{
    public string ResourceName;
    public string EntryName;
}
