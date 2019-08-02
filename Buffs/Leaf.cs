using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Leaf : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Leaf");
			Description.SetDefault("Summons a TurtleTem to assist you");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
		
Main.lightPet[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			
			player.buffTime[buffIndex] = 18000;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.TurtleTem = true;
			bool petProjectileNotSpawned = true;
			if (player.ownedProjectileCounts[mod.ProjectileType("SnowTurtleTem")] > 0)
			{
				petProjectileNotSpawned = false;
			}
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, mod.ProjectileType("SnowTurtleTem"), 0, 0f, player.whoAmI, 0f, 0f);
}
        }
    }
}