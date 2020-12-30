using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class TwilightShard : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Shard");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
            item.width = 14;     
            item.height = 34;
            item.value = Item.sellPrice(0, 0, 4, 0);
            item.rare = 9;
		}
	}
	public class StarlightAlloy : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/StarlightAlloyGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = 9;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "FragmentOfOtherworld", 1);
			recipe.AddIngredient(null, "TwilightShard", 1);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HardlightAlloy", 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TwilightGel"), 20);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(ItemID.FallenStar, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(ItemID.FallenStar, 1);
			recipe.AddRecipe();
		}
	}
	public class HardlightAlloy : ModItem
	{
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = 9;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TwilightGel", 20);
			recipe.AddIngredient(null, "FragmentOfOtherworld", 1);
			recipe.AddIngredient(null, "TwilightShard", 1);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarlightAlloy", 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(mod.ItemType("TwilightGel"), 20);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(mod.ItemType("TwilightGel"), 20);
			recipe.AddRecipe();
		}
	}
	public class OtherworldlyAlloy : ModItem
	{
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = 9;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddIngredient(null, "FragmentOfOtherworld", 1);
			recipe.AddIngredient(null, "TwilightShard", 1);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HardlightAlloy", 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarlightAlloy", 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TwilightGel"), 20);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(ItemID.MeteoriteBar, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(ItemID.MeteoriteBar, 1);
			recipe.AddRecipe();
		}
	}
}