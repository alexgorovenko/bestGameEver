using System.Collections;
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
  [SerializeField] GameObject hand1Layer;
  [SerializeField] GameObject hand2Layer;
  [SerializeField] GameObject flanksLayer;
  [SerializeField] GameObject flanks1Layer;
  [SerializeField] GameObject flanks2Layer;
  [SerializeField] GameObject commandorsLayer;
  [SerializeField] GameObject HQ1Layer;
  [SerializeField] GameObject HQ2Layer;
  private uint commandorsChosen = 0;
  private int playedSquadCards = 0;
  private int playedSupportCards = 0;
  private int playedFortificationCards = 0;
  public Card currentDraggableCard;
  private List<GameObject> selectedCards = new List<GameObject>();
  Dictionary<CurrentPlayer, ContainerDeck> decks;
  public ContainerDeck deck1;
  public ContainerDeck deck2;
  Dictionary<CurrentPlayer, ContainerDrop> drops;
  public ContainerDrop drop1;
  public ContainerDrop drop2;
  Dictionary<CurrentPlayer, ContainerHand> hands;
  public ContainerHand hand1;
  public ContainerHand hand2;
  private int rearRaidCounter;
  private delegate void Callback(Card card);
  private Callback callback;

  // Start is called before the first frame update
  void Start()
  {
    game = new Game();

    hands = new Dictionary<CurrentPlayer, ContainerHand>();
    hands.Add(CurrentPlayer.FIRST, hand1);
    hands.Add(CurrentPlayer.SECOND, hand2);

    decks = new Dictionary<CurrentPlayer, ContainerDeck>();
    decks.Add(CurrentPlayer.FIRST, deck1);
    decks.Add(CurrentPlayer.SECOND, deck2);

    drops = new Dictionary<CurrentPlayer, ContainerDrop>();
    drops.Add(CurrentPlayer.FIRST, drop1);
    drops.Add(CurrentPlayer.SECOND, drop2);

    ShowCommandorsChooseMenu();
    List<AbstractCard> _cards1 = game.GetSeveralCards(CurrentPlayer.FIRST, game.GetRemainedCards(CurrentPlayer.FIRST));
    foreach (AbstractCard aCard in _cards1)
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(deck1.transform.Find("CardsContainer").transform);
      Card card = _card.GetComponent<Card>();
      card.SetCard(aCard);
      deck1.AddCard(card);
    }
    List<AbstractCard> _cards2 = game.GetSeveralCards(CurrentPlayer.SECOND, game.GetRemainedCards(CurrentPlayer.SECOND));
    foreach (AbstractCard aCard in _cards2)
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(deck2.transform.Find("CardsContainer").transform);
      Card card = _card.GetComponent<Card>();
      card.SetCard(aCard);
      deck2.AddCard(card);
    }
  }
  public void GameStart()
  {
    CurrentPlayer currentPlayer = game.GetCurrentStep();
    GetCardsFromDeckToHand(currentPlayer, 4);
    hands[game.GetCurrentStep()].gameObject.SetActive(false);
    // muligan?
    Next();
    currentPlayer = game.GetCurrentStep();
    GetCardsFromDeckToHand(currentPlayer, 4);

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

    UpdateCommandors();
    hands[game.GetCurrentStep()].gameObject.SetActive(false);
    game.NextStep();
    hands[game.GetCurrentStep()].gameObject.SetActive(true);
    GetCardsFromDeckToHand(game.GetCurrentStep(), 2);

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
      game.NextStep();
    }
    if (commandorsChosen == 4)
    {
      GameStart();
      HideCommandorsChooseMenu();
    }
  }
  public void ActivateCommandor(CardCommandorView cardCommandorView)
  {

  }

  public void UpdateCommandors()
  {

  }
  private Card GetCardFromDeck(CurrentPlayer currentPlayer)
  {
    Card card = decks[currentPlayer].GetCard();
    return card;
  }
  public void GetCardFromDeckToHand(CurrentPlayer currentPlayer)
  {
    Card card = GetCardFromDeck(currentPlayer);
    if (card == null) return;
    List<AbstractCard> _cards = new List<AbstractCard>();
    _cards.Add(card.card);
    card.transform.SetParent(hands[currentPlayer].transform);
    game.AddCardsToHand(currentPlayer, _cards);
  }
  public void GetCardsFromDeckToHand(CurrentPlayer currentPlayer, int amount)
  {
    for (int i = 0; i < amount; i++)
    {
      GetCardFromDeckToHand(currentPlayer);
    }
  }
  public void AddCardToHand(Card card)
  {
    CurrentPlayer currentPlayer = game.GetCurrentStep();
    List<AbstractCard> addedCards = new List<AbstractCard>();
    AbstractCard cardModel = card.GetComponent<Card>().card;
    addedCards.Add(cardModel);
    game.AddCardsToHand(currentPlayer, addedCards);
    card.transform.SetParent(hands[currentPlayer].transform);
  }
  public void DropCardToFlank(Card card, GameObject flank)
  {
    AbstractCard cardModel = card.GetComponent<Card>().card;
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

  public void DropCardToDrop(Card card, bool isEnemy)
  {
    CurrentPlayer player;
    if (isEnemy)
    {
      player = game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
    }
    else
    {
      player = game.GetCurrentStep();
    }
    game.DropCardFromHand(player, card.card);
    card.transform.SetParent(drops[player].transform);
  }

  // Attack

  public void AttackStart()
  {
    // выбраны те, кто атакуют
    // запрещаем выбирать чужие карты
    // выбираем тех, кто защищается
    // начать атаку => в бой
    // применить активные скилы
  }

  // Support cards callbacks

  public void SupportMobilization()
  {
    Debug.Log("Support: Mobilization");
    HashSet<SquadCard> noobsLeft = new HashSet<SquadCard>();
    HashSet<SquadCard> noobsRight = new HashSet<SquadCard>();
    SquadCard noobLeft = new SquadCard(Rarity.General, "Новобанец", "", 1, 2, 1, new Skills());
    SquadCard noobRight = new SquadCard(Rarity.General, "Новобанец", "", 1, 2, 1, new Skills());
    noobsLeft.Add(noobLeft);
    noobsRight.Add(noobRight);
    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
    if (game.GetCardsCount(game.GetCurrentStep(), Flank.Left) != 4)
    {
      game.AddCardsToFlank(game.GetCurrentStep(), noobsLeft, Flank.Left);
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(GameObject.Find($"FlankLeft{step}").transform.Find("CardsContainer").transform);
      Card card = _card.GetComponent<Card>();
      card.SetCard(noobLeft);
    }

    if (game.GetCardsCount(game.GetCurrentStep(), Flank.Right) != 4)
    {
      game.AddCardsToFlank(game.GetCurrentStep(), noobsRight, Flank.Right);
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(GameObject.Find($"FlankRight{step}").transform.Find("CardsContainer").transform);
      Card card = _card.GetComponent<Card>();
      card.SetCard(noobRight);
    }
  }
  public void SupportTactical()
  {
    Debug.Log("Support: Tactical Move");
    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 1;

    commandorsLayer.SetActive(false);
    flanksLayer.SetActive(false);
    HQ1Layer.SetActive(false);
    HQ2Layer.SetActive(false);
    if (step == 1)
    {
      hand1Layer.SetActive(false);
    }
    else
    {
      hand2Layer.SetActive(false);
    }

    temporary.gameObject.SetActive(true);

    List<Card> temporaryCards = new List<Card>();

    temporaryCards.Add(GetCardFromDeck(game.GetCurrentStep()));
    temporaryCards.Add(GetCardFromDeck(game.GetCurrentStep()));
    temporaryCards.Add(GetCardFromDeck(game.GetCurrentStep()));

    foreach (Card card in temporaryCards)
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform);
      _card.GetComponent<Card>().SetCard(card.card);
    }

    callback = SupportTacticalEnd;
  }

  public void SupportTacticalEnd(Card card)
  {
    Debug.Log("Support: Tactical Move End");
    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 1;

    temporary.gameObject.SetActive(false);

    AddCardToHand(card);
    // foreach (var _card in temporary.GetCards())
    // {
    //   DropCardToDrop(_card);
    // }

    commandorsLayer.SetActive(true);
    flanksLayer.SetActive(true);
    HQ1Layer.SetActive(true);
    HQ2Layer.SetActive(true);
    if (step == 1)
    {
      hand1Layer.SetActive(true);
    }
    else
    {
      hand2Layer.SetActive(true);
    }

    ResetSelectionCards();
    callback = null;
  }

  public void SupportSniper()
  {
    CurrentPlayer currentEnemy = game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
    // flanks[currentEnemy].GetComponent<FlankHand>().SetCardHandler(true);

    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
    commandorsLayer.SetActive(false);
    flanksLayer.SetActive(false);
    HQ1Layer.SetActive(false);
    HQ2Layer.SetActive(false);
    if (step == 1)
    {
      hand1Layer.SetActive(false);
    }
    else
    {
      hand2Layer.SetActive(false);
    }
    callback = SupportSniperEnd;
  }

  private void SupportSniperEnd(Card card)
  {
    Skills skills = new Skills();
    skills.shelling = 3;
    // game.HitSquad(card.card, skills);
    // flanks[game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].GetComponent<FlankHand>().SetCardHandler(false);
    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
    commandorsLayer.SetActive(true);
    flanksLayer.SetActive(true);
    HQ1Layer.SetActive(true);
    HQ2Layer.SetActive(true);
    if (step == 1)
    {
      hand1Layer.SetActive(true);
    }
    else
    {
      hand2Layer.SetActive(true);
    }
    callback = null;
  }

  public void RearRaid_Start()
  {
    this.rearRaidCounter = 2;
    CurrentPlayer currentEnemy = game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
    hands[currentEnemy].GetComponent<ContainerHand>().SetCardHandler(true);

    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
    commandorsLayer.SetActive(false);
    flanksLayer.SetActive(false);
    HQ1Layer.SetActive(false);
    HQ2Layer.SetActive(false);
    if (step == 1)
    {
      hand1Layer.SetActive(false);
    }
    else
    {
      hand2Layer.SetActive(false);
    }

    callback = RearRaid_End;
  }
  public void RearRaid_End(Card card)
  {
    this.DropCardToDrop(card, true);
    this.rearRaidCounter--;
    if (this.rearRaidCounter == 0)
    {
      CurrentPlayer currentEnemy = game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
      hands[currentEnemy].GetComponent<ContainerHand>().SetCardHandler(false);
      int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
      commandorsLayer.SetActive(true);
      flanksLayer.SetActive(true);
      HQ1Layer.SetActive(true);
      HQ2Layer.SetActive(true);
      if (step == 1)
      {
        hand1Layer.SetActive(true);
      }
      else
      {
        hand2Layer.SetActive(true);
      }
      this.callback = null;
    }
  }
  public void SelectCard(GameObject card)
  {
    Debug.Log("SelectCard");
    Debug.Log(selectedCards.Count);
    if (card.GetComponent<Card>().isSelectable == false) return;
    card.GetComponent<Card>().isSelectable = false;
    selectedCards.Add(card);

    if (this.callback != null)
    {
      this.callback(card.GetComponent<Card>());
    }
  }
  public List<GameObject> GetSelectedCards()
  {
    return selectedCards;
  }
  public void ResetSelectionCards()
  {
    foreach (var card in selectedCards)
    {
      card.GetComponent<Card>().isSelectable = true;
    }
    selectedCards.Clear();
  }
}
