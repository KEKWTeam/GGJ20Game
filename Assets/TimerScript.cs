using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = FindGameObjectsWithLayer(9)[0];
        text.text = player.GetComponent<PlayerMovement>().time.ToString();
        //GameObject rect = GameObject.FindGameObjectWithTag("barra");

        //Vector3 ancho = rect.transform.localScale;

       // ancho.x =player.GetComponent<PlayerMovement>().time / 100;
        //rect.transform.localScale = ancho;
    }

    GameObject[] FindGameObjectsWithLayer(int layer) { GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[]; List<GameObject> goList = new List<GameObject>(); for (int i = 0; i < goArray.Length; i++) { if (goArray[i].layer == layer) { goList.Add(goArray[i]); } } if (goList.Count == 0) { return null; } return goList.ToArray(); }
}
