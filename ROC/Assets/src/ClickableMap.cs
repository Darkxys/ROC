using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableMap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Game _game;
    [SerializeField] private GameObject _player;
    public void OnPointerClick(PointerEventData eventData)
    {
        float scaleX = Screen.width / _canvas.rect.width;
        float scaleY = Screen.height / _canvas.rect.height;

        float widthRect = gameObject.GetComponent<RectTransform>().rect.width;
        float heightRect = gameObject.GetComponent<RectTransform>().rect.height;

        float x = scaleX * (_canvas.rect.width / 2 - widthRect / 2);
        float y = scaleY * (_canvas.rect.height / 2 - heightRect / 2);

        float xScaleGame = _game.size / (widthRect * scaleX);
        float yScaleGame = _game.size / (heightRect * scaleY);

        float xPos = xScaleGame * (eventData.position.x - x);
        float yPos = yScaleGame * (eventData.position.y - y);

        if (_game.CanTP((int)xPos,(int)yPos))
        {
            _player.transform.position = new Vector3(xPos, yPos, 0);

            this.transform.parent.gameObject.SetActive(false);
            _game.SetActiveEnnemies(true);
        }
    }
}


