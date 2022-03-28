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
using SOTS.NPCs.Boss;
using SOTS.Dusts;

namespace SOTS.Projectiles.Celestial
{    
    public class EnergySerpentHead2 : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Serpent");
		}
		List<Vector2> segments = new List<Vector2>();
		List<float> segmentsRotation = new List<float>();
		bool runOnce = true;
		public override void SetDefaults()
        {
			projectile.width = 40;
			projectile.height = 40;
			projectile.friendly = false;
			projectile.timeLeft = 1200;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 0;
			projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (runOnce)
				return false;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 here = segments[i];
				int width = 36;
				Rectangle hitbox = new Rectangle((int)here.X - width/2, (int)here.Y - width / 2, width, width);
				if(hitbox.Intersects(targetHitbox))
                {
					return true;
                }
			}
			return null;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			DrawWorm(spriteBatch, true);
			DrawWorm(spriteBatch, false);
			return false;
		}
		public void DrawWorm(SpriteBatch spriteBatch, bool outer)
		{
			Color color = new Color(255, 100, 100, 0);
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/EnergySerpentHead2");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 first = projectile.Center;
			if (outer)
			{
				for (int a = 0; a < 360; a += 60)
				{
					Vector2 circular = new Vector2(Main.rand.NextFloat(3.0f, 4), 0).RotatedBy(MathHelper.ToRadians(a));
					color = new Color(255, 100, 100, 0) * 0.2f;
					spriteBatch.Draw(texture, first + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, 1.05f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}
			}
			else
			{
				color = new Color(255, 200, 200);
				spriteBatch.Draw(texture, first - Main.screenPosition, null, color, projectile.rotation, origin, 1.05f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
			}
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 toOther = first - segments[i];
				if (segmentsRotation[i] != 0)
				{
					var num1090 = MathHelper.WrapAngle(segmentsRotation[i]);
					var spinningpoint60 = toOther;
					var radians64 = (double)(num1090 * 0.1f);
					Vector2 vector = default(Vector2);
					toOther = spinningpoint60.RotatedBy(radians64, vector);
				}
				if (i != segments.Count - 1)
					texture = mod.GetTexture("Projectiles/Celestial/EnergySerpentBody2");
				else
					texture = mod.GetTexture("Projectiles/Celestial/EnergySerpentTail2");
				origin = new Vector2(texture.Width / 2, texture.Height / 2);
				float rotation = segmentsRotation[i];
				int spriteDirection = toOther.X > 0f ? 1 : -1;
				if(outer)
				{
					for (int a = 0; a < 360; a += 60)
					{
						Vector2 circular = new Vector2(Main.rand.NextFloat(3.0f, 4), 0).RotatedBy(MathHelper.ToRadians(a));
						color = new Color(255, 100, 100, 0) * 0.2f;
						spriteBatch.Draw(texture, segments[i] + projectile.velocity + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), rotation, origin, 1.05f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				else
				{
					color = new Color(255, 200, 200);
					spriteBatch.Draw(texture, segments[i] + projectile.velocity - Main.screenPosition, null, color, rotation, origin, 1.05f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
				}
				first = segments[i];
			}
		}
		public void BodyTailMovement(ref Vector2 position, Vector2 prevPosition, ref float segmentsRotation, float segmentsRotation2, int i)
		{
			float width = 44;
			if (i != segments.Count - 1 && i > 0)
				width = 36;
			else if(i != 0)
				width = 46;
			width -= 4;
			Vector2 npcCenter = position;
			float dirX = prevPosition.X - npcCenter.X;
			float dirY = prevPosition.Y - npcCenter.Y;
			segmentsRotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
			float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
			float dist = (length - width) / length;
			float posX = dirX * dist;
			float posY = dirY * dist;
			position.X += posX;
			position.Y += posY;
			/*
			float height = 40f;
			if (i != segments.Count - 1 && i != 0)
				height = 36f;
			if (i == segments.Count - 2)
				height = 48f;
			Vector2 toOther = prevPosition - position;
			if (segmentsRotation != segmentsRotation2)
			{
				var num1090 = MathHelper.WrapAngle(segmentsRotation2 - segmentsRotation);
				var radians64 = (double)(num1090 * 0.3f);
				toOther = toOther.RotatedBy(radians64);
			}
			segmentsRotation = toOther.ToRotation() + MathHelper.ToRadians(90);
			position = prevPosition - toOther.SafeNormalize(new Vector2(1, 0)) * height; */
		}
		Vector2 directVelo;
		int counter = 0;
		int counter2 = 0;
		bool wallMode = false;
		public override bool PreAI()
		{
			if (projectile.timeLeft <= 255)
				projectile.alpha++;
			if (runOnce)
			{
				if (projectile.ai[0] <= 0)
					projectile.ai[0] = 12;
				for (int i = 0; i < projectile.ai[0]; i++)
				{
					segments.Add(projectile.Center + new Vector2(0, 16));
					segmentsRotation.Add(0f);
				}
				directVelo = projectile.velocity;
				runOnce = false;
				if (projectile.ai[1] >= 0)
				{
					projectile.timeLeft = 1200;
					counter = Main.rand.Next(360);
					wallMode = true;
				}
				else
					projectile.timeLeft = 360;
			}
			Vector2 first = projectile.Center;
			float firstRot = projectile.rotation;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 pos = segments[i];
				if (projectile.timeLeft < Main.rand.Next(3) + 1)
				{
					for (int k = 0; k < Main.rand.Next(3) + 1; k++)
					{
						int dust2 = Dust.NewDust(new Vector2(pos.X, pos.Y) - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = new Color(255, 100, 100, 0);
						if(Main.rand.NextBool(3))
							dust.color = Color.Black;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 3.5f;
						dust.velocity *= 2.5f;
					}
				}
				float rotation = segmentsRotation[i];
				BodyTailMovement(ref pos, first, ref rotation, firstRot, i);
				segments[i] = pos;
				segmentsRotation[i] = rotation;
				first = pos;
				firstRot = segmentsRotation[i];
			}
			if (!wallMode)
            {
				ApplySlither();
				counter2++;
				if(counter2 % 3 == 0)
				{
					for (int i = 0; i < 8; i++)
					{
						int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = new Color(255, 100, 100, 0);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 3.5f;
						dust.velocity *= 2.5f;
					}
					Vector2 velo = projectile.velocity.SafeNormalize(new Vector2(1, 0)).RotatedBy(MathHelper.ToRadians(90)) * 5.0f;// * (i * 2 - 1);
					if (Main.netMode != 1)
					{
						Projectile.NewProjectile(projectile.Center, velo, ModContent.ProjectileType<InfernoPhaseBolt>(), projectile.damage, 0, Main.myPlayer);
					}
				}
			}
			else
			{
				counter2++;
				if(counter2 > 29)
				{
					float mult = 1 - (counter2 / 100f);
					if (mult > 1)
						mult = 1;
					if (mult < 0)
						mult = 0;
					NPC parent = Main.npc[(int)projectile.ai[1]];
					if(parent.type != ModContent.NPCType<SubspaceSerpentHead>() || !parent.active)
						projectile.Kill();
					Player player = Main.player[parent.target];
					bool left = player.Center.X > Main.maxTilesX / 2;
					int worldSide = left ? -1 : 1;
					counter += 1;
					projectile.velocity *= mult;
					SlitherWall(worldSide, counter);
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			return true;
		}
		public void SlitherWall(int direction, float rotate)
		{
			float mult = counter2 / 100f;
			if (mult > 1)
				mult = 1;
			if (mult < 0)
				mult = 0;
			NPC parent = Main.npc[(int)projectile.ai[1]];
			Player player = Main.player[parent.target];
			Vector2 circular = new Vector2(0, -440).RotatedBy(MathHelper.ToRadians(rotate * 2f * direction));
			Vector2 toLocation = new Vector2(parent.Center.X - 20 * direction, player.Center.Y + circular.Y);
			Vector2 goTo = toLocation - projectile.Center;
			float speed = 9f + goTo.Length() * 0.000575f;
			if (speed > goTo.Length())
				speed = goTo.Length();
			projectile.velocity += mult * goTo.SafeNormalize(Vector2.Zero) * speed;
		}
		public void ApplySlither()
		{
			counter += 7;
			float deg = 37.5f;
			if ((int)projectile.ai[1] == -2)
				deg = 1;
			Vector2 rotations = new Vector2(deg, 0).RotatedBy(MathHelper.ToRadians(counter));
			projectile.velocity = directVelo.RotatedBy(MathHelper.ToRadians(rotations.X));
		}
	}
}
		