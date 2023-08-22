using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Projectiles.Planetarium;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class OtherworldlyConstructHead : ModNPC
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
			NPC.lifeMax = 300;  
			NPC.damage = 40; 
			NPC.defense = 14;  
			NPC.knockBackResist = 0.1f;
			NPC.width = 72;
			NPC.height = 74; 
			NPC.value = 9550;
			NPC.npcSlots = 4f;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = false;
			NPC.netAlways = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			dir = (float)Math.Atan2(aimTo.Y - NPC.Center.Y, aimTo.X - NPC.Center.X);
			NPC.rotation = dir + (NPC.spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180);
			return true;
		}
		bool glow = false;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Constructs/OtherworldlyConstructHeadGlow").Value;
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
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore1"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore3"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore4"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore5"), 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/OtherworldlyConstructs/OtherworldlyConstructGore6"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
		Vector2 aimTo = new Vector2(-1, -1);
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			if((aimTo.X == -1 && aimTo.Y == -1))
			{
				aimTo = NPC.Center;
				return false;
			}
			aimTo = player.Center;
			return true;
		}
		bool flag = false;
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
			if ((player.Center - newLocation).Length() < maxDistPlayer)
			{
				NPC.velocity = -0.3f * added + -0.1f * dynamicAddition;
			}
			if(!flag)
			{
				NPC.velocity = added + dynamicAddition;
			}
		}
		public override void PostAI()
		{
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			Vector2 playerLoc = player.Center;
			if (toPlayer.Length() > 900 && NPC.ai[0] < 270)
				return;
			if(toPlayer.Length() < 240)
			{
				float rot = toPlayer.ToRotation();
				Vector2 circular = new Vector2(240, 0).RotatedBy(rot);
				playerLoc = circular + NPC.Center;
			}
			NPC.ai[0]++;
			if(NPC.ai[0] >= 270)
			{
				glow = true;
				NPC.velocity *= 0.25f;
				if(NPC.ai[0] % 90 == 0)
				{
					int damage = NPC.GetBaseDamage() / 2;
					NPC.ai[1]++;
					if(NPC.ai[1] < 4)
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
							Projectile.NewProjectile(NPC.GetSource_FromAI(), locX, locY, 0, 0, ModContent.ProjectileType<OtherworldlyTracer>(), damage, 0f, Main.myPlayer, 571 - NPC.ai[0], NPC.whoAmI);
					}
				}
				if(NPC.ai[1] >= 4)
				{
					NPC.ai[1] = 0;
					NPC.ai[0] = -90;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if(proj.active && proj.type == ModContent.ProjectileType<OtherworldlyTracer>() && proj.ai[1] == NPC.whoAmI)
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
			if (Main.netMode != 1)
				Main.npc[n].netUpdate = true;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfOtherworld>(), 1, 4, 7));
			npcLoot.Add(ItemDropRule.ByCondition(new Common.ItemDropConditions.DownedAdvisorDropCondition(), ModContent.ItemType<TwilightShard>()));
		}
	}
}