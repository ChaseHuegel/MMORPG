using System.Collections.Generic;

using Swordfish.Library.Types;

namespace Mmorpg.Data
{
    public class CharacterClasses : DynamicEnum<CharacterClasses>
    {
        protected override IEnumerable<DynamicEnumValue> Initialize()
        {
            return new DynamicEnumValue[4] {
                "Fighter",
                "Rogue",
                "Mage",
                "Cleric",
            };
        }
    }
}
