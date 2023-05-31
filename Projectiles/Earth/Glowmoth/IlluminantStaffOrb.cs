using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Otherworld;

namespace SOTS.Projectiles.Earth.Glowmoth
{    
    public class IlluminantStaffOrb : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.height = 22;
			Projectile.width = 22;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 7200;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 130, 210, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float scale = 4 - k * 0.75f;
				float alphaMult = 0.4f + k * 0.1f;
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * alphaMult, MathHelper.ToRadians((2 + k) * timer * (k % 2 * 2 - 1)) + Projectile.rotation, drawOrigin, scale * Projectile.scale * 0.75f * (0.6f + 0.4f * spin / 900f) * 0.9f, SpriteEffects.None, 0f);
			}
			texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<IlluminantStaffShard>()].Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int j = 1; j <= 5; j++)
			{
				float scaleMult = 1.5f - j * 0.1f;
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * 0.2f, Projectile.rotation, drawOrigin, new Vector2(1 + (1 - (spin / 900f)), 1.1f) * scaleMult * 0.9f, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, drawOrigin, Projectile.scale * new Vector2(1 + (1 - (spin / 900f)), 1.1f) * 0.9f, SpriteEffects.None, 0f);
		}
		public override bool PreDraw(ref Color lightColor)
        {
			return false;
		}
		bool foundTarget = false;
		int lastNpc = -1;
		int timer = 0;
		float spin = 900;
		public Vector2 FindTarget()
		{
			float distanceFromTarget = 480f;
			Vector2 targetCenter = Projectile.Center;
			Vector2 cursor = Main.MouseWorld;
			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && npc.active)
					{
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						if ((closest || !foundTarget) && inRange && lineOfSight)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center + npc.velocity * 2;
							foundTarget = true;
							lastNpc = i;
						}
					}
				}
			}
			else
			{
				NPC npc = Main.npc[lastNpc];
				if (npc.CanBeChasedBy() && npc.active)
				{
					targetCenter = npc.Center + npc.velocity * 2;
				}
				else
                {
					foundTarget = false;
                }
			}
			if(targetCenter != Projectile.Center)
				return targetCenter;
			return cursor;	
		}
		public override void AI()
		{
			timer++;
			Projectile.velocity *= 0.9625f;
			if(spin > 0)
            {
				spin -= 3;
				spin *= 0.97f;
            }
			if(spin < 0)
            {
				spin = 0;
            }
			if(Main.myPlayer == Projectile.owner)
			{
				Projectile.ai[0] = (FindTarget() - Projectile.Center).ToRotation();
				Projectile.netUpdate = true;
			}
			if (Projectile.alpha > 0)
				Projectile.alpha -= 15;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;
			Projectile.rotation = Projectile.ai[0] + MathHelper.ToRadians(spin);
			if(spin <= 0)
			{
				Projectile.velocity *= 0.825f;
				Projectile.ai[1]++;
				if (Projectile.ai[1] >= 30)
				{
					SOTSUtils.PlaySound(SoundID.Item25, Projectile.Center, 0.7f, -0.3f, 0.1f);
                    if (Projectile.ai[0] != 0)
					{
						if (Main.myPlayer == Projectile.owner)
						{
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(5.5f, 0).RotatedBy(Projectile.rotation), ModContent.ProjectileType<IlluminantStaffShard>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
						}
					}
					Projectile.Kill();
				}
			}
			else
			{
				Projectile.velocity.Y += 0.15f;
				for (int i = 0; i < 10; i++)
				{
					if(Main.rand.NextBool(50))
					{
						Vector2 circular = new Vector2(6, 0).RotatedBy(MathHelper.TwoPi * i / 10f);
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4) + circular * 2.2f, 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>());
						Color color2 = Projectile.GetAlpha(IlluminantStaffShard.getColor);
						dust.color = color2;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = dust.scale * 0.5f + 0.5f;
						dust.alpha = Projectile.alpha;
						dust.velocity *= 0.1f;
						dust.velocity += Projectile.velocity * 0.01f + circular * 0.1f;
					}
				}
			}
		}	
	}
}
		