using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Chaos;
using SOTS.Items.Earth;
using SOTS.Items.Permafrost;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.Utilities;
using static SOTS.Items.Furniture.Functional.MineralariumTE;

namespace SOTS.Items.Furniture.Functional
{
	public class Mineralarium : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 48;
			Item.height = 42;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<MineralariumTile>();
		}
	}
	public class MineralariumTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.Width = 7;
			TileObjectData.newTile.Origin = new Point16(3, 0);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<MineralariumTE>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(SOTSTile.EarthenPlatingColor, name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.Iron;
		}
        public override bool CanPlace(int i, int j)
        {
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);
			if(tileBelow.TileType == Type)
            {
				if(tileBelow.TileFrameX != 54)
                {
					return false;
                }
            }
			if(OreType.CountsAsOre(tileBelow.TileType))
            {
				return false;
            }
            return base.CanPlace(i, j);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = ModContent.ItemType<Mineralarium>();
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 112, 16, drop);
			ModContent.GetInstance<MineralariumTE>().Kill(i, j);
		}
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail || effectOnly)
				return;
			Tile tile = Main.tile[i, j];
			i -= tile.TileFrameX / 18;
			i += 3;
			j -= 1;
			for (int k = -1; k <= 1; k++)
			{
				for (int h = 0; h >= -2; h--)
				{
					Point16 tilePos = new Point16(i + k, j + h);
					tile = Framing.GetTileSafely(tilePos);
					if (tile.HasTile)
					{
						if ((OreType.CountsAsOre(tile.TileType) && h >= -2))
						{
							WorldGen.KillTile(i + k, j + h, false, false, false);
							NetMessage.SendData(MessageID.TileManipulation, Main.myPlayer, Main.myPlayer, null, 0, i + k, j + h, 0f, 0, 0, 0);
						}
					}
				}
			}
		}
        public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.cursorItemIconID = ModContent.ItemType<Mineralarium>();
			//player.cursorItemIconText = "";
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
		public override bool RightClick(int i, int j)
		{
			Main.mouseRightRelease = true;
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;

			int index = ModContent.GetInstance<MineralariumTE>().Find(left, top);
			if (index == -1)
			{
				return false;
			}
			MineralariumTE entity = (MineralariumTE)TileEntity.ByID[index];
			float percent = 1f - entity.GenerationDuration / (float)entity.GenDurationMAX;
			percent = Math.Clamp(percent, 0, 1);
			int percentToString = (int)(percent * 100);
			if (Main.tile[i, j - 1].TileType == Type)
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.MineralariumTileText.0"), ColorHelpers.EarthColor);
			}
			else if (entity.isObstructed)
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.MineralariumTileText.1"), Color.Red);
			}
			else
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.MineralariumTileText.2", percentToString), Color.Lerp(Color.OrangeRed, Color.LimeGreen, percent));
			}
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.5f;
			g = 0.4f;
			b = 0.3f;
		}
		public static void Draw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.TileFrameX / 18;
			int top = j - tile.TileFrameY / 18;
			int index = ModContent.GetInstance<MineralariumTE>().Find(left, top);
			if (index == -1)
			{
				return;
			}
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			MineralariumTE entity = (MineralariumTE)TileEntity.ByID[index];
			Texture2D texture = SOTSTile.GetTileDrawTexture(i, j); //hopefully should get paint properly
			Texture2D textureGlow = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Furniture/Functional/MineralariumTileGlow");
			int frameOffset = 0;
			bool hasOtherMineralarium = false;
			for (int k = 0; k < 5; k++)
			{
				Color color = Lighting.GetColor(i, j - k, Color.White);
				if (k == 1)
					frameOffset = 74;
				if (k == 2)
					frameOffset = 56;
				if (k == 3)
					frameOffset = 38;
				if (k == 4)
					frameOffset = 20;
				Vector2 pos = new Vector2(i * 16, (j - k) * 16) - Main.screenPosition + zero;
				spriteBatch.Draw(texture, pos, new Rectangle(tile.TileFrameX, tile.TileFrameY + frameOffset, 16, k == 0 ? 18 : 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(textureGlow, pos, new Rectangle(tile.TileFrameX, tile.TileFrameY + frameOffset, 16, k == 0 ? 18 : 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				if (Main.tile[i, j - 1 - k].HasTile && Main.tile[i, j - 1 - k].TileType == ModContent.TileType<MineralariumTile>() && Main.tile[i, j - 1 - k].TileFrameX == tile.TileFrameX)
				{
					hasOtherMineralarium = true;
					break;
				}
			}
			if(tile.TileFrameX == 108 && (!hasOtherMineralarium || !entity.isObstructed || entity.fakeObstructed))
			{
				Color lightVisuals = ColorHelpers.EarthColor;
				lightVisuals.A = 0;
				left += 3;
				top -= 3;
				bool hasValue = false;
				int bonusHeight = -1;
				float progress = 0;
				float size = 0;
				float scaleX;
				float firstProgress = 1;
				Vector2 blockPosition = new Vector2(left, top) * 16 + new Vector2(8, 8);
				Texture2D textureLine = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraBorder");
				Texture2D textureLineFade = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/LongGradient");
				if (entity.genPos.HasValue && !entity.isObstructed)
				{
					bonusHeight = 0;
					progress = 1 - entity.GenerationDuration / (float)entity.GenDurationMAX;
					blockPosition = entity.genPos.Value.ToVector2() * 16 + new Vector2(8, 8);
					firstProgress = (entity.GenDurationMAX - entity.GenerationDuration) / 120f;
					firstProgress = MathHelper.Clamp(firstProgress, 0, 1);
					progress *= firstProgress;
					size = 14 * progress;
					scaleX = size / textureLine.Width;
					Vector2 origin = new Vector2(0, 1);
					for (int r = 0; r < 4; r++)
					{
						Vector2 position = new Vector2(-size / 2 - 1, size / 2).RotatedBy(r * MathHelper.PiOver2);
						spriteBatch.Draw(textureLine, blockPosition - Main.screenPosition + zero + position, null, lightVisuals * firstProgress, r * MathHelper.PiOver2, origin, new Vector2(scaleX, 1f), SpriteEffects.None, 0);
					}
					hasValue = true;
					if(entity.oreType > 0)
                    {
						if(!Terraria.GameContent.TextureAssets.Tile[entity.oreType].IsLoaded)
						{
							Main.instance.LoadTiles(entity.oreType);
						}
						Texture2D textureBasedOnOre = Terraria.GameContent.TextureAssets.Tile[entity.oreType].Value;
						int xFrame = 9; //10 or 11
						if(SOTSWorld.GlobalCounter % 600 < 200)
							xFrame = 10;
						if (SOTSWorld.GlobalCounter % 600 >= 400)
							xFrame = 11;
						int yFrame = 3;
						Rectangle oreFrame = new Rectangle(xFrame * 18, yFrame * 18, 16, 16);
						spriteBatch.Draw(textureBasedOnOre, blockPosition - Main.screenPosition + zero, oreFrame, Color.Lerp(lightVisuals * firstProgress * 1.5f, Color.White, progress * progress * 0.64f), 0, new Vector2(8, 8), progress, SpriteEffects.None, 0);
					}
				}
				Texture2D texturePointer = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Furniture/Functional/MineralariumPointer");
				Texture2D texturePointerG = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Furniture/Functional/MineralariumPointerGlow");
				for (int direction = -1; direction <= 1; direction += 2)
				{
					Vector2 origin = new Vector2(texturePointer.Width / 2, texturePointer.Height / 2 + bonusHeight);
					Vector2 center = new Vector2(left * 16, blockPosition.Y) + new Vector2(8, 0) + new Vector2(32 * direction, 0);
					Vector2 centerToBlock = (blockPosition - center).SafeNormalize(Vector2.Zero);
					if (hasValue)
					{
						for (int m = 0; m <= 4; m++)
						{
							int mod = m == 0 ? 0 : 1;
							float determinedSize = size / 2 + 1;
							Vector2 position = new Vector2(determinedSize * 1.42f, determinedSize * 1.42f).RotatedBy(MathHelper.TwoPi * 2 * progress * mod + m * MathHelper.TwoPi / 4) * direction;
							position.X = Math.Clamp(position.X, -determinedSize, determinedSize);
							position.Y = Math.Clamp(position.Y, -determinedSize, determinedSize);
							position.Y *= mod;
							Vector2 cornerPos = blockPosition + position;
							Vector2 lightBeamPosition = center - new Vector2(6 * direction, 0);
							Vector2 toCorner = cornerPos - lightBeamPosition;
							float dist = toCorner.Length();
							scaleX = dist / textureLineFade.Width;
							for(int n = 0; n < 2; n++)
								spriteBatch.Draw(textureLineFade, lightBeamPosition - Main.screenPosition + zero, null, lightVisuals * (0.675f - mod * 0.45f) * firstProgress, toCorner.ToRotation(), new Vector2(0, 1), new Vector2(scaleX, 1f), SpriteEffects.None, 0);
						}
					}
					float rotation = centerToBlock.ToRotation();
					spriteBatch.Draw(texturePointer, center - Main.screenPosition + zero, null, Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), rotation + (direction == 1 ? MathHelper.Pi : 0), origin, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
					spriteBatch.Draw(texturePointerG, center - Main.screenPosition + zero, null, Color.White, rotation + (direction == 1 ? MathHelper.Pi : 0), origin, 1f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
					//dust:
					if(hasValue && !Main.gamePaused)
					{
						if(Main.rand.NextBool(8))
						{
							Dust dust = Dust.NewDustDirect(center - new Vector2(5 * direction, 0) - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, lightVisuals, Main.rand.NextFloat(0.7f, 0.9f));
							dust.noGravity = true;
							dust.velocity = new Vector2(direction * Main.rand.NextFloat(0.5f, 1.5f), Main.rand.NextFloat(-1, 1) * 0.7f) * 0.25f;
							dust.fadeIn = 10;
						}
						if(Main.rand.NextBool(5))
						{
							Dust dust = Dust.NewDustDirect(blockPosition - new Vector2((size / 2 + 5) * direction, 0) - new Vector2(5, 5 + Main.rand.NextFloat(-1, 1) * size / 2), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, lightVisuals, Main.rand.NextFloat(0.8f, 1f + size / 30f));
							dust.noGravity = true;
							dust.velocity = new Vector2(direction * (0.5f + size / 24f), Main.rand.NextFloat(-1, 1) * 0.35f);
							dust.fadeIn = 8;
						}
					}
				}

				Texture2D textureScreen = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Furniture/Functional/MineralariumTileScreen");
				for (int b = 0; b < 2; b++)
                {
					Rectangle slice = new Rectangle(0, textureScreen.Height / 2 * b, textureScreen.Width, textureScreen.Height / 2);
					Vector2 origin = textureScreen.Size() / 2;
					float sliceWidth = textureScreen.Width;
					if (b == 0)
						sliceWidth *= 2 * (1f - progress);
					else
						sliceWidth *= 4 * progress;
					sliceWidth %= textureScreen.Width;
					Rectangle secondSlice = new Rectangle((int)sliceWidth, slice.Y, slice.Width - (int)sliceWidth, slice.Height);
					slice = new Rectangle(slice.X, slice.Y, (int)sliceWidth, slice.Height);
					for (int k = 0; k < 2; k++)
					{
						Vector2 start = Vector2.Zero;
						if (k == 0)
							start.X += secondSlice.Width;
						if (k == 1)
							start.X += 0;
						spriteBatch.Draw(textureScreen, new Vector2(i - 3, j) * 16 + new Vector2(8, 10 + b * 4) - Main.screenPosition + zero + start, k == 0 ? slice : secondSlice, Color.White, 0, origin, 1f, SpriteEffects.None, 0);
					}
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Draw(i, j, spriteBatch);
			return false;
		}
	}
	public class MineralariumTE : ModTileEntity
	{
		internal int timer = -2;
		public int oreType = -1;
		public int GenDurationMAX = -1;
		public int GenerationDuration = -1;
		public bool previousHasDetectedTile = false;
		public bool isObstructed = false;
		public bool fakeObstructed = false;
		public Point16? genPos;
		public override void Update()
		{
			int genSpeed = GetSpeedFromBelowGenerators();
			int i = this.Position.X + 3;
			int j = this.Position.Y - 1;
			isObstructed = false;
			fakeObstructed = false;
			bool hasDetectedTile = false;
			int tempOreType = -1;
			for(int k = -1; k <= 1; k++)
            {
				for(int h = 0; h >= -2; h--)
                {
					Point16 tilePos = new Point16(i + k, j + h);
					Tile tile = Framing.GetTileSafely(tilePos);
					if(tile.HasTile)
                    {
						if (OreType.CountsAsOre(tile.TileType))
						{
							if (tempOreType == -1)
							{
								hasDetectedTile = true;
								tempOreType = tile.TileType;
							}
							else if (tempOreType != tile.TileType)
								isObstructed = true;
						}
						else
						{
							isObstructed = true;
                        }
                    }
                }
            }
			if(hasDetectedTile)
				oreType = tempOreType;
			if (isObstructed)
			{
				return;
			}
			if(!genPos.HasValue || (genPos.HasValue && (Framing.GetTileSafely(genPos.Value).HasTile || !Framing.GetTileSafely(genPos.Value.X, genPos.Value.Y + 1).HasTile)))
            {
				genPos = OreType.findPositionFrom3x3Square(new Point16(i, j));
            }
			if(!genPos.HasValue)
            {
				fakeObstructed = true;
				isObstructed = true;
				return;
            }
			if(oreType == -1 && !previousHasDetectedTile && !hasDetectedTile && !isObstructed) //if no ore was detected, there is no current type, and there is no obstruction. Set the ore to a random type
            {
				oreType = OreType.GetRandomType();
				GenerationDuration = GenDurationMAX = OreType.DurationBasedOnType(oreType);
            }
			else if(oreType != -1 && hasDetectedTile && GenerationDuration == -1) //if it already detected an ore, use that ore
            {
				if (oreType == ModContent.TileType<FrigidIceTile>())
					oreType = ModContent.TileType<FrigidIceTileSafe>();
				GenerationDuration = GenDurationMAX = OreType.DurationBasedOnType(oreType);
			}
			if(GenerationDuration <= 0 && genPos.HasValue)
			{
				Projectile.NewProjectile(new EntitySource_Misc("SOTS:Mineralarium"), new Vector2(genPos.Value.X * 16 + 8, genPos.Value.Y * 16 + 8), Vector2.Zero, ModContent.ProjectileType<MineralariumProjectile>(), 0, 0, Main.myPlayer, 0, oreType);
				GenerationDuration = GenDurationMAX = - 1;
				//this means it is ready to spawn a new ore
			}
			else
            {
				GenerationDuration -= genSpeed;
				if (GenerationDuration < 0)
					GenerationDuration = 0;
            }
			if(previousHasDetectedTile && !hasDetectedTile && oreType != -1) //if there was a tile previously, but now there are none, and there was a previously saved type
            {
				//this means that all tiles on the platform were broken
				GenerationDuration = GenDurationMAX = - 1;
				oreType = -1;
            }
			//if(SOTSWorld.GlobalCounter % 5 == 0)
			//	Main.NewText(oreType + ": " + GenerationDuration + "-- " + isObstructed);
			previousHasDetectedTile = hasDetectedTile;
		}
		public int GetSpeedFromBelowGenerators()
		{
			int i = this.Position.X + 3;
			int amt = 0;
			for (int a = 0; a < 3600; a++)
			{
				if (WorldGen.InWorld(i, Position.Y + a, 10) && 
					Main.tile[i, Position.Y + a].HasTile && 
					Main.tile[i, Position.Y + a].TileType == ModContent.TileType<MineralariumTile>())
				{
					amt++;
				}
				else
				{
					break;
				}
			}
			return amt;
		}
		public override void NetReceive(BinaryReader reader)
		{
			timer = reader.ReadInt32();
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(timer);
		}
		public override void SaveData(TagCompound tag)/* Edit tag parameter rather than returning new TagCompound */
		{
			tag["timer"] = timer;
		}
		public override void LoadData(TagCompound tag)
		{
			timer = tag.Get<int>("timer");
		}
		public override bool IsTileValidForEntity(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.HasTile && tile.TileType == (ushort)ModContent.TileType<MineralariumTile>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
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
		public static class OreType
		{
			public static int DurationBasedOnType(int oreID)
			{
				if (oreID == TileID.Copper)
					return 300;
				if (oreID == TileID.Tin)
					return 330;
				if (oreID == TileID.Iron)
					return 360;
				if (oreID == TileID.Lead)
					return 390;
				if (oreID == TileID.Silver)
					return 420;
				if (oreID == TileID.Tungsten)
					return 450;
				if (oreID == TileID.Gold)
					return 500;
				if (oreID == TileID.Platinum)
					return 570;
				if (oreID == TileID.Meteorite)
					return 600;
				if (oreID == TileID.Demonite)
					return 650;
				if (oreID == TileID.Crimtane)
					return 666;
				if (oreID == TileID.Obsidian)
					return 600;
				if (oreID == TileID.Hellstone)
					return 800;
				if (oreID == TileID.Cobalt)
					return 1000;
				if (oreID == TileID.Palladium)
					return 1050;
				if (oreID == TileID.Mythril)
					return 1200;
				if (oreID == TileID.Orichalcum)
					return 1250;
				if (oreID == TileID.Adamantite)
					return 1400;
				if (oreID == TileID.Titanium)
					return 1450;
				if (oreID == TileID.Chlorophyte)
					return 1150;
				if (oreID == ModContent.TileType<FrigidIceTile>() || oreID == ModContent.TileType<FrigidIceTileSafe>())
				{
					return 525;
				}
				if (oreID == ModContent.TileType<VibrantOreTile>())
					return 425;
				if (oreID == ModContent.TileType<PhaseOreTile>())
					return 1525;
				if (oreID == TileID.LunarOre)
					return 2000;
				return -1;
			}
			public static int GetRandomType()
            {
				WeightedRandom<int> types = new WeightedRandom<int>();
				types.Add(TileID.Copper, 0.2);
				types.Add(TileID.Tin, 0.2);
				types.Add(TileID.Iron, 0.25);
				types.Add(TileID.Lead, 0.25);
				types.Add(TileID.Silver, 0.3);
				types.Add(TileID.Tungsten, 0.3);
				types.Add(TileID.Gold, 0.35);
				types.Add(TileID.Platinum, 0.35);
				types.Add(TileID.Demonite, 0.5);
				types.Add(TileID.Crimtane, 0.5);
				types.Add(TileID.Obsidian, 0.2);
				types.Add(TileID.Meteorite, 0.5);
				if(NPC.downedBoss1)
					types.Add(ModContent.TileType<VibrantOreTile>(), 0.6);
				if (NPC.downedBoss2)
					types.Add(ModContent.TileType<FrigidIceTile>(), 0.75);
				if (NPC.downedBoss3 || SOTSWorld.downedAdvisor)
					types.Add(TileID.Hellstone, 1);
				if(Main.hardMode)
                {
					types.Add(TileID.Cobalt, 0.6);
					types.Add(TileID.Palladium, 0.6);
					types.Add(TileID.Mythril, 0.65);
					types.Add(TileID.Orichalcum, 0.65);
					if(NPC.downedMechBossAny)
					{
						types.Add(TileID.Adamantite, 0.7);
						types.Add(TileID.Titanium, 0.7);
					}
					if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
					{
						types.Add(TileID.Chlorophyte, 1.25);
					}
					if(SOTSWorld.downedLux)
                    {
						types.Add(ModContent.TileType<PhaseOreTile>(), 1.5);
					}
					if (NPC.downedMoonlord)
					{
						types.Add(TileID.LunarOre, 2);
					}
				}
				return types.Get();
            }
			public static bool CountsAsOre(int t)
            {
				return t == TileID.Copper || t == TileID.Tin || t == TileID.Iron || t == TileID.Lead
					 || t == TileID.Silver || t == TileID.Tungsten || t == TileID.Gold || t == TileID.Platinum || t == TileID.Meteorite || t == TileID.Demonite || t == TileID.Crimtane || t == TileID.Obsidian
					 || t == TileID.Hellstone || t == TileID.Cobalt || t == TileID.Palladium || t == TileID.Mythril || t == TileID.Orichalcum || t == TileID.Adamantite || t == TileID.Titanium
					 || t == TileID.Chlorophyte || t == TileID.LunarOre || t == ModContent.TileType<FrigidIceTile>() || t == ModContent.TileType<FrigidIceTileSafe>() || t == ModContent.TileType<FrigidIceTile>()
					 || t == ModContent.TileType<PhaseOreTile>() || t == ModContent.TileType<VibrantOreTile>();
			}
			public static Point16? findPositionFrom3x3Square(Point16 center)
			{
				int i = center.X;
				int j = center.Y;
				WeightedRandom<Point16> availablePositions = new WeightedRandom<Point16>();
				for (int k = -1; k <= 1; k++)
				{
					for (int h = 0; h >= -2; h--)
					{
						Point16 tilePos = new Point16(i + k, j + h);
						Tile tile = Framing.GetTileSafely(tilePos);
						if (!tile.HasTile)
						{
							if(Framing.GetTileSafely(i, j + 1).HasTile)
                            {
								availablePositions.Add(new Point16(i + k, j + h), 1.5 + h * 0.65);
							}
						}
					}
				}
				if(availablePositions.elements.Count > 0)
                {
					return availablePositions.Get();
                }
				return null;
			}
		}
	}
	public class MineralariumProjectile : ModProjectile
	{
        public override string Texture => "SOTS/Items/Furniture/Functional/Mineralarium";
        public override void SetDefaults() //Do you enjoy how all my net sycning is done via projectiles?
		{
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.width = 26;
			Projectile.height = 36;
			Projectile.hide = true;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override void AI()
		{
			Projectile.alpha = 255;
			Projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			int tileID = (int)Projectile.ai[1];
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			Color color = ColorHelpers.EarthColor;
			color.A = 0;
			WorldGen.PlaceTile(i, j, tileID, false, true, -1, 0);
			WorldGen.KillTile(i, j, true, true);
			WorldGen.KillTile(i, j, true, true);
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
			SOTSUtils.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.Center, 0.9f, 0.1f);
			Vector2 position = Projectile.Center;
			for (int k = 0; k < 360; k += 12)
			{
				Vector2 circularLocation = new Vector2(-3.25f * Main.rand.NextFloat(0.2f, 1.0f), 0).RotatedBy(MathHelper.ToRadians(k));
				circularLocation += new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * 0.8f;
				int type = ModContent.DustType<PixelDust>();
				Dust dust = Dust.NewDustDirect(new Vector2(position.X + circularLocation.X - 12, position.Y + circularLocation.Y - 12), 16, 16, type, 0, 0, 0, color);
				dust.noGravity = true;
				dust.velocity = circularLocation;
				dust.fadeIn = 6;
			}
		}
	}
}