using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class IntimidatingPresence : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Intimidating Presence");
			Description.SetDefault("Drastically lowers enemy spawns");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
		}
    }
}