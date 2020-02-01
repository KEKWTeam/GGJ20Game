using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    Rigidbody2D rb;
    public GameObject rb2;
    public float jumpforce = 1;
    public float player_speed = 5;
    public float diversificador_tiempo = 20;

    bool canjump = true;
    bool alive = true;
    float tiempo = 0;
    bool canhold = true;
    bool canfix = false;
    bool canthrow = false;

    bool throwing = false; 

    bool looking_right = false;

    public float attached_offset = 0.2f;
    float offset_anim = 0.05f;

    bool atached = false;

    GameObject attached_object;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;

        LifeCounter();
        if (alive) 
        {
            Movement();
            Mechanism();
        }

        if (tiempo > diversificador_tiempo)
        {
            tiempo = 0;
            alive = false;
            Destroy(rb);
            Destroy(GetComponent<BoxCollider2D>());
            Instantiate(rb2);
        }

        Attachements();

        ThrowingObjetcts();



    }

    void OnCollisionEnter2D(Collision2D col)
    {
     //   Debug.Log(col.gameObject.tag); //Debug del objeto con el que choca 
        canjump = true;
        if(col.gameObject.tag == "Ground"){ //Si el tag es Ground puede saltar 
            canjump = true;
        }
    }


    //Controles

    void Movement() {

        if (Input.GetKeyDown("space") && canjump)
        {
            canjump = false;

            rb.AddForce(Vector2.up * jumpforce);
        }

        Vector2 movement = Vector2.zero;

        movement.x = Input.GetAxis("Horizontal");

        transform.Translate((movement * player_speed) * Time.deltaTime);


    }

    void Mechanism() {


        if (Input.GetKeyDown(KeyCode.Z)) {

            if (attached_object) {
                FindNearBrokeObjetc();
            }

            if (canfix)
            { 
                Destroy(attached_object);
                atached = false; 
                canfix = false; 
            }
            else {
                HoldNearObjetc();
            }
            Debug.Log(canfix);
        }

        if (canthrow) {

            if (Input.GetKeyDown(KeyCode.F)) {
                ThrowAttachedObject();
            }
        
        }
    }




    //Mecanicas del juego


    void LifeCounter() {

        tiempo = tiempo + Time.deltaTime;

        if (tiempo >= 20) {
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

                distancia = (attached_object.transform.position - go.transform.position).magnitude;

                if (distancia < 1)
                {
                    canfix = true;
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
            else
            {
                pos.x = pos.x + -attached_offset;
            }

            obj.transform.position = pos;

        }

    }

    void Attachements() {

        if (!atached) {
            return;
        }
        else 
        {
            AtachObjectToObject(attached_object);
            canthrow = true; 

        }
    
    }

    void ThrowAttachedObject() {

        if (attached_object) {
            atached = false;
            throwing = true; 
           
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
                canthrow = false;
            }

        }
    }
}
