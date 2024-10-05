using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
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
			Projectile.localNPCHitCooldown = 0;
			Projectile.hide = true;
		}
		Vector2 finalPosition = Vector2.Zero;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		public override bool PreAI()
		{
			InitializeLaser();
			Player player = Main.player[Projectile.owner];
			if (player.channel || Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 2;
			else
            {
				Projectile.Kill();
            }
			int GunID = (int)Projectile.ai[0];
			for(int i = 0; i < 1000; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.identity == GunID && proj.active && proj.type == ModContent.ProjectileType<PixelBlaster>())
                {
					Projectile.velocity = proj.velocity;
					Projectile.Center = proj.Center + new Vector2(22, -2 * proj.direction).RotatedBy(proj.velocity.ToRotation());
					break;
				}
            }
			if (Projectile.oldVelocity != Projectile.velocity || Projectile.oldPosition != Projectile.position)
				Projectile.netUpdate = true;
            return true;
        }
        public override void AI() 
		{
			Projectile.hide = false;
		}
		bool hasInit = false;
		public void InitializeLaser()
		{
			Color color = ColorHelper.EarthColor;
			color.A = 0;
			Vector2 startingPosition = Projectile.Center;
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			for(int b = 0; b < 640; b ++)
			{
				startingPosition += Projectile.velocity * 2.5f;
				finalPosition = startingPosition;
				int i = (int)startingPosition.X / 16;
				int j = (int)startingPosition.Y / 16;
				if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
					break;
                }
				Rectangle projHit = new Rectangle((int)startingPosition.X - 4, (int)startingPosition.Y - 4, 8, 8);
				bool hasCollided = false;
				for(int ID = 0; ID < Main.npc.Length; ID++)
				{
					NPC npc = Main.npc[ID];
					if(npc.active && !npc.friendly && !npc.dontTakeDamage)
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
				bool extra = !hasInit;
				int chance = SOTS.Config.lowFidelityMode ? 36 : 12;
				if(Main.rand.NextBool(chance) || extra)
				{
					Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color, 0.75f);
					dust.noGravity = true;
					if (!extra)
					{
						dust.velocity = dust.velocity * 0.25f;
						dust.fadeIn = 17;
					}
					else
                    {
						dust.velocity *= 1.25f;
						dust.velocity += Projectile.velocity * Main.rand.NextFloat(5f, 8f);
						dust.fadeIn = 12;
						dust.scale *= 1.25f;
                    }
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
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Rectangle projHit = new Rectangle((int)finalPosition.X - 6, (int)finalPosition.Y - 6, 12, 12);
			if (targetHitbox.Intersects(projHit))
			{
				return true;
			}
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (!hasInit)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 origin = new Vector2(0, 1);
			float rotation = Projectile.velocity.ToRotation();
			Vector2 start = Projectile.Center;
			Vector2 final = finalPosition;
			float xScale = Vector2.Distance(start, final) / texture.Width;
			Color color = ColorHelper.EarthColor;
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