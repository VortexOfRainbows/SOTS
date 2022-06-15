using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Nature
{
	public class NaturePlatingBlastDoor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Plating Blast Door");
			Tooltip.SetDefault("Cannot be opened by NPCs");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.width = 16;
			Item.height = 32;
			Item.createTile = ModContent.TileType<NaturePlatingBlastDoorTileClosed>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NaturePlating>(), 6).AddTile(TileID.Anvils).Register();
		}
	}
	public class NaturePlatingBlastDoorTileClosed : BlastDoorClosed<NaturePlatingBlastDoor, NaturePlatingBlastDoorTileOpen>
	{
        public override string GetName()
        {
			return "Nature Plating Blast Door";
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
	}
	public class NaturePlatingBlastDoorTileOpen : BlastDoorOpen<NaturePlatingBlastDoor, NaturePlatingBlastDoorTileClosed>
	{
		public override string GetName()
		{
			return "Nature Plating Blast Door";
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
	}
}