using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSkills : NetworkBehaviour
{
    [SerializeField] public List<Skill> ActiveSkills = new List<Skill>{SelfHarm, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty};

    public override void OnNetworkSpawn(){
        if(ActiveSkills.Count == 0)
            ActiveSkills = new List<Skill>{ Empty, SelfHarm,  Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty};
    }

    static Skill SelfHarm = (player) => { player.TakeDamageServerRpc(5);};
    static Skill Empty = (player) => {};
    [System.Serializable]
    public delegate void Skill(Player player);
}
