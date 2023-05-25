using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultItem : MonoBehaviour
{
    [SerializeField] private RectTransform _itemRectTransform;
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _punchesText;
    [SerializeField] private TextMeshProUGUI _failsText;
    [SerializeField] private TextMeshProUGUI _expText;

    private void FixedUpdate()
    {
        _itemRectTransform.sizeDelta = new Vector2(ResultItemsContainer.Instance.ViewportWidth, _itemRectTransform.sizeDelta.y);
    }

    public void Set(SessionResultItem resultItem)
    {
        _rankText.text = resultItem.Rank.ToString();
        _nameText.text = resultItem.Name;
        _coinsText.text = resultItem.Coins.ToString();
        _punchesText.text = resultItem.Punches.ToString();
        _failsText.text = resultItem.Fails.ToString();
    }
}
