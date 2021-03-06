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
using SOTS.Items.Otherworld.FromChests;
using System.Linq;
using System.Collections.Generic;
using SOTS.Items.Pyramid;
using SOTS.Void;
using SOTS.Items.Celestial;
using SOTS.Items.Otherworld;
using SOTS.Items.ChestItems;
using SOTS.Items;

namespace SOTS
{
	public class SOTSItem : GlobalItem
	{
        public static int[] rarities1;
		public static int[] rarities2;
		public static int[] dedicatedOrange;
		public static int[] dedicatedBlue;
		public static int[] dedicatedPurpleRed;
		public static int[] dedicatedPastelPink;
		public static int[] dedicatedRainbow;
		static bool runOnce = true;
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if(runOnce)
			{
				rarities1 = new int[] { mod.ItemType("StarlightAlloy"), mod.ItemType("HardlightAlloy"), mod.ItemType("OtherworldlyAlloy"), mod.ItemType("PotGenerator"), mod.ItemType("PrecariousCluster"), mod.ItemType("Calculator") };
				rarities2 = new int[] { ItemType<RefractingCrystal>(), ItemType<CursedApple>() };
				dedicatedOrange = new int[] { mod.ItemType("TerminatorAcorns"), ItemType<PlasmaCutterButOnAChain>(), ItemType<CoconutGun>() };
				dedicatedBlue = new int[] { mod.ItemType("Calculator") };
				dedicatedPurpleRed = new int[] { ItemType<CursedApple>(), ItemType<ArcStaffMk2>() };
				dedicatedPastelPink = new int[] { ItemType<StrangeFruit>() };
				dedicatedRainbow = new int[] { ItemType<PhotonGeyser>() };
				runOnce = false;
            }
			if (rarities1.Contains(item.type))
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.mod == "Terraria" && line2.Name == "ItemName")
					{
						line2.overrideColor = new Color(0, 130, 235, 255);
					}
				}
			}
			if (rarities2.Contains(item.type))
			{
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.mod == "Terraria" && line2.Name == "ItemName")
					{
						line2.overrideColor = new Color(210, 0, 0);
					}
				}
			}
			bool dedicated = false;
			Color dedicatedColor = Color.White;
			if(dedicatedOrange.Contains(item.type))
			{
				dedicatedColor = new Color(255, 115, 0);
				dedicated = true;
			}
			if (dedicatedBlue.Contains(item.type))
			{
				dedicatedColor = new Color(0, 130, 235, 255);
				dedicated = true;
			}
			if (dedicatedPurpleRed.Contains(item.type))
            {
				dedicatedColor = VoidPlayer.soulLootingColor;
				dedicated = true;
			}
			if (dedicatedRainbow.Contains(item.type))
			{
				dedicatedColor = VoidPlayer.pastelRainbow;
				dedicated = true;
			}
			if (dedicatedPastelPink.Contains(item.type))
			{
				Color color = new Color(211, 0, 194);
				foreach (TooltipLine line2 in tooltips)
				{
					if (line2.mod == "Terraria" && line2.Name == "ItemName")
					{
						line2.overrideColor = color;
					}
				}
				dedicatedColor = new Color(255, 158, 235);
				dedicated = true;
			}
			if (dedicated)
			{
				TooltipLine line = new TooltipLine(mod, "Dedicated", "Dedicated Item");
				line.overrideColor = dedicatedColor;
				tooltips.Add(line);
			}
		}
		public Tile FindTATile(Player player)
        {
			int x = (int)player.Center.X / 16;
			int y = (int)player.Center.Y / 16;
			Vector2 between = new Vector2(100000, 0);
			Tile bestTile = (Tile)null;
			for (int i = -9; i < 10; i++)
            {
				for(int j = -9; j < 10; j++)
                {
					Tile tile = Framing.GetTileSafely(x + i, y + j);
					int type = 0;
					if (Main.tile[x + i, y + j].frameX >= 18 && Main.tile[x + i, y + j].frameX < 36 && Main.tile[x + i, y + j].frameY % 36 >= 18)
						type = 1;
					if (tile.type == mod.TileType("TransmutationAltarTile") && type == 1)
					{
						Vector2 newBetween = new Vector2(i * 16, j * 16);
						if (newBetween.Length() < between.Length())
						{
							between = newBetween;
							bestTile = tile;
						}
					}
				}
            }
			return bestTile;
		}
		public Point16 FindTATileIJ(Player player)
		{
			int x = (int)player.Center.X / 16;
			int y = (int)player.Center.Y / 16;
			Vector2 between = new Vector2(100000, 0);
			Point16 bestTile = new Point16(x, y);
			for (int i = -9; i < 10; i++)
			{
				for (int j = -9; j < 10; j++)
				{
					Tile tile = Framing.GetTileSafely(x + i, y + j);
					int type = 0;
					if (Main.tile[x + i, y + j].frameX >= 18 && Main.tile[x + i, y + j].frameX < 36 && Main.tile[x + i, y + j].frameY % 36 >= 18)
						type = 1;
					if (tile.type == mod.TileType("TransmutationAltarTile") && type == 1)
					{
						Vector2 newBetween = new Vector2(i * 16, j * 16);
						if (newBetween.Length() < between.Length())
						{
							between = newBetween;
							bestTile = new Point16(x + i, y + j);
						}
					}
				}
			}
			return bestTile;
		}
		public override void OnCraft(Item item, Recipe recipe)
		{
			Player player = Main.LocalPlayer;
			if (recipe.requiredTile[0] == TileID.DemonAltar || recipe.requiredTile[0] == mod.TileType("TransmutationAltarTile"))
            {
				Tile tile = FindTATile(player);
				if(tile != null)
				{
					Point16 ij = FindTATileIJ(player);
					int left = ij.X - tile.frameX / 18;
					int top = ij.Y - tile.frameY / 18;
					int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
					if (index == -1)
					{
						return;
					}
					Item item2 = recipe.createItem;
					TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];


					Projectile projectile = Main.projectile[Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("DataTransferProj"), 0, 0, Main.myPlayer, index, 0)];
					DataTransferProj proj = (DataTransferProj)projectile.modProjectile;
					proj.itemsArray[0] = item2.type;
					proj.itemAmountsArray[0] = item2.stack;
					proj.itemFrames[0] = Main.itemTexture[item2.type].Height / recipe.requiredItem[0].height;
					int amountOfUniqueItems = 0;
					for (int l = 0; l < recipe.requiredItem.Length; l++)
					{
						if (recipe.requiredItem[l].type != 0)
						{
							amountOfUniqueItems++;
						}
						else
							break;
					}
					for (int i = 0; i < (amountOfUniqueItems < 20 ? amountOfUniqueItems : 19); i++)
					{
						int itemType = recipe.requiredItem[i].type;
						int itemStack = recipe.requiredItem[i].stack;
						int itemFrames = Main.itemTexture[itemType].Height / recipe.requiredItem[i].height;
						proj.itemsArray[i + 1] = itemType;
						proj.itemAmountsArray[i + 1] = itemStack;
						proj.itemFrames[i + 1] = itemFrames;
					}
					for (int i = amountOfUniqueItems; i < 19; i++)
					{
						proj.itemsArray[i + 1] = 0;
						proj.itemAmountsArray[i + 1] = 0;
						proj.itemFrames[i + 1] = 0;
					}
					projectile.netUpdate = true;
					//Main.NewText("I am Netmode: " + Main.netMode);
				}
			}
		}
    }
	public class DataTransferProj : ModProjectile
	{
		public int[] itemsArray = new int[20];
		public int[] itemAmountsArray = new int[20];
		public int[] itemFrames = new int[20];
		public override void SendExtraAI(BinaryWriter writer)
		{
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
			base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
		{
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
			base.ReceiveExtraAI(reader);
        }
        public static DataTransferProj ModProjectile(Projectile proj)
		{
			return (DataTransferProj)GetModProjectile(proj.type);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Data Transfer Proj"); //Do you enjoy how all my net sycning is done via projectiles?
		}
		public override void SetDefaults()
		{
			projectile.alpha = 255;
			projectile.timeLeft = 24;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.width = 36;
			projectile.height = 36;
			//projectile.extraUpdates = 4;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
        public override bool PreAI()
        {
			if(projectile.owner == Main.myPlayer)
            {
				projectile.netUpdate = true;
            }
			return true;
        }
        public override void AI()
		{
			projectile.alpha = 255;
			if(projectile.timeLeft < 22)
				projectile.Kill();
		}
		public bool checkArraySame(int[] arr1, int[] arr2)
        {
			if (arr1.Length != arr2.Length)
				return false;

			bool diff = false;
			for(int i = 0; i < arr1.Length; i++)
            {
				if (arr1[i] != arr2[i])
				{
					diff = true;
					break;
				}
            }

			return !diff;
        }
		public override void Kill(int timeLeft)
		{
			TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[(int)projectile.ai[0]];
			if(Main.netMode != 1)
            {
				if (!checkArraySame(entity.itemAmountsArray, itemAmountsArray) || !checkArraySame(entity.itemsArray, itemsArray) || !checkArraySame(entity.itemFrames, itemFrames))
				{
					entity.itemAmountsArray = itemAmountsArray;
					entity.itemsArray = itemsArray;
					entity.itemFrames = itemFrames;
					entity.netUpdate = true;

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
					Vector2 pos = new Vector2((float)(entity.Position.X * 16 + 24), (float)(entity.Position.Y * 16 + 24));
					pos.Y -= 80 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 0.5f;
					Projectile.NewProjectile(pos, Vector2.Zero, mod.ProjectileType("UndoParticles"), 0, 1, Main.myPlayer, entity.Position.X, entity.Position.Y);
				}
            }
		}
	}
	public class ItemUseGlow : GlobalItem
	{
		public Texture2D glowTexture = null;
		public int glowOffsetY = 0;
		public int glowOffsetX = 0;
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;
	}

	public class PlayerUseGlow : ModPlayer
	{
		public static readonly PlayerLayer ItemUseGlow = new PlayerLayer("SOTS", "ItemUseGlow", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(drawPlayer);
			Mod mod = ModLoader.GetMod("SOTS");
			if (drawInfo.shadow != 0)
				return;
			if (!drawPlayer.HeldItem.IsAir)
			{
				Item item = drawPlayer.HeldItem;
				Texture2D texture = item.GetGlobalItem<ItemUseGlow>().glowTexture;
				Vector2 zero2 = Vector2.Zero;

				bool isTwilightPole = item.type == mod.ItemType("TwilightFishingPole") && drawPlayer.ownedProjectileCounts[mod.ProjectileType("TwilightBobber")]> 0;
				if (texture != null && (drawPlayer.itemAnimation > 0 || isTwilightPole))
				{
					Vector2 location = drawInfo.itemLocation;
					if (item.useStyle == 5)
					{
						if (Item.staff[item.type])
						{
							float rotation = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
							int width = 0;
							Vector2 origin = new Vector2(0f, (float)Main.itemTexture[item.type].Height);

							if (drawPlayer.gravDir == -1f)
							{
								if (drawPlayer.direction == -1)
								{
									rotation += 1.57f;
									origin = new Vector2((float)Main.itemTexture[item.type].Width, 0f);
									width -= Main.itemTexture[item.type].Width;
								}
								else
								{
									rotation -= 1.57f;
									origin = Vector2.Zero;
								}
							}
							else if (drawPlayer.direction == -1)
							{
								origin = new Vector2((float)Main.itemTexture[item.type].Width, (float)Main.itemTexture[item.type].Height);
								width -= Main.itemTexture[item.type].Width;
							}

							DrawData value = new DrawData(texture, new Vector2((float)((int)(location.X - Main.screenPosition.X + origin.X + (float)width)), (float)((int)(location.Y - Main.screenPosition.Y))), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height)), Color.White, rotation, origin, item.scale, drawInfo.spriteEffects, 0);
							Main.playerDrawData.Add(value);
						}
						else
						{
							Vector2 vector10 = new Vector2((float)(Main.itemTexture[item.type].Width / 2), (float)(Main.itemTexture[item.type].Height / 2));

							//Vector2 vector11 = this.DrawPlayerItemPos(drawPlayer.gravDir, item.type);
							Vector2 vector11 = new Vector2(10, texture.Height / 2);
							if (item.GetGlobalItem<ItemUseGlow>().glowOffsetX != 0)
							{
								vector11.X = item.GetGlobalItem<ItemUseGlow>().glowOffsetX;
							}
							vector11.Y += item.GetGlobalItem<ItemUseGlow>().glowOffsetY * drawPlayer.gravDir;
							int num107 = (int)vector11.X;
							vector10.Y = vector11.Y;
							Vector2 origin5 = new Vector2((float)(-(float)num107), (float)(Main.itemTexture[item.type].Height / 2));
							if (drawPlayer.direction == -1)
							{
								origin5 = new Vector2((float)(Main.itemTexture[item.type].Width + num107), (float)(Main.itemTexture[item.type].Height / 2));
							}

							//value = new DrawData(Main.itemTexture[item.type], new Vector2((float)((int)(value2.X - Main.screenPosition.X + vector10.X)), (float)((int)(value2.Y - Main.screenPosition.Y + vector10.Y))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height)), item.GetAlpha(color37), drawPlayer.itemRotation, origin5, item.scale, effect, 0);
							//Main.playerDrawData.Add(value);

							Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
							int recurse = 1;
							bool rainbow = item.type == ModContent.ItemType<PhaseCannon>() && modPlayer.rainbowGlowmasks;
							if (rainbow)
							{
								recurse = 2;
							}
							for (int i = 0; i < recurse; i++)
							{
								DrawData value = new DrawData(texture, new Vector2((float)((int)(location.X - Main.screenPosition.X + vector10.X)), (float)((int)(location.Y - Main.screenPosition.Y + vector10.Y))), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height)), rainbow ? color : Color.White, drawPlayer.itemRotation, origin5, item.scale, drawInfo.spriteEffects, 0);
								Main.playerDrawData.Add(value);
							}
						}
					}
					else
					{
						Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
						int recurse = 1;
						if(modPlayer.rainbowGlowmasks)
                        {
							recurse = 2;
                        }
						for (int i = 0; i < recurse; i++)
						{
							DrawData value = new DrawData(texture,
								new Vector2((float)((int)(location.X - Main.screenPosition.X)),
								(float)((int)(location.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)),
								modPlayer.rainbowGlowmasks ? color : Color.White,
								drawPlayer.itemRotation,
								 new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height),
								item.scale,
								drawInfo.spriteEffects,
								0);

							Main.playerDrawData.Add(value);
						}
					}
				}
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int itemLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HeldItem"));
			if (itemLayer != -1)
			{
				ItemUseGlow.visible = true;
				layers.Insert(itemLayer + 1, ItemUseGlow);
			}
		}
	}
}