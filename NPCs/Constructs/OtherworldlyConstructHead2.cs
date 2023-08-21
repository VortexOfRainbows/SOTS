using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Otherworld;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class OtherworldlyConstructHead2 : ModNPC
	{
		int ai1 = 0;
		float dir = 0f;
        public override void SetStaticDefaults()
		{
			NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle =0;
			NPC.lifeMax = 750;  
			NPC.damage = 45; 
			NPC.defense = 16;  
			NPC.knockBackResist = 0.1f;
			NPC.width = 72;
			NPC.height = 74;
			NPC.value = 12550;
			NPC.npcSlots = 0f;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = false;
			NPC.netAlways = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			dir = (float)Math.Atan2(aimTo.Y - NPC.Center.Y, aimTo.X - NPC.Center.X);
			NPC.rotation = dir + (NPC.spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180);
			if (hookTile.X < 0 || hookTile.Y < 0)
				return true;
			Draw(spriteBatch, screenPos, true, drawColor);
			return true;
		}
		public void Draw(SpriteBatch spriteBatch, Vector2 screenPos, bool drawTexture, Color drawColor)
		{
			float npcRadians = NPC.rotation + (NPC.spriteDirection + 1) * 0.5f * MathHelper.ToRadians(180);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/OtherworldVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 npcPos = new Vector2(NPC.Center.X, NPC.Center.Y) + new Vector2(NPC.width / 2, 0).RotatedBy(npcRadians);
			Vector2 distanceToOwner = npcPos - hookTile;
			Vector2 centerOfCircle = hookTile + distanceToOwner / 4;
			float startingRadians = distanceToOwner.ToRotation();
			float radius = distanceToOwner.Length() / 4;
			Vector2 vector2_1 = new Vector2(-1, 0).RotatedBy(MathHelper.ToRadians(0.3f * ai1));
			int max = 16;
			for (int i = max; i > 0; i--)
			{
				float minDist = 40;
				float maxDist = 150;
				float dist = maxDist - distanceToOwner.Length() / 4;
				if (dist < minDist) dist = minDist;
				if (dist > maxDist) dist = maxDist;
				Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(180 / max * i));
				rotationPos.Y *= dist / Math.Abs(radius); //will make the max length always dist
				rotationPos.Y *= vector2_1.X;
				rotationPos = rotationPos.RotatedBy(startingRadians);
				Vector2 pos = rotationPos + centerOfCircle;
				Vector2 dynamicAddition = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(i * 180 / max + ai1));
				Vector2 drawPos = pos - screenPos;
				if (drawTexture)
				{
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, NPC.GetAlpha(drawColor), MathHelper.ToRadians(180 / max * i - 45) + startingRadians, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
				}
				else
				{
					if(Main.rand.NextBool(2))
					{
						Gore.NewGore(NPC.GetSource_Death(), pos - Vector2.One * texture.Width / 2, NPC.velocity, ModGores.GoreType("Gores/OtherworldVineGore"), 1f);
					}
				}
			}
			centerOfCircle += distanceToOwner / 2;
			for (int i = max; i > 0; i--)
			{
				float minDist = 40;
				float maxDist = 150;
				float dist = maxDist - distanceToOwner.Length() / 4;
				if (dist < minDist) dist = minDist;
				if (dist > maxDist) dist = maxDist;
				Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(180 / max * i));
				rotationPos.Y *= dist / Math.Abs(radius); //will make the max length always dist
				rotationPos.Y *= -vector2_1.X;
				rotationPos = rotationPos.RotatedBy(startingRadians);
				Vector2 pos = rotationPos + centerOfCircle;
				Vector2 dynamicAddition = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(i * 180 / max + ai1 + 180));
				Vector2 drawPos = pos - screenPos;
				if (drawTexture)
				{
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, NPC.GetAlpha(drawColor), MathHelper.ToRadians(180 / max * i - 45) + startingRadians, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
				}
				else
				{
					if (Main.rand.NextBool(2))
					{
						Gore.NewGore(NPC.GetSource_Death(), pos - Vector2.One * texture.Width / 2, NPC.velocity, ModGores.GoreType("Gores/OtherworldVineGore"), 1f);
					}
				}
			}
		}
		bool glow = false;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Constructs/OtherworldlyConstructHead2Glow").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if (glow)
				for (int k = 0; k < 5; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					spriteBatch.Draw(texture, new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y + 2), new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), color * ((255 - NPC.alpha) / 255f), NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
		}
		public override bool CheckActive()
        {
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 82, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore2"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore3"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore4"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore5"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore6"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
				if(Main.netMode != NetmodeID.Server)
                {
					Draw(null, Main.screenPosition, false, Color.White);
				}
			}
		}
		Vector2 hookTile = new Vector2(-1, -1);
		Vector2 aimTo = new Vector2(-1, -1);
		public Vector2 findClosestTile()
		{ 
			float blockDist = 16;
			bool foundTile = false;
			int endnow = 0;

			while(!foundTile)
			{
				endnow++;
				if(endnow > 64)
				{
					break;
				}
				for(float r = 0; r < 180; r += 30f / endnow)
				{
					if (r > 180)
						break;
					Vector2 rotationalDistance = new Vector2(-blockDist, 0).RotatedBy(MathHelper.ToRadians(r));
					Vector2 pos = NPC.Center + rotationalDistance;
					int i = (int)(pos.X / 16);
					int j = (int)(pos.Y / 16);
					Tile tile = Framing.GetTileSafely(i, j);
					if (tile.HasUnactuatedTile && Main.tileSolid[(int)tile.TileType])
					{
						foundTile = true;
						return new Vector2(i * 16, j * 16) + new Vector2(8, 8);
					}
				}
				blockDist += 16f;
			}
			blockDist = 16;
			endnow = 0;
			while (!foundTile)
			{
				endnow++;
				if (endnow > 64)
				{
					break;
				}
				for (float r = 180; r < 360; r += 30f / endnow)
				{
					if (r > 360)
						break;
					Vector2 rotationalDistance = new Vector2(-blockDist, 0).RotatedBy(MathHelper.ToRadians(r));
					Vector2 pos = NPC.Center + rotationalDistance;
					int i = (int)(pos.X / 16);
					int j = (int)(pos.Y / 16);
					Tile tile = Framing.GetTileSafely(i, j);
					if (tile.HasUnactuatedTile && Main.tileSolid[(int)tile.TileType])
					{
						foundTile = true;
						return new Vector2(i * 16, j * 16) + new Vector2(8, 8);
					}
				}
				blockDist += 16f;
			}
			return new Vector2(NPC.Center.X, NPC.Center.Y);
		}
		public override bool PreAI()
		{
			NPC.dontCountMe = true;
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			if((aimTo.X == -1 && aimTo.Y == -1) || (hookTile.X == -1 && hookTile.Y == -1))
			{
				aimTo = NPC.Center;
				if(hookTile.X == -1 && hookTile.Y == -1)
					hookTile = findClosestTile();
				return false;
			}
			aimTo = player.Center;
			if(NPC.ai[2] <= 0 && NPC.ai[3] <= 0)
			{
				NPC.ai[2] = hookTile.X;
				NPC.ai[3] = hookTile.Y;
			}
			return true;
		}
		bool flag = false;
		bool flag2 = false;
		Vector2 rotateVector = new Vector2(12, 0);
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			Vector2 dynamicAddition = new Vector2(0.4f, 0).RotatedBy(MathHelper.ToRadians(ai1));
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.45f / 155f, (255 - NPC.alpha) * 0.25f / 155f, (255 - NPC.alpha) * 0.45f / 155f);
			ai1++;
			if(NPC.ai[0] < 0)
			{
				NPC.velocity *= 0.98f;
				return;
			}
			NPC.velocity = dynamicAddition;
			float dir2 = (float)Math.Atan2(aimTo.Y - NPC.Center.Y, aimTo.X - NPC.Center.X);
			Vector2 distanceToOwner = hookTile - NPC.Center;
			Vector2 distanceToOwner2 = hookTile - NPC.Center;
			Vector2 distanceToTarget = player.Center - NPC.Center;
			Vector2 distanceToTarget2 = player.Center - NPC.Center;

			distanceToTarget.Normalize();
			rotateVector += distanceToTarget * 1;
			rotateVector = new Vector2(12, 0).RotatedBy(rotateVector.ToRotation());
			int maxDistPlayer = 96;
			int maxDistNPC = 512;
			Vector2 added = new Vector2(3f, 0).RotatedBy(rotateVector.ToRotation());
			if (distanceToTarget2.Length() > 800f)
			{
				added = Vector2.Zero;
			}
			Vector2 newLocation = NPC.Center + added + dynamicAddition;
			if((player.Center - newLocation).Length() < maxDistPlayer + 60)
			{
				flag = true;
			}
			if ((player.Center - newLocation).Length() > maxDistPlayer + 110)
			{
				flag = false;
			}
			if(distanceToOwner.Length() < maxDistNPC - 80)
			{
				flag2 = false;
			}
			if ((player.Center - newLocation).Length() < maxDistPlayer)
			{
				NPC.velocity = -0.3f * added + -0.1f * dynamicAddition;
			}
			if(!flag)
			{
				NPC.velocity = added + dynamicAddition;
			}
			if(distanceToOwner.Length() > maxDistNPC || flag2)
			{
				flag2 = true;
				NPC.velocity = distanceToOwner2 * 0.002f + distanceToOwner2.SafeNormalize(new Vector2(0, 1)) * 0.5f + -0.1f * dynamicAddition;
			}

		}
		public override void PostAI()
		{
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			Vector2 playerLoc = player.Center;
			bool lineOfSight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
			if ((toPlayer.Length() > 900 || !lineOfSight) && NPC.ai[0] < 270)
				return;
			if(toPlayer.Length() < 240)
			{
				float rot = toPlayer.ToRotation();
				Vector2 circular = new Vector2(240, 0).RotatedBy(rot);
				playerLoc = circular + NPC.Center;
			}
			NPC.ai[0]++;
			if (NPC.ai[0] >= 270)
			{
				glow = true;
				NPC.velocity *= 0.25f;
				if (NPC.ai[0] % 90 == 0)
				{
					int damage = NPC.GetBaseDamage() / 2;
					NPC.ai[1]++;
					if (NPC.ai[1] < 6)
					{
						float locX = playerLoc.X + Main.rand.Next(-200, 201);
						float locY = playerLoc.Y + Main.rand.Next(-200, 201);
						bool inBlock = true;
						while (inBlock)
						{
							int i = (int)locX / 16;
							int j = (int)locY / 16;
							if (Main.tileSolid[Main.tile[i, j ].TileType] && Main.tile[i, j].HasTile && !Main.tileSolidTop[Main.tile[i, j ].TileType])
							{
								locX = playerLoc.X + Main.rand.Next(-200, 201);
								locY = playerLoc.Y + Main.rand.Next(-200, 201);
								inBlock = true;
							}
							else
							{
								inBlock = false;
								break;
							}
						}
						SOTSUtils.PlaySound(SoundID.Item30, (int)locX, (int)locY, 0.2f);
						if (Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectile(NPC.GetSource_FromAI(), locX, locY, 0, 0, ModContent.ProjectileType<OtherworldlyTracer>(), damage, 0f, Main.myPlayer, 751 - NPC.ai[0], NPC.whoAmI);
					}
				}
				if (NPC.ai[1] >= 6)
				{
					NPC.ai[1] = 0;
					NPC.ai[0] = -90;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.active && proj.type == ModContent.ProjectileType<OtherworldlyTracer>() && proj.ai[1] == NPC.whoAmI)
						{
							int damage = NPC.GetBaseDamage() / 2;
							Vector2 toProj = proj.Center - NPC.Center;
							toProj /= 30f;
							if (Main.netMode != NetmodeID.MultiplayerClient)
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, toProj.X, toProj.Y, ModContent.ProjectileType<OtherworldlyBall>(), damage, 0, Main.myPlayer);
						}
					}
					NPC.velocity = -12 * toPlayer.SafeNormalize(new Vector2(0, 1));
				}
			}
			else
				glow = false;
		}
		public override void OnKill()
		{
			int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<OtherworldlySpirit>());
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].localAI[1] = -1;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfOtherworld>(), 1, 3, 4));
			npcLoot.Add(ItemDropRule.ByCondition(new Common.ItemDropConditions.DownedAdvisorDropCondition(), ModContent.ItemType<TwilightShard>(), 2));
		}
	}
}