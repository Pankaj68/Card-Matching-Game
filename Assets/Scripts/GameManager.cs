using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    
    public Transform boardPanel;          
    public GameObject cardPrefab;         
    public Sprite cardBackSprite;         

    
    public List<Sprite> frontSprites;     
    public int pairsCount = 6;            

   
    public float revealDelay = 0.9f;      
    public bool previewOnStart = true;    
    public float previewTime = 2f;        

    public bool IsBusy = false;

    List<Card> _allCards = new List<Card>();
    Card _firstRevealed = null;
    int _score = 0;
    UIManager _uiManager;

    
    int _turns = 0;

    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        StartGame();
    }

    public void StartGame()
    {
        // clear old
        foreach (Transform t in boardPanel) Destroy(t.gameObject);
        _allCards.Clear();
        _firstRevealed = null;
        _score = 0;
        _turns = 0;                      
        _uiManager?.UpdateScore(_score);
        _uiManager?.UpdateTurns(_turns); 

        // choose sprites (ensure enough sprites in list)
        var chosen = frontSprites.Take(pairsCount).ToList();

        // create pairs

        List<Sprite> pairList = new List<Sprite>();
        foreach (var s in chosen)
        {
            pairList.Add(s);
            pairList.Add(s); 
        }

        // shuffle
        pairList = pairList.OrderBy(x => Random.value).ToList();

        // instantiate cards
        foreach (var sprite in pairList)
        {
            GameObject go = Instantiate(cardPrefab, boardPanel);
            Card card = go.GetComponent<Card>();
            card.Initialize(sprite, cardBackSprite, this);
            _allCards.Add(card);
        }

        
        if (previewOnStart)
        {
            StartCoroutine(PreviewSequence());
        }
        else
        {
            IsBusy = false;
        }
    }

    IEnumerator PreviewSequence()
    {
        // block input during preview
        IsBusy = true;

        
        foreach (var c in _allCards)
        {
            c.RevealImmediate();
        }

       
        yield return new WaitForSeconds(previewTime);

      
        List<Coroutine> running = new List<Coroutine>();
        foreach (var c in _allCards)
        {
            
            if (!c.IsMatched && c.IsRevealed)
            {
                
                running.Add(StartCoroutine(c.FlipToBack()));
               
                yield return new WaitForSeconds(0.03f);
            }
        }

        
        
        yield return new WaitForSeconds(0.2f);

    
        _firstRevealed = null;
        IsBusy = false;
    }

    public void OnCardClicked(Card card)
    {
        if (IsBusy || card.IsRevealed || card.IsMatched) return;
        StartCoroutine(HandleCardClick(card));
    }

    IEnumerator HandleCardClick(Card card)
    {
        IsBusy = true;
        // flip to front
        yield return StartCoroutine(card.FlipToFront());
      
        AudioManager.Instance?.PlayFlip();

        if (_firstRevealed == null)
        {
            _firstRevealed = card;
            IsBusy = false;
        }
        else
        {
           
            _turns++;
            _uiManager?.UpdateTurns(_turns);

            // check match
            if (_firstRevealed.frontSprite == card.frontSprite)
            {
                
                _firstRevealed.SetMatched();
                card.SetMatched();
                _score += 10;
                _uiManager?.UpdateScore(_score);
                AudioManager.Instance?.PlayMatch();
                _firstRevealed = null;
                IsBusy = false;

                // check win
                if (_allCards.All(c => c.IsMatched))
                {
                    _uiManager?.ShowWin(_score,_turns);
                    
                }
            }
            else
            {
                AudioManager.Instance?.PlayMismatch();
                // not matched - wait then flip back both
                yield return new WaitForSeconds(revealDelay);
                yield return StartCoroutine(_firstRevealed.FlipToBack());
                yield return StartCoroutine(card.FlipToBack());
               
                _firstRevealed = null;
                IsBusy = false;
            }
        }
    }

    public void Restart()
    {
        StartGame();
    }
}
