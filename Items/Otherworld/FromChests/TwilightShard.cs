using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class TwilightShard : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Plate");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
            item.width = 36;     
            item.height = 26;
            item.value = Item.sellPrice(0, 0, 4, 0);
            item.rare = ItemRarityID.Cyan;
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
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = ItemRarityID.Cyan;
			item.placeStyle = 3;
			item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TwilightShard>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TwilightGel>(), 20);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(ItemID.FallenStar, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(ItemID.FallenStar, 1);
			recipe.AddRecipe();
		}
	}
	public class HardlightAlloy : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = ItemRarityID.Cyan;
			item.placeStyle = 1;
			item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TwilightGel>(), 20);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TwilightShard>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarlightAlloy>(), 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(ModContent.ItemType<TwilightGel>(), 20);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(ModContent.ItemType<TwilightGel>(), 20);
			recipe.AddRecipe();
		}
	}
	public class OtherworldlyAlloy : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Alloy");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = ItemRarityID.Cyan;
			item.placeStyle = 5;
			item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TwilightShard>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarlightAlloy>(), 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TwilightGel>(), 20);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(ItemID.MeteoriteBar, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(ModContent.TileType<TransmutationAltarTile>());
			recipe.SetResult(ItemID.MeteoriteBar, 1);
			recipe.AddRecipe();
		}
	}
}