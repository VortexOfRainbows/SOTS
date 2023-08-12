using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Tide
{
	public class ArkhalisChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.consumable = true;
			Item.createTile = ModContent.TileType<ArkhalisChainTile>();
			Item.placeStyle = 0;
		}
	}	
	public class ArkhalisChainTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(127, 127, 127), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<ArkhalisChain>();
			DustType = ModContent.DustType<AvaritianDust>();
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (Main.tile[i, j].TileFrameX >= 18)
			{
				player.cursorItemIconID = ItemID.Terragrim;
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
			}
			else
				player.cursorItemIconEnabled = false;
		}
		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			MouseOver(i, j);
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
        public override bool RightClick(int i, int j)
        {
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int key = ItemID.Terragrim;
			if (Main.tile[i, j].TileFrameX < 18 && player.ConsumeItem(key))
			{
				SOTSUtils.PlaySound(SoundID.Grab, (int)player.Center.X, (int)player.Center.Y, 1.1f, -0.2f);
				tile.TileFrameX = 18;
				NetMessage.SendTileSquare(-1, i, j, 2);
			}
			else if(Main.tile[i, j].TileFrameX >= 18)
			{
				SOTSUtils.PlaySound(SoundID.Grab, (int)player.Center.X, (int)player.Center.Y, 1.1f, -0.2f);
				int item = Item.NewItem(new EntitySource_TileInteraction(player, i, j), i * 16, (j + 6) * 16, 16, 16, ItemID.Terragrim, 1, false, 0, true);
				NetMessage.SendData(MessageID.SyncItem, player.whoAmI, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
				tile.TileFrameX = 0;
				NetMessage.SendTileSquare(-1, i, j, 2);
			}
			return true;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return Main.tile[i, j].TileFrameX < 18;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			if(Main.tile[i, j].TileFrameX >= 18 && !fail && !effectOnly)
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, (j + 6) * 16, 16, 16, ItemID.Terragrim, 1);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.12f;
			g = 0.225f;
			b = 0.2f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (!TextureAssets.Item[ItemID.Terragrim].IsLoaded)
			{
				Main.instance.LoadItem(ItemID.Terragrim);
			}
			Texture2D texture = TextureAssets.Tile[Type].Value;
			Texture2D textureSword = TextureAssets.Item[ItemID.Terragrim].Value;
			Vector2 origin = new Vector2(8, 10);
			//float height = 16;
			float timer = Main.GlobalTimeWrappedHourly * 40 + (i + j) * 4;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			bool wave = true;
			int maxLength = 6;
			if(Main.tile[i, j].TileFrameX < 18)
			{
				for (int j2 = 1; j2 < maxLength; j2++)
				{
					Tile tile2 = Framing.GetTileSafely(i, j + j2);
					if ((tile2.HasTile && Main.tileSolid[tile2.TileType] && !Main.tileSolidTop[tile2.TileType]) || !WorldGen.InWorld(i, j + j2, 27))
					{
						wave = false;
						maxLength = j2 + 1;
						break;
					}
				}
			}
			maxLength++;
			Vector2 previous = new Vector2(i * 16 + 8, j * 16 + 20);
			for (int z = 2; z < maxLength; z ++)
			{
				//dynamicMult = 0.52f + 0.48f * (float)Math.Cos(MathHelper.ToRadians(180f * z / maxLength));
				Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 4);
				float sin = (float)Math.Sin(MathHelper.ToRadians(timer));
				if (!wave)
                {
					sin = 0;
				}
				Vector2 speed = new Vector2(0, z * 16).RotatedBy(MathHelper.ToRadians(sin * 2.4f * (float)Math.Pow(z - 1, 0.4)));
				pos += speed;
				Color color = WorldGen.paintColor(Main.tile[i, j].TileColor);
				color = Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16, color);
				Vector2 rotateTo = pos - previous;
				//float lengthTo = rotateTo.Length();
				//float stretch = lengthTo / height * 1.00275f;
				//Vector2 scaleVector2 = new Vector2(1, stretch);
				if (z == maxLength - 1)
				{
					origin = new Vector2(10, 10);
					Rectangle frame = new Rectangle(0, 36, 20, 20);
					Main.spriteBatch.Draw(texture, previous + zero - Main.screenPosition, frame, color, rotateTo.ToRotation() - MathHelper.ToRadians(90), origin, 1f, SpriteEffects.None, 0f);
					if(Main.tile[i,j].TileFrameX >= 18)
					{
						Main.spriteBatch.Draw(textureSword, previous + zero + speed.SafeNormalize(Vector2.Zero) * 14f - Main.screenPosition, null, Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16, Color.White), rotateTo.ToRotation() + MathHelper.ToRadians(45), textureSword.Size() / 2, 1.1f, SpriteEffects.None, 0f);
					}
				}
				else
				{
					Rectangle frame = new Rectangle(0, 16, 16, 20);
					Main.spriteBatch.Draw(texture, previous + zero - Main.screenPosition, frame, color, rotateTo.ToRotation() - MathHelper.ToRadians(90), origin, 1f, SpriteEffects.None, 0f);
				}
				previous = pos;
			}
			return true;
		}
	}
}