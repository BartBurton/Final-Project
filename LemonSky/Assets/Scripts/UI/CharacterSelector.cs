using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] PlayerType _targetPlayerType;
    [SerializeField] private SkinnedMeshRenderer _skin;
    [SerializeField] private Color _hoverColor;
    [SerializeField] private Color _selectColor;

    private void Start()
    {
        SelectCharacterManager.Instance.OnPlayerTypeChange += HandlePlayerTypeChange;
    }

    private void OnMouseOver()
    {
        if (SelectCharacterManager.Instance.SelectedPlayer == _targetPlayerType) return;
        SetColor(_hoverColor);
    }

    private void OnMouseExit()
    {
        if (SelectCharacterManager.Instance.SelectedPlayer == _targetPlayerType) return;
        SetColor(Color.black);
    }

    private void OnMouseUp()
    {
        SelectCharacterManager.Instance.SelectedPlayer = _targetPlayerType;
    }

    private void HandlePlayerTypeChange()
    {
        if (SelectCharacterManager.Instance.SelectedPlayer == _targetPlayerType)
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
}
