using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultItemsContainer : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _resultItemPrefab;
    [SerializeField] private RectTransform _titlesRectTransform;
    [SerializeField] private RectTransform _totalsRectTransform;


    public static ResultItemsContainer Instance { get; private set; }

    public float ViewportWidth => _canvas.GetComponent<RectTransform>().rect.width - 100;

    void Awake()
    {
        Instance = this;
    }

    void FixedUpdate()
    {
        _titlesRectTransform.sizeDelta = new Vector2(ViewportWidth, _titlesRectTransform.sizeDelta.y);
        _totalsRectTransform.sizeDelta = new Vector2(ViewportWidth, _totalsRectTransform.sizeDelta.y);
    }

    public void AddResultItem(SessionResultItem resultItem)
    {
        var gayObj = Instantiate(_resultItemPrefab);
        gayObj.GetComponent<ResultItem>().Set(resultItem);

        gayObj.transform.SetParent(transform, false);
    }
}
