using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    public float maxSpeed = 5f;
    public float accel = 5f;
    public float decel = 5f;

    [HideInInspector]
    public float speed = 0f;

    public Vector3 input;

    public Transform cam;
    public float turnSpeedSmooth = 0.05f;
    public float turnSmoothVelocity;

    private Vector3 cameraForward, characterPosition;

    public float jumpVelocity = 5f;

    bool isGrounded;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // Vector3 direction = input.normalized;

        if(input.magnitude >= 0.05f){
            
            // float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeedSmooth);

            // transform.rotation = Quaternion.Euler(0, angle, 0);

            if(speed < maxSpeed){
                speed += accel * Time.deltaTime;
            } else {
                //ensure slow speeds dont make char jitter
                speed = Mathf.Lerp(speed, maxSpeed, 0.1f);
            }


            Vector3 characterPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            Vector3 cameraForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);

            this.transform.LookAt(characterPosition + cameraForward);

            // transform.forward = direction;
        } else {
            speed -= decel * Time.deltaTime;
        }

        anim.SetFloat("speed", speed);
        anim.SetBool("isGrounded", isGrounded);

        if (speed < 0){
            speed = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            anim.SetTrigger("jump");
        }

        if(!isGrounded){
            Vector3 freeze = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            this.transform.eulerAngles = freeze;
        }
        
    }

    public void FixedUpdate(){
        CheckGround();
    }

    public void CheckGround(){
        if(Physics.CheckSphere(transform.position, 0.05f, groundLayer)){
            isGrounded = true;
            
        } else {
            isGrounded = false;
        }
    }

    public void Jump(){
        //Vector3 vel = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        //rb.velocity = vel;
        rb.AddForce(new Vector3(0, jumpVelocity, 0), ForceMode.Impulse);
        anim.applyRootMotion = false;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        // Gizmos.DrawLine(characterPosition, (characterPosition + cameraForward) * 100f);
        Gizmos.DrawLine(cam.transform.position, this.transform.position);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.forward); // Global Forward
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward);  // Local Forward
    }
}