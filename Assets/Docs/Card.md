## Card
```js
card: {
  active: bool,
  name: string,
  rarity: Rarity,
  tag: string,
};

squad: {
  attack: uint,
  isActive: bool,
  protection: uint,
  skills: Skills,
  stamina: int,
};

commandor: {
  period: uint,
  skills: Skills,
};

fortification: {
  skill: CallbackFortification,
};

support: {
  action: ViewAction,
};
```
