using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HudScript : MonoBehaviour
{
    private int deaths = 0;
    private int fixs = 0; 
    public Text deathtext = null;

    private string scene1 = "Level1";
    private string scene2 = "Level2";
    private string scene3 = "Level3";

    public Text fixtext = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deathtext.text = ": " + deaths.ToString();

        UpdateFixes();

        CheckFixes();
    }

    public void OnPlayerDeath()
    {
        deaths++;
    }

    public void WriteDeaths()
    {
        string scene = SceneManager.GetActiveScene().name;
        Debug.Log(scene);
        Debug.Log(scene1);
        Debug.Log(deaths);
        if (scene.Equals(scene1))
        {
            GenerateData(deaths.ToString(), scene);
        }
        else if (scene.Equals(scene2))
        {
            GenerateData(deaths.ToString(), scene);
        }
        else if (scene.Equals(scene3))
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

    public string[] loadDeaths()
    {
        string[] date1 = System.IO.File.ReadAllLines(@"Assets\files\" + scene1 + ".txt");
        string[] date2 = System.IO.File.ReadAllLines(@"Assets\files\" + scene2 + ".txt");
        string[] date3 = System.IO.File.ReadAllLines(@"Assets\files\" + scene3 + ".txt");

        string[] death_list = { date1[0], date2[0], date3[0] };
        Debug.Log(date1[0]);
        Debug.Log(date2[0]);
        Debug.Log(date3[0]);


        return death_list;

    }

    void UpdateFixes() {

        GameObject[] rotos = GameObject.FindGameObjectsWithTag("roto");

        fixtext.text = fixs.ToString() +" / " + (rotos.Length).ToString();
    
    }

    public void FixRoto() {

        fixs++;
    
    }

    void CheckFixes() {

        GameObject[] rotos = GameObject.FindGameObjectsWithTag("roto");

        if (fixs == rotos.Length) {
            SceneManager.LoadScene("Score");
        }
    }
}
