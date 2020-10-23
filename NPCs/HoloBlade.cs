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
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		bool drawTrail = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holo Blade");
			NPCID.Sets.TrailCacheLength[npc.type] = 8;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 40;   
            npc.damage = 45; 
            npc.defense = 20;  
            npc.knockBackResist = 0.5f;
            npc.width = 26;
            npc.height = 54;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 200;
            npc.npcSlots = 2f;
            npc.HitSound = SoundID.NPCHit53;
            npc.DeathSound = SoundID.NPCDeath14;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.noTileCollide = true;
			npc.noGravity = true;
			//banner = npc.type;
			//bannerItem = ItemType<SittingMushroomBanner>();
		}
		Vector2 consistentAcceleration = new Vector2(0, 0);
		Vector2 savePos = new Vector2(0, 0);
		public override void AI()
		{
			Player player = Main.player[npc.target];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			Vector2 toPlayer = npc.Center - player.Center;
			npc.ai[1]++;
			float bonus = 0;
			if(npc.ai[1] >= 120)
			{
				if(savePos.X == 0 && savePos.Y == 0)
				{
					savePos = npc.Center;
				}
				if(npc.ai[1] >= 165)
				{
					npc.ai[1]++;
				}
				Vector2 vector_1 = new Vector2(90, 0).RotatedBy(MathHelper.ToRadians(npc.ai[1] + npc.ai[2] - 120) * 2);
				bonus = vector_1.Y;
				Vector2 rotatePos = new Vector2(bonus, 0).RotatedBy(MathHelper.ToRadians(npc.ai[0] * (npc.whoAmI % 2 * 2 - 1)));
				Vector2 toPos = rotatePos + savePos;
				Vector2 goToPos = npc.Center - toPos;
				float length = goToPos.Length() + 0.1f;
				if (length > 12)
				{
					length = 12;
				}
				if (length > 1.1f)
				{
					goToPos.Normalize();
					npc.velocity = goToPos * -length;
					npc.rotation = rotatePos.ToRotation() + MathHelper.ToRadians(90);
				}
				else npc.velocity *= 0f;

				if (npc.ai[1] >= 210)
				{
					savePos *= 0f;
					npc.ai[1] = -22;
					rotatePos.Normalize();
					npc.velocity = rotatePos * -23;
					consistentAcceleration = npc.velocity;
					Main.PlaySound(SoundID.Item71, npc.Center);
					drawTrail = true;
					return;
				}
			}
			else if (npc.ai[1] > 0)
			{
				drawTrail = false;
				if (npc.ai[0] == 0)
				{
					npc.ai[0] = Main.rand.Next(360);
				}
				npc.ai[0]++;
				Vector2 rotatePos = new Vector2(200, 0).RotatedBy(MathHelper.ToRadians(npc.ai[0] * (npc.whoAmI % 2 * 2 - 1)));
				Vector2 toPos = rotatePos + player.Center;
				Vector2 goToPos = npc.Center - toPos;
				float length = goToPos.Length() + 0.1f;
				if (length > 12)
				{
					length = 12;
				}
				if (length > 1.1f)
				{
					goToPos.Normalize();
					npc.velocity = goToPos * -length;
					npc.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(90);
				}
				else npc.velocity *= 0f;
			}
			else
			{
				npc.velocity = consistentAcceleration;
				npc.rotation = npc.velocity.ToRotation() - MathHelper.ToRadians(90);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Texture2D texture2 = mod.GetTexture("NPCs/HoloBladeOutline");
			Texture2D texture4 = mod.GetTexture("NPCs/HoloBladeFill");
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			Color color = new Color(110, 110, 110, 0);
			if (drawTrail)
				for (int i = 0; i < npc.oldPos.Length; i++)
				{
					for (int k = 0; k < 5; k++)
					{
						Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
						color = color * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
						if (k == 0)
							Main.spriteBatch.Draw(texture4, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) - 4), null, color * 0.9f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texture2, drawPos, null, color * 0.9f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
					}
				}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/HoloBladeOutline");
			Texture2D texture4 = mod.GetTexture("NPCs/HoloBladeFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;

				if (k == 0)
					Main.spriteBatch.Draw(texture4, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) - 4), null, color * 0.5f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y - 4), null, color * ((255 - npc.alpha) / 255f), npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 0 && SOTSWorld.downedAdvisor) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightShard"), 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightGel"), Main.rand.Next(2) + 1);

			if (Main.rand.Next(10) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfOtherworld"), Main.rand.Next(2) + 1);

			if (Main.rand.Next(75) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BladeGenerator"), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 40.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, (float)(2 * hitDirection), -2f, 0, default, 0.5f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, (float)(2 * hitDirection), -2f, 0, default, 1f);
				}
			}
		}
	}
}