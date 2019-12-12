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
  [SerializeField] GameObject flanks;
  private uint commandorsChosen = 0;
  private int playedSquadCards = 0;
  private int playedSupportCards = 0;
  private int playedFortificationCards = 0;
  public GameObject currentDraggableCard;
  private List<GameObject> selectedCards = new List<GameObject>();
  Dictionary<CurrentPlayer, ContainerDeck> decks;
  public ContainerDeck deck1;
  public ContainerDeck deck2;
  Dictionary<CurrentPlayer, ContainerHand> hands;
  public ContainerHand hand1;
  public ContainerHand hand2;
  private int rearRaidCounter;

  private delegate void Callback(AbstractCard card);
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
  public void StartGame()
  {
    CurrentPlayer currentPlayer = game.GetCurrentStep();
    GetCardsFromDeck(currentPlayer, 4);
    // muligan?
    Next();
    currentPlayer = game.GetCurrentStep();
    GetCardsFromDeck(currentPlayer, 6);

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
  public void GetCardFromDeck(CurrentPlayer currentPlayer)
  {
    Card card = decks[currentPlayer].GetCard();
    List<AbstractCard> _cards = new List<AbstractCard>();
    _cards.Add(card.card);
    card.transform.SetParent(hands[currentPlayer].transform);
    game.AddCardsToHand(currentPlayer, _cards);
  }
  public void GetCardsFromDeck(CurrentPlayer currentPlayer, int amount)
  {
    for (int i = 0; i < amount; i++)
    {
      GetCardFromDeck(currentPlayer);
    }
  }
  public void AddCardToHand(GameObject card)
  {
    CurrentPlayer currentPlayer = game.GetCurrentStep();
    List<AbstractCard> addedCards = new List<AbstractCard>();
    AbstractCard cardModel = card.GetComponent<Card>().card;
    addedCards.Add(cardModel);
    game.AddCardsToHand(currentPlayer, addedCards);
    card.transform.SetParent(hands[currentPlayer].transform);
  }
  public void DropCardToFlank(GameObject card, GameObject flank)
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
  public void SupportMobilization()
  {
    Debug.Log("TADA");
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
      _card.GetComponent<Card>().SetCard(card);
    }
    // => drag one => two to drop
  }

  public void SupportSniper()
  {
    CurrentPlayer currentEnemy = game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
    flanks[currentEnemy].GetComponent<FlankHand>().SetCardHandler(true);

    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
    Hide("Commandors");
    Hide("Flanks");
    Hide("HQ1");
    Hide("HQ2");
    Hide($"Hand{step}");
    callback = SupportSniperCallback;
  }

  private void SupportSniperCallback(AbstractCard card)
  {
    Skills skills = new Skills();
    skills.shelling = 3;
<<<<<<< HEAD
    // game.HitSquad(card, skills);
    hands[game.GetCurrentStep()].GetComponent<ContainerHand>().SetCardHandler(false);
    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
=======
    game.HitSquad(card, skills);
    flanks[game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST].GetComponent<FlankHand>().SetCardHandler(false);
    int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2
>>>>>>> 95edb8f1883cb0d09f4d254cd2ddfb152c2f4274
    Show("Commandors");
    Show("Flanks");
    Show("HQ1");
    Show("HQ2");
    Show($"Hand{step}");
    callback = null;
  }

  public void RearRaid_Start()
  {
    this.rearRaidCounter = 2;
    CurrentPlayer currentEnemy = game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
    hands[currentEnemy].GetComponent<ContainerHand>().SetCardHandler(true);

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
    this.game.DropCardFromHand(game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST, card);
    this.rearRaidCounter--;
<<<<<<< HEAD
    if (this.rearRaidCounter == 0)
    {
      hands[game.GetCurrentStep()].GetComponent<ContainerHand>().SetCardHandler(false);
      int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
=======
    if (this.rearRaidCounter == 0) {
      CurrentPlayer currentEnemy = game.GetCurrentStep() == CurrentPlayer.FIRST ? CurrentPlayer.SECOND : CurrentPlayer.FIRST;
      hands[currentEnemy].GetComponent<ContainerHand>().SetCardHandler(false);
      int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2
>>>>>>> 95edb8f1883cb0d09f4d254cd2ddfb152c2f4274
      Show("Commandors");
      Show("Flanks");
      Show("HQ1");
      Show("HQ2");
      Show($"Hand{step}");
      this.callback = null;
    }
  }
  public void SelectCard(GameObject card)
  {
    Debug.Log("TADA");
    selectedCards.Add(card);
    if (this.callback != null)
    {
      this.callback(card.GetComponent<Card>().card);
    }
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
