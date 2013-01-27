using System.Linq;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Game.Spells
{
  public class WeaponSpell : Spell
  {
    public WeaponSpell(Weapon weapon)
    {
      _weapon = weapon;
      Template = new Protocol.Data.Spell();
      Template.id = 0;
      Template.descriptionId = weapon.descriptionId;
      Template.nameId = weapon.nameId;
      Position = 0;
      LevelTemplate = GetLevelTemplate(1);
    }

    protected override SpellLevel GetLevelTemplate(int level)
    {
      LevelTemplate = new SpellLevel();
      // We only take effects with a duration or damage or healing effect into considération. Others are probably constant effects on the caster when holding the weapon. 
      LevelTemplate.effects = _weapon.possibleEffects.OfType<EffectInstanceDice>().Where(effect => (effect.duration != 0) || ((GetEffectCategories(effect.effectId, 0) & (SpellCategory.Damages | SpellCategory.Healing)) > 0)).ToList();
      foreach (var effect in LevelTemplate.effects)
        effect.rawZone = _weapon.type.rawZone;
      LevelTemplate.criticalEffect = LevelTemplate.effects;
      Level = 1;
      LevelTemplate.minRange = (uint)_weapon.minRange;
      LevelTemplate.range = (uint)_weapon.range;
      LevelTemplate.apCost = (uint)_weapon.apCost;
      LevelTemplate.castInDiagonal = _weapon.castInDiagonal;
      LevelTemplate.castInLine = _weapon.castInLine;
      LevelTemplate.castTestLos = _weapon.castTestLos;
      LevelTemplate.criticalHitProbability = (uint)_weapon.criticalHitProbability;
      LevelTemplate.criticalFailureProbability = (uint)_weapon.criticalFailureProbability;
      return LevelTemplate;
    }

    protected Weapon _weapon { get; set; }

  }
}
