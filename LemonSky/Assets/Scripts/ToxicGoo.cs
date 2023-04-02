using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ToxicGoo : MonoBehaviour
{
    [SerializeField] int ContactDamage = 35;
    public void OnTriggerEnterInChild(Collider other){
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Player>().TakeDamage(ContactDamage);
            var pos = SpawnManager.Instance.NextPosition();
            Debug.Log("Change - " + pos);
            other.gameObject.GetComponent<ThirdPersonController>().Teleportation(pos);
            Debug.Log("Changed!!!");
        }
    }
}
