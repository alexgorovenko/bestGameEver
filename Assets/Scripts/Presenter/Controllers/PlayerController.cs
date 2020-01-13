using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AbstractController
{
    [SerializeField]
    GameObject gameUI;
    [SerializeField]
    GameObject temporary;
    [SerializeField]
    CardCommandorView cardCommandor;
    [SerializeField]
    Card cardUniversal;
    [SerializeField]
    CardSquad cardSquad;
    [SerializeField]
    public CardPlaceholder cardPlaceholder;
    [SerializeField]
    GameObject commandorsOV;
    [SerializeField]
    GameObject chooseCommandors;
    [SerializeField]
    List<GameObject> commandorFields;
    [SerializeField]
    GameObject hand1Layer;
    [SerializeField]
    GameObject hand2Layer;
    [SerializeField]
    GameObject flanksLayer;
    [SerializeField]
    GameObject flanks1Layer;
    [SerializeField]
    GameObject flanks2Layer;
    [SerializeField]
    GameObject commandorsLayer;
    [SerializeField]
    GameObject HQ1Layer;
    [SerializeField]
    GameObject HQ2Layer;
    [SerializeField]
    GameObject AttackButton;
    private uint commandorsChosen = 0;
    private int playedSquadCards = 0;
    private int playedSupportCards = 0;
    private int playedFortificationCards = 0;
    public Card currentDraggableCard;
    private List<Card> attackCards = new List<Card>(8);
    private List<Card> defenceCards = new List<Card>(8);
    List<ContainerFlank> flanks;
    public ContainerFlank flankLeft1;
    public ContainerFlank flankLeft2;
    public ContainerFlank flankRight1;
    public ContainerFlank flankRight2;
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
    private AttackState attackState = AttackState.ATTACK;
    private Skills tempSkills = null;
    private int position = 0;

    void UpdateSprite(string cardName, Card _card)
    {
        switch (cardName)
        {
            case "Линейная пехота":
                _card.Image = Resources.Load<Sprite>("Пехота 1");
                break;
            case "Рейд по тылам":
                _card.Image = Resources.Load<Sprite>("Рейды по тылам3");
                break;
            case "Дворфы защитники":
                _card.Image = Resources.Load<Sprite>("дворф");
                break;
            case "Колючая проволока":
                _card.Image = Resources.Load<Sprite>("колючая проволока1");
                break;
            case "Мобилизация":
                _card.Image = Resources.Load<Sprite>("мобилизация");
                break;
            case "Полевая медицина":
                _card.Image = Resources.Load<Sprite>("полевая медицина5");
                break;
            case "Прыгуны":
                _card.Image = Resources.Load<Sprite>("прыгун");
                break;
            case "Пулеметчики":
                _card.Image = Resources.Load<Sprite>("пулеметчики1");
                break;
            case "Снайпер":
                _card.Image = Resources.Load<Sprite>("снайпер2");
                break;
            case "Тактический ход":
                _card.Image = Resources.Load<Sprite>("тактический ход1");
                break;
            case "Танк":
                _card.Image = Resources.Load<Sprite>("танкБритания");
                break;
            case "Техномаг":
                _card.Image = Resources.Load<Sprite>("техномаги");
                break;
            case "Укрепленная траншея":
                _card.Image = Resources.Load<Sprite>("укрепленная траншея");
                break;
            case "Штрафники":
                _card.Image = Resources.Load<Sprite>("штрафники1");
                break;
            case "Штурмовики":
                _card.Image = Resources.Load<Sprite>("Пехота 2");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            attackCards.Add(null);
            defenceCards.Add(null);
        }

        game = new Game();

        callback = AttackCallback;

        hands = new Dictionary<CurrentPlayer, ContainerHand>();
        hands.Add(CurrentPlayer.FIRST, hand1);
        hands.Add(CurrentPlayer.SECOND, hand2);

        decks = new Dictionary<CurrentPlayer, ContainerDeck>();
        decks.Add(CurrentPlayer.FIRST, deck1);
        decks.Add(CurrentPlayer.SECOND, deck2);

        drops = new Dictionary<CurrentPlayer, ContainerDrop>();
        drops.Add(CurrentPlayer.FIRST, drop1);
        drops.Add(CurrentPlayer.SECOND, drop2);

        flanks = new List<ContainerFlank>();
        flanks.Add(flankLeft1);
        flanks.Add(flankRight1);
        flanks.Add(flankLeft2);
        flanks.Add(flankRight2);

        foreach (var flank in flanks)
        {
            for (int i = 0; i < 4; i++)
            {
                CardPlaceholder _card = Instantiate(cardPlaceholder);
                _card.transform.SetParent(flank.GetComponent<ContainerFlank>().squads[i].transform, false);
            }
        }

        ShowCommandorsChooseMenu();
        List<AbstractCard> _cards1 = game.GetSeveralCards(
          CurrentPlayer.FIRST,
          game.GetRemainedCards(CurrentPlayer.FIRST)
        );
        foreach (AbstractCard aCard in _cards1)
        {
            switch (aCard)
            {
                case SquadCard s:
                    CardSquad cs = Instantiate(cardSquad);
                    UpdateSprite(aCard.name, cs);
                    cs.transform.localScale = deck1.transform.localScale;
                    cs.transform.SetParent(deck1.transform.Find("CardsContainer").transform, false);
                    cs.SetCard((SquadCard)aCard);
                    cs.Highlight(false);
                    deck1.AddCard(cs);
                    break;
                default:
                    Card cu = Instantiate(cardUniversal);
                    UpdateSprite(aCard.name, cu);
                    cu.transform.localScale = deck1.transform.localScale;
                    cu.transform.SetParent(deck1.transform.Find("CardsContainer").transform, false);
                    cu.SetCard(aCard);
                    cu.Highlight(false);
                    deck1.AddCard(cu);
                    break;
            }
        }
        List<AbstractCard> _cards2 = game.GetSeveralCards(
          CurrentPlayer.SECOND,
          game.GetRemainedCards(CurrentPlayer.SECOND)
        );
        foreach (AbstractCard aCard in _cards2)
        {
            switch (aCard)
            {
                case SquadCard s:
                    CardSquad cs = Instantiate(cardSquad);
                    UpdateSprite(aCard.name, cs);
                    cs.transform.localScale = deck2.transform.localScale;
                    cs.transform.SetParent(deck2.transform.Find("CardsContainer").transform, false);
                    cs.SetCard(aCard);
                    cs.Highlight(false);
                    deck2.AddCard(cs);
                    break;
                default:
                    Card cu = Instantiate(cardUniversal);
                    UpdateSprite(aCard.name, cu);
                    cu.transform.localScale = deck2.transform.localScale;
                    cu.transform.SetParent(deck2.transform.Find("CardsContainer").transform, false);
                    cu.SetCard(aCard);
                    cu.Highlight(false);
                    deck2.AddCard(cu);
                    break;
            }
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

        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;

        flanks[step].GetComponent<ContainerFlank>().RefreshActive();
        flanks[step + 1].GetComponent<ContainerFlank>().RefreshActive();

        hands[game.GetCurrentStep()].gameObject.SetActive(false);
        game.NextStep();
        hands[game.GetCurrentStep()].gameObject.SetActive(true);
        GetCardsFromDeckToHand(game.GetCurrentStep(), 2);

        HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
        HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";

    }
    public void ShowCommandorsChooseMenu()
    {
        commandorsOV.SetActive(true);
        // render commandors
        foreach (CommandorCard commandor in game.freeCommandors)
        {
            CardCommandorView _cardCommandor = Instantiate(cardCommandor);
            switch (commandor.name)
            {
                case "Мастер защиты":
                    _cardCommandor.Image = Resources.Load<Sprite>("Командир1");
                    break;
                case "Мастер атаки":
                    _cardCommandor.Image = Resources.Load<Sprite>("командир4");
                    break;
                case "Координатор":
                    _cardCommandor.Image = Resources.Load<Sprite>("командир6");
                    break;
                case "Ветеран войны":
                    _cardCommandor.Image = Resources.Load<Sprite>("командир7");
                    break;
            }
            _cardCommandor.transform.SetParent(chooseCommandors.transform, false);
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
        transform.SetParent(commandorFields[(int)commandorsChosen].transform, false);
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
        card.transform.SetParent(hands[currentPlayer].transform, false);
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
        card.transform.SetParent(hands[currentPlayer].transform, false);
    }
    public void DropCardToFlank(Card card, int position, ContainerFlank flank)
    {
        AbstractCard cardModel = card.GetComponent<Card>().card;
        Flank flankModel = flank.GetComponent<ContainerFlank>().flank;
        switch (cardModel)
        {
            case SquadCard s:
                if (playedSquadCards < 2)
                {
                    if (flank.GetComponent<ContainerFlank>().GetCardAt(position) == null)
                    {
                        // add card to model
                        HashSet<SquadCard> placedCards = new HashSet<SquadCard>();
                        placedCards.Add((SquadCard)cardModel);
                        game.AddCardsToFlank(game.GetCurrentStep(), placedCards, flankModel);
                        // add card to view
                        flank.GetComponent<ContainerFlank>().PlaceCard(card, position);
                        Transform container = flank.transform.Find("Squads").Find($"CardsContainer-{position}");
                        GameObject.Destroy(container.Find("CardPlaceholder(Clone)").gameObject);
                        card.transform.SetParent(container.transform, false);
                        card.isDraggable = false;
                        card.isSelectable = s.isActive;
                        playedSquadCards++;
                    }
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
            player = game.GetNextStep();
        }
        else
        {
            player = game.GetCurrentStep();
        }
        game.DropCardFromHand(player, card.card);
        card.transform.SetParent(drops[player].transform, false);
    }

    // Attack

    public void Attack()
    {
        if (attackState == AttackState.ATTACK)
        {
            AttackStart();
            AttackButton.GetComponentInChildren<Text>().text = "В бой!";
            attackState = AttackState.DEFENCE;
        }
        else
        {
            ApplyActiveSkills();
            AttackButton.GetComponentInChildren<Text>().text = "Атака!";
            attackState = AttackState.ATTACK;
        }
    }

    public void AttackStart()
    {
        Debug.Log("in AttackStart");
        // выбраны те, кто атакуют
        // запрещаем выбирать чужие карты
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        flanks[step].RefreshActive();
        flanks[step + 1].RefreshActive();
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step].SetActive(false);
        flanks[step + 1].SetActive(false);
        // выбираем тех, кто защищается

        // начать атаку => в бой
        // применить активные скилы
    }

    public void DefenceStart()
    {
        CurrentPlayer deffendersStep = game.GetNextStep();
        // уже выбраны защитники
        List<SquadCard> cards = new List<SquadCard>();
        Debug.Log(attackCards[0]);
        cards.Add(attackCards[0] == null ? null : (SquadCard)attackCards[0].card);
        cards.Add(attackCards[1] == null ? null : (SquadCard)attackCards[1].card);
        cards.Add(attackCards[2] == null ? null : (SquadCard)attackCards[2].card);
        cards.Add(attackCards[3] == null ? null : (SquadCard)attackCards[3].card);
        game.SetAttackers(game.GetCurrentStep(), cards, Flank.Left);
        cards = new List<SquadCard>(4);
        cards.Add(defenceCards[0] == null ? null : (SquadCard)defenceCards[0].card);
        cards.Add(defenceCards[1] == null ? null : (SquadCard)defenceCards[1].card);
        cards.Add(defenceCards[2] == null ? null : (SquadCard)defenceCards[2].card);
        cards.Add(defenceCards[3] == null ? null : (SquadCard)defenceCards[3].card);
        game.SetDeffenders(deffendersStep, cards, Flank.Left);
        cards = new List<SquadCard>(4);
        cards.Add(attackCards[4] == null ? null : (SquadCard)attackCards[4].card);
        cards.Add(attackCards[5] == null ? null : (SquadCard)attackCards[5].card);
        cards.Add(attackCards[6] == null ? null : (SquadCard)attackCards[6].card);
        cards.Add(attackCards[7] == null ? null : (SquadCard)attackCards[7].card);
        game.SetAttackers(game.GetCurrentStep(), cards, Flank.Right);
        cards = new List<SquadCard>(4);
        cards.Add(defenceCards[4] == null ? null : (SquadCard)defenceCards[4].card);
        cards.Add(defenceCards[5] == null ? null : (SquadCard)defenceCards[5].card);
        cards.Add(defenceCards[6] == null ? null : (SquadCard)defenceCards[6].card);
        cards.Add(defenceCards[7] == null ? null : (SquadCard)defenceCards[7].card);
        game.SetDeffenders(deffendersStep, cards, Flank.Right);
        game.Attack(game.GetCurrentStep());
        game.ApplyAttack(game.GetCurrentStep());
        //  Update UI

        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step].GetComponent<ContainerFlank>().DestroyDead();
        flanks[step + 1].GetComponent<ContainerFlank>().DestroyDead();
        flanks[step].GetComponent<ContainerFlank>().RefreshActive();
        flanks[step + 1].GetComponent<ContainerFlank>().RefreshActive();
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        flanks[step].GetComponent<ContainerFlank>().DestroyDead();
        flanks[step + 1].GetComponent<ContainerFlank>().DestroyDead();
        flanks[step].GetComponent<ContainerFlank>().SetActive(false);
        flanks[step + 1].GetComponent<ContainerFlank>().SetActive(false);

        for (int i = 0; i < 8; i++)
        {
            if (attackCards[i] != null)
            {
                attackCards[i].Highlight(false);
                attackCards[i] = null;
            }
            if (defenceCards[i] != null)
            {
                defenceCards[i].Highlight(false);
                defenceCards[i] = null;
            }
        }

        HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
        HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
        if (game.GetHeadsquaterHealth(CurrentPlayer.FIRST) <= 0)
        {
            //TODO SECOND WIN UI
        }
        else if (game.GetHeadsquaterHealth(CurrentPlayer.SECOND) <= 0)
        {
            //TODO FIRST WIN UI
        }
        else
        {
            Next();
        }
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
            int position = -1;
            ContainerFlank flank = GameObject.Find($"FlankLeft{step}").GetComponent<ContainerFlank>();
            for (int i = 0; i < 4; i++)
            {
                if (flank.GetCardAt(i) == null) position = i;
            }
            Card card = Instantiate(cardUniversal);
            card.SetCard(noobLeft);
            game.AddCardsToFlank(game.GetCurrentStep(), noobsLeft, Flank.Left);
            flank.PlaceCard(card, position);
            Transform container = flank.transform.Find("Squads").Find($"CardsContainer-{position}");
            GameObject.Destroy(container.Find("CardPlaceholder(Clone)").gameObject);
            card.transform.SetParent(container.transform, false);
            card.isDraggable = false;
            card.isSelectable = false;
        }

        if (game.GetCardsCount(game.GetCurrentStep(), Flank.Right) != 4)
        {
            int position = -1;
            ContainerFlank flank = GameObject.Find($"FlankRight{step}").GetComponent<ContainerFlank>();
            for (int i = 4; i < 8; i++)
            {
                if (flank.GetCardAt(i) == null) position = i;
            }
            Card card = Instantiate(cardUniversal);
            card.SetCard(noobRight);
            game.AddCardsToFlank(game.GetCurrentStep(), noobsRight, Flank.Right);
            flank.PlaceCard(card, position);
            Transform container = flank.transform.Find("Squads").Find($"CardsContainer-{position}");
            GameObject.Destroy(container.Find("CardPlaceholder(Clone)").gameObject);
            card.transform.SetParent(container.transform, false);
            card.isDraggable = false;
            card.isSelectable = false;
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
            Card _card = Instantiate(cardUniversal);
            _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
        }

        callback = SupportTacticalEnd;
    }

    public void SupportTacticalEnd(Card card)
    {
        Debug.Log("Support: Tactical Move End");
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 1;

        temporary.gameObject.SetActive(false);

        AddCardToHand(card);
        foreach (var _card in temporary.GetComponent<AbstractContainer>().GetCards())
        {
            DropCardToDrop(_card, false);
        }

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
        callback = AttackCallback;
    }

    public void SupportSniper(Skills skills)
    {
        this.tempSkills = skills;
        CurrentPlayer currentEnemy = game.GetNextStep();
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
        //TODO
        // game.HitSquad(card.card, this.tempSkills);
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
        callback = AttackCallback;
        if (this.attackState == AttackState.DEFENCE)
        {
            this.ApplyActiveSkills();
        }
    }

    public void RearRaid_Start()
    {
        this.rearRaidCounter = 2;
        CurrentPlayer currentEnemy = game.GetNextStep();
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
            CurrentPlayer currentEnemy = game.GetNextStep();
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
            this.callback = AttackCallback;
        }
    }
    private void AttackCallback(Card card)
    {
        Debug.Log("in attack callback");
        Debug.Log(position);
        if (attackState == AttackState.ATTACK)
        {
            attackCards[position] = card;
        }
        else
        {
            defenceCards[position] = card;
        }
        card.Highlight(true);
    }
    public void SelectCard(GameObject card, int position)
    {
        Debug.Log("SelectCard");
        Debug.Log(card.GetComponent<Card>().card.active);
        if (card.GetComponent<Card>().isSelectable == false) return;
        card.GetComponent<Card>().isSelectable = false;
        this.position = position;
        this.callback(card.GetComponent<Card>());
    }
    public void ResetSelectionCards()
    {
        foreach (var card in attackCards)
        {
            card.Highlight(false);
            card.isSelectable = true;
        }
        attackCards.Clear();
    }
    public void SupportMedicine_Start(Skills skills)
    {
        this.tempSkills = skills;
        this.callback = SupportMedicine_End;
        //TODO
    }
    public void SupportMedicine_End(Card card)
    {
        //TODO
        this.tempSkills = null;
        this.callback = AttackCallback;
        if (this.attackState == AttackState.DEFENCE)
        {
            ApplyActiveSkills();
        }
    }
    public void SupportScouting_Start(Skills skills)
    {
        this.tempSkills = skills;
        this.callback = SupportScouting_End;
        //TODO
    }
    public void SupportScouting_End(Card card)
    {
        //TODO
        this.tempSkills = null;
        this.callback = AttackCallback;
        if (this.attackState == AttackState.DEFENCE)
        {
            ApplyActiveSkills();
        }
    }
    public void SupportSapper_Start(Skills skills)
    {
        this.tempSkills = skills;
        this.callback = SupportSapper_End;
        //TODO
    }
    public void SupportSapper_End(Card card)
    {
        //TODO
        this.tempSkills = null;
        this.callback = AttackCallback;
        if (this.attackState == AttackState.DEFENCE)
        {
            ApplyActiveSkills();
        }
    }
    public void ApplyActiveSkills()
    {
        // foreach (var card in attackCards)
        // {
        //   if (card == null) continue;
        //   Skills skills = ((SquadCard)(card.card)).skills;
        //   if (skills.medicine > 0)
        //   {
        //     this.SupportMedic_Start(skills);
        //     return;
        //   }
        //   if (skills.shelling > 0)
        //   {
        //     this.SupportSniper(skills);
        //     return;
        //   }
        //   if (skills.sapper > 0)
        //   {
        //     this.SupportSniper(skills);
        //     return;
        //   }
        //   if (skills.scouting > 0)
        //   {
        //     this.SupportScouting_Start(skills);
        //     return;
        //   }
        // }
        // foreach (var card in defenceCards)
        // {
        //   if (card == null) continue;
        //   Skills skills = ((SquadCard)(card.card)).skills;
        //   if (skills.medicine > 0)
        //   {
        //     this.SupportMedicine_Start(skills);
        //     return;
        //   }
        //   if (skills.shelling > 0)
        //   {
        //     this.SupportSniper(skills);
        //     return;
        //   }
        //   if (skills.sapper > 0)
        //   {
        //     this.SupportSniper(skills);
        //     return;
        //   }
        //   if (skills.scouting > 0)
        //   {
        //     this.SupportScouting_Start(skills);
        //     return;
        //   }
        // }
        this.DefenceStart();
    }
}
