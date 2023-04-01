using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGoo : MonoBehaviour
{
    void OnCollisionEnter(Collision other){
        Debug.Log("Coll");
        if(other.gameObject.tag == "Player"){
            Debug.Log(other.gameObject.GetComponent<Player>().GetHealth());
        }
    }
}
