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
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _hoverColor;

    private bool _isSelected = false;
    private PlayerType _playerType;
    private Stuff _stuff;

    private void Start()
    {
        _baseColor = GetComponent<Image>().color;
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
        GetComponent<Image>().color = _selectedColor;

        StoreUI.Instance.UnselectPrev();
        StoreUI.Instance.SelectChatacter(_playerType);
        StoreUI.Instance.UnselectPrev = () =>
        {
            _isSelected = false;
            GetComponent<Image>().color = _baseColor;
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;
        GetComponent<Image>().color = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        GetComponent<Image>().color = _baseColor;
    }
}
