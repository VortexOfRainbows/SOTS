using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class CosmicRadiation : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Cosmic Radiation");
			Description.SetDefault("Eradicates all defenses and then some");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
            player.statDefense = -92;
			Projectile.NewProjectile(player.Center.X, player.position.Y - 20, 0, 0, mod.ProjectileType("CosmicPlague"), 0, 0, 0);
			player.endurance -= 400;
			
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
            Projectile.NewProjectile(npc.Center.X, npc.position.Y - 20, 0, 0, mod.ProjectileType("CosmicPlague"), 0, 0, 0);
			npc.defense = -92;
            npc.immune[npc.target] = 0;
}
    }
}