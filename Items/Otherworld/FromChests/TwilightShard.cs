using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.Furniture;
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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
            Item.width = 36;     
            Item.height = 26;
            Item.value = Item.sellPrice(0, 0, 4, 0);
            Item.rare = ItemRarityID.Cyan;
		}
	}
	public class StarlightAlloy : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarlightAlloyGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 45, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.placeStyle = 3;
			Item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.FallenStar, 1).AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 1).AddIngredient(ModContent.ItemType<TwilightShard>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TwilightGel>(), 20).AddTile(ModContent.TileType<TransmutationAltarTile>()).ReplaceResult(ItemID.FallenStar);
			CreateRecipe(1).AddIngredient(ItemID.MeteoriteBar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).ReplaceResult(ItemID.FallenStar);
		}
	}
	public class HardlightAlloy : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 45, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.placeStyle = 1;
			Item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TwilightGel>(), 20).AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 1).AddIngredient(ModContent.ItemType<TwilightShard>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			CreateRecipe(20).AddIngredient(ItemID.FallenStar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).ReplaceResult(ModContent.ItemType<TwilightGel>());
			CreateRecipe(20).AddIngredient(ItemID.MeteoriteBar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).ReplaceResult(ModContent.ItemType<TwilightGel>());
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
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 45, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.placeStyle = 5;
			Item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.MeteoriteBar, 1).AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 1).AddIngredient(ModContent.ItemType<TwilightShard>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TwilightGel>(), 20).AddTile(ModContent.TileType<TransmutationAltarTile>()).ReplaceResult(ItemID.MeteoriteBar);
			CreateRecipe(1).AddIngredient(ItemID.FallenStar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).ReplaceResult(ItemID.MeteoriteBar);
		}
	}
}