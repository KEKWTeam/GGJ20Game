using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    private int deaths = 0;
    public Text deathtext = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deathtext.text = "Muertes: " + deaths.ToString();
    }

    public void OnPlayerDeath()
    {
        deaths++;
    }
}
