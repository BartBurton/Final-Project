using UnityEngine;
using TMPro;

[RequireComponent(typeof(Player))]
public class PlayerHudManager : MonoBehaviour
{
    [SerializeField] HeatlthBar HeatlthBar;
    [SerializeField] TextMeshProUGUI Name;


    void Start(){
        var player = this.gameObject.GetComponent<Player>();
        Name.text = player.Name.Value.ToString();
        player.Name.OnValueChanged += (prev, next) => {Name.text = player.Name.Value.ToString();};
        
        HeatlthBar.SetMaxHealth(player.GetHealth());
        player.Health.OnValueChanged += (prev, next) => { HeatlthBar.SetHealth(next);};
    }
}
