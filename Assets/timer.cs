using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{

    private float tiempo;  
    // Start is called before the first frame update
    void Start()
    {
        tiempo = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo >= 10) {
            SceneManager.LoadScene("Level1");
        }

    }
}
