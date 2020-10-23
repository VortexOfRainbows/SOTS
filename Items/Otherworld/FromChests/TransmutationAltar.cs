using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;


namespace SOTS.Items.Otherworld.FromChests
{
	public class TransmutationAltar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transmutation Altar");
			Tooltip.SetDefault("Used for crafting\nAllows the conversion of material and equipment to equivalent but different forms\nAlso counts as a demon altar for crafting");
		}
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 40;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.consumable = true;
			item.createTile = mod.TileType("TransmutationAltarTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PrecariousCluster", 1);
			recipe.AddIngredient(null, "TwilightShard", 20);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 20);
			recipe.AddIngredient(null, "HardlightAlloy", 10);
			recipe.AddIngredient(null, "StarlightAlloy", 10);
			recipe.AddTile(mod.TileType("HardlightFabricator"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class TransmutationAltarTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(GetInstance<TransmutationAltarStorage>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Transmutation Altar");
			AddMapEntry(new Color(125, 55, 55), name);
			disableSmartCursor = true;
			dustType = mod.DustType("AvaritianDust");
			adjTiles = new int[] { TileID.DemonAltar };
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = mod.ItemType("TransmutationAltar");
			Item.NewItem(i * 16, j * 16, 32, 16, drop);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].frameX < 18 || Main.tile[i, j].frameX > 35 || Main.tile[i, j].frameY % 36 < 18)
				return;

			r = 1.0f;
			g = 0.1f;
			b = 0.1f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;
			int type = 0;
			if (Main.tile[i, j].frameX >= 18 && Main.tile[i, j].frameX < 36 && Main.tile[i, j].frameY % 36 >= 18)
				type = 1;
			int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
			if (index == -1 || type != 1)
			{
				return true;
			}
			TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];
			Texture2D texture = mod.GetTexture("Items/Otherworld/HardlightGearBorder");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/HardlightGearFill");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 40));
			int amountOfUniqueItems = 1;
			int totalItems = 0;
			for (int l = 1; l < entity.itemsArray.Length; l++)
			{
				if (entity.itemsArray[l] != 0)
				{
					amountOfUniqueItems++;
					totalItems += entity.itemAmountsArray[l];
				}
			}
			int currentItem = 0;
			for (int l = 1; l < amountOfUniqueItems; l++)
			{
				texture = Main.itemTexture[entity.itemsArray[l]];
				for (int g = 0; g < entity.itemAmountsArray[l]; g++)
				{
					int frame = entity.itemFrames[l] <= 0 ? 1 : entity.itemFrames[l];
					currentItem++;
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					pos.Y -= 50 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 3;
					Vector2 circularLocation = new Vector2(32 + totalItems * 3, 0).RotatedBy(MathHelper.ToRadians(currentItem * 360f / totalItems) + MathHelper.ToRadians(Main.GlobalTime * 50));
					pos += circularLocation;
					Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);
					for (int k = 0; k < 10; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f;
						float y = Main.rand.Next(-10, 11) * 0.2f;
						Main.spriteBatch.Draw(texture, pos + new Vector2(x, y), new Rectangle(0, 0, texture.Width, texture.Height/frame), color, 0f, origin, 0.8f, SpriteEffects.None, 0f);
						if (k == 9)
							Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height / frame), new Color(255, 255, 255), 0f, origin, 0.8f, SpriteEffects.None, 0f);
					}
				}
			}
			texture = Main.itemTexture[entity.itemsArray[0]];
			for (int g = 0; g < entity.itemAmountsArray[0]; g++)
			{
				int frame = entity.itemFrames[0] <= 0 ? 1 : entity.itemFrames[0];
				currentItem++;
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
				pos.Y -= 50 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 3;
				Vector2 circularLocation = new Vector2(entity.itemAmountsArray[0] * 3, 0).RotatedBy(MathHelper.ToRadians(currentItem * 360f / entity.itemAmountsArray[0]) + MathHelper.ToRadians(Main.GlobalTime * 50));
				pos += circularLocation;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);
				for (int n = 0; n < 10; n++)
				{
					float x = Main.rand.Next(-10, 11) * 0.2f;
					float y = Main.rand.Next(-10, 11) * 0.2f;
					Main.spriteBatch.Draw(texture, pos + new Vector2(x, y), new Rectangle(0, 0, texture.Width, texture.Height / frame), color, 0f, origin, 0.8f, SpriteEffects.None, 0f);
					if (n == 9)
						Main.spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height / frame), new Color(255, 255, 255), 0f, origin, 0.8f, SpriteEffects.None, 0f);
				}
			}
			return true;
		}
	}
	public class TransmutationAltarStorage : ModTileEntity
	{
		internal bool netUpdate = false;
		internal int[] itemsArray = new int[20];
		internal int[] itemAmountsArray = new int[20];
		internal int[] itemFrames = new int[20];
		internal int timer = -2;
		internal int style = 0;
		public override void Update()
		{
			if(timer == -2)
			{
				itemsArray[0] = ItemID.FallenStar;
				itemAmountsArray[0] = 10;
				itemFrames[0] = 1;
				itemsArray[1] = ItemID.ManaCrystal;
				itemAmountsArray[1] = 3;
				itemFrames[1] = 1;
				timer = -1;
			}
			if (netUpdate)
			{
				netUpdate = false;
				NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
			}
		}

		public override void NetReceive(BinaryReader reader, bool lightReceive)
		{
			timer = reader.ReadInt32();
			style = reader.ReadInt32();
			for (int i = 0; i < 20; i++)
			{
				itemsArray[i] = reader.ReadInt32();
			}
			for (int i = 0; i < 20; i++)
			{
				itemAmountsArray[i] = reader.ReadInt32();
			}
			for (int i = 0; i < 20; i++)
			{
				itemFrames[i] = reader.ReadInt32();
			}
		}

		public override void NetSend(BinaryWriter writer, bool lightSend)
		{
			writer.Write(timer);
			writer.Write(style);
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemsArray[i]);
			}
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemAmountsArray[i]);
			}
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemFrames[i]);
			}
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"timer", timer},
				{"style", style},
				{"itemsArray", itemsArray},
				{"itemAmountsArray", itemAmountsArray},
				{"itemFrames", itemFrames},
			};
		}

		public override void Load(TagCompound tag)
		{
			timer = tag.Get<int>("timer");
			style = tag.Get<int>("style");
			itemsArray = tag.Get<int[]>("itemsArray");
			itemAmountsArray = tag.Get<int[]>("itemAmountsArray");
			itemFrames = tag.Get<int[]>("itemFrames");
		}

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.type == (ushort)mod.TileType("TransmutationAltarTile") && tile.frameX == 0 && tile.frameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			//Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
	}
}