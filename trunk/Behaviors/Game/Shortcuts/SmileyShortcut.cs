using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class SmileyShortcut : GeneralShortcut
    {
        public SmileyShortcut(PlayedCharacter character, ShortcutSmiley smiley)
            : base(smiley.slot)
        {
            SmileyId = smiley.smileyId;
        }

        public int SmileyId { get; set; }
    }
}