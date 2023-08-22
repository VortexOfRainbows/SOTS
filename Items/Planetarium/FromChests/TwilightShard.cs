using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium.FromChests
{
	public class TwilightShard : ModItem	
	{	
		public override void SetStaticDefaults()
		{
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
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/StarlightAlloyGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
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
			Recipe.Create(ItemID.FallenStar).AddIngredient(ModContent.ItemType<TwilightGel>(), 20).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			Recipe.Create(ItemID.FallenStar).AddIngredient(ItemID.MeteoriteBar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
		}
	}
	public class HardlightAlloy : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
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
			Recipe.Create(ModContent.ItemType<TwilightGel>(), 20).AddIngredient(ItemID.FallenStar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			Recipe.Create(ModContent.ItemType<TwilightGel>(), 20).AddIngredient(ItemID.MeteoriteBar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
		}
	}
	public class OtherworldlyAlloy : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			Recipe.Create(ItemID.MeteoriteBar).AddIngredient(ModContent.ItemType<TwilightGel>(), 20).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
			Recipe.Create(ItemID.MeteoriteBar).AddIngredient(ItemID.FallenStar, 1).AddTile(ModContent.TileType<TransmutationAltarTile>()).Register();
		}
	}
}