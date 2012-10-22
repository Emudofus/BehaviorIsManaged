using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class SmileyShortcut : GeneralShortcut
    {
        public SmileyShortcut(PlayedCharacter character, ShortcutSmiley smiley)
            : base(character, smiley.slot)
        {
            SmileyId = smiley.smileyId;
        }

        public int SmileyId
        {
            get;
            private set;
        }

        public override void Update(Protocol.Types.Shortcut shortcut)
        {
            base.Update(shortcut);

            if (shortcut is ShortcutSmiley)
            {
                SmileyId = ( shortcut as ShortcutSmiley ).smileyId;
            }
        }
    }
}