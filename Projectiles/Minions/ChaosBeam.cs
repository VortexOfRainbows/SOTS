using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class ChaosBeam : ModProjectile 
    {
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chaos Beam");
		}
		public override void SetDefaults()
        {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 25;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.immune[player.whoAmI] = 0;
		}
		public override bool ShouldUpdatePosition()
        {
            return false; 
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            return true;
        }
		float scaleMod = 0.4f;
		int counter = 0;
		List<Vector2> drawPoints = new List<Vector2>();
		Vector2 ogPos = Vector2.Zero;
		bool runOnce = true;
		bool stop = false;
		public override bool PreAI()
		{
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item91, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f);
				runOnce = false;
				ogPos = Projectile.Center;
				for (int i = 0; i < 2; i++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<CopyDust4>(), 0, 0, 100, default, 1.6f);
					dust2.velocity += Projectile.velocity * 0.5f;
					dust2.noGravity = true;
					dust2.color = ColorHelpers.pastelRainbow;
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 1.4f;
				}
			}
			Player player = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation();
			counter = SOTSPlayer.ModPlayer(player).orbitalCounter;
			NPC target = Main.npc[(int)Projectile.ai[0]];
			if (!target.friendly && target.dontTakeDamage == false && target.active && target.CanBeChasedBy() && stop == false)
			{
				drawPoints = new List<Vector2>();
				Vector2 center = Projectile.Center;
				float scale = 0.1f;
				float length = 20;
				for (int j = 0; j < 2400; j++)
				{
					counter++;
					if (scale < 1)
						scale += 0.05f;
					for (int i = -1; i < 2; i += 2)
					{
						Vector2 rotational = new Vector2((8 + i * 4) * i * scale, 0).RotatedBy(MathHelper.ToRadians(counter * 6));
						drawPoints.Add(center + new Vector2(0, rotational.X).RotatedBy(Projectile.velocity.ToRotation()));
						float width = Projectile.width * Projectile.scale;
						float height = Projectile.height * Projectile.scale;
						Rectangle projHitbox = new Rectangle((int)center.X - (int)width / 2, (int)center.Y - (int)height / 2, (int)width, (int)height);
						if (target.Hitbox.Intersects(projHitbox))
						{
							if (j < 2398)
								j = 2398;
						}
					}
					float turnModifier = j * 0.03f;
					if(turnModifier > 6f)
                    {
						turnModifier = 6f;
                    }
					if(j > 40)
					{
						Vector2 goTo = new Vector2(target.Center.X, target.Center.Y);
						Vector2 turn = new Vector2(7f - turnModifier, 0).RotatedBy(Projectile.rotation);
						Vector2 turnTo = goTo - center;
						turnTo = turnTo.SafeNormalize(Vector2.Zero);
						turn += turnTo;
						Projectile.rotation = turn.ToRotation();
					}
					Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0).RotatedBy(Projectile.rotation);
					center += Projectile.velocity.SafeNormalize(new Vector2(1, 0)) * length * scale * scaleMod;
				}
				for (int j = drawPoints.Count - 3; j < drawPoints.Count; j++)
				{
					center = drawPoints[j];
					Dust dust = Dust.NewDustDirect(center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 100, default, 1.6f);
					dust.velocity += Projectile.velocity * 0.3f;
					dust.noGravity = true;
					dust.color = ColorHelpers.pastelRainbow;
					dust.noGravity = true;
					dust.fadeIn = 0.2f;
					dust.alpha = Projectile.alpha;
				}
				if (Projectile.owner == Main.myPlayer)
					Projectile.netUpdate = true;
			}
			stop = true;
			Projectile.Center = ogPos;
			Projectile.alpha += 10;
			return true;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
			return target.whoAmI == (int)Projectile.ai[0] && Projectile.timeLeft > 20;
        }
        public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Player player = Main.player[Projectile.owner];
			float scale = 0.1f;
			float rotation = Projectile.velocity.ToRotation();
			for (int i = 0; i < drawPoints.Count - 2; i += 2)
			{
				if (scale < 1)
					scale += 0.05f;
				Vector2 drawPos = drawPoints[i];
				Vector2 drawPos2 = drawPoints[i + 2];
				rotation = (drawPos - drawPos2).ToRotation();
				for(int k = 0; k < 6; k++)
                {
					Color color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(k * 60));
					Vector2 modi = new Vector2(3f * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSPlayer.ModPlayer(player).orbitalCounter + i));
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + modi, null, new Color(color.R, color.G, color.B, 0) * 0.2f * ((255 - Projectile.alpha) / 255f), rotation, origin, scale * scaleMod, SpriteEffects.None, 0f);
				}
			}
			rotation = Projectile.velocity.ToRotation();
			scale = 0.1f;
			for (int i = 1; i < drawPoints.Count - 2; i += 2)
			{
				if (scale < 1)
					scale += 0.05f;
				Vector2 drawPos = drawPoints[i];
				Vector2 drawPos2 = drawPoints[i + 2];
				rotation = (drawPos - drawPos2).ToRotation();
				for (int k = 0; k < 6; k++)
				{
					Color color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(k * 60));
					Vector2 modi = new Vector2(3f * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSPlayer.ModPlayer(player).orbitalCounter + i));
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + modi, null, new Color(color.R, color.G, color.B, 0) * 0.2f * ((255 - Projectile.alpha) / 255f), rotation, origin, scale * scaleMod, SpriteEffects.None, 0f);
				}
			}
		}
        public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
    }
}
		