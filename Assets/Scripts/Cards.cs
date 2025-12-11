using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    
    public Sprite frontSprite;      
    public Sprite backSprite;       

    public float flipDuration = 0.18f;

    [HideInInspector] public bool IsMatched = false;
    [HideInInspector] public bool IsRevealed = false;

    Image _image;
    Button _button;
    GameManager _gameManager;

    void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void Initialize(Sprite front, Sprite back, GameManager gm)
    {
        frontSprite = front;
        backSprite = back;
        _gameManager = gm;
        ShowBackImmediate();
    }

    void OnClick()
    {
        if (IsMatched || IsRevealed || _gameManager.IsBusy) return;
        _gameManager.OnCardClicked(this);
    }

    public void RevealImmediate()
    {
        IsRevealed = true;
        _image.sprite = frontSprite;
    }

    public void HideImmediate()
    {
        IsRevealed = false;
        _image.sprite = backSprite;
    }

    public void ShowBackImmediate()
    {
        HideImmediate();
    }

    // animate flip using scale X
    public IEnumerator FlipToFront()
    {
        IsRevealed = true;
        // shrink X to 0
        yield return StartCoroutine(FlipScale(1f, 0f, flipDuration));
        _image.sprite = frontSprite;
        // expand X to 1
        yield return StartCoroutine(FlipScale(0f, 1f, flipDuration));
    }

    public IEnumerator FlipToBack()
    {
        IsRevealed = false;
        yield return StartCoroutine(FlipScale(1f, 0f, flipDuration));
        _image.sprite = backSprite;
        yield return StartCoroutine(FlipScale(0f, 1f, flipDuration));
    }

    IEnumerator FlipScale(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(from, to, t / duration);
            transform.localScale = new Vector3(s, 1f, 1f);
            yield return null;
        }
        transform.localScale = new Vector3(to, 1f, 1f);
    }


    public void SetMatched()
    {
        IsMatched = true;
        _button.interactable = false;
    }
}
