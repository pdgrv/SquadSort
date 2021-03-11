using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitType _type;

    public UnitType Type => _type;
}

public enum UnitType
{
    Soldier,
    Archer,
    Knight
}