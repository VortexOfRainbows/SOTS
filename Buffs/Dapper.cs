using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class Dapper : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Dapper");
			Description.SetDefault("Summons a Dapperaichu to assist you");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
		
Main.lightPet[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			
			player.buffTime[buffIndex] = 18000;
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.DapperChu = true;
			bool petProjectileNotSpawned = true;
			if (player.ownedProjectileCounts[mod.ProjectileType("DapperRaichu")] > 0)
			{
				petProjectileNotSpawned = false;
			}
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, mod.ProjectileType("DapperRaichu"), 0, 0f, player.whoAmI, 0f, 0f);
}
        }
    }
}