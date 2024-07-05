using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Nature
{
	public class NaturePlatingTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
			ItemID.Sets.Torches[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Torch);
			Item.Size = new Vector2(14, 14);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<NaturePlatingTorchTile>();
		}
		public override void HoldItem(Player player)
		{
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, SOTSTile.NaturePlatingLight * 2.7f);
		}
		public override void PostUpdate()
		{
			if (!Item.wet)
			{
				Lighting.AddLight(new Vector2((Item.position.X + Item.width / 2) / 16f, (Item.position.Y + Item.height / 2) / 16f), SOTSTile.EarthenPlatingLight * 2.7f);
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.Torch, 3).AddIngredient(ModContent.ItemType<NaturePlating>()).Register();
		}
	}
	public class NaturePlatingTorchTile : ModTile
	{
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, DustID.RainbowMk2);
			dust.color = new Color(SOTSTile.NaturePlatingLight);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = false;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(SOTSTile.NaturePlatingLight * 3), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.GoldCoin;
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Torches };
			TileID.Sets.Torch[Type] = true;
		}
        public override bool CanPlace(int i, int j)
        {
			return Main.tile[i, j].LiquidAmount == 0;
        }
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Vector3 color = SOTSTile.NaturePlatingLight * 2.5f;
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX < 66)
			{
				r = color.X;
				g = color.Y;
				b = color.Z;
			}
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Color color = new Color(100, 100, 100, 0);
			int frameX = Main.tile[i, j].TileFrameX;
			int frameY = Main.tile[i, j].TileFrameY;
			int width = 20;
			int height = 20;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 drawPosition = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) - (width - 16f) / 2f, (float)(j * 16 - (int)Main.screenPosition.Y)) + zero;
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow")), drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.25f), new Rectangle(frameX, frameY, width, height), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}