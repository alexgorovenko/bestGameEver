using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Card : MonoBehaviour, ICardContainerItem
{
    static Dictionary<Rarity, Color> rarityColors = new Dictionary<Rarity, Color>();
    public AbstractCard card;
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public TextMeshProUGUI fieldName;
    [SerializeField]
    public GameObject border;
    [SerializeField]
    public GameObject image;
    [SerializeField]
    public Sprite Image
    {
        get { return image.GetComponent<Image>().sprite; }
        set { image.GetComponent<Image>().sprite = value; }
    }
    public bool isSelectable = false;
    public bool isDraggable = true;
    public bool isHighlighted = true;
    public int position = -1;
    public Card() : base()
    {
        rarityColors[Rarity.General] = new Color(0.8f, 0.8f, 0.8f);
        rarityColors[Rarity.Rare] = new Color(24.0f / 255.0f, 97.0f / 255.0f, 242.0f / 255.0f);
        rarityColors[Rarity.Epic] = new Color(97.0f / 255.0f, 24.0f / 255.0f, 242.0f / 255.0f);
        rarityColors[Rarity.Legendary] = new Color(242.0f / 255.0f, 170.0f / 255.0f, 24.0f / 255.0f);
    }
    public void OnAdd() { }
    public void OnPlay() { }
    public void OnRemove() { }
    public void Highlight(bool state)
    {
        ColorBlock colors = border.GetComponent<Button>().colors;
        if (state)
        {
            colors.normalColor = new Color(140.0f / 255.0f, 242.0f / 255.0f, 140.0f / 255.0f);
            colors.selectedColor = new Color(140.0f / 255.0f, 242.0f / 255.0f, 140.0f / 255.0f);
        }
        else
        {
            colors.normalColor = rarityColors[card.rarity];
        }
        border.GetComponent<Button>().colors = colors;
    }
    public void SetCard(AbstractCard card)
    {
        if (card.GetType() == typeof(SupportCard)) isDraggable = false;
        this.card = card;
        fieldName.SetText(card.name);
    }
    public void SetActiveSelectHandler(bool state)
    {

    }
    public void OnPointerClick()
    {
        PlayerController script = player.GetComponent<PlayerController>();
        if (isSelectable && !(card is SupportCard)) script.SelectCard(gameObject, position);
        Skills skills;
        switch (card)
        {
            case SquadCard s:
                break;
            case FortificationCard f:
                break;
            case SupportCard s:
                if (script.points == 0) return;
                script.points--;
                script.DropCardToDrop(this, false);
                switch (s.action)
                {
                    case ViewAction.ACIDRAIN:
                        {
                            script.SupportAcidRain_Start();
                            break;
                        }
                    case ViewAction.AMBUSH:
                        {
                            script.SupportAmbush_Start();
                            break;
                        }
                    case ViewAction.BATTLECRY:
                        {
                            script.SupportBattleCry_Start();
                            break;
                        }
                    case ViewAction.CLANCALL:
                        {
                            script.SupportClanCall_Start();
                            break;
                        }
                    case ViewAction.FIELDMEDICINE:
                        {
                            script.SupportFieldMedicine_Start();
                            break;
                        }
                    case ViewAction.HEROESOFLEGENDS:
                        {
                            script.SupportHeroesOfLegends_Start();
                            break;
                        }
                    case ViewAction.INSURRECTION:
                        {
                            script.SupportInsurrection_Start();
                            break;
                        }
                    case ViewAction.RAIDONTHEROCKS:
                        {
                            script.SupportRaidOnTheRocks_Start();
                            break;
                        }
                    case ViewAction.SMUGGLING:
                        {
                            script.SupportSmuggling_Start();
                            break;
                        }
                    case ViewAction.TREMBLINGEARTH:
                        {
                            script.SupportTremblingEarth_Start();
                            break;
                        }
                }
                break;
            case CommandorCard c:
                break;
            default:
                break;
        }
    }
}
