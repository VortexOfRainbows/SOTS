using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class HoloBlade : ModNPC
	{
		private float direction
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		bool drawTrail = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holo Blade");
			NPCID.Sets.TrailCacheLength[NPC.type] = 8;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 40;   
            NPC.damage = 45; 
            NPC.defense = 20;  
            NPC.knockBackResist = 0.5f;
            NPC.width = 30;
            NPC.height = 54;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 200;
            NPC.npcSlots = 2f;
            NPC.HitSound = SoundID.NPCHit53;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			Banner = NPC.type;
			BannerItem = ItemType<HoloSwordBanner>();
		}
		Vector2 consistentAcceleration = new Vector2(0, 0);
		Vector2 savePos = new Vector2(0, 0);
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Vector2 toPlayer = NPC.Center - player.Center;
			NPC.ai[1]++;
			float bonus = 0;
			if(NPC.ai[1] >= 120)
			{
				if(savePos.X == 0 && savePos.Y == 0)
				{
					savePos = NPC.Center;
				}
				if(NPC.ai[1] >= 165)
				{
					NPC.ai[1]++;
				}
				Vector2 vector_1 = new Vector2(90, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[1] + NPC.ai[2] - 120) * 2);
				bonus = vector_1.Y;
				Vector2 rotatePos = new Vector2(bonus, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * (NPC.whoAmI % 2 * 2 - 1)));
				Vector2 toPos = rotatePos + savePos;
				Vector2 goToPos = NPC.Center - toPos;
				float length = goToPos.Length() + 0.1f;
				if (length > 12)
				{
					length = 12;
				}
				if (length > 1.1f)
				{
					goToPos.Normalize();
					NPC.velocity = goToPos * -length;
					NPC.rotation = rotatePos.ToRotation() + MathHelper.ToRadians(90);
				}
				else NPC.velocity *= 0f;

				if (NPC.ai[1] >= 210)
				{
					savePos *= 0f;
					NPC.ai[1] = -22;
					rotatePos.Normalize();
					NPC.velocity = rotatePos * -23;
					consistentAcceleration = NPC.velocity;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, NPC.Center);
					drawTrail = true;
					return;
				}
			}
			else if (NPC.ai[1] > 0)
			{
				drawTrail = false;
				if (NPC.ai[0] == 0)
				{
					NPC.ai[0] = Main.rand.Next(360);
				}
				NPC.ai[0]++;
				Vector2 rotatePos = new Vector2(200, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * (NPC.whoAmI % 2 * 2 - 1)));
				Vector2 toPos = rotatePos + player.Center;
				Vector2 goToPos = NPC.Center - toPos;
				float length = goToPos.Length() + 0.1f;
				if (length > 12)
				{
					length = 12;
				}
				if (length > 1.1f)
				{
					goToPos.Normalize();
					NPC.velocity = goToPos * -length;
					NPC.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(90);
				}
				else NPC.velocity *= 0f;
			}
			else
			{
				NPC.velocity = consistentAcceleration;
				NPC.rotation = NPC.velocity.ToRotation() - MathHelper.ToRadians(90);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/HoloBladeOutline").Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("NPCs/HoloBladeFill").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			Color color = new Color(110, 110, 110, 0);
			if (drawTrail)
				for (int i = 0; i < NPC.oldPos.Length; i++)
				{
					for (int k = 0; k < 5; k++)
					{
						Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
						color = color * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
						if (k == 0)
							Main.spriteBatch.Draw(texture4, new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X), (float)(NPC.Center.Y - (int)Main.screenPosition.Y) - 4), null, color * 0.9f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texture2, drawPos, null, color * 0.9f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
					}
				}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/HoloBladeOutline").Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("NPCs/HoloBladeFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;

				if (k == 0)
					Main.spriteBatch.Draw(texture4, new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X), (float)(NPC.Center.Y - (int)Main.screenPosition.Y) - 4), null, color * 0.5f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X) + x, (float)(NPC.Center.Y - (int)Main.screenPosition.Y) + y - 4), null, color * ((255 - NPC.alpha) / 255f), NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 0 && SOTSWorld.downedAdvisor) Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("TwilightShard").Type, 1);
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("TwilightGel").Type, Main.rand.Next(2) + 1);

			if (Main.rand.Next(10) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("FragmentOfOtherworld").Type, Main.rand.Next(2) + 1);

			if (Main.rand.Next(75) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("BladeGenerator").Type, 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 40.0)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, (float)(2 * hitDirection), -2f, 0, default, 0.55f * scale);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, (float)(2 * hitDirection), -2f, 0, default, scale);
				}
			}
		}
	}
}