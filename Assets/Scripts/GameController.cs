using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject spawn_point;

    int sorting_layer = 1;

    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        GameObject spawned_go = Instantiate(player, spawn_point.transform.position, Quaternion.identity);
        spawned_go.GetComponent<BoxCollider2D>().enabled = true;
        spawned_go.GetComponent<PlayerMovement>().can_move = true;
        spawned_go.GetComponent<SpriteRenderer>().sortingOrder = sorting_layer;
        sorting_layer += 1;
    }
}
