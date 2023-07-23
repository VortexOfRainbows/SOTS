using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;

namespace SOTS.Items.Pyramid
{
	public class TaintedKeystoneShard : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 26;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<TaintedKeystoneShardTile>();
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, 10 / 255f, 10 / 255f, 10 / 255f);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/TaintedKeystoneShard").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			position += drawOrigin * scale;
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(50, 255, 255, 0);
						break;
					case 1:
						color = new Color(255, 50, 255, 0);
						break;
					case 2:
						color = new Color(255, 255, 50, 0);
						break;
					case 3:
						color = new Color(100, 255, 255, 0);
						break;
					case 4:
						color = new Color(255, 100, 255, 0);
						break;
					case 5:
						color = new Color(255, 255, 100, 0);
						break;
				}
				Vector2 rotationAround = new Vector2((3 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y) + rotationAround, null, color, 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/TaintedKeystoneShard").Value;
			Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, Color.Lerp(drawColor, Color.Black, 0.7f), 0f, drawOrigin, scale * 1.0f, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Pyramid/TaintedKeystoneShard").Value;
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-2.5f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(50, 255, 255, 0);
						break;
					case 1:
						color = new Color(255, 50, 255, 0);
						break;
					case 2:
						color = new Color(255, 255, 50, 0);
						break;
					case 3:
						color = new Color(100, 255, 255, 0);
						break;
					case 4:
						color = new Color(255, 100, 255, 0);
						break;
					case 5:
						color = new Color(255, 255, 100, 0);
						break;
				}
				Vector2 rotationAround2 = 0.5f * new Vector2((6 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture2, rotationAround2 + Item.Center - Main.screenPosition, null, color, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture2 = Mod.Assets.Request<Texture2D>("Items/Pyramid/TaintedKeystoneShard").Value;
			Main.spriteBatch.Draw(texture2, Item.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.Black, 0.7f), rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			return false;
		}
		/*public override void AddRecipes()
		{
			Recipe.Create(ModContent.ItemType<RoyalRubyShard>()).AddIngredient(this, 1).AddIngredient(ModContent.ItemType<PrecariousCluster>(), 1).AddTile(TileID.DemonAltar).Register();
		}*/
	}
	public class TaintedKeystoneShardTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			ItemDrop = ModContent.ItemType<TaintedKeystoneShard>();
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(24, 24, 24), name);
			HitSound = SoundID.Item27;
			DustType = 195;
		}
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			bool canLive = ModifyFrames(i, j);
			if(!canLive)
            {
				WorldGen.KillTile(i, j);
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendData(17, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				}
			}
            return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.5f;
			g = 0.0375f;
			b = 0.125f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Tile[tile.TileType].Value;
			Vector2 drawOffSet = Vector2.Zero;
			if(tile.TileFrameY == 0) //below is active
				drawOffSet.Y += 2;
			if (tile.TileFrameY == 18) //above is active
				drawOffSet.Y -= 2;
			if (tile.TileFrameY == 36) //right is active
				drawOffSet.X += 2;
			if (tile.TileFrameY == 54) //left is active
				drawOffSet.X -= 2;
			Vector2 location = new Vector2(i * 16, j * 16) + drawOffSet;
			Color color2 = Lighting.GetColor(i, j, WorldGen.paintColor(tile.TileColor));
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			for (int k = 0; k < 6; k++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (k)
				{
					case 0:
						color = new Color(50, 255, 255, 0);
						break;
					case 1:
						color = new Color(255, 50, 255, 0);
						break;
					case 2:
						color = new Color(255, 255, 50, 0);
						break;
					case 3:
						color = new Color(100, 255, 255, 0);
						break;
					case 4:
						color = new Color(255, 100, 255, 0);
						break;
					case 5:
						color = new Color(255, 255, 100, 0);
						break;
				}
				Vector2 rotationAround2 = new Vector2(2 + mult, 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
				Main.spriteBatch.Draw(texture, location + zero - Main.screenPosition + rotationAround2, frame, color, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame, Color.Lerp(color2, Color.Black, 0.7f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override bool CanPlace(int i, int j)
		{
			return TileIsCapable(i, j + 1) || TileIsCapable(i, j - 1) || TileIsCapable(i + 1, j) || TileIsCapable(i - 1, j);
		}
		private bool TileIsCapable(Tile tile)
        {
			return tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType] && tile.Slope == 0 && !tile.IsHalfBlock && !tile.IsActuated;
		}
		private bool TileIsCapable(int i, int j)
        {
			return TileIsCapable(Main.tile[i, j]);
        }
		public bool ModifyFrames(int i, int j, bool randomize = false)
		{
			bool flag = true;
			if (TileIsCapable(i, j + 1)) //checks if below tile is active
			{
				Main.tile[i, j].TileFrameY = 0;
			}
			else if (TileIsCapable(i - 1, j)) //checks if left tile is active
			{
				Main.tile[i, j].TileFrameY = 54;
			}
			else if (TileIsCapable(i + 1, j)) //checks if right tile is active
			{
				Main.tile[i, j].TileFrameY = 36;
			}
			else if (TileIsCapable(i, j - 1)) //checks if above tile is active
			{
				Main.tile[i, j].TileFrameY = 18;
			}
			else
            {
				flag = false;
			}
			if(flag && randomize)
			{
				Main.tile[i, j].TileFrameX = (short)(WorldGen.genRand.Next(18) * 18);
				WorldGen.SquareTileFrame(i, j, true);
				NetMessage.SendTileSquare(-1, i, j, 2, TileChangeType.None);
				//NetMessage.SendData(17, -1, -1, null, 1, i, j, Type);
			}
			return flag;
		}
		public override void PlaceInWorld(int i, int j, Item item)
		{
			ModifyFrames(i, j, true);
		}
	}
}