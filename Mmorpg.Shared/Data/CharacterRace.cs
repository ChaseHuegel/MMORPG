using System;
using Swordfish.Library.Types;

namespace Mmorpg.Shared.Data
{
    public struct CharacterRace
    {
        public int ID;

        public string Name;

        public string Brief;

        public string Description;

        public Bitmask ClassFlags;

        public override string ToString() => Name;
    }
}
