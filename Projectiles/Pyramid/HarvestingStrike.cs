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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 90;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 255;
		}
        public override bool? CanHitNPC(NPC target)
		{
			return false;
        }
		bool hasHitEnemy = false;
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (Main.myPlayer == Projectile.owner && Projectile.timeLeft >= 89 && !hasHitEnemy)
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					int amt = DebuffNPC.HarvestCost(npc);
					if (voidPlayer.lootingSouls >= amt && !npc.immortal && !npc.friendly)
						if (npc.active && npc.Hitbox.Intersects(Projectile.Hitbox) && (npc.realLife == npc.whoAmI || npc.realLife <= 0) && !npc.dontTakeDamage)
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