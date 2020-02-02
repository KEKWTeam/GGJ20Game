using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotacion : MonoBehaviour
{
    float degrees = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        degrees+=10;
        transform.eulerAngles = new Vector3(1,1 ,1 * degrees ) ;
    }
}
