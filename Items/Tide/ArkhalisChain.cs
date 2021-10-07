using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Tide
{
	public class ArkhalisChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Chain");
			Tooltip.SetDefault("'It probably didn't come from here'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.consumable = true;
			item.createTile = ModContent.TileType<ArkhalisChainTile>();
			item.placeStyle = 0;
		}
	}	
	public class ArkhalisChainTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Lost Chain");
			AddMapEntry(new Color(127, 127, 127), name);
			disableSmartCursor = true;
			drop = ModContent.ItemType<ArkhalisChain>();
			dustType = ModContent.DustType<AvaritianDust>();
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (Main.tile[i, j].frameX >= 18)
			{
				player.showItemIcon2 = ItemID.Arkhalis;
				player.noThrow = 2;
				player.showItemIcon = true;
			}
			else
				player.showItemIcon = false;
		}
		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			MouseOver(i, j);
			if (player.showItemIconText == "")
			{
				player.showItemIcon = false;
				player.showItemIcon2 = 0;
			}
		}
		public override bool NewRightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int key = ItemID.Arkhalis;
			if (Main.tile[i, j].frameX < 18 && player.ConsumeItem(key))
			{
				Main.PlaySound(SoundID.Grab, (int)player.Center.X, (int)player.Center.Y, 0, 1.1f, -0.2f);
				tile.frameX = 18;
				NetMessage.SendTileSquare(-1, i, j, 2);
			}
			else if(Main.tile[i, j].frameX >= 18)
			{
				Main.PlaySound(SoundID.Grab, (int)player.Center.X, (int)player.Center.Y, 0, 1.1f, -0.2f);
				int item = Item.NewItem(i * 16, (j + 6) * 16, 16, 16, ItemID.Arkhalis, 1, false, 0, true);
				NetMessage.SendData(MessageID.SyncItem, player.whoAmI, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
				tile.frameX = 0;
				NetMessage.SendTileSquare(-1, i, j, 2);
			}
			return true;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return Main.tile[i, j].frameX < 18;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			if(Main.tile[i, j].frameX >= 18 && !fail && !effectOnly)
				Item.NewItem(i * 16, (j + 6) * 16, 16, 16, ItemID.Arkhalis, 1);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.12f;
			g = 0.225f;
			b = 0.2f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
			Texture2D texture = Main.tileTexture[Type];
			Texture2D textureSword = Main.itemTexture[ItemID.Arkhalis];
			Vector2 origin = new Vector2(8, 10);
			//float height = 16;
			float timer = Main.GlobalTime * 40 + (i + j) * 4;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			bool wave = true;
			int maxLength = 6;
			if(Main.tile[i, j].frameX < 18)
			{
				for (int j2 = 1; j2 < maxLength; j2++)
				{
					Tile tile2 = Framing.GetTileSafely(i, j + j2);
					if ((tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]) || !WorldGen.InWorld(i, j + j2, 27))
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
				Color color = WorldGen.paintColor(Main.tile[i, j].color());
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
					if(Main.tile[i,j].frameX >= 18)
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