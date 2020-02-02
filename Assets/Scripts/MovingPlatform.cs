using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float bottom_position = 0.0f;
    public float top_position = 1.0f;
    public bool moving_up = true;
    public bool can_switch = false;
    public float time_switch = 2.0f;

    float max_distance = 0;

    void Start()
    {
        max_distance = top_position - bottom_position;
    }

    // Update is called once per frame
    void Update()
    {
       
            
    }

    public IEnumerator ActivatePlatform()
    {
        can_switch = false;
        Debug.Log("Pre 5 secs");
        yield return new WaitForSeconds(time_switch);
        Debug.Log("Post 5 secs");


        if (moving_up)
        {
            Debug.Log("Going up");
            LeanTween.cancel(gameObject);
            //Debug.Log(top_position);
            transform.LeanMoveY(top_position, speed*(Mathf.Abs(top_position - transform.position.y)/ max_distance) );
            moving_up = false;
        }
        else
        {
            LeanTween.cancel(gameObject);
            //Debug.Log(bottom_position);
            transform.LeanMoveY(bottom_position, speed * (Mathf.Abs( bottom_position- transform.position.y )/ max_distance ));
            moving_up = true;
        }
        can_switch = true;

    }

}
