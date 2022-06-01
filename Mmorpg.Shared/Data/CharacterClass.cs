using System;
using Swordfish.Library.Types;

namespace Mmorpg.Shared.Data
{
    public struct CharacterClass
    {
        public int ID;

        public string Name;

        public string Brief;

        public string Description;

        public Bitmask64 AbilityFlags;
        
        public Bitmask ResourceFlags;
        
        public Bitmask64 ArmorProficiencyFlags;
        
        public Bitmask64 WeaponProficiencyFlags;

        public Bitmask64 OtherProficiencyFlags;

        public override string ToString() => Name;
    }
}
