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
using SOTS.Projectiles.Lightning;

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
			projectile.timeLeft = 1200;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
        }
        public override bool ShouldUpdatePosition()
        {
            return !(runOnce || projectile.timeLeft > 1196);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce || projectile.timeLeft > 1196)
				return false;
			DrawWorm(spriteBatch, lightColor);
			return false;
		}
		int counter = 0;
		public void DrawWorm(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture3 = mod.GetTexture("NPCs/Boss/SubspaceSerpentHeadFill");
			Texture2D texture2 = mod.GetTexture("NPCs/Boss/SubspaceSerpentHeadGlow");
			Texture2D texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentHead");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 16);
			Vector2 first = projectile.Center;
			Color color = Lighting.GetColor((int)first.X / 16, (int)first.Y / 16, Color.White);
			Rectangle frame = new Rectangle(0, projectile.frame * texture.Height / 8, texture.Width, texture.Height / 8);
			counter++;
			if (counter > 12)
				counter = 0;
			if (segmentsDead <= 0)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 toTheSide = new Vector2(2, 0).RotatedBy(projectile.rotation + MathHelper.ToRadians(i * -90));
					spriteBatch.Draw(texture3, first - Main.screenPosition + toTheSide, frame, new Color(0, 255, 0), projectile.rotation, origin, 1f, SpriteEffects.None, 0);
				}
				spriteBatch.Draw(texture, first - Main.screenPosition, frame, color, projectile.rotation, origin, 1.00f, SpriteEffects.None, 1f);
				spriteBatch.Draw(texture2, first - Main.screenPosition, frame, Color.White, projectile.rotation, origin, 1.00f, SpriteEffects.None, 0);
				for (int j = 0; j < 2; j++)
				{
					float bonusAlphaMult = 1 - 1 * (counter / 12f);
					float dir = j * 2 - 1;
					Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(projectile.rotation);
					Main.spriteBatch.Draw(texture2, first - Main.screenPosition + offset, frame, new Color(100, 100, 100, 0) * bonusAlphaMult, projectile.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
				}
			}
			for (int i = 0; i < segments.Count; i++)
			{
				color = Lighting.GetColor((int)segments[i].X / 16, (int)segments[i].Y / 16, Color.White);
				if (i != segments.Count - 1)
				{
					texture3 = mod.GetTexture("NPCs/Boss/SubspaceSerpentBodyFill");
					texture2 = mod.GetTexture("NPCs/Boss/SubspaceSerpentBodyGlow");
					texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentBody");
				}
				else
				{
					texture3 = mod.GetTexture("NPCs/Boss/SubspaceSerpentTailFill");
					texture2 = mod.GetTexture("NPCs/Boss/SubspaceSerpentTailGlow");
					texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentTail");
				}
				origin = new Vector2(texture.Width / 2, texture.Height / 16);
				frame = new Rectangle(0, texture.Height / 8 * segmentFrame[i], texture.Width, texture.Height / 8);
				float rotation = segmentsRotation[i];
				if (segmentsDead <= i + 1)
				{
					for (int a = 0; a < 3; a++)
					{
						if(a != 1 || i == segments.Count - 1)
						{
							Vector2 toTheSide = new Vector2(2, 0).RotatedBy(rotation + MathHelper.ToRadians(a * 90));
							spriteBatch.Draw(texture3, segments[i] + projectile.velocity - Main.screenPosition + toTheSide, frame, new Color(0, 255, 0), rotation, origin, 1f, SpriteEffects.None, 0);
						}
					}
					spriteBatch.Draw(texture, segments[i] + projectile.velocity - Main.screenPosition, frame, color, rotation, origin, 1.00f, SpriteEffects.None, 1f);
					spriteBatch.Draw(texture2, segments[i] + projectile.velocity - Main.screenPosition, frame, Color.White, rotation, origin, 1.00f, SpriteEffects.None, 0);
					for (int j = 0; j < 2; j++)
					{
						float bonusAlphaMult = 1 - 1 * (counter / 12f);
						float dir = j * 2 - 1;
						Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(rotation);
						spriteBatch.Draw(texture2, segments[i] + projectile.velocity - Main.screenPosition + offset, frame, new Color(100, 100, 100, 0) * bonusAlphaMult, rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
					}
				}
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
							segmentFrame.Add((npc4.frame.Y / npc4.height) % 8);
							break;
						}
					}
				}
			}
			else
				projectile.Kill();
		}
		int deathCounter = 0;
		int segmentsDead = -8;
		public override bool PreAI()
		{
			if (runOnce)
			{
				Initiate();
				runOnce = false;
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			}
			Vector2 first = projectile.Center;
			Lighting.AddLight(first, 2.5f, 1.6f, 2.4f);
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
			projectile.velocity *= 0.985f;
			deathCounter++;
			if (deathCounter >= 5)
            {
				deathCounter = 0;
				if(segmentsDead <= segments.Count && segmentsDead >= 0)
				{
					Vector2 atLoc = projectile.Center;
					if (segmentsDead >= 1)
					{
						atLoc = segments[segmentsDead - 1];
					}
					for (int i = 0; i < 360; i += 5)
					{
						float rand = Main.rand.NextFloat(24f);
						if (Main.rand.NextBool(3))
						{
							Vector2 circularLocation = new Vector2(4 + rand, 0).RotatedBy(MathHelper.ToRadians(i));
							circularLocation.Y *= 1.25f;
							Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X - 4, atLoc.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.velocity *= 0.8f;
							dust.velocity += circularLocation;
							dust.scale *= 2.75f - (1f * rand / 24f);
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
					Gore gore2 = Main.gore[Gore.NewGore(atLoc - new Vector2(18, 18), default(Vector2), gore, 1.0f)];
					gore2.velocity *= 0.2f;
					if(segmentsDead % 4 == 0)
						Main.PlaySound(SoundID.NPCKilled, (int)atLoc.X, (int)atLoc.Y, 39, 0.95f, -0.3f);
					else
						Main.PlaySound(2, (int)atLoc.X, (int)atLoc.Y, 62, 1.25f, -0.3f);
					if (Main.netMode != 1)
                    {
						Projectile.NewProjectile(atLoc, new Vector2(0, -1), ModContent.ProjectileType<GreenLightning2>(), 0, 0, Main.myPlayer);
						Vector2 circular = new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
						if (segmentsDead % 4 == 0)
							Projectile.NewProjectile(atLoc, circular, ModContent.ProjectileType<PurgatoryGhost>(), 0, projectile.knockBack, Main.myPlayer, 0, Main.rand.Next(2) * 2 - 1);
						for(int j = 0; j < 2; j++)
						{
							if(Main.rand.NextBool(3))
							{
								Vector2 perturbedSpeed = (circular.SafeNormalize(Vector2.Zero) * 5.5f * Main.rand.NextFloat(0.75f, 1.25f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(180f) + j * 180));
								Projectile.NewProjectile(atLoc, perturbedSpeed, ModContent.ProjectileType<PurgatoryLightning>(), 0, 1f, Main.myPlayer, Main.rand.Next(2));
							}
						}
					}
					SOTSPlayer sOTSPlayer = SOTSPlayer.ModPlayer(Main.player[Main.myPlayer]);
					sOTSPlayer.screenShakeMultiplier += 7;
					if (segmentsDead == segments.Count)
					{
						projectile.Kill();
					}
				}
				segmentsDead++;
			}
			return true;
		}
	}
}
		