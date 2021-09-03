using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EquipSystem
{
    public static bool isEquipped;

    private static Vector3 targetPistolPosition = new Vector3(4.8499999f,-0.829999983f,4.72495842f);
    private static Vector3 targetPistolRotation = new Vector3(270f,180f,0f);
    private static Vector3 targetPistolScale = new Vector3(19.9762573f,17.7629871f,10f);

    public static void Equip(GameObject obj, Vector3 targetPosition, Transform player)
    { 
        switch (obj.GetComponent<Collider>().name)
        {
            case "Pistol": obj.GetComponent<Pistol>().Equip(targetPosition,player); break;
        }
        
        Debug.Log("Equipped " + obj.name);
        isEquipped = true;
    }

    public static void Unequip(GameObject obj, Transform worldObj)
    {
        switch (obj.GetComponent<Collider>().name)
        {
            case "Pistol": obj.GetComponent<Pistol>().Unequip(worldObj); break;
        }

        Debug.Log("Unequipped " + obj.name);
        isEquipped = false;
    }

    static void UnequipPistol(GameObject obj, Transform worldObj)
    {
        obj.transform.SetParent(worldObj);
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * 2, ForceMode.Impulse);
    }
}
