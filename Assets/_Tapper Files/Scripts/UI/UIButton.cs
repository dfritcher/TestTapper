using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
public class UIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] protected Transform _transform = null;    
    [SerializeField] protected AudioClip _clickedAudioClip = null;
    [SerializeField] protected AudioSource _audioSource = null;
    [SerializeField] protected Vector3 _originalSize;
    [SerializeField] protected Vector3 _pushSize;
    [SerializeField] protected TextMeshProUGUI _textDisplay = null;

    public delegate void UIButtonEvent(UIButton uIButton);
    public event UIButtonEvent OnClicked;

    public void SetButtonVisibility(bool isVisible)
    {
        _transform.gameObject.SetActive(isVisible);
    }

    public void SetText(string text)
    {
        _textDisplay.text = text;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _transform.localScale = _originalSize;
        StartCoroutine(PointerUpCoroutine());        
    }

    protected IEnumerator PointerUpCoroutine()
    {
        _audioSource.PlayOneShot(_clickedAudioClip);
        yield return new WaitForSeconds(.5f);
        OnClicked?.Invoke(this);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _transform.localScale = _pushSize;
    }

    public virtual void Reset()
    {
        _transform.gameObject.SetActive(true);
        _transform.localScale = _originalSize;
    }
}
