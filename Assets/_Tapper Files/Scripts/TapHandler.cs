using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapHandler : MonoBehaviour
{
    [SerializeField] private Button _tapButton = null;
    [SerializeField] private GameManager _gameManager = null;
    private int _tapCount = 0;


    public void SetButtonState(bool enabled)
    {
        _tapButton.interactable = enabled;        
    }

    public void Reset()
    {
        _tapCount = 0;
    }

    public void OnTapClicked()
    {
        _tapCount++;
        _gameManager.UpdateTapCount(_tapCount);
    }
}
