using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SkinChanger : MonoBehaviour
{
    [SerializeField] List<GameObject> Skins;
    [SerializeField] int selectedSkin;
    void Awake(){
        selectedSkin = new System.Random().Next(2);
        SetSkin(0);
    }
    public void SetSkin(int id){
        if(id >= Skins.Count) return;
        Instantiate(Skins[id], transform.GetChild(3).transform);
    } 
    public void SetSkin(){
        Instantiate(Skins[selectedSkin], transform.GetChild(3).transform);
    } 
}
