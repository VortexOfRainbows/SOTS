using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class NatureBeam : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nature Beam");
		}
		public override void SetDefaults()
        {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 32;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 0;
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
		bool hasHit = false;
		Vector2 ogPos = new Vector2(0, 0);
		bool runOnce = true;
		List<Vector2> drawPoints = new List<Vector2>();
		public override bool PreAI()
		{
			Player player  = Main.player[Projectile.owner];
			if (runOnce)
			{
				counter = Main.rand.Next(60);
				ogPos = Projectile.Center;
				for (int i = 0; i < 15; i++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
					dust2.velocity += Projectile.velocity * 0.5f;
					dust2.noGravity = true;
					dust2.color = ColorHelpers.NatureColor;
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 1.4f;
				}
				Vector2 center = Projectile.Center;
				hasHit = true;
				float scale = 0.1f;
				for (int j = 0; j < 300; j++)
				{
					counter++;
					if (scale < 1)
						scale += 0.05f;
					for (int i = -1; i < 2; i += 2)
					{
						Vector2 rotational = new Vector2((8 + i * 4) * i * scale, 0).RotatedBy(MathHelper.ToRadians(counter * 6));
						drawPoints.Add(center + new Vector2(0, rotational.X).RotatedBy(Projectile.velocity.ToRotation()));
						for(int k = 0; k < Main.npc.Length; k++)
						{
							NPC target = Main.npc[k];
							if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
							{
								float width = Projectile.width * Projectile.scale;
								float height = Projectile.height * Projectile.scale;
								Rectangle projHitbox = new Rectangle((int)center.X - (int)width / 2, (int)center.Y - (int)height / 2, (int)width, (int)height);
								if (target.Hitbox.Intersects(projHitbox))
								{
									if(j < 298)
										j = 298;
									hasHit = false;
								}
							}
						}
					}
					center += Projectile.velocity.SafeNormalize(new Vector2(1, 0)) * 14 * scale * scaleMod;
					Projectile.Center = center;
				}
				runOnce = false;
			}
			for(int j = drawPoints.Count - 3; j < drawPoints.Count; j++)
            {
				if(Main.rand.NextBool(4))
				{
					Vector2 center = drawPoints[j];
					Dust dust = Dust.NewDustDirect(center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
					dust.velocity += Projectile.velocity * 0.3f;
					dust.noGravity = true;
					dust.color = ColorHelpers.NatureColor;
					dust.noGravity = true;
					dust.fadeIn = 0.2f;
					dust.alpha = Projectile.alpha;
				}
			}
			Projectile.alpha += 8;
			return true;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return hasHit ? false : (bool?)null;
        }
        public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				dust.velocity += Projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.color = ColorHelpers.NatureColor;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Player player = Main.player[Projectile.owner];
			float scale = 0.1f;
			int alpha = 0;
			float rotation = Projectile.velocity.ToRotation();
			Color color = new Color(ColorHelpers.NatureColor.R, ColorHelpers.NatureColor.G, ColorHelpers.NatureColor.B, 0);
			for (int i = 0; i < drawPoints.Count; i += 2)
			{
				if(scale < 1)
					scale += 0.05f;
				Vector2 drawPos = drawPoints[i];
				if(i < drawPoints.Count - 2)
				{
					Vector2 drawPos2 = drawPoints[i + 2];
					rotation = (drawPos - drawPos2).ToRotation();
				}
				Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * ((255 - Projectile.alpha) / 255f) * ((255 - alpha) / 255f), rotation, origin, scale * scaleMod, SpriteEffects.None, 0f);
			}
			rotation = Projectile.velocity.ToRotation();
			scale = 0.1f;
			for (int i = 1; i < drawPoints.Count; i += 2)
			{
				if (scale < 1)
					scale += 0.05f;
				Vector2 drawPos = drawPoints[i];
				if (i < drawPoints.Count - 2)
				{
					Vector2 drawPos2 = drawPoints[i + 2];
					rotation = (drawPos - drawPos2).ToRotation();
				}
				Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * ((255 - Projectile.alpha) / 255f) * ((255 - alpha) / 255f), rotation, origin, scale * scaleMod, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.immune[player.whoAmI] = 0;
			int heal = 1;
			hasHit = true;
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), ogPos.X, ogPos.Y, 0, 0, ModContent.ProjectileType<Base.HealProj>(), 0, 0, player.whoAmI, heal, 9);
			}
        }
    }
}
		