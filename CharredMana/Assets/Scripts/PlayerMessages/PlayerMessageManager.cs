using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMessageManager : MonoBehaviour
{
    private class PlayerMessage
    {
        public float SecondsElapsed = 0;
        public Textbox Textbox;
        public CanvasGroup CanvasGroup;
        public float Height;
        public float TargetY = 0;
    }

    [SerializeField] private GameObject _textboxPrefab;
    [SerializeField] private Transform _messagesParent;
    [SerializeField] private Transition _fading = new(1, 3, 1);
    [SerializeField] private float _fontSize = 30;
    [SerializeField] private float _messageWidth = 400;
    [SerializeField] private float _messageSpacing = 15; // pixels between messages
    [SerializeField] private float _speed = 50; // pixels per second messages can move

    private readonly Queue<PlayerMessage> _messages = new();

    private void Awake()
    {
        Assert.IsTrue(TryGetComponent<Canvas>(out var _), $"No canvas component on {name}");
        Assert.IsNotNull(_textboxPrefab);
        Assert.IsNotNull(_messagesParent);
    }

    private void Update()
    {
        // message fading
        foreach (var message in _messages)
        {
            message.SecondsElapsed += Time.deltaTime;

            float t = _fading.GetTransition(message.SecondsElapsed);
            message.CanvasGroup.alpha = t;
        }

        // remove messages that finished fading out
        while (_messages.Count > 0 && _fading.IsTransitionFinished(_messages.Last().SecondsElapsed))
        {
            PlayerMessage lastMessage = _messages.Dequeue();
            Destroy(lastMessage.Textbox.gameObject);
            float heightAndSpacing = lastMessage.Height + _messageSpacing;
        }

        // update positions
        float deltaY = _speed * Time.deltaTime;
        foreach (var message in _messages)
        {
            MoveToward(message.Textbox.transform, message.TargetY, deltaY);
        }
    }

    public void SendPlayerMessage(string messageContents)
    {
        PlayerMessage playerMessage = new();
        GameObject go = Instantiate(_textboxPrefab, _messagesParent);

        playerMessage.Textbox = go.GetComponent<Textbox>();
        playerMessage.Textbox.FloatingAnimationEnabled = false;
        playerMessage.Textbox.SetTextAndFontSize(messageContents, _fontSize, _messageWidth);

        playerMessage.Height = playerMessage.Textbox.GetComponent<RectTransform>().sizeDelta.y;
        float heightAndSpacing = playerMessage.Height + _messageSpacing;
        foreach (var message in _messages)
        {
            message.TargetY += heightAndSpacing;
        }
        go.GetComponent<RectTransform>().pivot = Vector2.zero;
        go.transform.localPosition = Vector3.down * heightAndSpacing;
        if (_messages.Count > 0) go.transform.localPosition += _messages.Last().Textbox.transform.localPosition;

        playerMessage.CanvasGroup = go.AddComponent<CanvasGroup>();
        playerMessage.CanvasGroup.alpha = 0;

        _messages.Enqueue(playerMessage);
    }

    private void MoveToward(Transform transform, float targetY, float distance)
    {
        float currentY = transform.localPosition.y;
        float newY = Mathf.MoveTowards(currentY, targetY, distance);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}
