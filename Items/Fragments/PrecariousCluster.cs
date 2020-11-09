using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class PrecariousCluster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primordial Cluster");
			Tooltip.SetDefault("Reduces damage dealt by 10%, endurance by 10%, movespeed by 20%, and gravity while in the inventory\n'A great gift for your friend's inventory!'");
		}
		public override void SetDefaults()
		{
			item.width = 66;
			item.height = 66;
            item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 3;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.AddIngredient(null, "DissolvingEarth", 1);
			recipe.AddIngredient(null, "DissolvingAurora", 1);
			recipe.AddIngredient(null, "DissolvingNature", 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Fragments/PrecariousClusterSymbols");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f * 0.5f);
			position += new Vector2(33 * scale, 33 * scale);
			float counter = Main.GlobalTime * 160;
			int bonus = (int)(counter / 360f);
			float mult = new Vector2(-11f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 3; i++)
			{
				int frameNum = (i + bonus) % 4;
				Rectangle frame = new Rectangle(0, 22 * frameNum, 22, 22);
				Vector2 rotationAround = new Vector2((11 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(120 * i + counter));
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(position.X + x), (float)(position.Y + y)) + rotationAround, frame, new Color(100, 100, 100, 0) * (1f - (item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
			for (int k = 0; k < 7; k++)
			{
				int frameNum = (3 + bonus) % 4;
				Rectangle frame = new Rectangle(0, 22 * frameNum, 22, 22);
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(position.X + x), (float)(position.Y + y)), frame, new Color(100, 100, 100, 0) * (1f - (item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Fragments/PrecariousClusterSymbols");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f * 0.5f);
			float counter = Main.GlobalTime * 160;
			int bonus = (int)(counter / 360f);
			float mult = new Vector2(-11f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 3; i++)
			{
				int frameNum = (i + bonus) % 4;
				Rectangle frame = new Rectangle(0, 22 * frameNum, 22, 22);
				Vector2 rotationAround = new Vector2((11 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(120 * i + counter));
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y) + rotationAround, frame, new Color(100, 100, 100, 0) * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
			for (int k = 0; k < 7; k++)
			{
				int frameNum = (3 + bonus) % 4;
				Rectangle frame = new Rectangle(0, 22 * frameNum, 22, 22);
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y), frame, new Color(100, 100, 100, 0) * (1f - (item.alpha / 255f)), 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			AetherPlayer aetherPlayer = (AetherPlayer)player.GetModPlayer(mod, "AetherPlayer");
			aetherPlayer.aetherNum += item.stack;
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
				if (player.moveSpeed > 0f)
				{
					player.moveSpeed -= 0.2f;
				}
				else
				{
					player.moveSpeed = 0;
				}
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
}