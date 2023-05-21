using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBackgrounds : MonoBehaviour
{
    [SerializeField] int WaitSeconds;
    [SerializeField] List<Sprite> Sourse;
    int _current;
    Image _background;
    Animator _animator;

    void Awake()
    {
        _background = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        StartCoroutine(ChangeImage());
    }

    IEnumerator ChangeImage()
    {
        var random = new System.Random();
        var index = 0;
        while (true)
        {
            while (index == _current)
                index = random.Next(0, Sourse.Count);
            _background.sprite = Sourse.ElementAt(index);
            _current = index;
            _animator.SetTrigger("Show");
            yield return new WaitForSeconds(WaitSeconds);
            _animator.SetTrigger("Hide");
            yield return new WaitForSeconds(1);
        }
    }
}
