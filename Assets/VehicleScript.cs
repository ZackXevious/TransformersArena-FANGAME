using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour
{
    public GameObject[] Wheelmeshes;
    public float AccellRate;
    public float MaxSpeed;
    public Rigidbody rb;
    public float turnSpeed;
    public float CenterOfGravityheight;
    [Header("Overrides")]
    public GameObject chest;
    public GameObject shoulder;
    public GameObject legs;
    public GameObject feet;
    [Header("Kibble")]
    public GameObject chestKibble;
    public GameObject backKibble;
    public GameObject shoulderKibble;
    public GameObject armKibble;
    public GameObject calfKibble;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate() {
        if (Physics.Raycast(transform.position, -transform.up, .35f)) {
            var locVel = transform.InverseTransformDirection(rb.velocity);
            float steer = Input.GetAxis("Horizontal");
            float accell = Input.GetAxis("Vertical");
            if (Mathf.Abs(steer) > .1f) {
                rb.angularVelocity = new Vector3(0.0f, steer* turnSpeed, 0.0f);
                //rb.AddRelativeTorque(new Vector3(0.0f,  * 6000, 0.0f));
            }
            if (accell > 0.1f) {
                if (locVel.z < MaxSpeed) {
                    locVel.z += (AccellRate * accell) * Time.deltaTime;
                }
            } else if (accell < -0.1f) {
                locVel.z = AccellRate * accell;
            }
            locVel.x = locVel.x/2;
            rb.velocity = transform.TransformDirection(locVel);
        } else {
            if (Physics.Raycast(transform.position, transform.up, .5f)) {

                //.transform.rotation.eulerAngles=new Vector3(0.0f,0.0f,0.0f);
            }
        }


        /*if (wheelcolliders[3].isGrounded && wheelcolliders[2].isGrounded) {
            
        }*/

    }
}
