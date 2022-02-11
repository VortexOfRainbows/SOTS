using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.NPCs.ArtificialDebuffs;
using SOTS.Void;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class EvilExplosion : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evil");
		}
		public override void SetDefaults()
		{
			projectile.height = 180;
			projectile.width = 180;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 8;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.localNPCHitCooldown = 10;
			projectile.usesLocalNPCImmunity = true;
			projectile.hide = true;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//if (Main.myPlayer == projectile.owner && !target.boss && !DebuffNPC.miniBosses.Contains(target.type))
				//DebuffNPC.SetTimeFreeze(Main.player[projectile.owner], target, 150);
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.7f, -0.2f);
				for (int i = 0; i < 360; i += 4)
				{
					Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4.5f, 18f), 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.scale = dust.scale * 0.65f + 2.55f;
					dust.fadeIn = 0.1f;
					dust.color = VoidPlayer.EvilColor * 1.5f;
					dust.velocity *= 0.8f;
					dust.velocity += circularLocation * 1.0f;
					if (!Main.rand.NextBool(4))
					{
						circularLocation = new Vector2(Main.rand.NextFloat(3f, 12f), 0).RotatedBy(MathHelper.ToRadians(i));
						dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, Main.rand.NextBool(2) ? 62 : 60); //purple and red torch respectively
						dust.noGravity = true;
						dust.scale = dust.scale * 0.8f + 2.1f;
						dust.velocity *= 1.1f;
						dust.velocity += circularLocation * 1.1f;
					}
				}
				runOnce = false;
            }
		}
	}
}
		