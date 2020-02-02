using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour

{

    public AudioClip steps;
    public AudioClip jump;
    public AudioClip palanca;
    public AudioClip repair;
    AudioSource audio_source;
    
    Rigidbody2D rb;
    Animator animator;
    GameObject attached_object;

    SpriteRenderer sprite; 

    public LayerMask mask;

    public GameObject rb2;
    public Camera camera;
    public MovingPlatform movingPlatform;
    private HudScript hud = null;

    GameController game_controller = null;

    public float jumpforce = 15;
    public float player_speed = 5;
    public float diversificador_tiempo;
    public float attached_offset;
    public bool can_move = true;

    bool can_jump = true;
    bool alive;
    bool can_hold = true;
    bool can_fix = false;
    bool can_throw = false;
    bool throwing = false; 
    bool looking_right = false;
    bool attached = false;

    float time = 0;
    float offset_anim = 0.05f;
    private float time_switch = 2.0f;
    float current_switch_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        movingPlatform = GameObject.FindGameObjectWithTag("Platform1").GetComponent<MovingPlatform>();
        rb = GetComponent<Rigidbody2D>();
        GameObject hud_go = GameObject.FindGameObjectWithTag("GameController");
        if (hud_go)
        {
            hud = hud_go.GetComponent<HudScript>();
            game_controller = hud.gameObject.GetComponent<GameController>();
        }
        animator = GetComponent<Animator>();
        alive = true;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        sprite = GetComponent<SpriteRenderer>();
}

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        //LifeCounter();
        if (alive)
        {
            
            hud.WriteDeaths();
            if (can_move){
                Movement();
                Mechanism();
            }
            else
            {
                current_switch_time += Time.deltaTime;
                if(current_switch_time > time_switch)
                {
                    audio_source.PlayOneShot(palanca);

                    can_move = true;
                    current_switch_time = 0.0f;
                }
            }
            CameraFollow();
            Attachements();
        }

        if (time > diversificador_tiempo && alive)
        {
            if (attached_object)
            {
                attached_object.GetComponent<BoxCollider2D>().enabled = true;
            }
            attached = false;
            attached_object = null;

            alive = false;
            if(animator)
                
                animator.SetBool("is_dead", true);



            if (hud != null)
            {
                hud.OnPlayerDeath();
            }
            else
            {
                Debug.LogError("No existe HudScript");
            }

            gameObject.layer = 11;

            game_controller.SpawnPlayer();

            zoomIn();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }


    //Controles

    void Movement() {

        RaycastHit2D hit = Physics2D.Raycast(GetComponent<BoxCollider2D>().bounds.center, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f, mask);
        if(hit.collider != null)
            //Debug.Log(hit.collider.name);
        if (hit.collider != null && hit.collider.tag == "Ground" || hit.collider.tag == "Platform1")
        {
            can_jump = true;
            animator.SetBool("grounded", true);
        }
        else
        {
            animator.SetBool("grounded", false);
        }

       

        Vector2 movement = Vector2.zero;

        movement.x = Input.GetAxis("Horizontal");

        if (movement.x < -0.1)
        {
            sprite.flipX = true;
            audio_source.PlayOneShot(steps);
        }
        else if (movement.x > 0.1)
        {
            sprite.flipX = false;
            audio_source.PlayOneShot(steps);
        }

        if (Input.GetKeyDown("space") && can_jump)
        {
            can_jump = false;
            animator.SetTrigger("jump");
            rb.AddForce(Vector2.up * jumpforce);
            audio_source.PlayOneShot(jump);
        }

        if (animator)
            animator.SetFloat("speed", Mathf.Abs(movement.x));
        rb.AddForce(Vector2.right * movement.x * 100);
        if (rb.velocity.x > player_speed)
        {
            Vector2 new_vel = rb.velocity;
            new_vel.x = player_speed;
            rb.velocity = new_vel;
        }
        else if(rb.velocity.x < -player_speed)
        {
            Vector2 new_vel = rb.velocity;
            new_vel.x = -player_speed;
            rb.velocity = new_vel;
        }




    }

    void Mechanism() {


        if (Input.GetKeyDown(KeyCode.Z)) {

            if (attached_object)
            {
                FindNearBrokeObjetc();
            }
            else
            {
                HoldNearObjetc();
            }

             if (can_fix)
            {
                if (hud)
                {
                    hud.FixRoto();
                    Debug.Log("Fixed");
                }
                attached = false;
                can_fix = false; 
            }

        } 
    }


    void HoldNearObjetc() {

        GameObject[] objetos = GameObject.FindGameObjectsWithTag("material");

        if (objetos.Length > 0) {

            //Debug.Log(objetos.Length);

            GameObject closest = objetos[0];

            float pdistancia = 9999999;
            float distancia;
            float distancia2;

            foreach (GameObject go in objetos)
            {

                distancia = (transform.position - go.transform.position).magnitude;

                if (distancia < 2)
                {
                    if ((distancia < pdistancia))
                    {

                        pdistancia = distancia;
                        closest = go;

                    }
                }

            }
            distancia2 = (transform.position - closest.transform.position).magnitude;
            if (distancia2 > 2)
            {
                attached = false;
                attached_object = null;
            }
            else
            {
                attached = true;
                attached_object = closest;
            }
        }
        else
        {
            attached = false;
            attached_object = null;
        }

       


       // Debug.Log(closest);
    }

    void FindNearBrokeObjetc()
    {
        attached = false;

        GameObject[] objetos = GameObject.FindGameObjectsWithTag("roto");
        if (objetos.Length > 0)
        {
            GameObject closest = objetos[0];


            float distancia;
             foreach (GameObject go in objetos)
             {
                 print(go.name);
                
                 distancia = Vector2.Distance(go.transform.position, transform.position);
                 Debug.Log(distancia);
                 if (distancia < 2)
                 {
                    audio_source.PlayOneShot(repair);
                     Debug.Log("SHould repair");
                     can_fix = true;
                     go.GetComponent<BrokenTile>().setRepairedPiece(attached_object);
                     return;
                 }

             }
            
        }


        //Debug.Log(closest);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2);
    }

    void AttachObjectToObject() {

        
            Vector2 pos = transform.position;

            if (Input.GetAxis("Horizontal") > 0)
            {

                pos.x = pos.x + attached_offset;
            }
            if(Input.GetAxis("Horizontal") < 0)
            {
                //Debug.Log(attached_offset);
                pos.x = pos.x + -attached_offset;
            }
            attached_object.GetComponent<BoxCollider2D>().enabled = false;
            attached_object.transform.position = pos;

        

    }

    void Attachements() {

        if (!attached) {
            if (attached_object != null) {
                attached_object.GetComponent<BoxCollider2D>().enabled = true;
                animator.SetBool("atached", false);
                attached_object = null;
            }
        }
        else 
        {
            AttachObjectToObject();
            animator.SetBool("atached", true);
        }
    
    }

    void CameraFollow()
    {

        GameObject[] player = GameObject.FindGameObjectsWithTag("MainCamera");
        Vector3 offset;
        offset.x = 0; 
        offset.y = 0; 
        player[0].transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, -10); // Camera follows the player with specified offset position

    }


    void zoomIn()
    {
        camera.orthographicSize = 7.0f;
        LeanTween.value(camera.gameObject, camera.orthographicSize, 5f, 7f).setOnUpdate((float flt) => {
            camera.orthographicSize = flt;
        });
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Switch")
        {
            movingPlatform.can_switch = true;
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Switch")
        {
            if (Input.GetKeyDown(KeyCode.X) && alive && movingPlatform.can_switch)
            {
                movingPlatform.can_switch = false;

                animator.SetTrigger("lever");
                can_move = false;
                StartCoroutine(movingPlatform.ActivatePlatform());
                //can_move = true;
            }
        }

    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Switch")
        {
            movingPlatform.can_switch = false;
        }

    }
}
