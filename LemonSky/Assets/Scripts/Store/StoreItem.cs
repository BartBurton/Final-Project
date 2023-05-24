using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image _avatar;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _costText;

    private Color _baseColor;
    private Color _curentColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _hoverColor;

    private bool _isSelected = false;
    private PlayerType _playerType;
    private Stuff _stuff;

    public event Action<double> UpdateCash;

    private void Start()
    {
        _baseColor = GetComponent<Image>().color;
        _buyButton.onClick.AddListener(BuyItem);
    }

    private void FixedUpdate()
    {
        GetComponent<Image>().color = _curentColor;
    }

    private async void BuyItem()
    {
        AudioShot.Instance.Play("main");
        if (_stuff != null)
        {
            try
            {
                await APIRequests.BuyStuff(_stuff.Id);
                StoreUI.Instance.UnselectPrev();
                StoreUI.Instance.UnSelectCharacter();
                UpdateCash?.Invoke(_stuff.Price);
                Destroy(gameObject);
            }
            catch (System.Exception e)
            {
                StoreUI.Instance.ActivateError();
            }
        }
    }

    public void SetStuff(Stuff stuff, Sprite sprite, PlayerType playerType)
    {
        _stuff = stuff;
        _playerType = playerType;
        _avatar.sprite = sprite;
        _nameText.text = _stuff.Name;
        _costText.text = _stuff.Price.ToString();
    }

    public void Select()
    {
        if (_isSelected) return;
        _isSelected = true;

        StoreUI.Instance.UnselectPrev();
        StoreUI.Instance.SelectChatacter(_playerType);
        StoreUI.Instance.UnselectPrev = () =>
        {
            _isSelected = false;
            _curentColor = _baseColor;
        };

        _curentColor = _selectedColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioShot.Instance.Play("main");
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;
        _curentColor = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        _curentColor = _baseColor;
    }
}
