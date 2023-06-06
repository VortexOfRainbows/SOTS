using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{
	public class PixelLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}

		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 2;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidGeneric>();
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		Vector2 finalPosition = Vector2.Zero;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override bool PreAI()
        {
			if(!hasInit)
            {
				InitializeLaser();
            }
            return true;
        }
        public override void AI() 
		{

		}
		bool hasInit = false;
		public void InitializeLaser()
		{
			Color color = ColorHelpers.EarthColor;
			color.A = 0;
			Vector2 startingPosition = Projectile.Center;
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			for(int b = 0; b < 800; b ++)
			{
				startingPosition += Projectile.velocity * 2;
				finalPosition = startingPosition;
				int i = (int)startingPosition.X / 16;
				int j = (int)startingPosition.Y / 16;
				if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
					break;
                }
				Rectangle projHit = new Rectangle((int)startingPosition.X - 2, (int)startingPosition.Y - 2, 4, 4);
				bool hasCollided = false;
				for(int ID = 0; ID < Main.npc.Length; ID++)
				{
					NPC npc = Main.npc[ID];
					if(npc.active && !npc.friendly)
					{
						if (npc.Hitbox.Intersects(projHit))
						{
							hasCollided = true;
							break;
						}
					}
				}
				if(hasCollided)
                {
					break;
                }
				int chance = SOTS.Config.lowFidelityMode ? 40 : 16;
				if(Main.rand.NextBool(chance))
				{
					Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 0.75f);
					dust.noGravity = true;
					dust.velocity = dust.velocity * 0.2f;
					dust.fadeIn = 18;
				}
			}
			for (int i = 3; i > 0; i--)
			{
				Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 1f);
				dust.noGravity = true;
				dust.velocity = dust.velocity * 0.2f + Projectile.velocity * Main.rand.NextFloat(0.1f, 1.0f);
				if (i == 2)
					dust.velocity += new Vector2(0, -Main.rand.NextFloat(0.1f, 0.3f));
				dust.fadeIn = 7;
			}
			hasInit = true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			hitDirection = 8921 * Math.Sign(hitDirection);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Rectangle projHit = new Rectangle((int)finalPosition.X - 4, (int)finalPosition.Y - 4, 8, 8);
			if (targetHitbox.Intersects(projHit))
			{
				return true;
			}
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 origin = new Vector2(0, 1);
			float rotation = Projectile.velocity.ToRotation();
			Vector2 start = Projectile.Center;
			Vector2 final = finalPosition;
			float xScale = Vector2.Distance(start, final) / texture.Width;
			Color color = ColorHelpers.EarthColor;
			color.A = 0;
			for(int i = 0; i <= 8; i++)
			{
				float colorScale = 0.1f;
				Vector2 sinusoid = new Vector2(0, 2 + i % 2 * 2).RotatedBy(i * MathHelper.PiOver4 + SOTSWorld.GlobalCounter * MathHelper.TwoPi / 45f);
				if (i == 0)
                {
					sinusoid *= 0f;
					colorScale = 1f;
				}
				Main.spriteBatch.Draw(texture, sinusoid + start - Main.screenPosition, null, color * colorScale, rotation, origin, new Vector2(xScale, 1f), SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}