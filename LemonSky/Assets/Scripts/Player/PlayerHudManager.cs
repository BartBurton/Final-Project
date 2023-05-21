using UnityEngine;
using TMPro;

[RequireComponent(typeof(Player))]
public class PlayerHudManager : MonoBehaviour
{
    [SerializeField] SliderBar HeatlthBar;
    [SerializeField] TextMeshProUGUI Name;


    void Start(){
        var player = this.gameObject.GetComponent<Player>();
        Name.text = player.Name.Value.ToString();
        player.Name.OnValueChanged += (prev, next) => {Name.text = player.Name.Value.ToString();};
        
        HeatlthBar.SetMax(player.GetHealth());
        player.Health.OnValueChanged += (prev, next) => { 
            if(next > HeatlthBar.Max)
            {
                HeatlthBar.SetMax(next);
            }
            HeatlthBar.Set(next);
        };
    }
}
