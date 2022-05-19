using System.Collections.Generic;

using Swordfish.Library.Types;

namespace Mmorpg.Data
{
    public class CharacterRaces : DynamicEnum<CharacterRaces>
    {
        protected override IEnumerable<DynamicEnumValue> Initialize()
        {
            return new DynamicEnumValue[3] {
                "Human",
                "Elf",
                "Dwarf",
            };
        }
    }
}
