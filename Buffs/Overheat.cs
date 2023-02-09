using Terraria;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Buffs
{
    public class Overheat : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overheat");
			Description.SetDefault("Movement speed and melee speed increased by 10%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
        }
		public override void Update(Player player, ref int buffIndex)
		{
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
            player.moveSpeed += 0.1f;
        }
    }
}