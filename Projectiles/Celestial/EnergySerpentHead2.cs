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
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.friendly = false;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.hide = true;
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
        public override bool PreDraw(ref Color lightColor)
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/EnergySerpentHead2").Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 first = Projectile.Center;
			if (outer)
			{
				for (int a = 0; a < 360; a += 60)
				{
					Vector2 circular = new Vector2(Main.rand.NextFloat(3.0f, 4), 0).RotatedBy(MathHelper.ToRadians(a));
					color = new Color(255, 100, 100, 0) * 0.2f;
					spriteBatch.Draw(texture, first + circular - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, 1.05f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}
			}
			else
			{
				color = new Color(255, 200, 200);
				spriteBatch.Draw(texture, first - Main.screenPosition, null, color, Projectile.rotation, origin, 1.05f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
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
					texture = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/EnergySerpentBody2").Value;
				else
					texture = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/EnergySerpentTail2").Value;
				origin = new Vector2(texture.Width / 2, texture.Height / 2);
				float rotation = segmentsRotation[i];
				int spriteDirection = toOther.X > 0f ? 1 : -1;
				if(outer)
				{
					for (int a = 0; a < 360; a += 60)
					{
						Vector2 circular = new Vector2(Main.rand.NextFloat(3.0f, 4), 0).RotatedBy(MathHelper.ToRadians(a));
						color = new Color(255, 100, 100, 0) * 0.2f;
						spriteBatch.Draw(texture, segments[i] + Projectile.velocity + circular - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f), rotation, origin, 1.05f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				else
				{
					color = new Color(255, 200, 200);
					spriteBatch.Draw(texture, segments[i] + Projectile.velocity - Main.screenPosition, null, color, rotation, origin, 1.05f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
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
			if (Projectile.timeLeft <= 255)
				Projectile.alpha++;
			if (runOnce)
			{
				if (Projectile.ai[0] <= 0)
					Projectile.ai[0] = 12;
				for (int i = 0; i < Projectile.ai[0]; i++)
				{
					segments.Add(Projectile.Center + new Vector2(0, 16));
					segmentsRotation.Add(0f);
				}
				directVelo = Projectile.velocity;
				runOnce = false;
				if (Projectile.ai[1] >= 0)
				{
					Projectile.timeLeft = 1200;
					counter = Main.rand.Next(360);
					wallMode = true;
				}
				else
					Projectile.timeLeft = 360;
			}
			Vector2 first = Projectile.Center;
			float firstRot = Projectile.rotation;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 pos = segments[i];
				if (Projectile.timeLeft < Main.rand.Next(3) + 1)
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
						int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = new Color(255, 100, 100, 0);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 3.5f;
						dust.velocity *= 2.5f;
					}
					Vector2 velo = Projectile.velocity.SafeNormalize(new Vector2(1, 0)).RotatedBy(MathHelper.ToRadians(90)) * 5.0f;// * (i * 2 - 1);
					if (Main.netMode != 1)
					{
						Projectile.NewProjectile(Projectile.Center, velo, ModContent.ProjectileType<InfernoPhaseBolt>(), Projectile.damage, 0, Main.myPlayer);
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
					NPC parent = Main.npc[(int)Projectile.ai[1]];
					if(parent.type != ModContent.NPCType<SubspaceSerpentHead>() || !parent.active)
						Projectile.Kill();
					Player player = Main.player[parent.target];
					bool left = player.Center.X > Main.maxTilesX / 2;
					int worldSide = left ? -1 : 1;
					counter += 1;
					Projectile.velocity *= mult;
					SlitherWall(worldSide, counter);
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			return true;
		}
		public void SlitherWall(int direction, float rotate)
		{
			float mult = counter2 / 100f;
			if (mult > 1)
				mult = 1;
			if (mult < 0)
				mult = 0;
			NPC parent = Main.npc[(int)Projectile.ai[1]];
			Player player = Main.player[parent.target];
			Vector2 circular = new Vector2(0, -440).RotatedBy(MathHelper.ToRadians(rotate * 2f * direction));
			Vector2 toLocation = new Vector2(parent.Center.X - 20 * direction, player.Center.Y + circular.Y);
			Vector2 goTo = toLocation - Projectile.Center;
			float speed = 9f + goTo.Length() * 0.000575f;
			if (speed > goTo.Length())
				speed = goTo.Length();
			Projectile.velocity += mult * goTo.SafeNormalize(Vector2.Zero) * speed;
		}
		public void ApplySlither()
		{
			counter += 7;
			float deg = 37.5f;
			if ((int)Projectile.ai[1] == -2)
				deg = 1;
			Vector2 rotations = new Vector2(deg, 0).RotatedBy(MathHelper.ToRadians(counter));
			Projectile.velocity = directVelo.RotatedBy(MathHelper.ToRadians(rotations.X));
		}
	}
}
		