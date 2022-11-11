using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Nvidia;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Gems
{
	public abstract class GemChest : ModItem
	{
		public virtual int PlaceStyle => 0;
		public virtual int GemType => ItemID.Ruby;
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Chest);
			Item.width = 32;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<GemChestTile>();
			Item.placeStyle = PlaceStyle + 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(GemType, 2).AddIngredient(ModContent.ItemType<Evostone>(), 8).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class RubyChest : GemChest
	{
		public override int PlaceStyle => 0;
		public override int GemType => ItemID.Ruby;
	}
	public class SapphireChest : GemChest
	{
		public override int PlaceStyle => 2;
		public override int GemType => ItemID.Sapphire;
	}
	public class EmeraldChest : GemChest
	{
		public override int PlaceStyle => 4;
		public override int GemType => ItemID.Emerald;
	}
	public class TopazChest : GemChest
	{
		public override int PlaceStyle => 6;
		public override int GemType => ItemID.Topaz;
	}
	public class AmethystChest : GemChest
	{
		public override int PlaceStyle => 8;
		public override int GemType => ItemID.Amethyst;
	}
	public class DiamondChest : GemChest
	{
		public override int PlaceStyle => 10;
		public override int GemType => ItemID.Diamond;
	}
	public class AmberChest : GemChest
	{
		public override int PlaceStyle => 12;
		public override int GemType => ItemID.Amber;
	}
	public class GemChestTile : Furniture.ContainerType
	{
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int unused = 0;
			if (IsLockedChest(i, j) && CanUnlockChest(i, j, ref unused))
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					zero = Vector2.Zero;
				}
				Vector2 origin = new Vector2(8, 8);
				Texture2D texture = TextureAssets.Tile[Type].Value;
				if (tile.TileFrameX % 36 == 0 && tile.TileFrameY == 0)
				{
					for (int c = 0; c < 2; c++)
					{ 
						for (int n = 0; n < 4; n++)
						{
							int yFrameSize = 16;
							if (n == 1 || n == 3)
								i++;
							else if (n == 2)
							{
								i--;
								j++;
							}
							if (n == 2 || n == 3)
							{
								yFrameSize = 18;
							}
							Tile tileToFrame = Main.tile[i, j];
							Vector2 location = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
							Rectangle frame2 = new Rectangle(tileToFrame.TileFrameX, tileToFrame.TileFrameY, 16, yFrameSize);
							if (c == 1)
							{
								Color color = Lighting.GetColor(i, j, WorldGen.paintColor(Main.tile[i, j].TileColor));
								color = Color.Lerp(color, Color.White, 0.5f);
								spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame2, color, 0f, origin, 1f, SpriteEffects.None, 0f); //draws a small highlight if the chest is ready to be unlocked
							}
							else
							{
								for (int b = 0; b < 6; b++)
								{
									Vector2 circular = new Vector2(0, 2).RotatedBy(MathHelper.ToRadians(b * 60 + SOTSWorld.GlobalCounter * 2));
									spriteBatch.Draw(texture, location + zero + circular - Main.screenPosition, frame2, new Color(120 - b * 7, 110 - b * 2, 100 + b * 4, 0), 0f, origin, 1f, SpriteEffects.None, 0f);
								}
							}
							if (n == 3)
							{
								i--;
								j--;
							}
						}
					}
				}
				return false;
			}
			return true;
		}
        protected override string ChestName => "Gem Chest";
		public bool CanUnlockChest(int i, int j, ref int dustType)
		{
			Tile tile = Main.tile[i, j];
			int chestType = tile.TileFrameX / 72;
			if (chestType == 0)
			{
				dustType = DustID.GemRuby;
				return SOTSWorld.RubyKeySlotted;
			}
			if (chestType == 1)
			{
				dustType = DustID.GemSapphire;
				return SOTSWorld.SapphireKeySlotted;
			}
			if (chestType == 2)
			{
				dustType = DustID.GemEmerald;
				return SOTSWorld.EmeraldKeySlotted;
			}
			if (chestType == 3)
			{
				dustType = DustID.GemTopaz;
				return SOTSWorld.TopazKeySlotted;
			}
			if (chestType == 4)
			{
				dustType = DustID.GemAmethyst;
				return SOTSWorld.AmethystKeySlotted;
			}
			if (chestType == 5)
			{
				dustType = DustID.GemDiamond;
				return SOTSWorld.DiamondKeySlotted;
			}
			if (chestType == 6)
			{
				dustType = DustID.GemAmber;
				return SOTSWorld.AmberKeySlotted;
			}
			return false;
		}
		public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
		{
			return CanUnlockChest(i, j, ref dustType);
		}
		public int ChestDrop(int frameX)
		{
			int chestDrop = ModContent.ItemType<RubyChest>();
			int chestType = frameX / 72;
			if (chestType == 1)
				chestDrop = ModContent.ItemType<SapphireChest>();
			if (chestType == 2)
				chestDrop = ModContent.ItemType<EmeraldChest>();
			if (chestType == 3)
				chestDrop = ModContent.ItemType<TopazChest>();
			if (chestType == 4)
				chestDrop = ModContent.ItemType<AmethystChest>();
			if (chestType == 5)
				chestDrop = ModContent.ItemType<DiamondChest>();
			if (chestType == 6)
				chestDrop = ModContent.ItemType<AmberChest>();
			return chestDrop;
		}
		public int ChestDisplayKey(int frameX)
		{
			int chestDrop = ModContent.ItemType<SOTSRubyGemLock>();
			int chestType = frameX / 72;
			if (chestType == 1)
				chestDrop = ModContent.ItemType<SOTSSapphireGemLock>();
			if (chestType == 2)
				chestDrop = ModContent.ItemType<SOTSEmeraldGemLock>();
			if (chestType == 3)
				chestDrop = ModContent.ItemType<SOTSTopazGemLock>();
			if (chestType == 4)
				chestDrop = ModContent.ItemType<SOTSAmethystGemLock>();
			if (chestType == 5)
				chestDrop = ModContent.ItemType<SOTSDiamondGemLock>();
			if (chestType == 6)
				chestDrop = ModContent.ItemType<SOTSAmberGemLock>();
			return chestDrop;
		}
		protected override int ChestKey => ModContent.ItemType<SOTSRubyGemLock>();
		protected override int DustType => 122;
		protected override void AddMapEntires()
		{
			ModTranslation name = CreateMapEntryName(Name + "R");
			name.SetDefault("Ruby Chest");
			AddMapEntry(new Color(212, 37, 24), name, MapChestName);
			name = CreateMapEntryName(Name + "R_Locked");
			name.SetDefault("Locked Ruby Chest");
			AddMapEntry(new Color(212, 37, 24), name, MapChestName);

			name = CreateMapEntryName(Name + "S");
			name.SetDefault("Sapphire Chest");
			AddMapEntry(new Color(18, 116, 211), name, MapChestName);
			name = CreateMapEntryName(Name + "S_Locked");
			name.SetDefault("Locked Sapphire Chest");
			AddMapEntry(new Color(18, 116, 211), name, MapChestName);

			name = CreateMapEntryName(Name + "E");
			name.SetDefault("Emerald Chest");
			AddMapEntry(new Color(33, 184, 115), name, MapChestName);
			name = CreateMapEntryName(Name + "E_Locked");
			name.SetDefault("Locked Emerald Chest");
			AddMapEntry(new Color(33, 184, 115), name, MapChestName);

			name = CreateMapEntryName(Name + "T");
			name.SetDefault("Topaz Chest");
			AddMapEntry(new Color(239, 167, 10), name, MapChestName);
			name = CreateMapEntryName(Name + "T_Locked");
			name.SetDefault("Locked Topaz Chest");
			AddMapEntry(new Color(239, 167, 10), name, MapChestName);

			name = CreateMapEntryName(Name + "A");
			name.SetDefault("Amethyst Chest");
			AddMapEntry(new Color(158, 0, 244), name, MapChestName);
			name = CreateMapEntryName(Name + "A_Locked");
			name.SetDefault("Locked Amethyst Chest");
			AddMapEntry(new Color(158, 0, 244), name, MapChestName);

			name = CreateMapEntryName(Name + "D");
			name.SetDefault("Diamond Chest");
			AddMapEntry(new Color(154, 197, 239), name, MapChestName);
			name = CreateMapEntryName(Name + "D_Locked");
			name.SetDefault("Locked Diamond Chest");
			AddMapEntry(new Color(158, 0, 244), name, MapChestName);

			name = CreateMapEntryName(Name + "Amber");
			name.SetDefault("Amber Chest");
			AddMapEntry(new Color(225, 124, 30), name, MapChestName);
			name = CreateMapEntryName(Name + "Amber_Locked");
			name.SetDefault("Locked Amber Chest");
			AddMapEntry(new Color(225, 124, 30), name, MapChestName);
		}
		public override bool IsLockedChest(int i, int j)
		{
			return (Main.tile[i, j].TileFrameX % 72) / 36 == 1;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ChestDrop(frameX));
			Chest.DestroyChest(i, j);
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}

			if (tile.TileFrameY != 0)
			{
				top--;
			}

			int chest = Chest.FindChest(left, top);
			player.cursorItemIconID = -1;
			if (chest < 0)
			{
				player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
			}
			else
			{
				string defaultName = TileLoader.ContainerName(tile.TileType); // This gets the ContainerName text for the currently selected language
				player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
				if (player.cursorItemIconText == defaultName)
				{
					player.cursorItemIconID = ChestDrop(tile.TileFrameX);
					if (IsLockedChest(i, j))
					{
						player.cursorItemIconID = ChestDisplayKey(tile.TileFrameX);
					}
					player.cursorItemIconText = "";
				}
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}
	}
}