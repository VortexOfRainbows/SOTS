using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class EvilConstruct : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evil Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 3000;  
			npc.damage = 40; 
			npc.defense = 14;  
			npc.knockBackResist = 0.1f;
			npc.width = 86;
			npc.height = 82;
			Main.npcFrameCount[npc.type] = 1;  
			npc.value = Item.buyPrice(0, 5, 0, 0);
			npc.npcSlots = 4f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.netAlways = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.type == ModContent.ProjectileType<EvilArm>() && proj.active && (int)proj.ai[0] == npc.whoAmI)
				{
					Vector2 toNPC = npc.Center - proj.Center;
					Draw(proj.Center + toNPC.SafeNormalize * 16);
				}
			}
			float dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			npc.rotation = dir + (npc.spriteDirection - 1) * 0.5f * MathHelper.ToRadians(180);
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), new Rectangle(0, npc.frame.Y, npc.width, npc.height), lightColor * ((255 - npc.alpha) / 255f), npc.rotation, drawOrigin, npc.scale * 0.95f, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public void Draw(Vector2 to, bool gore = false)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilDrillArm");
			Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilDrill");
			Vector2 position = to;
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float height = (float)texture.Height;
			Vector2 betweenPos = npc.Center - position;
			float rotation = betweenPos.ToRotation() - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(betweenPos.X) && float.IsNaN(betweenPos.Y))
				flag = false;
			bool flag2 = false;
			bool first = true;
			while (flag)
			{
				if ((double)betweenPos.Length() - texture.Height < 2.0)
				{
					flag = false;
				}
				else
				{
					float length = height;
					if (flag2)
					{
						Vector2 vector2_1 = betweenPos;
						vector2_1.Normalize();
						position += vector2_1 * height;
						betweenPos = npc.Center - position;
					}
					else
					{
						Vector2 vector2_1 = betweenPos;
						vector2_1.Normalize();
						position += vector2_1 * height * 0.5f;
						betweenPos = npc.Center - position;
					}
					if (!gore)
					{
						Color color2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y / 16.0));
						color2 = npc.GetAlpha(color2);
						if(first)
							Main.spriteBatch.Draw(texture2, position - Main.screenPosition, null, color2, rotation, texture2.Size()/2, 1.1f, SpriteEffects.None, 0.0f);
						else
							Main.spriteBatch.Draw(texture, position - Main.screenPosition, new Rectangle(0, 0, texture.Width, (int)length), color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
						first = false;
					}
					else
					{
						Gore.NewGore(position, Vector2.Zero, mod.GetGoreSlot("Gores/EvilConstruct/EvilDrillArmGore" + Main.rand.Next(1, 3)), 1f);
					}
					flag2 = true;
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/Constructs/EvilConstructGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), new Rectangle(0, npc.frame.Y, npc.width, npc.height), color * ((255 - npc.alpha) / 255f), npc.rotation, drawOrigin, npc.scale * 0.95f, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lead, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore6"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
		Vector2 aimTo = new Vector2(-1, -1);
		bool runOnce = true;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			Vector2 toPlayer = player.Center - npc.Center;
			int dmg2 = npc.damage / 2;
			if (Main.expertMode)
				dmg2 /= 2;
			int amt = 8;
			if (runOnce)
            {
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < amt; i++)
					{
						Vector2 center = npc.Center;
						Vector2 circular = new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(22.5f + i * 360f / amt));
						for (int j = 0; j < 60; j++)
						{
							int i2 = (int)center.X / 16;
							int j2 = (int)center.Y / 16;
							center += circular;
							if (SOTSWorldgenHelper.TrueTileSolid(i2, j2))
							{
								Projectile.NewProjectile(new Vector2(i2, j2) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<EvilArm>(), dmg2, 0, Main.myPlayer, npc.whoAmI);
								break;
							}
							else if (j == 59)
							{
								Projectile.NewProjectile(npc.Center + circular * 5, Vector2.Zero, ModContent.ProjectileType<EvilArm>(), dmg2, 0, Main.myPlayer, npc.whoAmI);
							}
						}
					}
				}
				runOnce = false;
				return false;
			}
			Vector2 targetPosition = npc.Center;
			int count = 1;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.type == ModContent.ProjectileType<EvilArm>() && proj.active && (int)proj.ai[0] == npc.whoAmI)
				{
					EvilArm arm = proj.modProjectile as EvilArm;
					if (arm.stuck)
					{
						targetPosition += proj.Center;
					}
					else
					{
						Vector2 circular = new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(22.5f + i * 360f / amt));
						arm.MoveTowards(npc.Center + circular * 5, 7f);
						Vector2 target = proj.Center * 2 - npc.Center;
						targetPosition += target;
					}
					arm.Update(npc.Center);
					count++;
				}
			}
			if(toPlayer.Length() < 1000)
            {
				targetPosition += player.Center;
				count++;
            }
			targetPosition /= (float)(count);
			npc.Center = Vector2.Lerp(npc.Center, targetPosition, 0.08f);
			npc.TargetClosest(true);
			aimTo = player.Center;
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.4f / 155f, (255 - npc.alpha) * 0.1f / 155f, (255 - npc.alpha) * 0.1f / 155f);
			if(npc.ai[0] < 0)
			{
				npc.velocity *= 0.98f;
				return;
			}
		}
		public override void PostAI()
		{

		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EvilSpirit>());	
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].localAI[1] = -1;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ModContent.ItemType<FragmentOfEvil>(), Main.rand.Next(4) + 4);
		}	
	}
	public class EvilArm : ModProjectile
    {
		public override string Texture => "SOTS/NPCs/Constructs/EvilDrillArm";
        public override void SetDefaults()
        {
			projectile.aiStyle = -1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.hide = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
        }
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Evil Construct Arm");
		}
		public bool stuck = false;
		public void MoveTowards(Vector2 goTo, float speed)
		{
			projectile.velocity *= 0.9f;
			Vector2 dirVector = goTo - projectile.Center;
			float length = dirVector.Length();
			if (length < speed)
			{
				projectile.velocity *= 0.5f;
				speed = length;
			}
			projectile.velocity += dirVector.SafeNormalize(Vector2.Zero) * speed;
        }
		public void Update(Vector2 center)
        {
			projectile.timeLeft = 6;
			if (runOnce)
				return;
			Vector2 fromNPC = center - projectile.Center;
			projectile.velocity *= 0.97f;
			projectile.Center += projectile.velocity;
        }
		bool runOnce = true;
        public override bool PreAI()
        {
			if(runOnce)
            {
				int i = (int)projectile.Center.X / 16;
				int j = (int)projectile.Center.Y / 16;
				if (SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
					stuck = true;
                }
				runOnce = false;
            }
            return base.PreAI();
        }
        public override void AI()
        {
            base.AI();
        }
    }
}