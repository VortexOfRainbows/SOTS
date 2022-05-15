using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class PurpleBalloon : ModBuff
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Purple Balloon");
			Description.SetDefault("Fishing friend");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			
			player.buffTime[buffIndex] = 18000;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.PurpleBalloon = true;
			bool petProjectileNotSpawned = true;
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("LuckyPurpleBalloon").Type] > 0)
			{
				petProjectileNotSpawned = false;
			}
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, Mod.Find<ModProjectile>("LuckyPurpleBalloon").Type, 0, 0f, player.whoAmI, 0f, 0f);
			}
        }
    }
}