using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text score1;
    public Text score2;
    public Text score3;
    public Text total;
    private HudScript hud = null;
    // Start is called before the first frame update
    void Start()
    {
        GameObject hud_go = GameObject.FindGameObjectWithTag("GameController");
        if (hud_go)
        {
            hud = hud_go.GetComponent<HudScript>();
            //hud.WriteDeaths();
            string[] die_list = hud.loadDeaths();
            score1.text = die_list[0];
            score2.text = die_list[1];
            score3.text = die_list[2];
            total.text = CalcTotal(die_list[0], die_list[1], die_list[2]).ToString();


        }
            

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int CalcTotal(string score1, string  score2, string score3)
    {
        int total;

        int int_score1;
        int int_score2;
        int int_score3;
        
        int.TryParse(score1, out int_score1);
        int.TryParse(score2, out int_score2);
        int.TryParse(score3, out int_score3);

        return total = int_score1 + int_score2 + int_score3;


    }
}
