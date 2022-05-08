using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Earthen
{
	public class EarthenPlatingBlastDoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Plating Blast Door");
			Tooltip.SetDefault("Cannot be opened by NPCs");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.width = 16;
			Item.height = 36;
			Item.createTile = ModContent.TileType<EarthenPlatingBlastDoorTileClosed>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingBlastDoorTileClosed : BlastDoorClosed<EarthenPlatingBlastDoor, EarthenPlatingBlastDoorTileOpen>
	{
		public override string GetName()
		{
			return "Earthen Plating Blast Door";
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
	}
	public class EarthenPlatingBlastDoorTileOpen : BlastDoorOpen<EarthenPlatingBlastDoor, EarthenPlatingBlastDoorTileClosed>
	{
		public override string GetName()
		{
			return "Earthen Plating Blast Door";
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
	}
}