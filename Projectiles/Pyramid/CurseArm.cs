using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseArm : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(aiCounter1);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			aiCounter1 = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
		}
        public override void SetDefaults()
        {
			projectile.height = 24;
			projectile.width = 24;
			projectile.friendly = false;
			projectile.timeLeft = 510;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.ai[0] = -1f;
			projectile.tileCollide = false;
		}
		Vector2 OwnerPos;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 4;
			height = 4;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.velocity *= 0.0f;
            return false;
        }
		float percent = 0;
        public bool DrawLimbs(List<CurseFoam> dustList, Rectangle targetHitbox, bool genProj = false)
		{
			int parentID = (int)projectile.ai[0];
			if (parentID >= 0)
			{
				NPC npc = Main.npc[parentID];
				if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
				{
					var Limb = this.projectile;
					Vector2 distanceToOwner = Limb.Center - npc.Center;
					float distance = distanceToOwner.Length();
					if (distance == 0)
						return false;
					float distanceModified = 0.8f * (32f - (float)Math.Sqrt(distanceToOwner.Length()));
					if (distanceModified < 0)
						distanceModified = 0;
					int max = 16 + (int)(distance / 21);
					int start = 0;
					int end = max;
					if(genProj)
                    {
						start = (int)(percent * max);
						end = start + 1;
					}
					for (float k = start; k < end;)
					{
						float percent = (float)k / max;
						Vector2 toARM = Limb.Center - npc.Center;
						toARM *= percent;
						Vector2 spiralAddition = new Vector2(distanceModified * (1.5f - 1.0f * percent), 0).RotatedBy(MathHelper.ToRadians(aiCounter1 * 3 + k * 24 * (float)Math.Pow((1280.0 / distance), 0.6)));
						spiralAddition = new Vector2(0, spiralAddition.X).RotatedBy(toARM.ToRotation());
						Vector2 finalPosition = npc.Center + toARM + spiralAddition;
						float scale = 0.5f * (1.55f - 0.75f * percent);
						Vector2 rotational = new Vector2(0, (1.45f - 0.55f * percent) / (1.0f * (float)Math.Pow((1280.0 / distance), 0.2))).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
						Vector2 rotationaPosMod = new Vector2(0, Main.rand.NextFloat(4) * scale).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
						rotational += Limb.velocity * 0.1f;
						if(targetHitbox.X != 0 || targetHitbox.Y != 0)
                        {
							int width = (int)(18 - 9 * percent);
							Rectangle hitbox = new Rectangle((int)(finalPosition.X - width), (int)(finalPosition.Y - width), width, width);
							if (hitbox.Intersects(targetHitbox))
								return true;
                        }
						else if(genProj)
						{ 
							if(Main.netMode != NetmodeID.MultiplayerClient)
                            {
								float degrees = 90;
								if (Main.expertMode)
									degrees = Main.rand.NextFloat(80f, 100f);
								Projectile.NewProjectile(finalPosition, toARM.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(degrees * side)), ModContent.ProjectileType<CurseExtension>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0, parentID);
                            }
							return false;
                        }
						else
                        {
							dustList.Add(new CurseFoam(finalPosition + rotationaPosMod.SafeNormalize(Vector2.Zero) * 2 * scale * (0.5f * (float)Math.Pow((1280.0 / distance), 0.2)), rotational, Main.rand.NextFloat(0.9f, 1.1f) * scale, true, draggingType ? 2.5f : 1));
						}
						k += genProj ? 1 : 0.3f + scale;
					}
				}
			}
			return false;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return DrawLimbs(null, targetHitbox);
		}
		int side = 1;
		bool doAttack = true;
		float aiCounter1 = 0;
		bool draggingType = false;
		public override bool PreAI()
		{
			if (projectile.ai[1] != 0 && !draggingType)
			{
				projectile.timeLeft = 1090;
				draggingType = true;
			}
			aiCounter1++; 
			if (aiCounter1 >= 60)
			{
				float veloLength = projectile.velocity.Length();
				if(veloLength < 64f)
					projectile.velocity *= 1.1f;
				if(aiCounter1 == 60)
				{
					Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 96, 1.25f, -0.2f);
				}
				if(aiCounter1 > 90)
                {
					projectile.velocity *= 0f;
                }
				if (!draggingType && aiCounter1 > 90 && aiCounter1 % 20 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (percent == 0)
						percent = 0.05f;
					percent += 0.1f;
					if (percent >= 1)
					{
						if(percent >= 1.05f)
						{
							percent = 0.2f;
						}
						else
							doAttack = false;
					}
					if(doAttack)
						DrawLimbs(null, new Rectangle(0, 0, 0, 0), true);
					side *= -1;
				}
				else if(draggingType && aiCounter1 > 90)
				{
					RotateAI();
				}
            }
			if (aiCounter1 < 60)
			{
				float mult = 1.0f;
				if (aiCounter1 > 20)
					mult = 0.30f;
				projectile.position += (float)Math.Sin(MathHelper.ToRadians(4.5f * aiCounter1 + 90)) * projectile.velocity * mult;
			}
			else
            {
				float veloLength = projectile.velocity.Length();
				int length = (int)(veloLength / 14);
				veloLength -= length * 14;
				Vector2 temp = projectile.velocity;
				projectile.position += projectile.velocity.SafeNormalize(Vector2.Zero) * veloLength;
				for(int i = 0; i < length; i++)
				{
					int x = (int)projectile.Center.X / 16;
					int y = (int)projectile.Center.Y / 16;
					Tile tile = Framing.GetTileSafely(x, y);
					if ((!WorldGen.InWorld(x, y, 20) || tile.active() && !Main.tileSolidTop[tile.type] && Main.tileSolid[tile.type] && tile.type == ModContent.TileType<TrueSandstoneTile>()) || tile.wall == ModContent.WallType<TrueSandstoneWallWall>())
					{
						projectile.velocity *= 0.0f;
					}
					projectile.position += projectile.velocity.SafeNormalize(Vector2.Zero) * 14;
				}
				if(draggingType && counter % 30 == 0 && aiCounter1 > 90)
                {
					Vector2 projVelo = temp.SafeNormalize(Vector2.Zero);
					//Projectile.NewProjectile(projectile.Center - projVelo * 24, projVelo * -4, ModContent.ProjectileType<CurseWave>(), projectile.damage, 0f, Main.myPlayer, (int)projectile.ai[0], 1f);
				}
            }
			return base.PreAI();
        }
		int counter = 0;
		float currentIterator = 1f;
		float nextIterator = -1.1f;
		float midIterator = 1f;
		float toNextIterator = 0f;
		public void RotateAI()
		{
			int parentID = (int)projectile.ai[0];
			Vector2 veloToGo = new Vector2(0, 1200).RotatedBy(MathHelper.ToRadians(projectile.ai[1]));
			projectile.ai[1] += midIterator;
			NPC npc = Main.npc[parentID];
			if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
			{
				projectile.Center = npc.Center;
				projectile.velocity = veloToGo;
			}
			else
			{
				projectile.Kill();
			}
			if(counter % 300 == 0 || (toNextIterator > 0 && toNextIterator <= 120))
            {
				TransitionIterator();
			}
			counter++;
		}
		public void TransitionIterator()
		{
			float iteratorCounter = toNextIterator;
			if (iteratorCounter >= 120)
			{
				toNextIterator = 0;
				currentIterator = midIterator;
				nextIterator = -currentIterator * 1.1f;
			}
			else
			{
				float mult = (float)Math.Sin(MathHelper.ToRadians(iteratorCounter * 0.75f));
				midIterator = currentIterator * (1 - mult) + nextIterator * mult;
				toNextIterator++;
			}
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void AI()
		{
			int parentID = (int)projectile.ai[0];
			if(parentID >= 0 && Main.netMode != NetmodeID.Server)
            {
				NPC npc = Main.npc[parentID];
				if(npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
                {
					OwnerPos = npc.Center;
					Vector2 distanceToOwner = projectile.Center - OwnerPos;
					PharaohsCurse curse = npc.modNPC as PharaohsCurse;
					DrawLimbs(curse.foamParticleList1, new Rectangle(0, 0, 0, 0));
					PharaohsCurse.SpawnPassiveDust(ModContent.GetTexture("SOTS/NPCs/Boss/Curse/CurseHookMask"), projectile.Center, 0.75f, curse.foamParticleList1, 0.2f, 3, 25, distanceToOwner.ToRotation() + MathHelper.ToRadians(90), draggingType ? 2.5f : 1);
				}
				else
                {
					projectile.Kill();
					OwnerPos = Vector2.Zero;
                }
            }
		}
	}
}
		