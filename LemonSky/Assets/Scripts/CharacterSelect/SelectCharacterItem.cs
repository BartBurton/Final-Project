using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCharacterItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public PlayerType TargetPlayerType;
    [SerializeField] private SkinnedMeshRenderer _skin;
    [SerializeField] private Color _hoverColor;
    [SerializeField] private Color _selectColor;

    [SerializeField] private TextMeshProUGUI _nameText;

    public void SetName(string value)
    {
        _nameText.text = value;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanInteract) return;

        SetColor(_hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanInteract) return;

        SetColor(Color.black);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanInteract) return;

        CharacterSelector.Instance.SelectedPlayer = TargetPlayerType;
        CharacterSelector.Instance.SlectedCharacterName = _nameText.text;
    }

    public void HandlePlayerTypeChange()
    {
        if (CharacterSelector.Instance.SelectedPlayer == TargetPlayerType)
        {
            SetColor(_selectColor);
        }
        else
        {
            SetColor(Color.black);
        }
    }

    private void SetColor(Color color)
    {
        for (int i = 0; i < _skin.materials.Length; i++)
        {
            _skin.materials[i].SetColor("_AffectColor", color);
        }
    }

    private bool CanInteract => CharacterSelector.Instance.SelectedPlayer != TargetPlayerType && !CharacterSelectReady.Instance.IsReady;
}
