using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Furniture.Permafrost
{
	public class PermafrostPlatingBlastDoor : ModItem
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
			Item.width = 12;
			Item.height = 32;
			Item.createTile = ModContent.TileType<PermafrostPlatingBlastDoorTileClosed>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PermafrostPlating>(), 6).AddTile(TileID.Anvils).Register();
		}
	}
	public class PermafrostPlatingBlastDoorTileClosed : BlastDoorClosed
	{
		public override int DoorItemID => ModContent.ItemType<Permafrost.PermafrostPlatingBlastDoor>();
		public override int OpenDoorTile => ModContent.TileType<Permafrost.PermafrostPlatingBlastDoorTileOpen>();
		public override string GetName()
        {
			return Language.GetTextValue("Mods.SOTS.ItemName.PermafrostPlatingBlastDoor");
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
	}
	public class PermafrostPlatingBlastDoorTileOpen : BlastDoorOpen
	{
		public override int DoorItemID => ModContent.ItemType<Permafrost.PermafrostPlatingBlastDoor>();
		public override int ClosedDoorTile => ModContent.TileType<Permafrost.PermafrostPlatingBlastDoorTileClosed>();
		public override string GetName()
		{
			return Language.GetTextValue("Mods.SOTS.ItemName.PermafrostPlatingBlastDoor");
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
	}
}