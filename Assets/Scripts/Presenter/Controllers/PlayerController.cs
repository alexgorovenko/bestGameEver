using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AbstractController
{
  [SerializeField] GameObject temporary;
  [SerializeField] GameObject cardCommandor;
  [SerializeField] GameObject cardUniversal;
  [SerializeField] GameObject commandorsOV;
  [SerializeField] GameObject chooseCommandors;
  [SerializeField] List<GameObject> commandorFields;
  [SerializeField] GameObject flanks;
  [SerializeField] GameObject hand1;
  [SerializeField] GameObject hand2;
  Dictionary<CurrentPlayer, GameObject> hands;
  private uint commandorsChosen = 0;
  private int selectedSquad = -1;
  private int playedSquadCards = 0;
  private int playedSupportCards = 0;
  private int playedFortificationCards = 0;
  public GameObject currentDraggableCard;
  public ContainerHand hand;

  // Start is called before the first frame update
  void Start()
  {
    game = new Game();
    hands = new Dictionary<CurrentPlayer, GameObject>();
    hands.Add(CurrentPlayer.FIRST, hand1);
    hands.Add(CurrentPlayer.SECOND, hand2);
    ShowCommandorsChooseMenu();
  }

  public void StartGame()
  {
    // add 4 cards to player 1
    game.AddCardsToHand(game.GetCurrentStep(), game.GetSeveralCards(game.GetCurrentStep(), 4));

    // render cards
    foreach (AbstractCard card in game.GetCardsOnHand(game.GetCurrentStep()))
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(hands[game.GetCurrentStep()].transform);
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

  public void PlaySquadCard(SquadCard squad)
  {
    // select flank
  }

  public void ApplyFortificationCard()
  {
    // get card
    // select flank
    // apply card
    // game.ApplyFortificationCard(game.GetCurrentStep(), card, flank);
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

    game.AddCardsToFlank(game.GetCurrentStep(), noobsLeft, Flank.Left);
    game.AddCardsToFlank(game.GetCurrentStep(), noobsRight, Flank.Right);
  }
  public void SupportTactical()
  {
    temporary.gameObject.SetActive(true);
    List<AbstractCard> temporaryCards = game.GetSeveralCards(game.GetCurrentStep(), 3);
    foreach (AbstractCard card in temporaryCards)
    {
      GameObject _card = Instantiate(cardUniversal);
      _card.transform.SetParent(temporary.transform);
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
}
