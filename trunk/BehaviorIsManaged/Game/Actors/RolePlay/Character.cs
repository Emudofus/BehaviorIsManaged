using System;
using BiM.Game.Actors.Interfaces;
using BiM.Game.Alignement;
using BiM.Game.World;
using BiM.Protocol.Types;

namespace BiM.Game.Actors.RolePlay
{
    public class Character : Humanoid, IAlignedActor
    {
        public Character()
        {
            
        }

        public Character(GameRolePlayCharacterInformations characterInformations, Map map)
            : base(characterInformations.humanoidInfo)
        {
            if (characterInformations == null) throw new ArgumentNullException("characterInformations");
            if (map == null) throw new ArgumentNullException("map");

            // do not care about this warnings, this ctor is never called by his inheriter
            Id = characterInformations.contextualId;
            Look = characterInformations.look;
            Position = new ObjectPosition(map, characterInformations.disposition);
            Name = characterInformations.name;
            Alignement = new AlignmentInformations(characterInformations.alignmentInfos);
        }

        public AlignmentInformations Alignement
        {
            get;
            protected set;
        }
    }
}