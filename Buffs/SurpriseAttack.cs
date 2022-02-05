using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Buffs
{
    public class SurpriseAttack : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Surprise Attack");
			Description.SetDefault("Melee damage increased by 50%\n'*Teleports behind you*'");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            player.meleeDamage += 0.5f;
        }
    }
}