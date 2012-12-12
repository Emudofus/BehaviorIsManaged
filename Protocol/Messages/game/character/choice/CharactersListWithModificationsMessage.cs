

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharactersListWithModificationsMessage : CharactersListMessage
    {
        public const uint Id = 6120;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterToRecolorInformation[] charactersToRecolor;
        public int[] charactersToRename;
        public int[] unusableCharacters;
        public Types.CharacterToRelookInformation[] charactersToRelook;
        
        public CharactersListWithModificationsMessage()
        {
        }
        
        public CharactersListWithModificationsMessage(bool hasStartupActions, Types.CharacterBaseInformations[] characters, Types.CharacterToRecolorInformation[] charactersToRecolor, int[] charactersToRename, int[] unusableCharacters, Types.CharacterToRelookInformation[] charactersToRelook)
         : base(hasStartupActions, characters)
        {
            this.charactersToRecolor = charactersToRecolor;
            this.charactersToRename = charactersToRename;
            this.unusableCharacters = unusableCharacters;
            this.charactersToRelook = charactersToRelook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)charactersToRecolor.Length);
            foreach (var entry in charactersToRecolor)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)charactersToRename.Length);
            foreach (var entry in charactersToRename)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)unusableCharacters.Length);
            foreach (var entry in unusableCharacters)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)charactersToRelook.Length);
            foreach (var entry in charactersToRelook)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            charactersToRecolor = new Types.CharacterToRecolorInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersToRecolor[i] = new Types.CharacterToRecolorInformation();
                 charactersToRecolor[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            charactersToRename = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersToRename[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            unusableCharacters = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 unusableCharacters[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            charactersToRelook = new Types.CharacterToRelookInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersToRelook[i] = new Types.CharacterToRelookInformation();
                 charactersToRelook[i].Deserialize(reader);
            }
        }
        
    }
    
}