using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Chaos;
using SOTS.Items.Earth;
using SOTS.Items.Permafrost;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
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
			int seconds = entity.timer / 60;
			int secondsLeft = 60 - seconds;
			if (Main.tile[i, j - 1].TileType == Type)
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.MineralariumTileText.0"));
			}
			else if (entity.timer < 0)
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.MineralariumTileText.1"));
			}
			else
			{
				Main.NewText(Language.GetTextValue("Mods.SOTS.MineralariumTileText.2", secondsLeft));
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
				if (Main.tile[i, j - 1 - k].HasTile && Main.tile[i, j - 1 - k].TileType == ModContent.TileType<MineralariumTile>())
				{
					break;
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
		public int GenerationDuration = -1;
		public bool previousHasDetectedTile = false;
		public Point16? genPos;
		public override void Update()
		{
			int i = this.Position.X + 3;
			int j = this.Position.Y - 1;
			bool isObstructed = false;
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
				return;
			if(!genPos.HasValue || (genPos.HasValue && (Framing.GetTileSafely(genPos.Value).HasTile || !Framing.GetTileSafely(genPos.Value.X, genPos.Value.Y + 1).HasTile)))
            {
				genPos = OreType.findPositionFrom3x3Square(new Point16(i, j));
            }
			if(!genPos.HasValue)
            {
				return;
            }
			if(oreType == -1 && !previousHasDetectedTile && !hasDetectedTile && !isObstructed) //if no ore was detected, there is no current type, and there is no obstruction. Set the ore to a random type
            {
				oreType = OreType.GetRandomType();
				GenerationDuration = OreType.DurationBasedOnType(oreType);
            }
			else if(oreType != -1 && hasDetectedTile && GenerationDuration == -1) //if it already detected an ore, use that ore
            {
				if (oreType == ModContent.TileType<FrigidIceTile>())
					oreType = ModContent.TileType<FrigidIceTileSafe>();
				GenerationDuration = OreType.DurationBasedOnType(oreType);
			}
			if(GenerationDuration <= 0 && genPos.HasValue)
			{
				Projectile.NewProjectile(new EntitySource_Misc("SOTS:Mineralarium"), new Vector2(genPos.Value.X * 16 + 8, genPos.Value.Y * 16 + 8), Vector2.Zero, ModContent.ProjectileType<MineralariumProjectile>(), 0, 0, Main.myPlayer, 0, oreType);
				GenerationDuration = -1;
				//this means it is ready to spawn a new ore
			}
			else
            {
				GenerationDuration -= 5;
				if (GenerationDuration < 0)
					GenerationDuration = 0;
            }
			if(previousHasDetectedTile && !hasDetectedTile && oreType != -1) //if there was a tile previously, but now there are none, and there was a previously saved type
            {
				//this means that all tiles on the platform were broken
				GenerationDuration = -1;
				oreType = -1;
            }
			if(SOTSWorld.GlobalCounter % 5 == 0)
				Main.NewText(oreType + ": " + GenerationDuration + "-- " + isObstructed);
			previousHasDetectedTile = hasDetectedTile;
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
					 || t == TileID.Silver || t == TileID.Tungsten || t == TileID.Gold || t == TileID.Platinum || t == TileID.Meteorite || t == TileID.Demonite || t == TileID.Crimtane
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
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
			Vector2 position = Projectile.Center;
			for (int k = 0; k < 360; k += 15)
			{
				Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(k));
				circularLocation += new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
				int type = ModContent.DustType<PixelDust>();
				Dust dust = Dust.NewDustDirect(new Vector2(position.X + circularLocation.X - 4, position.Y + circularLocation.Y - 4), 4, 4, type, 0, 0, 0, color);
				dust.noGravity = true;
				dust.velocity = circularLocation;
				dust.fadeIn = 5;
			}
		}
	}
}