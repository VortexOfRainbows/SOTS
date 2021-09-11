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
    public class SubspaceDeathAnimation : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subspace Death Animation");
		}
		public List<Vector2> segments = new List<Vector2>();
		public List<float> segmentsRotation = new List<float>();
		public List<int> segmentFrame = new List<int>();
		bool runOnce = true;
		public override void SetDefaults()
        {
			projectile.width = 40;
			projectile.height = 40;
			projectile.friendly = false;
			projectile.timeLeft = 1000;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.hide = true;
			projectile.extraUpdates = 1;
		}
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			DrawWorm(spriteBatch, lightColor);
			return false;
		}
		int counter = 0;
		public void DrawWorm(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = lightColor; 
			Texture2D texture2 = mod.GetTexture("NPCs/Boss/SubspaceSerpentHeadGlow");
			Texture2D texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentHead");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 16);
			Vector2 first = projectile.Center;
			Rectangle frame = new Rectangle(0, projectile.frame * texture.Height / 8, texture.Width, texture.Height / 8);
			counter++;
			if (counter > 12)
				counter = 0;
			if (segmentsDead <= 0)
			{
				Vector2 forward = new Vector2(0, (float)Math.Pow(deathCounter, 1.7) * -1).RotatedBy(projectile.rotation);
				if (segmentsDead != 0)
					forward *= 0;
				else
				{
					for(int i = -1; i < 2; i+= 2)
						DrawLightningBetween(spriteBatch, segments[0], first + forward, i);
				}
				spriteBatch.Draw(texture, first + forward - Main.screenPosition, frame, color, projectile.rotation, origin, 1.00f, SpriteEffects.None, 1f);
				spriteBatch.Draw(texture2, first + forward - Main.screenPosition, frame, Color.White, projectile.rotation, origin, 1.00f, SpriteEffects.None, 0);
				for (int j = 0; j < 2; j++)
				{
					float bonusAlphaMult = 1 - 1 * (counter / 12f);
					float dir = j * 2 - 1;
					Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(projectile.rotation);
					Main.spriteBatch.Draw(texture2, first + forward - Main.screenPosition + offset, frame, new Color(100, 100, 100, 0) * bonusAlphaMult, projectile.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
				}
			}
			for (int i = 0; i < segments.Count; i++)
			{
				color = Lighting.GetColor((int)segments[i].X / 16, (int)segments[i].Y / 16, Color.White);
				if (i != segments.Count - 1)
				{
					texture2 = mod.GetTexture("NPCs/Boss/SubspaceSerpentBodyGlow");
					texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentBody");
				}
				else
				{
					texture2 = mod.GetTexture("NPCs/Boss/SubspaceSerpentTailGlow");
					texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentTail");
				}
				origin = new Vector2(texture.Width / 2, texture.Height / 16);
				frame = new Rectangle(0, texture.Height / 8 * segmentFrame[i], texture.Width, texture.Height / 8);
				float rotation = segmentsRotation[i];
				Vector2 forward = new Vector2(0, (float)Math.Pow(deathCounter, 1.7) * -1).RotatedBy(rotation);
				if (segmentsDead != i + 1)
					forward *= 0;
				else if(i != segments.Count - 1)
				{
					for (int k = -1; k < 2; k += 2)
						DrawLightningBetween(spriteBatch, segments[i + 1] + projectile.velocity, segments[i] + projectile.velocity + forward, k);
                }
				if (segmentsDead <= i + 1)
				{
					spriteBatch.Draw(texture, segments[i] + forward + projectile.velocity - Main.screenPosition, frame, color, rotation, origin, 1.00f, SpriteEffects.None, 1f);
					#region glow stuff
					spriteBatch.Draw(texture2, segments[i] + forward + projectile.velocity - Main.screenPosition, frame, Color.White, rotation, origin, 1.00f, SpriteEffects.None, 0);
					for (int j = 0; j < 2; j++)
					{
						float bonusAlphaMult = 1 - 1 * (counter / 12f);
						float dir = j * 2 - 1;
						Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(rotation);
						spriteBatch.Draw(texture2, segments[i] + forward + projectile.velocity - Main.screenPosition + offset, frame, new Color(100, 100, 100, 0) * bonusAlphaMult, rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
					}
				}
				#endregion
			}
		}
		public void DrawLightningBetween(SpriteBatch spriteBatch, Vector2 from, Vector2 to, int direction = -1)
        {
			List<Vector2> drawAreas = new List<Vector2>();
			Vector2 goTo = to - from;
			goTo *= 0.1f;
			for(int i = 0; i < 10; i++)
			{
				from += goTo;
				Vector2 temp = from;
				float mult = (float)Math.Sin(i * 18 * MathHelper.Pi / 180f);
				temp += new Vector2(0, 12 * direction * mult).RotatedBy(goTo.ToRotation());
				temp += new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3)) * mult;
				drawAreas.Add(temp);
			}
			Texture2D texture = mod.GetTexture("Projectiles/Lightning/CataclysmLightning");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = from;
			Color color = new Color(140, 170, 140, 0) * ((255 - projectile.alpha) / 255f);
			for (int k = 0; k < drawAreas.Count; k++)
			{
				float scale = 0.6f;
				scale *= 0.5f + 0.5f * (float)Math.Sin(k * 18 * MathHelper.Pi/180f);
				Vector2 drawPos = drawAreas[k] - Main.screenPosition;
				Vector2 currentPos = drawAreas[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.5f);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
						{
							x = 0;
							y = 0;
						}
						spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
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
		public void Initiate()
		{
			NPC master = Main.npc[(int)projectile.ai[1]];
			if (master.active && master.type == ModContent.NPCType<SubspaceSerpentHead>())
			{
				projectile.frame = master.frame.Y / master.height;
				int latest = master.whoAmI;
				for (int i = 0; i < 51; i++)
				{
					for (int j = 0; j < 200; j++)
					{
						NPC npc4 = Main.npc[j];
						if (npc4.active && npc4.realLife == master.whoAmI && npc4.ai[1] == latest)
						{
							latest = npc4.whoAmI;
							segments.Add(npc4.Center);
							segmentsRotation.Add(npc4.rotation);
							segmentFrame.Add(npc4.frame.Y / npc4.height);
							break;
						}
					}
				}
			}
			else
				projectile.Kill();
		}
		int deathCounter = 0;
		int segmentsDead = -6;
		public override bool PreAI()
		{
			if (runOnce)
			{
				Initiate();
				runOnce = false;
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			}
			Vector2 first = projectile.Center;
			float firstRot = projectile.rotation;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 pos = segments[i];
				/*if (projectile.timeLeft < Main.rand.Next(3) + 1)
				{
					for (int k = 0; k < Main.rand.Next(3) + 1; k++)
					{
						int dust2 = Dust.NewDust(new Vector2(pos.X, pos.Y) - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = new Color(100, 255, 100, 0);
						if(Main.rand.NextBool(3))
							dust.color = Color.Black;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 3.5f;
						dust.velocity *= 2.5f;
					}
				}*/
				float rotation = segmentsRotation[i];
				BodyTailMovement(ref pos, first, ref rotation, firstRot, i);
				segments[i] = pos;
				segmentsRotation[i] = rotation;
				first = pos;
				firstRot = segmentsRotation[i];
				Lighting.AddLight(segments[i], 2.5f, 1.6f, 2.4f);
			}
			projectile.velocity *= 0.9765f;
			deathCounter++;
			if (deathCounter >= 10)
            {
				deathCounter = 0;
				if(segmentsDead <= segments.Count)
				{
					Vector2 atLoc = projectile.Center;
					if (segmentsDead >= 1)
					{
						atLoc = segments[segmentsDead - 1];
					}
					Vector2 cVelo = new Vector2(0, -4).RotatedBy(projectile.rotation);
					if (segmentsDead >= 1)
						cVelo = new Vector2(0, -4).RotatedBy(segmentsRotation[segmentsDead - 1]);
					for (int i = 0; i < 360; i += 6)
					{
						if (Main.rand.NextBool(4))
						{
							Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(i));
							circularLocation.Y *= 1.25f;
							Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X - 4, atLoc.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.velocity *= 0.8f;
							dust.velocity += circularLocation + cVelo;
							dust.scale *= 2.25f;
							dust.fadeIn = 0.1f;
							dust.color = new Color(50, 150, 50);
						}
					}
					int gore = mod.GetGoreSlot("Gores/Subspace/SubspaceSerpentBodyGore");
					if (segmentsDead == 0)
					{
						gore = mod.GetGoreSlot("Gores/Subspace/SubspaceSerpentHeadGore");
					}
					else if (segmentsDead == segments.Count)
					{
						gore = mod.GetGoreSlot("Gores/Subspace/SubspaceSerpentTailGore");
					}
					Main.PlaySound(2, (int)atLoc.X, (int)atLoc.Y, 62, 1.25f, -0.3f);
					Gore gore2 = Main.gore[Gore.NewGore(atLoc + cVelo * 18 - new Vector2(18, 18), default(Vector2), gore, 1.0f)];
					gore2.velocity += cVelo;
					if (segmentsDead == segments.Count)
					{
						projectile.Kill();
					}
					segmentsDead++; 
				}
			}
			return true;
		}
	}
}
		