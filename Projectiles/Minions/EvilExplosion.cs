using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Common.GlobalNPCs;
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
			Projectile.height = 180;
			Projectile.width = 180;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 8;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.localNPCHitCooldown = 10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.hide = true;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//if (Main.myPlayer == Projectile.owner && !target.boss && !DebuffNPC.miniBosses.Contains(target.type))
				//DebuffNPC.SetTimeFreeze(Main.player[Projectile.owner], target, 150);
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, -0.2f);
				for (int i = 0; i < 360; i += 4)
				{
					Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4.5f, 18f), 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.scale = dust.scale * 0.65f + 2.55f;
					dust.fadeIn = 0.1f;
					dust.color = ColorHelpers.EvilColor * 1.5f;
					dust.velocity *= 0.8f;
					dust.velocity += circularLocation * 1.0f;
					if (!Main.rand.NextBool(4))
					{
						circularLocation = new Vector2(Main.rand.NextFloat(3f, 12f), 0).RotatedBy(MathHelper.ToRadians(i));
						dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, Main.rand.NextBool(2) ? 62 : 60); //purple and red torch respectively
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
		