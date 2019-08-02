using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Secrets.IceCream
{
	public class ColdFusionReactorTile : ModTile
	{	int repeatMessage = 0;
		bool fixVisualBug = false;
		bool between = false;
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileValue[Type] = 500;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Fusion Chest");
			AddMapEntry(new Color(200, 200, 200), name, MapChestName);
			dustType = 72;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Containers };
			chest = "Fusion Chest";
			chestDrop = mod.ItemType("ColdFusionReactor");
		}
		public override bool HasSmartInteract()
		{
			return true;
		}

		public string MapChestName(string name, int i, int j)
		{
			int left = i;
			int top = j;
			Tile tile = Main.tile[i, j];
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			int chest = Chest.FindChest(left, top);
			if (Main.chest[chest].name == "")
			{
				return name;
			}
			else
			{
				return name + ": " + Main.chest[chest].name;
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, chestDrop);
			Chest.DestroyChest(i, j);
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if(Main.tile[i, j - 1].type != mod.TileType("ColdFusionReactorTile") && Main.tile[i - 1, j - 1].type != mod.TileType("ColdFusionReactorTile") && Main.tile[i - 1, j].type != mod.TileType("ColdFusionReactorTile") && Main.tile[i + 2, j].type != mod.TileType("ColdFusionReactorTile") && Main.tile[i - 2, j].type != mod.TileType("ColdFusionReactorTile") && Main.tile[i + 3, j].type != mod.TileType("ColdFusionReactorTile") && fixVisualBug)
			{
				int left = i;
				int top = j;
				int container = Chest.FindChest(left, top);
				
			Player player = Main.LocalPlayer;
						
				Chest chest = Main.chest[container];
				int UCP = 0;
				int UCPCost = 0;
				int duplicateID = 0;
				bool invalid = false;
										for (int inventoryIndex = 0; inventoryIndex < 50; inventoryIndex++)
										{
											if(player.inventory[inventoryIndex].type == mod.ItemType("IceCreamOre"))
											{
												UCP += player.inventory[inventoryIndex].stack;
												
											}
										}
										Item duplicate = chest.item[39];
										if(duplicate.type != mod.ItemType("IceCreamOre"))
											{
												UCPCost = (int)((duplicate.value + (duplicate.damage * 5) + (duplicate.defense * 5) + (duplicate.width * 2) + (duplicate.height * 2)) * 0.3 + (duplicate.rare * 5));
												if(UCPCost < 500 && duplicate.expert)
												{
												UCPCost *= 100;	
												}
												if(UCPCost < 10000 && duplicate.expert)
												{
												UCPCost *= 30;	
												}
												if(duplicate.createTile > 0)
												{
												UCPCost = (int)(UCPCost * 0.1) + 3;
												}
												duplicateID = duplicate.type;
												if(duplicateID == ItemID.CopperCoin || duplicateID == ItemID.SilverCoin || duplicateID == ItemID.GoldCoin || duplicateID == ItemID.PlatinumCoin || duplicateID == ItemID.Present || duplicateID == ItemID.GoodieBag || duplicateID == mod.ItemType("PlanetariumOrb") || duplicateID == mod.ItemType("SoulFragment"))
												{
													invalid = true;
												}
												if(UCPCost <= 3)
												{
													UCPCost = 0;
													invalid = true;
												}
												if(duplicateID == mod.ItemType("IceCream"))
												{
													UCPCost = 5;
												}
												
											}
											
											if(player.chest == container && duplicateID == 0)
											{
												repeatMessage++;
											}
										if(repeatMessage >= 1 && duplicateID != 0)
										{
											repeatMessage = 0;
										string UCPtext = UCP.ToString();
										string CostText = UCPCost.ToString();
										string ItemIDString = duplicateID.ToString();
										Main.NewText("Your inventory holds " + UCPtext + " Alpha Virus", 125, 145, 125);
											if(invalid)
											{
											Main.NewText("Invalid item", 125, 145, 125);
											}
											else if(UCPCost != 0)
											{
											Main.NewText("Duplicating Item ID: " + ItemIDString + " will cost " + CostText + " Alpha Virus", 125, 145, 125);
											Main.NewText("Remove the item in the first slot to start duplicating", 125, 25, 25);
											}
											else
											{
											Main.NewText("Insert an item into the last slot", 125, 145, 125);
											}
											if(chest.item[0].type == 0 && !invalid)
											{
											chest.item[0].SetDefaults(ItemID.CopperCoin);
											chest.item[0].stack = 1;
											
											}
										}
										/*
											if(chest.item[0].type == 0 && between)
											{
											chest.item[0].SetDefaults(ItemID.CopperCoin);
											chest.item[0].stack = 1;
											between = false;
											}
											*/
											
										if(UCPCost <= UCP && chest.item[0].stack == 0)
										{
											for(int inventoryIndex = 0; inventoryIndex < 50; inventoryIndex++)
											{
												if(player.inventory[inventoryIndex].type == mod.ItemType("IceCreamOre"))
												{
													for(int stack = player.inventory[inventoryIndex].stack; stack > 0; stack--)
													{
														if(UCPCost <= 0)
														{
															chest.item[0].SetDefaults(duplicateID);
															chest.item[0].stack = 1;
															break;
														}
														player.inventory[inventoryIndex].stack--;
														UCPCost--;
														
													}
												}
											}
										}
			}
		}
		public override void RightClick(int i, int j)
		{
			fixVisualBug = true;
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int left = i;
			int top = j;
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			if (player.sign >= 0)
			{
				Main.PlaySound(SoundID.MenuClose);
				player.sign = -1;
				Main.editSign = false;
				Main.npcChatText = "";
			}
			if (Main.editChest)
			{
				Main.PlaySound(SoundID.MenuTick);
				Main.editChest = false;
				Main.npcChatText = "";
			}
			if (player.editedChestName)
			{
				NetMessage.SendData(33, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
				player.editedChestName = false;
			}
			if (Main.netMode == 1)
			{
				if (left == player.chestX && top == player.chestY && player.chest >= 0)
				{
					player.chest = -1;
					Recipe.FindRecipes();
					Main.PlaySound(SoundID.MenuClose);
				}
				else
				{
					NetMessage.SendData(31, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
					Main.stackSplit = 600;
				}
			}
			else
			{
				int chest = Chest.FindChest(left, top);
				if (chest >= 0)
				{
					Main.stackSplit = 600;
					if (chest == player.chest)
					{
						player.chest = -1;
						Main.PlaySound(SoundID.MenuClose);
					}
					else
					{
						player.chest = chest;
						Main.playerInventory = true;
						Main.recBigList = false;
						player.chestX = left;
						player.chestY = top;
						Main.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
					}
					Recipe.FindRecipes();
				}
			}
				
				
			
			
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
			int chest = Chest.FindChest(left, top);
			player.showItemIcon2 = -1;
			if (chest < 0)
			{
				player.showItemIconText = Language.GetTextValue("LegacyChestType.0");
			}
			else
			{
				player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : "Fusion Chest";
				if (player.showItemIconText == "Fusion Chest")
				{
					player.showItemIcon2 = mod.ItemType("ColdFusionReactor");
					player.showItemIconText = "";
				}
			}
			player.noThrow = 2;
			player.showItemIcon = true;
		}

		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.showItemIconText == "")
			{
				player.showItemIcon = false;
				player.showItemIcon2 = 0;
			}
		}
	}
}