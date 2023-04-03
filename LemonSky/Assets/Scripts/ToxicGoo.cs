using UnityEngine;
using StarterAssets;

public class ToxicGoo : MonoBehaviour
{
    [SerializeField] int ContactDamage = 35;
    public void OnTriggerEnterInChild(Collider other){
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Player>().TakeDamageServerRpc(ContactDamage);
            var pos = SpawnManager.Instance.NextPosition();
            other.gameObject.GetComponent<ThirdPersonController>().Teleportation(pos);
        }
    }
}
