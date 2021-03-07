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
			DisplayName.SetDefault("Nature Beam");
		}
		public override void SetDefaults()
        {
			projectile.width = 16;
			projectile.height = 16;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 32;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.netImportant = true;
			projectile.alpha = 0;
			projectile.extraUpdates = 0;
		}
        public override bool ShouldUpdatePosition()
        {
            return false; 
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 8;
			height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
		float scaleMod = 0.4f;
		int counter = 0;
		bool hasHit = false;
		Vector2 ogPos = new Vector2(0, 0);
		bool runOnce = true;
		List<Vector2> drawPoints = new List<Vector2>();
		public override bool PreAI()
		{
			Player player  = Main.player[projectile.owner];
			if (runOnce)
			{
				counter = Main.rand.Next(60);
				ogPos = projectile.Center;
				for (int i = 0; i < 15; i++)
				{
					Dust dust2 = Dust.NewDustDirect(projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
					dust2.velocity += projectile.velocity * 0.5f;
					dust2.noGravity = true;
					dust2.color = VoidPlayer.natureColor;
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 1.4f;
				}
				Vector2 center = projectile.Center;
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
						drawPoints.Add(center + new Vector2(0, rotational.X).RotatedBy(projectile.velocity.ToRotation()));
						for(int k = 0; k < Main.npc.Length; k++)
						{
							NPC target = Main.npc[k];
							if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
							{
								float width = projectile.width * projectile.scale;
								float height = projectile.height * projectile.scale;
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
					center += projectile.velocity.SafeNormalize(new Vector2(1, 0)) * 14 * scale * scaleMod;
					projectile.Center = center;
				}
				runOnce = false;
			}
			for(int j = drawPoints.Count - 3; j < drawPoints.Count; j++)
            {
				if(Main.rand.NextBool(4))
				{
					Vector2 center = drawPoints[j];
					Dust dust = Dust.NewDustDirect(center - new Vector2(5), 0, 0, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
					dust.velocity += projectile.velocity * 0.3f;
					dust.noGravity = true;
					dust.color = VoidPlayer.natureColor;
					dust.noGravity = true;
					dust.fadeIn = 0.2f;
					dust.alpha = projectile.alpha;
				}
			}
			projectile.alpha += 8;
			return true;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return hasHit ? false : (bool?)null;
        }
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
				dust.velocity += projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.color = VoidPlayer.natureColor;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Player player = Main.player[projectile.owner];
			float scale = 0.1f;
			int alpha = 0;
			float rotation = projectile.velocity.ToRotation();
			Color color = new Color(VoidPlayer.natureColor.R, VoidPlayer.natureColor.G, VoidPlayer.natureColor.B, 0);
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
				spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * ((255 - projectile.alpha) / 255f) * ((255 - alpha) / 255f), rotation, origin, scale * scaleMod, SpriteEffects.None, 0f);
			}
			rotation = projectile.velocity.ToRotation();
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
				spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * ((255 - projectile.alpha) / 255f) * ((255 - alpha) / 255f), rotation, origin, scale * scaleMod, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			target.immune[player.whoAmI] = 0;
			int heal = 1;
			if (Main.rand.NextBool(5)) heal = 2;
			hasHit = true;
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(ogPos.X, ogPos.Y, 0, 0, mod.ProjectileType("HealProj"), 0, 0, player.whoAmI, heal, 9);
			}
			base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
		