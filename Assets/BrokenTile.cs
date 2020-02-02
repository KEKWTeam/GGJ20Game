using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenTile : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject repaired_piece = null;
    public GameObject leak = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (repaired_piece)
        {
            repaired_piece.transform.position = transform.position;
        }
    }

    public void setRepairedPiece(GameObject go)
    {
        go.tag = "Untagged";
        repaired_piece = go;
        tag = "Repaired";
        if(leak)
            leak.SetActive(false);
    }
}
