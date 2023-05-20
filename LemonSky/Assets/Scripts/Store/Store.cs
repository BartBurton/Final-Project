using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

[Serializable]
public class StoreItemMeta
{
    [HideInInspector] public string GameKey;
    public PlayerType PlayerType;
    public Sprite Icon;
}

public class Store : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cashText;

    [SerializeField] private GameObject _itemsContainer;
    [SerializeField] private GameObject _storeItemPrefab;

    [SerializeField] private List<StoreItemMeta> _storeItemMetas;

    void Start()
    {
        foreach (var item in _storeItemMetas)
        {
            item.GameKey = "Character_" + item.PlayerType.ToString();
        }

        CreateAvailableItems(new() {
            new() { Id = Guid.NewGuid(), GameKey = "Character_Duck", Name = "Уткевич", Price = 0, Type = "Character" },
            new() { Id = Guid.NewGuid(), GameKey = "Character_Frog", Name = "Лягуш", Price = 1000, Type = "Character" },
            new() { Id = Guid.NewGuid(), GameKey = "Character_Hedgehog", Name = "Ёжило", Price = 2500, Type = "Character" },
            new() { Id = Guid.NewGuid(), GameKey = "Character_Alpaka", Name = "Альпачино", Price = 4000, Type = "Character" },
        });

        _cashText.text = "5000";
    }

    void CreateAvailableItems(List<Stuff> _stuffs)
    {
        bool isSelect = false;

        foreach (var stuff in _stuffs)
        {
            var item = _storeItemMetas.FirstOrDefault(sim => sim.GameKey == stuff.GameKey);
            if (item != null)
            {
                var gayItem = Instantiate(_storeItemPrefab);
                gayItem.GetComponent<StoreItem>().SetStuff(stuff, item.Icon, item.PlayerType);

                if(!isSelect)
                {
                    gayItem.GetComponent<StoreItem>().Select();
                    isSelect = true;
                }

                gayItem.transform.SetParent(_itemsContainer.transform, false);
            }
        }
    }
}
