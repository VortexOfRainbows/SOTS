using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Prim.Trails;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.Tide
{    
    public class Rainbolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rain Bolt");
		}
        public override void SetDefaults()
        {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.netImportant = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 48;
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
		}
		public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(140, 90, 160, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 circular = new Vector2(0, 2).RotatedBy(k * 1.57f + Projectile.rotation + MathHelper.ToRadians(counter * 5f));
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1) + new Vector2(0, Projectile.gfxOffY) + circular,
				null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation + circular.ToRotation(), drawOrigin, 1.1f, SpriteEffects.None, 0f);
			}
			return false;
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		int counter;
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				SOTS.primitives.CreateTrail(new WaterTrail(Projectile));
				runOnce = false;
			}
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.dead)
			{
				Projectile.Kill();
			}
			if ((modPlayer.aqueductDamage + 1) != Projectile.damage)
			{
				Projectile.Kill();
			}
			if (Projectile.timeLeft > 100)
			{
				Projectile.timeLeft = 300;
			}
			Vector2 toPlayer = player.Center - Projectile.Center;
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
				{
					if (proj == Projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			Vector2 initialLoop = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 4));
			initialLoop.X *= 0.5f;
			Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
			Projectile.Center = player.Center + properLoop;
			Projectile.velocity = new Vector2(-1, 0).RotatedBy(toPlayer.ToRotation());
			for (int i = -1; i <= 1; i += 2)
			{
				if(Main.rand.NextBool(3))
				{
					Vector2 circular = new Vector2((float)Math.Sin(MathHelper.ToRadians(counter * 2.2f)) * 8 * i, 0).RotatedBy(Projectile.velocity.ToRotation());
					Dust dust = Dust.NewDustPerfect(Projectile.Center + circular, 172);
					dust.scale = dust.scale * 0.3f + 1.0f;
					dust.noGravity = true;
					dust.velocity = dust.velocity * 0.5f + Projectile.velocity * i;
				}
			}
			counter++;
		}
	}
}
		