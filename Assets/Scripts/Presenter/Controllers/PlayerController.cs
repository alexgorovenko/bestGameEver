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
    [SerializeField]
    GameObject buttonCloseTemp;
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
    private bool isAttackActiveSkills = true;
    private int tempDamage;
    private int selected;
    private List<Tuple<Tuple<Skills.SkillCallback, int>, int>> attackInstants = new List<Tuple<Tuple<Skills.SkillCallback, int>, int>>();
    private List<Tuple<Tuple<Skills.SkillCallback, int>, int>> defenceInstants = new List<Tuple<Tuple<Skills.SkillCallback, int>, int>>();
    private int medicineCount;
    public int points = 0;
    private List<CardCommandorView> commandorsAI = new List<CardCommandorView>();
    private int round = 0;
    private bool isAIFirst = false;

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
        this.buttonCloseTemp.GetComponent<Button>().onClick.AddListener(this.HideDrop);
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
        round = 1;
        CurrentPlayer currentPlayer = game.GetCurrentStep();
        playedSquadCards = 0;
        playedSupportCards = 0;
        playedFortificationCards = 0;
        points = 4;
        GetCardsFromDeckToHand(currentPlayer, 4);
        GetCardsFromDeckToHand(game.GetNextStep(), 5);
        hands[game.GetNextStep()].gameObject.SetActive(false);
        hands[CurrentPlayer.SECOND].gameObject.SetActive(false);
        HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
        HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
        if (currentPlayer == CurrentPlayer.SECOND)
        {
            Next();
        }
    }
    private Card ChooseCardForPlay()
    {
        List<Card> cards = this.hand2.GetCards();
        List<int> priors = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            priors.Add(cards[i].card.priority);
        }
        if (isAIFirst)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i].card is SquadCard && ((SquadCard)cards[i].card).skills.agility)
                {
                    priors[i] *= 100;
                }
            }
        }
        bool hasBrotherhood = false;
        foreach (var card in flankLeft2.GetCards())
        {
            if (card == null) continue;
            if (((SquadCard)(card.card)).skills.brotherhood)
            {
                hasBrotherhood = true;
                break;
            }
        }
        if (!hasBrotherhood)
        {
            foreach (var card in flankRight2.GetCards())
            {
                if (card == null) continue;
                if (((SquadCard)(card.card)).skills.brotherhood)
                {
                    hasBrotherhood = true;
                    break;
                }
            }
        }
        if (hasBrotherhood)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i].card is SquadCard && ((SquadCard)cards[i].card).skills.brotherhood)
                {
                    priors[i] *= 100;
                }
            }
        }
        if (flankLeft2.GetCardsCapacity() == 4 && flankRight2.GetCardsCapacity() == 4)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i].card is SquadCard || cards[i].card is SupportCard)
                {
                    priors[i] = 0;
                }
            }
        }
        if (game.GetFortificationCard(CurrentPlayer.SECOND, Flank.Left) != null && game.GetFortificationCard(CurrentPlayer.SECOND, Flank.Right) != null)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i].card is FortificationCard)
                {
                    priors[i] = 0;
                }
            }
        }
        if (game.GetFortificationCard(CurrentPlayer.FIRST, Flank.Left) != null || game.GetFortificationCard(CurrentPlayer.FIRST, Flank.Right) != null)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i].card.name == "Инженеры-подрывники")
                {
                    priors[i] *= 100;
                }
            }
        }
        else
        {
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i].card.name == "Инженеры-подрывники")
                {
                    priors[i] *= 0;
                }
            }
        }
        Card res = null;
        int maxPr = 0;
        for (var i = 0; i < cards.Count; i++)
        {
            if (priors[i] > maxPr)
            {
                maxPr = priors[i];
                res = cards[i];
            }
        }
        return res;
    }
    private void SelectDefenceAI()
    {
        for (var i = 0; i < 4; i++)
        {
            if (attackCards[i] == null) continue;
            Card attackCard = attackCards[i];
            Card defenceCard = null;
            for (var j = 0; j < 4; j++)
            {
                if (flankLeft2.GetCardAt(j) == null || ((CardSquad)flankLeft2.GetCardAt(j)).attackCard != null) continue;
                Card card = flankLeft2.GetCardAt(j);
                if (defenceCard == null)
                {
                    defenceCard = card;
                    continue;
                }
                SquadCard _attackCard = (SquadCard)attackCard.card;
                SquadCard _defenceCard = (SquadCard)defenceCard.card;
                SquadCard _card = (SquadCard)card.card;
                if (_attackCard.stamina < _card.protection && _card.protection < _attackCard.attack)
                {
                    if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection < _attackCard.attack && _defenceCard.attack < _card.attack) continue;
                    defenceCard = card;
                }
                if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection < _attackCard.attack) continue;
                if (_attackCard.stamina < _card.protection && _card.protection == _attackCard.attack)
                {
                    if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection == _attackCard.attack && _defenceCard.attack < _card.attack) continue;
                    defenceCard = card;
                }
                if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection == _attackCard.attack) continue;
                if (_card.attack < _defenceCard.attack)
                {
                    defenceCard = card;
                }
            }
            if (defenceCard != null)
            {
                ((CardSquad)defenceCard).attackCard = (CardSquad)attackCard;
                ((CardSquad)attackCard).attackCard = (CardSquad)defenceCard;
                defenceCards[i] = defenceCard;
                defenceCard.Highlight(true);
            }
        }
        for (var i = 4; i < 8; i++)
        {
            if (attackCards[i] == null) continue;
            Card attackCard = attackCards[i];
            Card defenceCard = null;
            for (var j = 4; j < 8; j++)
            {
                if (flankRight2.GetCardAt(j) == null || ((CardSquad)flankRight2.GetCardAt(j)).attackCard != null) continue;
                Card card = flankRight2.GetCardAt(j);
                if (defenceCard == null)
                {
                    defenceCard = card;
                    continue;
                }
                SquadCard _attackCard = (SquadCard)attackCard.card;
                SquadCard _defenceCard = (SquadCard)defenceCard.card;
                SquadCard _card = (SquadCard)card.card;
                if (_attackCard.stamina < _card.protection && _card.protection < _attackCard.attack)
                {
                    if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection < _attackCard.attack && _defenceCard.attack < _card.attack) continue;
                    defenceCard = card;
                }
                if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection < _attackCard.attack) continue;
                if (_attackCard.stamina < _card.protection && _card.protection == _attackCard.attack)
                {
                    if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection == _attackCard.attack && _defenceCard.attack < _card.attack) continue;
                    defenceCard = card;
                }
                if (_attackCard.stamina < _defenceCard.protection && _defenceCard.protection == _attackCard.attack) continue;
                if (_card.attack < _defenceCard.attack)
                {
                    defenceCard = card;
                }
            }
            if (defenceCard != null)
            {
                ((CardSquad)defenceCard).attackCard = (CardSquad)attackCard;
                ((CardSquad)attackCard).attackCard = (CardSquad)defenceCard;
                defenceCards[i] = defenceCard;
                defenceCard.Highlight(true);
            }
        }
    }
    private void SelectAttackAI()
    {
        for (var i = 0; i < 4; i++)
        {
            Card card = flankLeft2.GetCardAt(i);
            if (card == null || ((SquadCard)card.card).attack < 2) continue;
            this.AttackCallback(card);
        }
        for(var i = 4; i < 8; i++)
        {
            Card card = flankRight2.GetCardAt(i);
            if (card == null || ((SquadCard)card.card).attack < 2) continue;
            this.AttackCallback(card);
        }
    }
    private bool CheckSapper (SquadCard card)
    {
        if (card.skills.instantSkills == null) return false;
        foreach (var skill in card.skills.instantSkills)
        {
            if (skill.Item1 == Skills.Sapper) return true;
        }
        return false;
    }
    private void ApplyShellingAI(Card card, Flank flank)
    {
        if (((SquadCard)(card.card)).skills.instantSkills == null ||
                        ((SquadCard)(card.card)).skills.instantSkills[0].Item1 != Skills.Shelling) return;
        Tuple<Skills.SkillCallback, int> skill = ((SquadCard)(card.card)).skills.instantSkills[0];
        Card aim = null;
        if (flank == Flank.Left)
        {
            for (var i = 0; i < 4; i++)
            {
                Card _card = flankLeft1.GetCardAt(i);
                if (_card == null) continue;
                if (((SquadCard)(_card.card)).stamina == skill.Item2)
                {
                    aim = _card;
                    break;
                }
                if (aim == null)
                {
                    aim = _card;
                    continue;
                }
                if (((SquadCard)(_card.card)).stamina < ((SquadCard)(aim.card)).stamina)
                {
                    aim = _card;
                }
            }
        }
        if (flank == Flank.Right)
        {
            for (var i = 4; i < 8; i++)
            {
                Card _card = flankRight1.GetCardAt(i);
                if (_card == null) continue;
                if (((SquadCard)(_card.card)).stamina == skill.Item2)
                {
                    aim = _card;
                    break;
                }
                if (aim == null)
                {
                    aim = _card;
                    continue;
                }
                if (((SquadCard)(_card.card)).stamina < ((SquadCard)(aim.card)).stamina)
                {
                    aim = _card;
                }
            }
        }
        if (aim != null) this.SupportShelling_End(aim);
        ((SquadCard)(card.card)).skills.instantSkills = null;
    }
    private bool TryToPutCardAI(Card card, Flank flank)
    {
        if (flank == Flank.Left)
        {
            for (var i = 0; i < 4; i++)
            {
                if (flankLeft2.GetCardAt(i) == null)
                {
                    this.ApplyShellingAI(card, flank);           
                    this.DropCardToFlank(card, i, flankLeft2);
                    return true;
                }
            }
        }
        if (flank == Flank.Right)
        {
            for (var i = 4; i < 8; i++)
            {
                if (flankRight2.GetCardAt(i) == null)
                {
                    this.ApplyShellingAI(card, flank);
                    this.DropCardToFlank(card, i, flankRight2);
                    return true;
                }
            }
        }
        return false;
    }
    private void PlayCardAI()
    {
        Card card = this.ChooseCardForPlay();
        if (card == null) return;
        if (card.card is SquadCard)
        {
            SquadCard _card = (SquadCard)card.card;
            if (_card.skills.agility)
            {
                if (flankLeft1.GetCardsCapacity() < flankRight1.GetCardsCapacity() && this.TryToPutCardAI(card, Flank.Left)) return;
                if (flankRight1.GetCardsCapacity() < flankLeft1.GetCardsCapacity() && this.TryToPutCardAI(card, Flank.Right)) return;
            }
            if (_card.skills.brotherhood)
            {
                int brotherhoodLeft = 0;
                int brotherhoodRight = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (flankLeft2.GetCardAt(i) != null && ((SquadCard)flankLeft2.GetCardAt(i).card).skills.brotherhood) brotherhoodLeft++;
                }
                for (int i = 4; i < 8; i++)
                {
                    if (flankRight2.GetCardAt(i) != null && ((SquadCard)flankRight2.GetCardAt(i).card).skills.brotherhood) brotherhoodRight++;
                }
                if (brotherhoodLeft > brotherhoodRight && this.TryToPutCardAI(card, Flank.Left)) return;
                if (brotherhoodRight > brotherhoodLeft && this.TryToPutCardAI(card, Flank.Right)) return;
            }
            if (this.CheckSapper(_card))
            {
                bool hasLeftFort = game.GetFortificationCard(CurrentPlayer.FIRST, Flank.Left) != null;
                bool hasRightFort = game.GetFortificationCard(CurrentPlayer.FIRST, Flank.Right) != null;
                if (hasLeftFort && !hasRightFort && this.TryToPutCardAI(card, Flank.Left)) return;
                if (!hasLeftFort && hasRightFort && this.TryToPutCardAI(card, Flank.Right)) return;
            }
            if (flankLeft2.GetCardsCapacity() < flankRight2.GetCardsCapacity() && this.TryToPutCardAI(card, Flank.Left)) return;
            if (flankRight2.GetCardsCapacity() < flankLeft2.GetCardsCapacity() && this.TryToPutCardAI(card, Flank.Right)) return;
            int r = (int)Math.Round(UnityEngine.Random.value);
            if (r == 0 && this.TryToPutCardAI(card, Flank.Left)) return;
            if (r == 1 && this.TryToPutCardAI(card, Flank.Right)) return;
            if (this.TryToPutCardAI(card, Flank.Left)) return;
            if (this.TryToPutCardAI(card, Flank.Right)) return;
        }
        if (card.card is FortificationCard)
        {
            if (game.GetFortificationCard(CurrentPlayer.SECOND, Flank.Left) == null) {
                this.DropCardToFlank(card, -1, flankLeft2);
            }
            else
            {
                this.DropCardToFlank(card, -1, flankRight2);
            }
        }
        if (card.card is SupportCard)
        {
            this.DropCardToDrop(card, false);
            this.SupportInsurrection_Start();
        }
    }
    private void PlayCardsAI(int quantity)
    {
        for (var i = 0; i < quantity; i++)
        {
            this.PlayCardAI();
        }
    }
    public void Next()
    {
        playedSquadCards = 0;
        playedSupportCards = 0;
        playedFortificationCards = 0;

        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 2 : 0;

        flanks[step].GetComponent<ContainerFlank>().RefreshActive();
        flanks[step + 1].GetComponent<ContainerFlank>().RefreshActive();

        hands[game.GetCurrentStep()].gameObject.SetActive(false);
        GetCardsFromDeckToHand(game.GetCurrentStep(), points);
        points = 4;
        game.NextStep();
        if (game.GetCurrentStep() == CurrentPlayer.SECOND)
        {
            round++;
            if (round == 1)
            {
                if (isAIFirst)
                {
                    this.PlayCardsAI(2);
                }
                else
                {
                    this.PlayCardsAI(3);
                }
            }
            else
            {
                switch (round % 4)
                {
                    case 0:
                        {
                            this.PlayCardsAI(1);
                            break;
                        }
                    case 1:
                        {
                            this.PlayCardsAI(2);
                            break;
                        }
                    case 2:
                        {
                            this.PlayCardsAI(2);
                            break;
                        }
                    case 3:
                        {
                            this.PlayCardsAI(3);
                            break;
                        }
                    default:
                        {
                            this.PlayCardsAI(2);
                            break;
                        }
                }
            }
            this.SelectAttackAI();
            attackState = AttackState.ATTACK;
            this.Attack();
            return;
        }
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
            this.commandorsAI.Add(_cardCommandor);
        }
        chooseCommandors2.SetActive(false);
    }
    public void HideCommandorsChooseMenu()
    {
        Destroy(commandorsOV);
    }
    private void SetCommandorAI (CardCommandorView card)
    {
        game.SetCommandor(CurrentPlayer.SECOND, (CommandorCard)card.card);
        card.transform.SetParent(commandorFields[(int)commandorsChosen].transform, false);
        commandorsChosen++;
    }
    public void SetCommandor(Transform transform, CommandorCard commandor)
    {
        CurrentPlayer currentPlayer = game.GetCurrentStep();
        game.SetCommandor(CurrentPlayer.FIRST, commandor);
        transform.SetParent(commandorFields[(int)commandorsChosen].transform, false);
        commandorsChosen++;
        if (commandorsChosen == 1)
        {
            if (currentPlayer == CurrentPlayer.SECOND)
            {
                this.SetCommandorAI(commandorsAI[3]);
                flankLeft2.commandor = (CommandorCard)commandorsAI[3].card;
                isAIFirst = true;
            }
            flankLeft1.commandor = commandor;
            if (currentPlayer == CurrentPlayer.FIRST)
            {
                if (commandor.rarity == Rarity.Rare)
                {
                    this.SetCommandorAI(commandorsAI[1]);
                    flankLeft2.commandor = (CommandorCard)commandorsAI[1].card;
                }
                else
                {
                    this.SetCommandorAI(commandorsAI[0]);
                    flankLeft2.commandor = (CommandorCard)commandorsAI[0].card;
                }
                this.SetCommandorAI(commandorsAI[3]);
                flankRight2.commandor = (CommandorCard)commandorsAI[3].card;
            }
        }
        else
        {
            flankRight1.commandor = commandor;
            if (currentPlayer == CurrentPlayer.SECOND)
            {
                if (commandor.rarity == Rarity.Rare)
                {
                    this.SetCommandorAI(commandorsAI[1]);
                    flankRight2.commandor = (CommandorCard)commandorsAI[1].card;
                }
                else
                {
                    this.SetCommandorAI(commandorsAI[0]);
                    flankRight2.commandor = (CommandorCard)commandorsAI[0].card;
                }
            }
            GameStart();
            HideCommandorsChooseMenu();
        }
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
        hands[currentPlayer].AddCard(card);
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
        hands[currentPlayer].AddCard(card);
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
                            HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
                            HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
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
                    if (game.GetFortificationCard(game.GetCurrentStep(), flankModel) != null)
                    {
                        this.DropCardToDrop(flank.transform.Find("FortificationContainer").GetComponentInChildren<Card>(), false);
                    }
                    game.ApplyAttackerFortificationCard(game.GetCurrentStep(), f, flankModel);
                    card.transform.SetParent(flank.transform.Find("FortificationContainer").transform);
                    playedFortificationCards++;
                    points--;
                }
                break;
        }
        hands[game.GetCurrentStep()].RemoveCard(card);
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
        drops[player].AddCard(card);
        hands[game.GetCurrentStep()].RemoveCard(card);
    }

    // Attack

    public void Attack()
    {
        if (callback != AttackCallback) return;
        if (attackState == AttackState.ATTACK)
        {
            if (game.GetCurrentStep() == CurrentPlayer.FIRST)
            {
                this.SelectDefenceAI();
                attackState = AttackState.DEFENCE;
                Attack();
                return;
            }
            AttackButton.GetComponentInChildren<Text>().text = "В бой!";
            AttackStart();
            attackState = AttackState.DEFENCE;
        }
        else
        {
            AttackButton.GetComponentInChildren<Text>().text = "Атака!";
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
            attackState = AttackState.ATTACK;
            this.DefenceStart();
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
        if (game.GetCurrentStep() == CurrentPlayer.FIRST)
        {
            flankLeft1.RefreshActive();
            flankRight1.RefreshActive();
        }
        this.GetOppositeFlank()._SetActive(false);
        this.GetOppositeFlank().DestroyDead();
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        callback = AttackCallback;
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
        if (game.GetCurrentStep() == CurrentPlayer.FIRST)
        {
            flankLeft1.RefreshActive();
            flankRight1.RefreshActive();
        }
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
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        callback = AttackCallback;
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
    }
    public void SupportSapper_Start(int power)
    {
        this.DropCardToDrop(this.GetOppositeFlank().transform.Find("FortificationContainer").GetComponentInChildren<Card>(), !this.isAttackActiveSkills);
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
            HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
            HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
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
        flanks[step].DestroyDead();
        flanks[step + 1].DestroyDead();
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
            HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
            HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
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
                HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
                HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
            }
            for (int i = 0; i < 4; i++)
            {
                if (flank.GetCardAt(i) == null) position = i;
            }
            CardSquad card = Instantiate(cardSquad);
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
                HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
                HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
            }
            for (int i = 4; i < 8; i++)
            {
                if (flank.GetCardAt(i) == null) position = i;
            }
            CardSquad card = Instantiate(cardSquad);
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
        temporary.gameObject.SetActive(true);
        
        foreach (Card card in temporaryCards)
        {
            CardSquad _card = Instantiate(cardSquad);
            _card.transform.SetParent(temporary.transform.Find("Temporary").Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
        }
        medicineCount = 2;
        callback = SupportFieldMedicine_End;
    }

    private void SupportFieldMedicine_End(Card card)
    {
        medicineCount--;
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
        drops[game.GetCurrentStep()].RemoveCard(card);
        AddCardToHand(card);
        if (medicineCount == 0)
        {
            temporary.gameObject.SetActive(false);
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
        temporary.gameObject.SetActive(true);

        foreach (Card card in temporaryCards)
        {
            CardSquad _card = Instantiate(cardSquad);
            _card.transform.SetParent(temporary.transform.Find("Temporary").Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
            _card.isSelectable = true;
        }
        callback = SupportHeroesOfLegends_End;
    }

    private void SupportHeroesOfLegends_End(Card card)
    {
        temporary.gameObject.SetActive(false);
        decks[game.GetCurrentStep()].RemoveCard(card);
        AddCardToHand(card);
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
            CardSquad card = Instantiate(cardSquad);
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
            CardSquad card = Instantiate(cardSquad);
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
        temporary.gameObject.SetActive(true);

        foreach (Card card in temporaryCards)
        {
            if (card is CardSquad)
            {
                CardSquad __card = Instantiate(cardSquad);
                __card.transform.SetParent(temporary.transform.Find("Temporary").Find("CardsContainer").transform, false);
                __card.SetCard(card.card);
                continue;
            }
            Card _card = Instantiate(cardUniversal);
            _card.transform.SetParent(temporary.transform.Find("Temporary").Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
        }
        callback = SupportSmuggling_End;
    }

    private void SupportSmuggling_End(Card card)
    {
        int step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 1 : 2;
        temporary.gameObject.SetActive(false);

        decks[game.GetCurrentStep()].RemoveCard(card);
        AddCardToHand(card);
        foreach (var _card in temporary.GetComponent<AbstractContainer>().GetCards())
        {
            DropCardToDrop(_card, false);
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
            HQ1Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.FIRST)}";
            HQ2Layer.transform.Find("Text").GetComponent<Text>().text = $"{game.GetHeadsquaterHealth(CurrentPlayer.SECOND)}";
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
        flank.DestroyDead();
        step = game.GetCurrentStep() == CurrentPlayer.FIRST ? 0 : 2;
        flanks[step].RefreshActive();
        flanks[step + 1].RefreshActive();
        AttackButton.GetComponentInChildren<Text>().text = "Атака!";
        ResetSelectionCards();
        callback = AttackCallback;
    }

    public void ShowDrop(ContainerDrop drop)
    {
        temporary.gameObject.SetActive(true);
        buttonCloseTemp.gameObject.SetActive(true);
        List<Card> temporaryCards = new List<Card>();
        temporaryCards.AddRange(drop.GetCards());
        foreach (Card card in temporaryCards)
        {
            if (card is CardSquad)
            {
                CardSquad __card = Instantiate(cardSquad);
                __card.transform.SetParent(temporary.transform.Find("CardsContainer").transform, false);
                __card.SetCard(card.card);
                __card.isSelectable = false;
                continue;
            }
            Card _card = Instantiate(cardUniversal);
            _card.transform.SetParent(temporary.transform.Find("CardsContainer").transform, false);
            _card.SetCard(card.card);
            _card.isSelectable = false;
        }
    }

    private void HideDrop()
    {
        temporary.gameObject.SetActive(false);
        buttonCloseTemp.gameObject.SetActive(false);
    }

}
