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
			//Tooltip.SetDefault("Cannot be opened by NPCs");
			this.SetResearchCost(1);
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<EarthenPlating>(), 6).AddTile(TileID.Anvils).Register();
		}
	}
	public class EarthenPlatingBlastDoorTileClosed : BlastDoorClosed
	{
		public override int DoorItemID => ModContent.ItemType<Earthen.EarthenPlatingBlastDoor>();
		public override int OpenDoorTile => ModContent.TileType<Earthen.EarthenPlatingBlastDoorTileOpen>();
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
	public class EarthenPlatingBlastDoorTileOpen : BlastDoorOpen
	{
		public override int DoorItemID => ModContent.ItemType<Earthen.EarthenPlatingBlastDoor>();
		public override int ClosedDoorTile => ModContent.TileType<Earthen.EarthenPlatingBlastDoorTileClosed>();
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