using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{

    public float detectorRadius;
    public Transform worlddObj;
    private Collider[] objects;
    private GameObject equippedObj;

    void Start()
    {
        
    }

    void Update()
    {
        objects = Physics.OverlapSphere(transform.position,detectorRadius,Physics.AllLayers);
        foreach(var obj in objects)
        {
            if(obj.GetComponent<Collider>().tag == "canBeEquipped" && Input.GetKey(KeyCode.E) && !EquipSystem.isEquipped)
            {
                equippedObj = obj.gameObject;
                EquipSystem.Equip(equippedObj,transform.position,Camera.main.transform);


                break;
            }
           // Debug.Log(obj.GetComponent<Collider>().gameObject.name);
        }

        if(Input.GetKeyDown(KeyCode.Q) && EquipSystem.isEquipped )
        {
            EquipSystem.Unequip(equippedObj,worlddObj);
        }
    }
}
