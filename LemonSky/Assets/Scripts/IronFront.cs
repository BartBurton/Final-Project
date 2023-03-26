using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;

public class IronFront : NetworkBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.gameObject.tag == "Player")
        {
            var controller = other.GetComponent<ThirdPersonController>();
            Debug.Log(this.gameObject.transform.forward);
            Debug.Log(controller.OwnerClientId);

            controller.ImpulseServerRpc(this.gameObject.transform.forward ,controller.OwnerClientId);
        }
    }
}
