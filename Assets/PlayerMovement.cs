using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    Rigidbody2D rb;
    public float jumpforce = 1;
    public float player_speed = 5; 
    bool canjump = true;
    bool alive = true;
    float tiempo = 0;
    bool canhold = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        
    }

    // Update is called once per frame
    void Update()
    {
        LifeCounter();
        if (alive) {
            Movement();
            Mechanism();
        }


    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag); //Debug del objeto con el que choca 
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

        if (Input.GetKeyDown("Z") && canhold) {

            HoldNearObjetc();
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

        GameObject closest = objetos[0];

        float pdistancia;
        float distancia; 

        foreach (GameObject go in objetos) {

            distancia = (transform.position - go.transform.position).magnitude;

            if (distancia < pdistancia) {

                closest = go;
            
            }

        }
    
    }
}
