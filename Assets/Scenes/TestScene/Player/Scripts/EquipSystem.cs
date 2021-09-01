using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EquipSystem
{
    public static bool isEquipped;

    public static void Equip(GameObject obj, Vector3 targetPosition)
    { 
        Debug.Log("Equipped " + obj.name);
        isEquipped = true;
    }

    public static void Unequip(GameObject obj)
    {
        Debug.Log("Unequipped " + obj.name);
        isEquipped = false;
    }
}
