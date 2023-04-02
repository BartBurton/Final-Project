using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTrigger<T> : MonoBehaviour where T : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        GetComponentInParent<T>().OnTriggerEnter(other);
    }
}
