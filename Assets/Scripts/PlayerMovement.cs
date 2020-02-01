using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour

{
    Rigidbody2D rb;
    Animator animator;
    GameObject attached_object;

    public LayerMask mask;

    public GameObject rb2;
    public Camera camera;
    public MovingPlatform movingPlatform;
    private HudScript hud = null;

    public float jumpforce = 15;
    public float player_speed = 5;
    float diversificador_tiempo = 10;
    public float attached_offset = 0.2f;
    public bool can_move = true;

    bool can_jump = true;
    bool alive;
    bool can_hold = true;
    bool can_fix = false;
    bool can_throw = false;
    bool throwing = false; 
    bool looking_right = false;
    bool atached = false;

    float time = 0;
    float offset_anim = 0.05f;
    public float time_switch = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject hud_go = GameObject.FindGameObjectWithTag("GameController");
        if(hud_go)
            hud = hud_go.GetComponent<HudScript>();
        animator = GetComponent<Animator>();
        alive = true;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        //LifeCounter();
        if (alive)
        {
            if (can_move){
                Movement();
                Mechanism();
            }
            CameraFollow();
        }

        if (time > diversificador_tiempo && alive)
        {
            time = 0;
            alive = false;
            if(animator)
                animator.SetBool("isDead", true);

            if (atached)
            {
                atached = false;
                Attachements();
            }
            if (hud != null)
            {
                hud.OnPlayerDeath();
            }
            else
            {
                Debug.LogError("No existe HudScript");
            }

            Destroy(GetComponent<BoxCollider2D>());
            Destroy(rb);
            
            GameObject new_robot = Instantiate(rb2);
            new_robot.GetComponent<BoxCollider2D>().enabled = true;
            new_robot.GetComponent<PlayerMovement>().can_move = true;
            zoomIn();
        }

        Attachements();

        //ThrowingObjetcts();



    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }


    //Controles

    void Movement() {

        RaycastHit2D hit = Physics2D.Raycast(GetComponent<BoxCollider2D>().bounds.center, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f, mask);
        if(hit.collider != null)
            Debug.Log(hit.collider.name);
        if (hit.collider != null && hit.collider.tag == "Ground")
        {
            can_jump = true;
            animator.SetBool("grounded", true);
        }
        else
        {
            animator.SetBool("grounded", false);
        }

        if (Input.GetKeyDown("space") && can_jump)
        {
            can_jump = false;

            rb.AddForce(Vector2.up * jumpforce);
        }

        Vector2 movement = Vector2.zero;

        movement.x = Input.GetAxis("Horizontal");

        if(animator)
            animator.SetFloat("speed", Mathf.Abs(movement.x));
        transform.Translate((movement * player_speed) * Time.deltaTime);


    }

    void Mechanism() {


        if (Input.GetKeyDown(KeyCode.Z)) {

            if (attached_object) {
                FindNearBrokeObjetc();
            }

            if (can_fix)
            { 
                Destroy(attached_object);
                hud.FixRoto();
                atached = false;
                can_fix = false; 
            }
            else {
                HoldNearObjetc();
            }
            Debug.Log(can_fix);
        }

        if (can_throw) {

            if (Input.GetKeyDown(KeyCode.F)) {
                ThrowAttachedObject();
            }
        
        }
    }




    //Mecanicas del juego


    void LifeCounter() {

        time = time + Time.deltaTime;

        if (time >= 20) {
            alive = false;
            ChangePlayer();
        }
    
    }

    void ChangePlayer() {


        ///Body
        ///

        alive = true; 
    
    }

    void HoldNearObjetc() {

        GameObject[] objetos = GameObject.FindGameObjectsWithTag("material");

        if (objetos.Length > 0) {

            GameObject closest = objetos[0];

            float pdistancia = 9999999;
            float distancia;
            float distancia2;

            foreach (GameObject go in objetos)
            {

                distancia = (transform.position - go.transform.position).magnitude;

                if (distancia < 1)
                {
                    if ((distancia < pdistancia))
                    {

                        pdistancia = distancia;
                        closest = go;

                    }
                }

            }

            atached = !atached;

            distancia2 = (transform.position - closest.transform.position).magnitude;


            if (distancia2 > 2)
            {
                atached = false;
            }
            if (!closest)
            {
                attached_object = null;

            }
            else
            {
                attached_object = closest;
            }

        }

       


       // Debug.Log(closest);
    }

    void FindNearBrokeObjetc()
    {

        GameObject[] objetos = GameObject.FindGameObjectsWithTag("roto");

        GameObject closest = objetos[0];

        float distancia;

        if (objetos.Length > 0) {
            foreach (GameObject go in objetos)
            {
                print(attached_object);
                print(can_fix);

                distancia = (attached_object.transform.position - go.transform.position).magnitude;

                if (distancia < 2)
                {
                    can_fix = true;
                }

            }
        }



        //Debug.Log(closest);
    }

    void AtachObjectToObject(GameObject obj) {

        if (obj) {
            Vector2 pos = transform.position;

            if (Input.GetAxis("Horizontal") > 0)
            {

                pos.x = pos.x + attached_offset;
            }
            if(Input.GetAxis("Horizontal") < 0)
            {
                pos.x = pos.x + -attached_offset;
            }
            obj.GetComponent<BoxCollider2D>().enabled = false;
            obj.transform.position = pos;

        }

    }

    void Attachements() {

        if (!atached) {
            if (attached_object) {
                attached_object.GetComponent<BoxCollider2D>().enabled = true;
            }

            return;
        }
        else 
        {
            AtachObjectToObject(attached_object);
            can_throw = true; 

        }
    
    }

    void ThrowAttachedObject() {

        if (attached_object) {
            atached = false;
            throwing = true;

            Vector2 throwforce = Vector2.zero;

            throwforce.y = 500;
            throwforce.x = 50.3f;

            attached_object.GetComponent<Rigidbody2D>().AddForce(throwforce);
            can_throw = false;
        }
    
    }

    void ThrowingObjetcts() {

        if (!throwing)
        {
            return;

        }
        else {
            Vector2 throwforce = Vector2.zero;

            throwforce.y = 1.2f;
            throwforce.x = 2.3f;

            if (attached_object) {
                attached_object.transform.Translate(throwforce * Time.deltaTime);
                can_throw = false;
            }

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

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Switch")
        {
            movingPlatform.can_switch = true;
            if (Input.GetKeyDown(KeyCode.X) && alive)
            {
                can_move = false;
                StartCoroutine(movingPlatform.ActivatePlatform());
                //can_move = true;
                StartCoroutine(BlockPlayer5Sec());
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


    public IEnumerator BlockPlayer5Sec()
    {
        yield return new WaitForSeconds(time_switch);
        can_move = true;
    }
}
