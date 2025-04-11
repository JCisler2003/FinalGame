using UnityEditor.Callbacks;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    static public Cowboy S{get; private set; }

    [Header("Inscribed")]
    public float speed = 10f;
    public float jumpForce = 5f;
    public float pitchMult = 30;
    public Animator anim;

    [Header("Dynamic")] [Range(0, 4)]

    private float _shieldLevel = 1; // remember the underscore
    [Tooltip( "This field holds a reference to the last triggering GameObject" )]
    private GameObject lastTriggerGo = null;
    private Rigidbody rb;
    private bool isGrounded = true;

    //public delegate void WeaponFireDelegate();
    //public event WeaponFireDelegate fireEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (S == null){
            S = this;
        }else{
            Debug.LogError("Attempted to assign second Cowboy.S");
        }

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        // float vAxis = Input.GetAxis("Vertical");

        Vector3 velocity = rb.linearVelocity;
        velocity.x = hAxis*speed;
        velocity.z = 0f;
        rb.linearVelocity = velocity;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("F key pressed - triggering Attack1");
            anim.SetTrigger("Attack1");
        }

        // Vector3 pos = transform.position;
        // pos.x += hAxis * speed * Time.deltaTime;
        // pos.y += vAxis * speed * Time.deltaTime;
        // transform.position = pos;
        
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        

        // make sure it's not the same triggering go as last time
        if ( go == lastTriggerGo ) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        //PowerUp pUp = go.GetComponent<PowerUp>();
        if (enemy != null) {
            _shieldLevel--;
            Destroy(go); 
            Destroy(this.gameObject);
            Main.HERO_DIED();
        }
        // else if (pUp != null) {    // if shield hit a powerup absorb the powerup
        //     AbsorbPowerUp(pUp);
        // }
        else {
            Debug.LogWarning("Shield trigger hit by non-Enemy: " +go.name);
        }
    }

    void OnCollisionEnter(Collision coll) {
    if (coll.gameObject.CompareTag("Ground")) {
        isGrounded = true;
        }
    }

}

