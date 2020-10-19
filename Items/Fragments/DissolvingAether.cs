using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
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
			Color color = new Color(167, 45, 225, 0);
			Color color2 = new Color(64, 178, 172, 0);
			color = Color.Lerp(color, color2, 0.5f + new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians((float)Main.GlobalTime * 50)).X);
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
			if(frameCounter >= 6)
			{
				frameCounter = 0;
				frame++;
			}
			if(frame >= 8)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(167, 45, 225, 0);
			Color color2 = new Color(64, 178, 172, 0);
			color = Color.Lerp(color, color2, 0.5f + new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians((float)Main.GlobalTime * 50)).X);
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
	public class AetherPlayer : ModPlayer
	{
		public int aetherNum = 0;
		public override void ResetEffects()
		{
			if(aetherNum == 0)
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
			if(player.gravity > projectedGravity)
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