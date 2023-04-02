using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGooDamageLayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        GetComponentInParent<ToxicGoo>().OnTriggerEnterInChild(other);
    }
}
