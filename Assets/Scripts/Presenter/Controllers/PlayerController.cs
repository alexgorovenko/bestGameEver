using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AbstractController
{
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
  private uint playedSquadCards = 0;
  private uint playedSupportCards = 0;
  private uint playedFortificationCards = 0;
  public GameObject currentDraggableCard;
  public Hand hand;

  public Flank leftFlank;
  public Flank rightFlank;

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

  public void PlaceCardToFlank(GameObject card, GameObject flank)
  {
    if (playedSquadCards < 2)
    {
      card.transform.SetParent(flank.transform);
      playedSquadCards++;
    }
  }

  public void SelectSquad()
  {
    Debug.Log("Select squad rendered");
    flanks.GetComponent<Image>().enabled = true;
  }
}
