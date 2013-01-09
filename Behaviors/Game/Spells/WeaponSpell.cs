using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Game.Spells
{
    public class WeaponSpell : Spell
    {
        public WeaponSpell(Weapon weapon)
        {
            _weapon = weapon;
            Template = new Protocol.Data.Spell();
            Template.descriptionId = weapon.descriptionId;
            Template.nameId = weapon.nameId;
            Position = 0;
            LevelTemplate = new SpellLevel();
            LevelTemplate.effects = weapon.possibleEffects.OfType<EffectInstanceDice>().ToList();
            foreach (var effect in LevelTemplate.effects)
                effect.rawZone = weapon.type.rawZone;
            LevelTemplate.criticalEffect = LevelTemplate.effects;
            Level = 1;
            LevelTemplate.minRange = (uint)weapon.minRange;
            LevelTemplate.range = (uint)weapon.range;
            LevelTemplate.apCost = (uint)weapon.apCost;
            LevelTemplate.castInDiagonal = weapon.castInDiagonal;
            LevelTemplate.castInLine = weapon.castInLine;
            LevelTemplate.castTestLos = weapon.castTestLos;
            LevelTemplate.criticalHitProbability = (uint)weapon.criticalHitProbability;
            LevelTemplate.criticalFailureProbability = (uint)weapon.criticalFailureProbability;             
        }

        protected override SpellLevel GetLevelTemplate(int level)
        {
            LevelTemplate = new SpellLevel();
            LevelTemplate.effects = _weapon.possibleEffects.OfType<EffectInstanceDice>().ToList();
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

        protected Weapon _weapon {get; set;}        
        
    }
}
