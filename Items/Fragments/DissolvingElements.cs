using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;

namespace SOTS.Items.Fragments
{
	public class DissolvingNature : ModItem
	{
		int frameCounter;
		int frame;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Nature");
			Tooltip.SetDefault("Reduces damage dealt by 10% while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 6));
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, 42 * this.frame, 26, 42), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 6)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 42 * frame, 26, 42), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 42;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 6)
			{
				frame = 0;
			}
			for (int i = 0; i < item.stack; i++)
			{
				if (player.allDamage > 0f)
				{
					player.allDamage -= 0.1f;
				}
				else
				{
					player.allDamage = 0;
				}
			}
		}
	}
	public class DissolvingEarth : ModItem
	{
		int frameCounter;
		int frame;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Earth");
			Tooltip.SetDefault("Reduces endurance by 10% while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, 42 * this.frame, 28, 42), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 42 * frame, 28, 42), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 42;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}
			for (int i = 0; i < item.stack; i++)
			{
				if (player.endurance > -1f)
				{
					player.endurance -= 0.1f;
				}
				else
				{
					player.endurance = -1;
				}
			}
		}
	}
	public class DissolvingAurora : ModItem
	{
		int frameCounter = 0;
		int frame = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Aurora");
			Tooltip.SetDefault("Reduces movespeed by 20% while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 5));
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 38;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, 38 * this.frame, 34, 38), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 8)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 5)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 38 * frame, 34, 38), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 8)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 5)
			{
				frame = 0;
			}

			for (int i = 0; i < item.stack; i++)
			{
				if (player.moveSpeed > 0f)
				{
					player.moveSpeed -= 0.2f;
				}
				else
				{
					player.moveSpeed = 0;
				}
			}
		}
	}
	public class DissolvingDeluge : ModItem
	{
		int frameCounter;
		int frame;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Deluge");
			Tooltip.SetDefault("Decreases max life and mana by 10 while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 12));
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, item.height * this.frame, item.width, item.height), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 12)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, item.height * frame, item.width, item.height), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 38;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 12)
			{
				frame = 0;
			}
			for (int i = 0; i < item.stack; i++)
			{
				if (player.statLifeMax2 > 100)
				{
					player.statLifeMax2 -= 10;
				}
				if (player.statManaMax2 > 40)
				{
					player.statManaMax2 -= 10;
				}
			}
		}
	}
	public class DissolvingAether : ModItem
	{
		int frameCounter = 0;
		int frame = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Aether");
			Tooltip.SetDefault("Reduces gravity while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 46;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = VoidPlayer.OtherworldColor;
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, 48 * this.frame, 34, 46), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}
			Texture2D texture = Main.itemTexture[item.type];
			Color color = VoidPlayer.OtherworldColor;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 48 * frame, 34, 46), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}
			AetherPlayer aetherPlayer = (AetherPlayer)player.GetModPlayer(mod, "AetherPlayer");
			aetherPlayer.aetherNum += item.stack;
		}
	}
	public class DissolvingUmbra : ModItem
	{
		int frameCounter = 0;
		int frame = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Umbra");
			Tooltip.SetDefault("Reduces max void by 20 while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 10));
		}
		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 48;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = VoidPlayer.EvilColor;
			for (int k = 0; k < 7; k++)
			{
				Main.spriteBatch.Draw(texture,
				position + Main.rand.NextVector2Circular(1.5f, 1.5f),
				new Rectangle(0, item.height * this.frame, item.width, item.height), color * 1.2f * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 10)
			{
				frame = 0;
			}
			Texture2D texture = Main.itemTexture[item.type];
			Color color = VoidPlayer.EvilColor;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Main.spriteBatch.Draw(texture,
				item.Center - Main.screenPosition + Main.rand.NextVector2Circular(1.5f, 1.5f),
				new Rectangle(0, item.height * frame, item.width, item.height), color * 1.2f * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 10)
			{
				frame = 0;
			}
			for (int i = 0; i < item.stack; i++)
			{
				if (vPlayer.voidMeterMax2 > 20)
				{
					vPlayer.voidMeterMax2 -= 20;
				}
				if(vPlayer.voidMeterMax2 < 20)
                {
					vPlayer.voidMeterMax2 = 20;
					break;
				}
			}
		}
	}
	public class DissolvingNether : ModItem
	{
		int frameCounter;
		int frame;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Nether");
			Tooltip.SetDefault("Decreases life regeneration by 2 while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 8));
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, 40 * this.frame, 42, 40), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 40 * frame, 42, 40), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 40;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}
			AetherPlayer aetherPlayer = (AetherPlayer)player.GetModPlayer(mod, "AetherPlayer");
			aetherPlayer.infernoNum++;
		}
	}
	public class DissolvingBrilliance : ModItem
	{
		int frameCounter;
		int frame;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Brilliance");
			//Tooltip.SetDefault("");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 8));
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, 66 * this.frame, 66, 66), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 66 * frame, 66, 66), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			item.width = 66;
			item.height = 66;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 8)
			{
				frame = 0;
			}
		}
	}
	public class AetherPlayer : ModPlayer
	{
		public int aetherNum = 0;
		public int infernoNum = 0;
        public override void UpdateBadLifeRegen()
        {
			if (infernoNum > 10)
				infernoNum = 10;
			player.lifeRegen -= infernoNum * 2;
			infernoNum = 0;
		}
        public override void ResetEffects()
		{
			if (aetherNum == 0)
			{
				return;
			}
			float projectedGravity = player.gravity;
			float projectedFallSpeed = player.maxFallSpeed;
			float projectedJumpSpeedBoost = player.jumpSpeedBoost;
			float mult = 1f - 1f / (0.3f * aetherNum + 1); //around 0.3f at 1
			float mult2 = 1f - 1f / (0.3f * aetherNum + 1); //around 0.3f at 1
			projectedGravity -= 1f * mult;
			projectedFallSpeed -= 10f * mult2;
			projectedJumpSpeedBoost += 5f * mult;
			if (projectedJumpSpeedBoost > 5)
			{
				projectedJumpSpeedBoost = 5;
			}
			if (projectedGravity < 0.125f)
			{
				projectedGravity = 0.125f;
			}
			if (projectedFallSpeed < 1.75f)
			{
				projectedFallSpeed = 1.75f;
			}
			if (player.gravity > projectedGravity)
				player.gravity = projectedGravity;

			if (player.maxFallSpeed > projectedFallSpeed)
				player.maxFallSpeed = projectedFallSpeed;

			if (player.jumpSpeedBoost < projectedJumpSpeedBoost)
				player.jumpSpeedBoost = projectedJumpSpeedBoost;
			if (aetherNum >= 4)
			{
				player.noFallDmg = true;
			}
			aetherNum = 0;
		}
	}
}