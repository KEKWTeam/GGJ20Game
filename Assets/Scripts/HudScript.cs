using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public void WriteDeaths()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene.Equals("Nivel1"))
        {
            GenerateData(deaths.ToString(), scene);
        }
        else if (scene.Equals("Nivel2"))
        {
            GenerateData(deaths.ToString(), scene);
        }
        else if (scene.Equals("Nivel3"))
        {
            GenerateData(deaths.ToString(), scene);
        }
        else
        {
            GenerateData("No hay escena o muertes", "Error");
        }

    }

    private void GenerateData(string dies, string scene)
    {
        string[] linesToWrite = { dies };
        System.IO.File.WriteAllLines(@"Assets\files\" + scene + ".txt", linesToWrite);
    }

    public void loadDeaths()
    {

    }
}
