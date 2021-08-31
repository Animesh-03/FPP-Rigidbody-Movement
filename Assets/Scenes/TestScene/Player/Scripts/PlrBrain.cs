using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Test
public class PlrBrain : MonoBehaviour
{
                                    //References
    private Rigidbody rb;
                                    //Input
    private float xInp;
    private float zInp;
    private Vector2 inpVect;
                                    //Ground Check
    private bool onGround;
    public int groundLayer;
    private Vector3 groundNormal;
    public float maxSlopeAngle;
    private bool cancellingGrounded;                                //Prevents the method being called every frame
                                            //Ground Movement
    public float walkForce;
    public float maxWalkSpeed;
    public float sprintForce;
    public float maxSprintSpeed;
    private bool sprinting;
    public float jumpForce;
    private bool jumpPressed;
    private Vector3 localVel;
    public float grdCtrMovement;
                                            //Aerial Movement
    public float aerialMoveForce;
    public float maxAerialSpeed;
    
                                                //Input Manager
    private void GetInp()
    {
        xInp = Input.GetAxisRaw("Horizontal");
        zInp = Input.GetAxisRaw("Vertical");
        inpVect = new Vector2(xInp,zInp);

        if(Input.GetKeyDown(KeyCode.LeftShift))         //Sprint
        {
            sprinting = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprinting = false;
        }

        if(Input.GetKey(KeyCode.Space))             //Jump
        {
            jumpPressed = true;
        }
        else
        {
            jumpPressed = false;
        }
    }
                                                //Ground Check
    private bool CheckMaxSlope(Vector3 normal)              //Determines if the collision surface's slope is under max slope
    {
        float angle = Vector3.Angle(Vector3.up,normal);
        return angle < maxSlopeAngle;
    }

    private void CancelGrounded()
    {
        onGround = false;
    }

    void OnCollisionStay(Collision other)                   //Called if collider stays after a collision (Ground Check)
    {
        int layer = other.gameObject.layer;
        if(layer != groundLayer) return;

        for(int i = 0; i < other.contactCount;i++)          //Checks all collisions of the player collider in the frame
        {
           Vector3 normal = other.contacts[i].normal;

            if(CheckMaxSlope(normal))
            {
                onGround = true;
                groundNormal = normal;
                cancellingGrounded = false;
                CancelInvoke(nameof(CancelGrounded));       //Cancels the CancelGrounded method if ground is found
            }
        }

        if(!cancellingGrounded)                                 // Invokes CancelGrounded Method after 5 frames;
        {
            cancellingGrounded = true;
            int shadowFrames = 5;
            Invoke(nameof(CancelGrounded),Time.deltaTime*shadowFrames);
        }
    }
 
    void GroundMovement()                               //Ground Movement
        {
            Vector3 direction = new Vector3(inpVect.x,0f,inpVect.y);
            if(!sprinting)
            {
                Walk(direction);
            }
            else if(sprinting)
            {
                Sprint(direction);
            }

            if(jumpPressed)
            {
                Jump();
            }

            void Walk(Vector3 direction)
            {
                if(rb.velocity.magnitude < maxWalkSpeed)                    //Adds force until maxWalkSpeed is reached
                {
                    rb.AddForce((transform.forward*direction.z + transform.right*direction.x)*walkForce*Time.deltaTime,ForceMode.VelocityChange);       //Walks in localSpace of player
                }
                WalkCounterMovement();
            }
            void WalkCounterMovement()
            {
                        //Moving without input                             or     Moving right with left Input            or         Moving left with right Input
                if((Mathf.Abs(localVel.x) > 0.01f && Mathf.Abs(inpVect.x) < 0.01f) || (localVel.x > 0.01f && inpVect.x < -0.01f ) || (localVel.x < -0.01f && inpVect.x > 0.01f))
                {
                    rb.AddForce(transform.right * -localVel.x * grdCtrMovement * walkForce * Time.deltaTime,ForceMode.VelocityChange);      //Applies force in the opposite direction
                }

                if((Mathf.Abs(localVel.z) > 0.01f && Mathf.Abs(inpVect.y) < 0.01f) || (localVel.z > 0.01f && inpVect.y < -0.01f) || (localVel.z < -0.01f && inpVect.y > 0.01f) )
                {
                    rb.AddForce(transform.forward * -localVel.z * grdCtrMovement * walkForce * Time.deltaTime,ForceMode.VelocityChange);
                }
            }

            void Sprint(Vector3 direction)
            {
                if(rb.velocity.magnitude < maxSprintSpeed)
                {
                    rb.AddForce((transform.forward*direction.z + transform.right*direction.x)*sprintForce*Time.deltaTime,ForceMode.VelocityChange);         //Sprints in localSpace of player
                }
                SprintCounterMovement();
            }

            void SprintCounterMovement()
            {
                                //Moving without input                             or     Moving right with left Input            or         Moving left with right Input
                if((Mathf.Abs(localVel.x) > 0.01f && Mathf.Abs(inpVect.x) < 0.01f) || (localVel.x > 0.01f && inpVect.x < -0.01f ) || (localVel.x < -0.01f && inpVect.x > 0.01f))
                {
                    rb.AddForce(transform.right * -localVel.x * grdCtrMovement * sprintForce * Time.deltaTime,ForceMode.VelocityChange);      //Applies force in the opposite direction
                }

                if((Mathf.Abs(localVel.z) > 0.01f && Mathf.Abs(inpVect.y) < 0.01f) || (localVel.z > 0.01f && inpVect.y < -0.01f ) || (localVel.z < -0.01f && inpVect.y > 0.01f))
                {
                    rb.AddForce(transform.forward * -localVel.z * grdCtrMovement * sprintForce * Time.deltaTime,ForceMode.VelocityChange);
                }
            }

            void Jump()
            {
                rb.AddForce(Vector3.up * jumpForce * Time.deltaTime,ForceMode.Impulse);
            }
        }

    void AerialMovement()
    {
        Vector3 direction = new Vector3(inpVect.x,0f,inpVect.y);
        float planeVel = Mathf.Sqrt(Mathf.Pow(rb.velocity.x,2) + Mathf.Pow(rb.velocity.z,2));

        if(planeVel < maxAerialSpeed)
        {
            rb.AddForce((transform.forward*direction.z + transform.right*direction.x)*aerialMoveForce*Time.deltaTime,ForceMode.VelocityChange);
        }

    }

    void GetLocalVelocity()
    {
        localVel = transform.InverseTransformDirection(rb.velocity);
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInp();
        GetLocalVelocity();
        if(onGround)
        {
            GroundMovement();
        }
        else if(!onGround)
        {
            AerialMovement();
        }
    }
}
