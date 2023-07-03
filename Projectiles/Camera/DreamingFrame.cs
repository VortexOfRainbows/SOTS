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
using SOTS.Buffs.Debuffs;

namespace SOTS.Projectiles.Camera
{    
    public class DreamingFrame : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(mousePosition.X);
			writer.Write(mousePosition.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			mousePosition.X = reader.ReadSingle();
			mousePosition.Y = reader.ReadSingle();
		}
        public Vector2 mousePosition = Vector2.Zero;
		public static Color Green1 => new Color(86, 226, 100, 0);
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
			Projectile.friendly = true;
			Projectile.width = 120;
			Projectile.height = 120;
			Projectile.timeLeft = 80;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
        public override bool? CanCutTiles()
        {
            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.timeLeft < 16 && Projectile.ai[0] == 1 && Vector2ListContainsX(npcVectors, target.whoAmI);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			overWiresUI.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			DrawAllGrabReticle();
			Player player = Main.player[Projectile.owner];
			float scaleMult = 1f;
			float windUpProgress = windUp / windUpTime;
			Color color = Green1;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraCenterCross");
			Texture2D textureGradient = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/LongGradient");
			Texture2D borderTexture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraBorder");
			Texture2D frameTexture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 center = Projectile.Center - Main.screenPosition;
			float squareRadius = 60;
			if(Projectile.timeLeft < 20)
            {
				squareRadius += (float)Math.Pow((20 - Projectile.timeLeft) * 0.4f, 2);
				float alphaMult = 1 - ((20 - Projectile.timeLeft) / 20f);
				color *= (0.55f * alphaMult + 0.45f * (float)Math.Pow(alphaMult, 2));
			}
			for (int i = 0; i < 4; i++)
			{
				float progress = windUpProgress;
				Vector2 playerToProjectile = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - player.MountedCenter;
				Vector2 offset = player.MountedCenter + new Vector2(28, -4.5f * player.direction).RotatedBy(playerToProjectile.ToRotation());
				Vector2 framePosition = new Vector2(-squareRadius, -squareRadius);
				float borderRotation = 0;
				progress /= 0.75f;
				if (i == 1)
				{
					borderRotation = MathHelper.PiOver2;
					framePosition = Vector2.Lerp(framePosition, new Vector2(squareRadius, -squareRadius), MathHelper.Clamp(progress, 0, 1));
				}
				if (i == 2)
				{
					borderRotation = MathHelper.Pi;
					framePosition = Vector2.Lerp(framePosition, new Vector2(squareRadius, squareRadius), MathHelper.Clamp(windUpProgress / 0.95f, 0, 1));
				}
				if (i == 3)
				{
					borderRotation = -MathHelper.PiOver2;
					framePosition = Vector2.Lerp(framePosition, new Vector2(-squareRadius, squareRadius), MathHelper.Clamp(progress, 0, 1));
				}
				float borderVisibility = 1;
				Vector2 laserFramePosition = framePosition;
				if(postCounter > 0)
                {
					float theta = (postCounter % 180) / 180f;
					Vector2 nextFramePosition = laserFramePosition.RotatedBy(MathHelper.PiOver2);
					laserFramePosition = Vector2.Lerp(laserFramePosition, nextFramePosition, (1 - (float)Math.Cos(theta * Math.PI)) / 2f);
                }
				Vector2 playerToFrame = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - offset + laserFramePosition;
				float lengthToFrame = playerToFrame.Length();
				for (int j = 0; j < 2; j++)
				{
					float bonusMultiplier = MathHelper.Clamp(windUpProgress, 0, 1) * (0.6f - j * 0.3f);
					Main.spriteBatch.Draw(textureGradient, offset - Main.screenPosition, null, color * bonusMultiplier, playerToFrame.ToRotation(), new Vector2(1, 1), new Vector2(1f / (textureGradient.Width - 24) * lengthToFrame, 1f), SpriteEffects.None, 0);
				}
				if (i == 1 || i == 2)
				{
					progress = progress * 0.75f - 0.75f;
					progress *= 4;
				}
				if (progress < 0)
					progress = 0;
				if (progress > 1)
					progress = 1;
				if (i == 1)
				{
					borderRotation = MathHelper.PiOver2;
					framePosition = new Vector2(squareRadius, -squareRadius);
				}
				if (i == 2)
				{
					borderRotation = MathHelper.Pi;
					framePosition = Vector2.Lerp(new Vector2(-squareRadius, squareRadius), new Vector2(squareRadius, squareRadius), progress);
				}
				Main.spriteBatch.Draw(borderTexture, center + framePosition, null, color * (0.5f + 0.75f * progress) * borderVisibility, borderRotation, new Vector2(0, 1), new Vector2(1f / borderTexture.Width * squareRadius * 2 * progress, 0.75f), SpriteEffects.None, 0);
			}
			float longerExplode = postCounter;
			if (longerExplode > 10)
				longerExplode = 10;
			float starWindUp = (windUp + longerExplode - ActivateRange + 3) / (windUpTime - ActivateRange + 13);
			if(starWindUp > 0)
			{
				scaleMult *= (1.2f - 0.2f * (float)Math.Cos(MathHelper.ToRadians(420 * starWindUp))) * (squareRadius / 60f); // 11 / 10 scale is final
				SOTSProjectile.DrawStar(Projectile.Center, color, 0.3f * starWindUp, MathHelper.PiOver4, 0f, 4, 12.8f * scaleMult, 12 * scaleMult, 1f, 540, 4.8f * scaleMult, 1);
				SOTSProjectile.DrawStar(Projectile.Center, color, 0.5f * starWindUp, 0, 0f, 4, 2.56f * scaleMult, 0, 1f, 240, 0, 1);
				for (int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(45 * i));
					Main.spriteBatch.Draw(texture, center + circular, null, color * 0.5f * starWindUp * starWindUp, 0, texture.Size() / 2, scaleMult * 0.8f, SpriteEffects.None, 0);
				}
			}
			for (int i = 0; i < 4; i++)
			{
				float rotation = MathHelper.PiOver2 * i;
				Vector2 framePosition = new Vector2(-squareRadius, -squareRadius).RotatedBy(rotation);
				float remove1 = (i == 1 || i == 3) ? 0.975f : i == 2 ? 1.95f : 0; 
				float progress = windUp / windUpTime * 2 - remove1;
				if (i == 2)
					progress *= 2;
				progress = Math.Clamp(progress, 0, 1);
				Vector2 startingPosition = new Vector2(0, 0);
				float width = 0;
				float height = 0;
				if(i == 0) //start from top left, end bottom right
                {
					width = 19;
					height = 19;
					if(progress > 1f / 4f)
                    {
						width = 27;
						height = 27;
					}
					if (progress > 2f / 4f)
					{
						width = 33;
						height = 33;
					}
					if (progress > 3f / 4f)
					{
						width = 36;
						height = 36;
					}
				}
				if(i == 1) //start from bottom left, but split, end top right
				{
					if(progress > 0)
					{
						startingPosition = new Vector2(0, 33);
						width = 19;
						height = 3;
					}
					if (progress > 1f / 7f)
					{
						startingPosition = new Vector2(0, 27);
						width = 19;
						height = 9;
					}
					if (progress > 2f / 7f)
					{
						startingPosition = new Vector2(0, 19);
						width = 19;
						height = 17;
					}
					if (progress > 3f / 7f)
					{
						startingPosition = new Vector2(0, 0);
						width = 19;
						height = 36;
					}
					if (progress > 4f / 7f)
					{
						width = 27;
						height = 36;
					}
					if (progress > 5f / 7f)
					{
						width = 33;
						height = 36;
					}
					if (progress > 6f / 7f)
					{
						width = 36;
						height = 36;
					}
				}
				if (i == 3) //start from top right, but split, end bottom left
				{
					if (progress > 0)
					{
						startingPosition = new Vector2(33, 0);
						height = 19;
						width = 3;
					}
					if (progress > 1f / 7f)
					{
						startingPosition = new Vector2(27, 0);
						height = 19;
						width = 9;
					}
					if (progress > 2f / 7f)
					{
						startingPosition = new Vector2(19, 0);
						height = 19;
						width = 17;
					}
					if (progress > 3f / 7f)
					{
						startingPosition = new Vector2(0, 0);
						height = 19;
						width = 36;
					}
					if (progress > 4f / 7f)
					{
						height = 27;
						width = 36;
					}
					if (progress > 5f / 7f)
					{
						height = 33;
						width = 36;
					}
					if (progress > 6f / 7f)
					{
						height = 36;
						width = 36;
					}
				}
				if (i == 2) //finish all at once
				{
					if(progress > 0)
                    {
						width = 36;
						height = 36;
                    }						
				}
				Rectangle frameRectangle = new Rectangle((int)startingPosition.X, (int)startingPosition.Y, (int)width, (int)height);
				float scaleMult2 = 1.25f * (0.75f + 0.25f * scaleMult);
				for (int j = 0; j < 8; j++)
				{
					Vector2 circular = new Vector2(1.0f, 0).RotatedBy(MathHelper.ToRadians(45 * j));
					Main.spriteBatch.Draw(frameTexture, center + framePosition + circular - new Vector2(startingPosition.Y, startingPosition.X) * scaleMult2, frameRectangle, color * 0.6f, rotation, new Vector2(9, 9), scaleMult2, SpriteEffects.None, 0);
				}
				Main.spriteBatch.Draw(frameTexture, center + framePosition - new Vector2(startingPosition.Y, startingPosition.X) * scaleMult2, frameRectangle, color * 1.1f, rotation, new Vector2(9, 9), scaleMult2, SpriteEffects.None, 0);
			}
			return false;
		}
		public void DrawAllGrabReticle()
		{
			Player player = Main.player[Projectile.owner];
			Vector2 playerToProjectile = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - player.MountedCenter;
			Vector2 offset = player.MountedCenter + new Vector2(28, -4.5f * player.direction).RotatedBy(playerToProjectile.ToRotation());
			Color color = Green1;
			Texture2D textureGradient = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/LongGradient");
			for (int i = 0; i < itemVectors.Count; i++)
            {
				Item item = Main.item[(int)itemVectors[i].X];
				float progress = itemVectors[i].Y / (float)timeToGrabVisual;
				float alternateProgress = (Projectile.timeLeft - 5f) / 15f;
				alternateProgress = Math.Clamp(alternateProgress, 0, 1);
				progress *= alternateProgress;
				if(Projectile.timeLeft > 8)
				{
					float scaleMult = 1.1f - 1f * (float)Math.Cos(MathHelper.ToRadians(Math.Clamp(510f * (1 - alternateProgress), 0, 360)));
					Vector2 itemToPlayer = item.Center - new Vector2(0, 2) - offset;
					Main.spriteBatch.Draw(textureGradient, offset - Main.screenPosition, null, color * (0.1f + 0.1f * scaleMult), itemToPlayer.ToRotation(), new Vector2(1, 1), new Vector2(1f / (textureGradient.Width - 32) * itemToPlayer.Length() * (float)Math.Sqrt(alternateProgress), 1 + 0.4f * scaleMult), SpriteEffects.None, 0);
				}
				DrawGrabReticle(item.Center + new Vector2(0, -2), progress, 0.75f);
			}
			for (int i = 0; i < npcVectors.Count; i++)
			{
				NPC npc = Main.npc[(int)npcVectors[i].X];
				float progress = npcVectors[i].Y / (float)timeToGrabVisual;
				if (Projectile.timeLeft < 20)
					progress *= (Projectile.timeLeft - 10f) / 10f;
				if (progress < 0)
					progress = 0;
				DrawGrabReticle(npc.Center + new Vector2(0, -2), progress, 1.0f);
			}
		}
		public static int timeToGrabVisual = 18;
		public void DrawGrabReticle(Vector2 position, float progress, float sizeMult = 1f)
		{
			Color color = Green1;
			float scaleMult = sizeMult;
			float starWindUp = progress;
			if (starWindUp > 0)
			{
				scaleMult *= (1.4f - 0.4f * (float)Math.Cos(MathHelper.ToRadians(420 * starWindUp)));
				SOTSProjectile.DrawStar(position, color, starWindUp, 0f, 0f, 4, 6.6f * scaleMult, 6 * scaleMult, 1f, 180, 2.7f * scaleMult, 0);
				/*SOTSProjectile.DrawStar(Projectile.Center, color, 0.4f * starWindUp, 0, 0f, 4, 2.56f * scaleMult, 0, 1f, 240, 0, 1);
				for (int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(45 * i));
					Main.spriteBatch.Draw(texture, center + circular, null, color * 0.6f * starWindUp * starWindUp, 0, texture.Size() / 2, scaleMult * 0.8f, SpriteEffects.None, 0);
				}*/
			}
		}
        private static float windUpTime = 18f;
		public static float ActivateRange = 12f;
		float postCounter = 0;
		bool runOnce = true;
		float windUp = 0f;
		public List<Vector2> npcVectors = new List<Vector2>();
		public List<Vector2> itemVectors = new List<Vector2>();
		public override void AI()
		{ 
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, new Vector3(86 / 255f, 226 / 255f, 100 / 255f) * windUp / windUpTime);
			Projectile.rotation += 0.2f;
			if (windUp < windUpTime)
				windUp += 1;
			else
				postCounter++;
			if (Main.myPlayer == Projectile.owner)
            {
				mousePosition = Main.MouseWorld;
				Projectile.netUpdate = true;
			}
			if (player.channel || Projectile.timeLeft > 22)
			{
				if(player.itemTime < 22)
				{
					player.itemAnimation = 21;
					player.itemTime = 21;
					Projectile.timeLeft = 21;
				}
				if(Projectile.timeLeft < 22)
					Projectile.timeLeft = 21;
				if (Projectile.alpha > 0) //alpha in this case controls the camera movement
					Projectile.alpha -= 6; //this should take 43 frames to fully remove
				Projectile.alpha = Math.Clamp(Projectile.alpha, 0, 255);
			}
			else if(!player.channel) //this is for the projectiles ending phase
			{
				if(Projectile.timeLeft == 19)
                {
					SOTSUtils.PlaySound(SoundID.Item84, player.MountedCenter, 0.45f, 0.36f);
                }
				if(Projectile.timeLeft < 15)
				{
					float squareRadius = 60;
					if (Projectile.timeLeft < 20)
					{
						squareRadius += (float)Math.Pow((20 - Projectile.timeLeft) * 0.4f, 2);
					}
					for (int i = 0; i < 4; i++)
                    {
						Vector2 corner = new Vector2(-squareRadius -32, -squareRadius).RotatedBy(MathHelper.PiOver2 * i);
						for(int j = 0; j < 15; j++)
                        {
							if(Main.rand.NextBool(Projectile.timeLeft + 3))
							{
								Vector2 onward = new Vector2((squareRadius * 2 + 32) * (float)Math.Sqrt(j / 15f), 0).RotatedBy(MathHelper.PiOver2 * i);
								Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) + onward + corner, 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>());
								dust.color = Green1 * 0.9f;
								dust.velocity = dust.velocity * 0.2f + onward.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3.5f, 4.2f) + onward * 0.0075f;
								dust.noGravity = true;
								dust.fadeIn = 0.1f;
								dust.scale = dust.scale * 0.4f + 0.7f + (0.5f * 60f / squareRadius);
							}
						}
                    }
                }
				if (Projectile.timeLeft < 16)
				{
					if (Projectile.timeLeft == 15)
					{
						for (int i = 0; i < npcVectors.Count; i++)
						{
							NPC target = Main.npc[(int)npcVectors[i].X];
							if (isNPCValidTarget(target))
							{
								if (target.catchItem > 0)
								{
									NPC.CheckCatchNPC(target, target.Hitbox, player.HeldItem, player, true);
								}
								else
									target.AddBuff(ModContent.BuffType<DendroChain>(), DendroChainNPCOperators.DendroChainStandardDuration + Projectile.damage);
								SOTSProjectile.DustStar(target.Center, Vector2.Zero, Green1 * 0.5f, 0f, 40, 0, 4, 12f, 9f, 1f, 0.9f, 0.08f);
							}
						}
					}
					Projectile.ai[0] = 1;
				}
				if (Projectile.alpha < 255)
				{
					Projectile.alpha += 13; //should take only 20 frames to return to normal
				}
				Projectile.alpha = Math.Clamp(Projectile.alpha, 0, 255);
				Projectile.velocity *= 0.5f;
				for(int i = 0; i < itemVectors.Count; i++)
				{
					Item item = Main.item[(int)itemVectors[i].X];
					if (item.active)
                    {
						if(Projectile.timeLeft <= 5)
							item.Center = player.MountedCenter;
						if(Projectile.timeLeft == 6)
							SOTSProjectile.DustStar(item.Center, Vector2.Zero, Green1 * 0.5f, 0f, 32, 0, 4, 8f, 6f, 1f, 0.75f, 0.04f);
					}
				}
				return;
            }
			if(windUp > ActivateRange)
            {
				if(runOnce)
				{
					Vector2 addedVelocity = Projectile.position - Projectile.oldPosition + Projectile.velocity;
					addedVelocity *= 0.9f;
					SOTSUtils.PlaySound(SoundID.Item28, Projectile.Center, 0.7f, -0.2f, 0f);
					int total = 16;
					for(int i = 0; i < total; i++)
					{
						float scaleMult = 1f;
						Vector2 FlowerVe = new Vector2(-60f, -60f).RotatedBy(MathHelper.ToRadians(i * 360f / total));
						if (i % 4 != 0)
							scaleMult = 0.6f;
						Vector2 velo = Main.rand.NextVector2Circular(1, 1) + FlowerVe.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2f, 3f);
						SOTSProjectile.DustStar(Projectile.Center + FlowerVe, addedVelocity + velo * scaleMult, Green1 * scaleMult, 0f, 40, 0, 4, 8f, 5f, 1f, 1f * (0.2f + 0.8f * scaleMult));
					}
					for (int i = 0; i < 60; i++)
					{
						Vector2 circular = new Vector2(Main.rand.NextFloat(10f, 15f), 0).RotatedBy(MathHelper.ToRadians(i * 6f + Main.rand.NextFloat(-3f, 3f)));
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>());
						dust.color = Green1;
						dust.velocity = dust.velocity * 0.2f + circular + addedVelocity;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = dust.scale * 0.5f + 1.0f;
					}
				}
				runOnce = false;
            }
			float speedMult = windUp / windUpTime;
			if (mousePosition.X > 0 && mousePosition.Y > 0)
            {
				Vector2 toMousePos = mousePosition - Projectile.Center;
				Vector2 velo = toMousePos.SafeNormalize(Vector2.Zero) * 3.2f * speedMult;
				if(velo.Length() > toMousePos.Length())
					velo = toMousePos;
				Projectile.velocity *= 0.4f;
				Projectile.velocity += velo;
				Projectile.Center = Vector2.Lerp(Projectile.Center, mousePosition, 0.016f * speedMult);
            }
			Projectile.oldPosition = Projectile.position;
			ManageListCounters();
			ManageHitbox();
		}
		public bool isNPCValidTarget(NPC npc)
        {
			return npc.active && !npc.friendly && !npc.immortal && !npc.dontTakeDamage;
        }
		public bool Vector2ListContainsX(List<Vector2> Vector2s, int x)
        {
			for(int i = 0; i < Vector2s.Count; i++)
            {
                if (Vector2s[i].X == x)
                {
					return true;
                }
            }
			return false;
        }
		public void ManageHitbox()
        {
			int width = 120;
			Rectangle hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width / 2, width, width);
			if(npcVectors.Count < 10)
			{
				for (int i = 0; i < 200; i++)
				{
					NPC target = Main.npc[i];
					if (!Vector2ListContainsX(npcVectors, i) && isNPCValidTarget(target) && target.Hitbox.Intersects(hitbox))
					{
						npcVectors.Add(new Vector2(i, 0));
					}
					if (npcVectors.Count >= 10)
					{
						break;
					}
				}
			}
			for (int i = 0; i < 400; i++)
			{
				Item target = Main.item[i];
				if (!Vector2ListContainsX(itemVectors, i) && target.Hitbox.Intersects(hitbox) && target.active)
				{
					itemVectors.Add(new Vector2(i, 0));
				}
			}
		}
		public void ManageListCounters()
		{
			for (int i = 0; i < npcVectors.Count; i++)
			{
				NPC npc = Main.npc[(int)npcVectors[i].X];
				if (!isNPCValidTarget(npc))
				{
					npcVectors.RemoveAt(i);
				}
				else if (npcVectors[i].Y < timeToGrabVisual)
				{
					npcVectors[i] = new Vector2(npcVectors[i].X, npcVectors[i].Y + 1);
					if (npcVectors[i].Y == 6)
					{
						SOTSUtils.PlaySound(SoundID.MenuTick, Main.player[Projectile.owner].Center, 1.2f, -0.1f);
					}
				}
			}
			for (int i = 0; i < itemVectors.Count; i++)
			{
				Item item = Main.item[(int)itemVectors[i].X];
				if (!item.active)
				{
					itemVectors.RemoveAt(i);
				}
				else if (itemVectors[i].Y < timeToGrabVisual)
				{
					itemVectors[i] = new Vector2(itemVectors[i].X, itemVectors[i].Y + 1);
                    if (itemVectors[i].Y == 6)
                    {
						SOTSUtils.PlaySound(SoundID.MenuTick, Main.player[Projectile.owner].Center, 1.2f, 0.1f);
                    }
				}
			}
		}
	}
}
		