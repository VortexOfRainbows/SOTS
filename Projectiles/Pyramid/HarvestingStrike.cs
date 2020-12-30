using Microsoft.Xna.Framework;
using SOTS.NPCs.ArtificialDebuffs;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class HarvestingStrike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harvesting Strike");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.timeLeft = 90;
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.aiStyle = 0;
			projectile.alpha = 255;
		}
        public override bool? CanHitNPC(NPC target)
		{
			return false;
        }
		bool hasHitEnemy = false;
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (Main.myPlayer == projectile.owner && projectile.timeLeft >= 89 && !hasHitEnemy)
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					int amt = DebuffNPC.HarvestCost(npc);
					if (voidPlayer.lootingSouls >= amt && !npc.immortal && !npc.friendly)
						if (npc.active && npc.Hitbox.Intersects(projectile.Hitbox) && (npc.realLife == npc.whoAmI || npc.realLife <= 0) && !npc.dontTakeDamage)
						{
							Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<HarvestLock>(), 0, 0, player.whoAmI, npc.whoAmI);
							DebuffNPC debuffNPC = (DebuffNPC)mod.GetGlobalNPC("DebuffNPC");
							debuffNPC = (DebuffNPC)debuffNPC.Instance(npc);
							if (debuffNPC.HarvestCurse >= 99)
							{
								hasHitEnemy = true;
							}
							hasHitEnemy = true;
						}
				}
			return true;
		}
    }
}