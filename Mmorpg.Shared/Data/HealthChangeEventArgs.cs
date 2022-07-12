using System.ComponentModel;
using Mmorpg.Shared.Enums;

namespace Mmorpg.Data
{
    public class HealthChangeEventArgs : CancelEventArgs
    {
        public int Amount;
        public EffectType Type;
        public HealthChangeCause Cause;
        public LivingEntity Target;
        public LivingEntity Source;
    }
}