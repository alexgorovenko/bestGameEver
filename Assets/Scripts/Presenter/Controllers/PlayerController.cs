using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    GameObject chooseCommandors1;
    [SerializeField]
    GameObject chooseCommandors2;
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
    [SerializeField]
    GameObject menuEscLayer;
    [SerializeField]
    GameObject buttonExitYes;
    [SerializeField]
    GameObject buttonExitNo;
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
    private int position = 0;
    private bool isAttackActiveSkills = false;
    private int tempDamage;
    private int selected;
    private List<Tuple<Tuple<Skills.SkillCallback, int>, int>> attackInstants = new List<Tuple<Tuple<Skills.SkillCallback, int>, int>>();
    private List<Tuple<Tuple<Skills.SkillCallback, int>, int>> defenceInstants = new List<Tuple<Tuple<Skills.SkillCallback, int>, int>>();
    private int medicineCount;
    public int points = 0;

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
        this.buttonExitYes.GetComponent<Button>().onClick.AddListener(this.onExitYes);
        this.buttonExitNo.GetComponent<Button>().onClick.AddListener(this.onExitNo);
    }

    void onExitYes()
    {
        SceneManager.LoadScene("Start Screen");
    }

    void onExitNo()
    {
        menuEscLayer.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menuEscLayer.SetActive(!menuEscLayer.activeSelf);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("Main game Screen");
        }
    }

    public void GameStart()
    {
        CurrentPlayer currentPlayer = game.GetCurrentStep();
        playedSquadCards = 0;
        playedSupportCards = 0;
        playedFortificationCards = 0;
        points = 4;
        GetCardsFromDeckToHand(currentPlayer, 4);
        GetCardsFromDeckToHand(game.GetNextStep(), 5);
        hands[game.GetNextStep()].gameObject.SetActive(false);
        HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
        HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
        // muligan?
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
        GetCardsFromDeckToHand(game.GetCurrentStep(), points);
        points = 4;
        game.NextStep();
        hands[game.GetCurrentStep()].gameObject.SetActive(true);

        HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
        HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";

    }
    public void ShowCommandorsChooseMenu()
    {
        commandorsOV.SetActive(true);
        // render commandors
        foreach (CommandorCard commandor in game.freeCommandors1)
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
            _cardCommandor.transform.SetParent(chooseCommandors1.transform, false);
            _cardCommandor.GetComponent<CardCommandorView>().SetCard(commandor);
        }
        foreach (CommandorCard commandor in game.freeCommandors2)
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
            _cardCommandor.transform.SetParent(chooseCommandors2.transform, false);
            _cardCommandor.GetComponent<CardCommandorView>().SetCard(commandor);
        }
        chooseCommandors2.SetActive(false);

    }
    public void HideCommandorsChooseMenu()
    {
        Destroy(commandorsOV);
    }
    public void SetCommandor(Transform transform, CommandorCard commandor)
    {
        game.SetCommandor(game.GetCurrentStep(), commandor);
        transform.SetParent(commandorFields[(int)commandorsChosen].transform, false);
        commandorsChosen++;
        if (commandorsChosen == 1)
        {
            flankLeft1.commandor = commandor;
            chooseCommandors1.SetActive(false);
            chooseCommandors2.SetActive(true);
        }
        if (commandorsChosen == 2)
        {
            flankLeft2.commandor = commandor;
        }
        if (commandorsChosen == 3)
        {
            flankRight2.commandor = commandor;
            chooseCommandors1.SetActive(true);
            chooseCommandors2.SetActive(false);
        }
        if (commandorsChosen != 2)
        {
            game.NextStep();
        }
        if (commandorsChosen == 4)
        {
            flankRight1.commandor = commandor;
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
    public CardSquad GetOppositeCard(Card card, int position)
    {
        if (position != -1 && this.attackCards[position] != null && this.attackCards[position] != card)
        {
            return (CardSquad)this.attackCards[position];
        }
        if (this.defenceCards[position] != card)
        {
            return (CardSquad)this.defenceCards[position];
        }
        return null;
    }
    public void DropCardToFlank(Card card, int position, ContainerFlank flank)
    {
        AbstractCard cardModel = card.GetComponent<Card>().card;
        Flank flankModel = flank.GetComponent<ContainerFlank>().flank;
        switch (cardModel)
        {
            case SquadCard s:              
                if (points > 0)
                {
                    if (flank.GetComponent<ContainerFlank>().GetCardAt(position) == null)
                    {
                        if (playedSquadCards == 0)
                        {
                            if (flank.GetComponent<ContainerFlank>().commandor.skills.forceAgility)
                            {
                                cardModel.active = true;
                            }
                        }
                        if (flank.GetComponent<ContainerFlank>().commandor.skills.forceRevenge)
                        {
                            game.HitHeadsquater(game.GetNextStep(), 1);
                        }
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
                        points--;
                    }
                }
                break;
            case FortificationCard f:
                if (points > 0)
                {
                    game.ApplyAttackerFortificationCard(game.GetCurrentStep(), f, flankModel);
                    card.transform.SetParent(flank.transform.Find("FortificationContainer").transform);
                    playedFortificationCards++;
                    points--;
                }
                break;
        }
    }

    public void DropCardToDrop(Card card, bool isEnemy)
    {
        if (card == null) return;
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
        if (callback != AttackCallback) return;
        if (attackState == AttackState.ATTACK)
        {
            AttackButton.GetComponentInChildren<Text>().text = "В бой!";
            AttackStart();
            attackState = AttackState.DEFENCE;
        }
        else
        {
            AttackButton.GetComponentInChildren<Text>().text = "Атака!";
            FillActiveSkills();
            bool needActive = true;
            if (game.GetCurrentStep() == CurrentPlayer.FIRST)
            {
                if (flankLeft1.commandor.skills.forceShelling)
                {
                    this.isAttackActiveSkills = true;
                    this.position = 0;
                    this.SupportShelling_Start(1);
                    needActive = false;
                }
                if (flankRight1.commandor.skills.forceShelling)
                {
                    this.isAttackActiveSkills = true;
                    this.position = 4;
                    this.SupportShelling_Start(1);
                    needActive = false;
                }
            }
            if (game.GetCurrentStep() == CurrentPlayer.SECOND)
            {
                if (flankLeft2.commandor.skills.forceShelling)
                {
                    this.isAttackActiveSkills = true;
                    this.position = 0;
                    this.SupportShelling_Start(1);
                    needActive = false;
                }
                if (flankRight2.commandor.skills.forceShelling)
                {
                    this.isAttackActiveSkills = true;
                    this.position = 4;
                    this.SupportShelling_Start(1);
                    needActive = false;
                }
            }
            if (needActive)
            {
                ApplyActiveSkills();
            }
            attackState = AttackState.ATTACK;
        }
    }

    public void AttackStart()
    {
        // выбраны те, кто атакуют
        // запрещаем выбирать чужие карты
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        flanks[step].SetEnabledDrag(false);
        flanks[step + 1].SetEnabledDrag(false);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step].SetDrag(CardsContainerDropHandler.State.DEFENCE);
        flanks[step + 1].SetDrag(CardsContainerDropHandler.State.DEFENCE);
        flanks[step].SetActive(false);
        flanks[step + 1].SetActive(false);
        // выбираем тех, кто защищается

        // начать атаку => в бой
        // применить активные скилы
    }

    public void DefenceStart()
    {
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        CurrentPlayer deffendersStep = game.GetNextStep();
        // уже выбраны защитники
        List<SquadCard> cards = new List<SquadCard>();
        for (var i = 0; i < 4; i++)
        {
            cards.Add(attackCards[i] == null ? null : (SquadCard)attackCards[i].card);
        }
        game.SetAttackers(game.GetCurrentStep(), cards, Flank.Left);
        cards = new List<SquadCard>(4);
        for (var i = 0; i < 4; i++)
        {
            if (attackCards[i] == null)
            {
                cards.Add(null);
                continue;
            }
            cards.Add(((CardSquad)(attackCards[i])).attackCard == null ? null : (SquadCard)((CardSquad)(attackCards[i])).attackCard.card);
        }
        game.SetDeffenders(deffendersStep, cards, Flank.Left);
        cards = new List<SquadCard>(4);
        for (var i = 4; i < 8; i++)
        {
            cards.Add(attackCards[i] == null ? null : (SquadCard)attackCards[i].card);
        }
        game.SetAttackers(game.GetCurrentStep(), cards, Flank.Right);
        cards = new List<SquadCard>(4);
        for (var i = 4; i < 8; i++)
        {
            if (attackCards[i] == null)
            {
                cards.Add(null);
                continue;
            }
            cards.Add(((CardSquad)(attackCards[i])).attackCard == null ? null : (SquadCard)((CardSquad)(attackCards[i])).attackCard.card);
        }
        game.SetDeffenders(deffendersStep, cards, Flank.Right);
        game.Attack(game.GetCurrentStep());
        game.ApplyAttack(game.GetCurrentStep());
        //  Update UI

        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        int count;
        count = flanks[step].GetComponent<ContainerFlank>().DestroyDead();
        if (flanks[step].GetComponent<ContainerFlank>().commandor.skills.revenge)
        {
            game.HitHeadsquater(game.GetNextStep(), count);
        }
        count = flanks[step + 1].GetComponent<ContainerFlank>().DestroyDead();
        if (flanks[step + 1].GetComponent<ContainerFlank>().commandor.skills.revenge)
        {
            game.HitHeadsquater(game.GetNextStep(), count);
        }
        flanks[step].GetComponent<ContainerFlank>().RefreshActive();
        flanks[step + 1].GetComponent<ContainerFlank>().RefreshActive();
        flanks[step].SetDrag(CardsContainerDropHandler.State.ATTACK);
        flanks[step + 1].SetDrag(CardsContainerDropHandler.State.ATTACK);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        count = flanks[step].GetComponent<ContainerFlank>().DestroyDead();
        if (flanks[step].GetComponent<ContainerFlank>().commandor.skills.revenge)
        {
            game.HitHeadsquater(game.GetCurrentStep(), count);
        }
        count = flanks[step + 1].GetComponent<ContainerFlank>().DestroyDead();
        if (flanks[step + 1].GetComponent<ContainerFlank>().commandor.skills.revenge)
        {
            game.HitHeadsquater(game.GetCurrentStep(), count);
        }
        flanks[step].GetComponent<ContainerFlank>().SetEnabledDrag(true);
        flanks[step + 1].GetComponent<ContainerFlank>().SetEnabledDrag(true);
        flanks[step].GetComponent<ContainerFlank>().SetActive(false);
        flanks[step + 1].GetComponent<ContainerFlank>().SetActive(false);
        for (int i = 0; i < 8; i++)
        {
            if (attackCards[i] != null)
            {
                attackCards[i].Highlight(false);
                ((CardSquad)(attackCards[i])).attackCard = null;
                attackCards[i] = null;
            }
            if (defenceCards[i] != null)
            {
                defenceCards[i].Highlight(false);
                ((CardSquad)(defenceCards[i])).attackCard = null;
                defenceCards[i] = null;
            }
        }

        HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
        HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
        if (game.GetHeadsquaterHealth(CurrentPlayer.FIRST) <= 0)
        {
            SceneManager.LoadScene("Loose");
        }
        else if (game.GetHeadsquaterHealth(CurrentPlayer.SECOND) <= 0)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            Next();
        }
    }

    // Support cards callbacks
    public void SupportShelling_Start(int power)
    {
        if (this.GetOppositeFlank().GetCardsCapacity() == 0) return;
        for (var i = 0; i < 4; i++)
        {
            this.flanks[i]._SetActive(false);
        }
        this.GetOppositeFlank()._SetActive(true);
        this.tempDamage = power;
        AttackButton.GetComponentInChildren<Text>().text = "Обстрел!";
        callback = SupportShelling_End;
    }

    private void SupportShelling_End(Card card)
    {
        game.HitSquad((SquadCard)card.card, this.tempDamage);
        this.GetOppositeFlank()._SetActive(false);
        callback = AttackCallback;
        this.ApplyActiveSkills();
    }

    public void SupportStun_Start(int power)
    {
        if (this.GetOppositeFlank().GetCardsCapacity() == 0) return;
        CurrentPlayer currentEnemy = this.isAttackActiveSkills ? game.GetNextStep() : game.GetCurrentStep();
        int step = currentEnemy == CurrentPlayer.FIRST ? 1 : 2;
        for (var i = 0; i < 4; i++)
        {
            this.flanks[i]._SetActive(false);
        }
        this.GetOppositeFlank()._SetActive(true);
        this.tempDamage = power;
        this.selected = 0;
        AttackButton.GetComponentInChildren<Text>().text = "Оглушение!";
        callback = SupportStun_End;
    }

    private void SupportStun_End(Card card)
    {
        SquadCard _card = (SquadCard)card.card;
        if (_card.rarity != Rarity.Epic)
        {
            _card.isDeaf = true;
        }
        this.selected++;
        if (this.selected < this.tempDamage && this.GetOppositeFlank().GetCardsCapacity() > this.selected)
        {
            return;
        }
        CurrentPlayer currentEnemy = this.isAttackActiveSkills ? game.GetNextStep() : game.GetCurrentStep();
        int step = currentEnemy == CurrentPlayer.FIRST ? 1 : 2;
        this.GetOppositeFlank()._SetActive(false);
        callback = AttackCallback;
        this.ApplyActiveSkills();
    }

    private void AttackCallback(Card card)
    {
        if (position == -1) return;
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
        if (card.GetComponent<Card>().isSelectable == false) return;
        card.GetComponent<Card>().isSelectable = false;
        this.position = position;
        this.callback(card.GetComponent<Card>());
    }
    public void ResetSelectionCards()
    {
        foreach (var card in attackCards)
        {
            if (card == null) continue;
            card.Highlight(false);
            card.isSelectable = true;
        }
        for (var i = 0; i < 8; i++)
        {
            attackCards[i] = null;
        }
    }
    public void SupportScouting_Start(int power)
    {
        this.GetCardsFromDeckToHand(this.isAttackActiveSkills ? this.game.GetCurrentStep() : this.game.GetNextStep(), power);
        this.ApplyActiveSkills();
    }
    public void SupportSapper_Start(int power)
    {
        this.DropCardToDrop(this.GetCurrentFlank().transform.Find("FortificationContainer").GetComponentInChildren<Card>(), !this.isAttackActiveSkills);
        this.ApplyActiveSkills();
    }
    private ContainerFlank GetOppositeFlank()
    {
        this.isAttackActiveSkills = !this.isAttackActiveSkills;
        ContainerFlank result = this.GetCurrentFlank();
        this.isAttackActiveSkills = !this.isAttackActiveSkills;
        return result;
    }
    private ContainerFlank GetCurrentFlank()
    {
        if (this.isAttackActiveSkills)
        {
            if (this.position > 3)
            {
                return this.game.GetCurrentStep() == CurrentPlayer.FIRST ? this.flankRight1 : this.flankRight2;
            }
            else
            {
                return this.game.GetCurrentStep() == CurrentPlayer.FIRST ? this.flankLeft1 : this.flankLeft2;
            }
        }
        else
        {
            if (this.position > 3)
            {
                return this.game.GetCurrentStep() == CurrentPlayer.SECOND ? this.flankRight1 : this.flankRight2;
            }
            else
            {
                return this.game.GetCurrentStep() == CurrentPlayer.SECOND ? this.flankLeft1 : this.flankLeft2;
            }
        }
    }
    private void FillActiveSkills()
    {
        for (var i = 0; i < 8; i++)
        {
            this.position = i;
            Card card = this.attackCards[i];
            if (card == null) continue;
            Skills skills = ((SquadCard)(card.card)).skills;
            if (skills.instantSkills == null) continue;
            foreach (var skill in skills.instantSkills)
            {
                this.attackInstants.Add(new Tuple<Tuple<Skills.SkillCallback, int>, int>(skill, i));
            }
        }
        this.isAttackActiveSkills = false;
        for (var i = 0; i < 8; i++)
        {
            this.position = i;
            Card card = this.defenceCards[i];
            if (card == null) continue;
            Skills skills = ((SquadCard)(card.card)).skills;
            if (skills.instantSkills == null) continue;
            foreach (var skill in skills.instantSkills)
            {
                this.defenceInstants.Add(new Tuple<Tuple<Skills.SkillCallback, int>, int>(skill, i));
            }
        }
    }
    public void ApplyActiveSkills()
    {
        Tuple<Skills.SkillCallback, int> instant;
        this.isAttackActiveSkills = true;
        if (this.attackInstants.Count > 0)
        {
            instant = this.attackInstants[0].Item1;
            this.position = this.attackInstants[0].Item2;
            this.attackInstants.RemoveAt(0);
            instant.Item1(instant.Item2, this);
            return;
        }
        this.isAttackActiveSkills = false;
        if (this.defenceInstants.Count > 0)
        {
            instant = this.defenceInstants[0].Item1;
            this.position = this.defenceInstants[0].Item2;
            this.defenceInstants.RemoveAt(0);
            instant.Item1(instant.Item2, this);
            return;
        }
        this.DefenceStart();
        ResetSelectionCards();
    }
    public void SupportAcidRain_Start()
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        if (flanks[step].GetCardsCapacity() == 0 && flanks[step].GetCardsCapacity() == 0) return;
        flanks[step]._SetActive(true);
        flanks[step + 1]._SetActive(true);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step]._SetActive(false);
        flanks[step + 1]._SetActive(false);
        AttackButton.GetComponentInChildren<Text>().text = "Кислотный дождь!";
        callback = SupportAcidRain_End;
    }
    private void SupportAcidRain_End(Card card)
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        ContainerFlank flank = card.position > 3 ? flanks[step + 1] : flanks[step];
        if (flank.GetComponent<ContainerFlank>().commandor.skills.supportRevenge)
        {
            game.HitHeadsquater(game.GetNextStep(), 2);
        }
        foreach (Card _card in flank.GetCards())
        {
            if (_card == null) continue;
            ((SquadCard)_card.card).isWeak = true;
        }
        flanks[step]._SetActive(false);
        flanks[step + 1]._SetActive(false);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step].RefreshActive();
        flanks[step + 1].RefreshActive();
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        ResetSelectionCards();
        callback = AttackCallback;
    }
    public void SupportAmbush_Start()
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        if (flanks[step].GetCardsCapacity() == 0 && flanks[step].GetCardsCapacity() == 0) return;
        flanks[step]._SetActive(true);
        flanks[step + 1]._SetActive(true);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step]._SetActive(false);
        flanks[step + 1]._SetActive(false);
        AttackButton.GetComponentInChildren<Text>().text = "Засада!";
        callback = SupportAmbush_End;
    }
    private void SupportAmbush_End(Card card)
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        game.HitSquad((SquadCard)card.card, 3);
        flanks[step]._SetActive(false);
        flanks[step + 1]._SetActive(false);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step].RefreshActive();
        flanks[step + 1].RefreshActive();
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        ResetSelectionCards();
        callback = AttackCallback;
    }

    public void SupportBattleCry_Start()
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        if (flanks[step].GetCardsCapacity() == 0 && flanks[step + 1].GetCardsCapacity() == 0) return;
        flanks[step]._SetActive(true);
        flanks[step + 1]._SetActive(true);
        AttackButton.GetComponentInChildren<Text>().text = "Боевой клич!";
        callback = SupportBattleCry_End;
    }

    private void SupportBattleCry_End(Card card)
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        ContainerFlank flank = card.position > 3 ? flanks[step + 1] : flanks[step];
        if (flank.GetComponent<ContainerFlank>().commandor.skills.supportRevenge)
        {
            game.HitHeadsquater(game.GetNextStep(), 2);
        }
        foreach (Card _card in flank.GetCards())
        {
            if (_card == null) continue;
            ((SquadCard)_card.card).addAttack++;
        }
        flanks[step].RefreshActive();
        flanks[step + 1].RefreshActive();
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        ResetSelectionCards();
        callback = AttackCallback;
    }

    public void SupportClanCall_Start()
    {
        HashSet<SquadCard> noobsLeft = new HashSet<SquadCard>();
        HashSet<SquadCard> noobsRight = new HashSet<SquadCard>();
        SquadCard noobLeft = new SquadCard(Rarity.Token, "ополчение горцев", "", 1, 2, 1, new Skills());
        SquadCard noobRight = new SquadCard(Rarity.Token, "ополчение горцев", "", 1, 2, 1, new Skills());
        noobsLeft.Add(noobLeft);
        noobsRight.Add(noobRight);
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
        if (game.GetCardsCount(game.GetCurrentStep(), Flank.Left) != 4)
        {
            int position = -1;
            ContainerFlank flank = GameObject.Find($"FlankLeft{step}").GetComponent<ContainerFlank>();
            if (flank.GetComponent<ContainerFlank>().commandor.skills.supportRevenge)
            {
                game.HitHeadsquater(game.GetNextStep(), 2);
            }
            for (int i = 0; i < 4; i++)
            {
                if (flank.GetCardAt(i) == null) position = i;
            }
            Card card = Instantiate(cardUniversal);
            card.SetCard(noobLeft);
            game.AddCardsToFlank(game.GetCurrentStep(), noobsLeft, Flank.Left);
            flank.PlaceCard(card, position);
            Transform container = flank.transform.Find("Squads").Find($"CardsContainer-{position}");
            Destroy(container.Find("CardPlaceholder(Clone)").gameObject);
            card.transform.SetParent(container.transform, false);
            card.isDraggable = false;
            card.isSelectable = false;
        }
        if (game.GetCardsCount(game.GetCurrentStep(), Flank.Right) != 4)
        {
            int position = -1;
            ContainerFlank flank = GameObject.Find($"FlankRight{step}").GetComponent<ContainerFlank>();
            if (flank.GetComponent<ContainerFlank>().commandor.skills.supportRevenge)
            {
                game.HitHeadsquater(game.GetNextStep(), 2);
            }
            for (int i = 4; i < 8; i++)
            {
                if (flank.GetCardAt(i) == null) position = i;
            }
            Card card = Instantiate(cardUniversal);
            card.SetCard(noobRight);
            game.AddCardsToFlank(game.GetCurrentStep(), noobsRight, Flank.Right);
            flank.PlaceCard(card, position);
            Transform container = flank.transform.Find("Squads").Find($"CardsContainer-{position}");
            Destroy(container.Find("CardPlaceholder(Clone)").gameObject);
            card.transform.SetParent(container.transform, false);
            card.isDraggable = false;
            card.isSelectable = false;
        }
    }

    public void SupportFieldMedicine_Start()
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;

        List<Card> temporaryCards = new List<Card>();

        if (step == 1)
        {
            temporaryCards.AddRange(drop1.GetCards());
        }
        else
        {
            temporaryCards.AddRange(drop2.GetCards());
        }

        for (var i = 0; i < temporaryCards.Count; i++)
        {
            if (temporaryCards[i].card.rarity != Rarity.General || temporaryCards[i].card.rarity != Rarity.Rare || !(temporaryCards[i].card is SquadCard))
            {
                temporaryCards.RemoveAt(i);
                i--;
            }
        }

        if (temporaryCards.Count == 0) return;

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
        
        foreach (Card card in temporaryCards)
        {
            Card _card = Instantiate(cardUniversal);
            _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
        }
        medicineCount = 2;
        callback = SupportFieldMedicine_End;
    }

    private void SupportFieldMedicine_End(Card card)
    {
        medicineCount--;
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
        AddCardToHand(card);
        if (medicineCount == 0)
        {
            temporary.gameObject.SetActive(false);
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
    }

    public void SupportHeroesOfLegends_Start()
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;

        List<Card> temporaryCards = new List<Card>();
        if (step == 1)
        {
            temporaryCards.AddRange(deck1.GetCards());
        }
        else
        {
            temporaryCards.AddRange(deck2.GetCards());
        }

        for (var i = 0; i < temporaryCards.Count; i++)
        {
            if (!(temporaryCards[i].card is SquadCard))
            {
                temporaryCards.RemoveAt(i);
                i--;
            }
        }

        if (temporaryCards.Count == 0) return;

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

        foreach (Card card in temporaryCards)
        {
            Card _card = Instantiate(cardUniversal);
            _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
        }
        callback = SupportHeroesOfLegends_End;
    }

    private void SupportHeroesOfLegends_End(Card card)
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
        temporary.gameObject.SetActive(false);

        AddCardToHand(card);

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

    public void SupportInsurrection_Start()
    {
        HashSet<SquadCard> noobsLeft = new HashSet<SquadCard>();
        HashSet<SquadCard> noobsRight = new HashSet<SquadCard>();
        SquadCard noobLeft = new SquadCard(Rarity.Token, "волонтёры", "", 1, 1, 1, new Skills());
        SquadCard noobRight = new SquadCard(Rarity.Token, "волонтёры", "", 1, 1, 1, new Skills());
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
            Destroy(container.Find("CardPlaceholder(Clone)").gameObject);
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
            Destroy(container.Find("CardPlaceholder(Clone)").gameObject);
            card.transform.SetParent(container.transform, false);
            card.isDraggable = false;
            card.isSelectable = false;
        }
    }

    public void SupportRaidOnTheRocks_Start()
    {
        ContainerHand hand = game.GetCurrentStep() == CurrentPlayer.FIRST ? hand2 : hand1;
        List<Card> cards = hand.GetCards();
        if (cards.Count > 0)
        {
            this.DropCardToDrop(cards[(int)Math.Round(UnityEngine.Random.value * (cards.Count - 1))], true);
        }
        if (cards.Count > 0)
        {
            this.DropCardToDrop(cards[(int)Math.Round(UnityEngine.Random.value * (cards.Count - 1))], true);
        }
    }

    public void SupportSmuggling_Start()
    {
        List<Card> temporaryCards = new List<Card>();
        temporaryCards.Add(GetCardFromDeck(game.GetCurrentStep()));
        temporaryCards.Add(GetCardFromDeck(game.GetCurrentStep()));
        temporaryCards.Add(GetCardFromDeck(game.GetCurrentStep()));
        if (temporaryCards.Count == 0) return;

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
        temporary.gameObject.SetActive(true);

        foreach (Card card in temporaryCards)
        {
            Card _card = Instantiate(cardUniversal);
            _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
        }
        callback = SupportSmuggling_End;
    }

    private void SupportSmuggling_End(Card card)
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
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

    public void SupportTremblingEarth_Start()
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        if (flanks[step].GetCardsCapacity() == 0 && flanks[step].GetCardsCapacity() == 0) return;
        flanks[step]._SetActive(true);
        flanks[step + 1]._SetActive(true);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step]._SetActive(false);
        flanks[step + 1]._SetActive(false);
        AttackButton.GetComponentInChildren<Text>().text = "Дрожь земли!";
        callback = SupportTremblingEarh_End;
    }

    private void SupportTremblingEarh_End(Card card)
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;
        ContainerFlank flank = card.position > 3 ? flanks[step + 1] : flanks[step];
        if (flank.GetComponent<ContainerFlank>().commandor.skills.supportRevenge)
        {
            game.HitHeadsquater(game.GetNextStep(), 2);
        }
        this.DropCardToDrop(flank.transform.Find("FortificationContainer").GetComponentInChildren<Card>(), !this.isAttackActiveSkills);
        List<Card> cards = flank.GetCards();
        if (flank.GetCardsCapacity() > 1)
        {
            Card _card = null;
            while (_card == null)
            {
                _card = cards[(int)Math.Round(UnityEngine.Random.value * (cards.Count - 1))];
            }
            game.HitSquad((SquadCard)_card.card, 1);
            _card = null;
            while (_card == null)
            {
                _card = cards[(int)Math.Round(UnityEngine.Random.value * (cards.Count - 1))];
            }
            game.HitSquad((SquadCard)_card.card, 1);
        }
        flanks[step]._SetActive(false);
        flanks[step + 1]._SetActive(false);
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step].RefreshActive();
        flanks[step + 1].RefreshActive();
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        ResetSelectionCards();
        callback = AttackCallback;
    }

    public void ShowDrop(ContainerDrop drop)
    {
        if (drop.GetCards().Count == 0) return;
        temporary.gameObject.SetActive(true);
        List<Card> temporaryCards = new List<Card>();
        temporaryCards.AddRange(drop.GetCards());
        foreach (Card card in temporaryCards)
        {
            Card _card = Instantiate(cardUniversal);
            _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
        }
        callback = HideDrop;
    }

    private void HideDrop(Card card)
    {
        temporary.gameObject.SetActive(false);
        callback = AttackCallback;
    }

}
