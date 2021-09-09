using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    //References
    private Rigidbody rb;
    private Camera cam;
    

    //Transforms on equip
    private Vector3 targetPistolPosition = new Vector3(4.8499999f,-0.829999983f,4.72495842f);
    private Vector3 targetPistolRotation = new Vector3(270f,180f,0f);
    private Vector3 targetPistolScale = new Vector3(19.9762573f,17.7629871f,10f);

    //Parameters
    private bool isEquipped;
    public float throwForce;
    public float shootForce;

    //Input
    private bool LMBPressed;
    private Vector3 mousePos;
    private RaycastHit hit;
    private Ray mouseRay;




    public void Equip(Vector3 targetPosition, Transform player)
    {
        transform.SetParent(player);

        rb.isKinematic = true;

        transform.localPosition = targetPistolPosition;
        transform.localEulerAngles = targetPistolRotation;
        transform.localScale = targetPistolScale;

        isEquipped = true;
    }

    public void Unequip(Transform worldObj)
    {
        transform.SetParent(worldObj);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        isEquipped = false;
    }

    private void GetInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            LMBPressed = true;
        }
        else
        {
            LMBPressed = false;
        }

        mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay,out hit))
        {
            Debug.DrawLine(transform.position,hit.point);
            Debug.Log(hit.collider.name);
        }
    }

    private void Shoot()
    {
        Rigidbody rbObj = hit.collider.transform.GetComponent<Rigidbody>();
        if(rbObj != null)
        {
            rbObj.AddForce(mouseRay.direction.normalized * shootForce,ForceMode.Impulse);
            Debug.Log("Obj found, " + mouseRay.direction * shootForce);
        }


        Debug.Log("Fired");
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Update()
    {
        GetInput();

        if(isEquipped && LMBPressed)
        {
            Shoot();
        }
    }
}
