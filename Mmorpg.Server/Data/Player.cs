using System;
using Mmorpg.Data;
using Mmorpg.Server.Control;
using Mmorpg.Shared.Enums;
using Swordfish.Library.Networking;

namespace Mmorpg.Server.Data
{
    public class Player : LivingEntity
    {
        private NetSession m_Session;
        public NetSession Session => m_Session ?? (m_Session = GameServer.Instance.GetSessions().First(x => x.ID == ID));

        protected override void OnHealthChanged(HealthChangeEventArgs e)
        {            
            Chat.SendAndBroadcast(
                Session,
                $"{e.Source.Name} {e.Cause.ToString().ToLower()} you for {Math.Abs(e.Amount)} {e.Type.ToString().ToLower()}.",
                $"{e.Source.Name} {e.Cause.ToString().ToLower()} {Name} for {Math.Abs(e.Amount)} {e.Type.ToString().ToLower()}.",
                ChatChannel.Combat
            );
        }

        protected override void OnDeath(HealthChangeEventArgs e)
        {
            if (e.Source != null)
            {
                Chat.SendAndBroadcast(
                    Session,
                    $"You were killed by {e.Source.Name}.",
                    $"{e.Source.Name} killed {Name}.",
                    ChatChannel.Local
                );
            }
            else
            {
                Chat.SendAndBroadcast(
                    Session,
                    $"You died.",
                    $"{Name} died.",
                    ChatChannel.Local
                );
            }
        }
    }
}
