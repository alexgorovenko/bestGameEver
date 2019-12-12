﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AbstractController
{
  [SerializeField] GameObject gameUI;
  [SerializeField] GameObject temporary;
  [SerializeField] GameObject cardCommandor;
  [SerializeField] GameObject cardUniversal;
  [SerializeField] GameObject commandorsOV;
  [SerializeField] GameObject chooseCommandors;
  [SerializeField] List<GameObject> commandorFields;
  [SerializeField] GameObject flanks;
  private uint commandorsChosen = 0;
  private int playedSquadCards = 0;
  private int playedSupportCards = 0;
  private int playedFortificationCards = 0;
  public GameObject currentDraggableCard;
  private List<GameObject> selectedCards;
  public ContainerDeck deck1;
  public ContainerDeck deck2;
  Dictionary<CurrentPlayer, ContainerHand> hands;
  public ContainerHand hand1;
  public ContainerHand hand2;

  private delegate void Callback(AbstractCard card);
  private Callback callback;

  // Start is called before the first frame update
  void Start()
  {
    game = new Game();
    hands = new Dictionary<CurrentPlayer, ContainerHand>();
    hands.Add(CurrentPlayer.FIRST, hand1);
    hands.Add(CurrentPlayer.SECOND, hand2);
    ShowCommandorsChooseMenu();

    ContainerDeck deck = new ContainerDeck();
    deck.AddCards(game.GetSeveralCards(CurrentPlayer.FIRST, game.GetRemainedCards(CurrentPlayer.FIRST)));

    // deck.AddCard();
  }

  public void StartGame()
  {
    // add 4 cards to player 1
    CurrentPlayer currentPlayer = game.GetCurrentStep();
    game.AddCardsToHand(currentPlayer, game.GetSeveralCards(currentPlayer, 4));

    // render cards
    foreach (AbstractCard card in game.GetCardsOnHand(currentPlayer))
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(hands[currentPlayer].transform);
      _card.GetComponent<CardView>().SetCard(card);
    }
    // muligan?
    Next();
    // add 6 cards to player 2
    game.AddCardsToHand(game.GetCurrentStep(), game.GetSeveralCards(game.GetCurrentStep(), 6));
    // muligan?
    // render cards
    foreach (AbstractCard card in game.GetCardsOnHand(game.GetCurrentStep()))
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(hands[game.GetCurrentStep()].transform);
      _card.GetComponent<CardView>().SetCard(card);
    }
    // play cards
    // select cards (TODO: drag n drop)
    // place cards
    // attack
    // defend
    // refresh
    // next step
  }
  public void Next()
  {
    playedSquadCards = 0;
    playedSupportCards = 0;
    playedFortificationCards = 0;
    game.NextStep();
  }
  public void Hide(string s)
  {
    GameObject.Find(s).SetActive(false);
  }
  public void Show(string s)
  {
    GameObject.Find(s).SetActive(true);
  }
  public void ShowCommandorsChooseMenu()
  {
    // render commandors
    foreach (CommandorCard commandor in game.freeCommandors)
    {
      GameObject _cardCommandor = Instantiate(cardCommandor);
      _cardCommandor.transform.SetParent(chooseCommandors.transform);
      _cardCommandor.GetComponent<CardCommandorView>().SetCard(commandor);
    }
  }
  public void HideCommandorsChooseMenu()
  {
    Destroy(commandorsOV);
  }
  public void SetCommandor(Transform transform, CommandorCard commandor)
  {
    Debug.Log($"{commandor.name} is set");
    game.SetCommandor(game.GetCurrentStep(), commandor);
    transform.SetParent(commandorFields[(int)commandorsChosen].transform);
    commandorsChosen++;
    if (commandorsChosen != 2)
    {
      Next();
    }
    if (commandorsChosen == 4)
    {
      StartGame();
      HideCommandorsChooseMenu();
    }
  }
  public void AddCardToHand(GameObject card)
  {
    CurrentPlayer currentPlayer = game.GetCurrentStep();
    List<AbstractCard> addedCards = new List<AbstractCard>();
    AbstractCard cardModel = card.GetComponent<CardView>().card;
    addedCards.Add(cardModel);
    game.AddCardsToHand(currentPlayer, addedCards);
    card.transform.SetParent(hands[currentPlayer].transform);
  }
  public void DropCardToFlank(GameObject card, GameObject flank)
  {
    AbstractCard cardModel = card.GetComponent<CardView>().card;
    Flank flankModel = flank.GetComponent<ContainerFlank>().flank;
    switch (cardModel)
    {
      case SquadCard s:
        if (playedSquadCards < 2)
        {
          HashSet<SquadCard> placedCards = new HashSet<SquadCard>();
          placedCards.Add((SquadCard)cardModel);
          game.AddCardsToFlank(game.GetCurrentStep(), placedCards, flankModel);
          card.transform.SetParent(flank.transform.Find("CardsContainer").transform);
          playedSquadCards++;
        }
        break;
      case FortificationCard f:
        if (playedFortificationCards < 1)
        {
          game.ApplyFortificationCard(game.GetCurrentStep(), f, flankModel);
          card.transform.SetParent(flank.transform.Find("FortificationContainer").transform);
          playedFortificationCards++;
        }
        break;
    }
  }
  public void SupportMobilization()
  {
    HashSet<SquadCard> noobsLeft = new HashSet<SquadCard>();
    HashSet<SquadCard> noobsRight = new HashSet<SquadCard>();
    noobsLeft.Add(new SquadCard(Rarity.General, "Новобанец", "", 1, 2, 1, new Skills()));
    noobsRight.Add(new SquadCard(Rarity.General, "Новобанец", "", 1, 2, 1, new Skills()));

    if (game.GetCardsCount(game.GetCurrentStep(), Flank.Left) == 4)
    {
      return;
    }
    else
    {
      game.AddCardsToFlank(game.GetCurrentStep(), noobsLeft, Flank.Left);
    }

    if (game.GetCardsCount(game.GetCurrentStep(), Flank.Right) == 4)
    {
      return;
    }
    else
    {
      game.AddCardsToFlank(game.GetCurrentStep(), noobsRight, Flank.Right);
    }
  }
  public void SupportTactical()
  {
    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 1;
    Hide("Commandors");
    Hide("Flanks");
    Hide("HQ1");
    Hide("HQ2");
    Hide($"Hand{step}");

    temporary.gameObject.SetActive(true);
    List<AbstractCard> temporaryCards = game.GetSeveralCards(game.GetCurrentStep(), 3);
    foreach (AbstractCard card in temporaryCards)
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform);
      _card.GetComponent<CardView>().SetCard(card);
    }
    // => drag one => two to drop
  }

  public void SupportSniper()
  {
    SquadCard card = null; //TODO get card
    Skills skills = new Skills();
    skills.shelling = 3;
    game.HitSquad(card, skills);
  }

  public void RearRaid_Start()
  {
    CurrentPlayer currentPlayer = game.GetCurrentStep();
    hands[currentPlayer].GetComponent<ContainerHand>().SetCardHandler(true);

    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
    Hide("Commandors");
    Hide("Flanks");
    Hide("HQ1");
    Hide("HQ2");
    Hide($"Hand{step}");

    callback = RearRaid_End;
  }
  public void RearRaid_End(AbstractCard card)
  {
  }
  public void SelectCard(GameObject card)
  {
    selectedCards.Add(card);
  }
  public List<GameObject> GetSelectedCards()
  {
    return selectedCards;
  }
  public void ResetSelectionCards()
  {
    selectedCards.Clear();
  }
}
