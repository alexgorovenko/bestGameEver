using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AbstractController
{
  [SerializeField] GameObject cardContainer;
  [SerializeField] GameObject cardCommandor;
  [SerializeField] GameObject cardUniversal;
  [SerializeField] GameObject chooseCommandors;
  [SerializeField] List<GameObject> commandorFields;
  [SerializeField] GameObject hand1;
  [SerializeField] GameObject hand2;
  Dictionary<CurrentPlayer, GameObject> hands;
  private uint commandorsChosen = 0;

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
    game.AddCardsToHand(game.GetCurrentStep(), game.GetSeveralCards(game.GetCurrentStep(), 4));

    // render cards
    foreach (AbstractCard card in game.GetCardsOnHand(game.GetCurrentStep()))
    {
      GameObject _cardContainer = Instantiate(cardContainer);
      GameObject _card = Instantiate(cardUniversal);
      _cardContainer.transform.SetParent(hands[game.GetCurrentStep()].transform);
      _card.transform.SetParent(_cardContainer.transform);
      _card.GetComponent<CardView>().SetCard(card);
    }
    // muligan?
    game.NextStep();
    game.AddCardsToHand(game.GetCurrentStep(), game.GetSeveralCards(game.GetCurrentStep(), 6));
    // muligan?
    // render cards
    foreach (AbstractCard card in game.GetCardsOnHand(game.GetCurrentStep()))
    {
      GameObject _cardContainer = Instantiate(cardContainer);
      GameObject _card = Instantiate(cardUniversal);
      _cardContainer.transform.SetParent(hands[game.GetCurrentStep()].transform);
      _card.transform.SetParent(_cardContainer.transform);
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
      GameObject _cardContainer = Instantiate(cardContainer);
      GameObject _cardCommandor = Instantiate(cardCommandor);
      _cardContainer.transform.SetParent(chooseCommandors.transform);
      _cardCommandor.transform.SetParent(_cardContainer.transform);
      _cardCommandor.GetComponent<CardCommandorView>().SetCard(commandor);
    }
  }

  public void HideCommandorsChooseMenu()
  {
    Destroy(chooseCommandors);
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
      StartGame();
      HideCommandorsChooseMenu();
    }
  }
}
