using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    public Vector2 force_direction = Vector2.zero;
    public float force_intensity = 1.0f;
    Vector2 final_force = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        final_force = force_direction.normalized * force_intensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
        if(rb)
        {
            rb.AddForce(final_force);
        }
    }
}
