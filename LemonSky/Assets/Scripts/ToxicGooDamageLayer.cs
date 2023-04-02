using UnityEngine;

public class ToxicGooDamageLayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        GetComponentInParent<ToxicGoo>().OnTriggerEnterInChild(other);
    }
}
