using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Items.Fragments;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SOTS.Items.Furniture.Functional
{
    public class NatureHydroponics : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(34, 30);
            Item.rare = ItemRarityID.Orange;
            Item.createTile = ModContent.TileType<Hydroponics>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingNature>(), 1).AddIngredient(ItemID.HerbBag, 1).AddIngredient(ItemID.DirtBlock, 40).AddIngredient(ModContent.ItemType<NaturePlating>(), 40).AddTile(TileID.Anvils).Register();
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingNature>(), 1).AddRecipeGroup("SOTS:AlchSeeds", 20).AddIngredient(ItemID.DirtBlock, 40).AddIngredient(ModContent.ItemType<NaturePlating>(), 40).AddTile(TileID.Anvils).Register();
        }
    }
    public class Hydroponics : ModTile
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 18 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 6, 0);
            TileObjectData.newTile.Origin = new Point16(3, 5);
            TileObjectData.addTile(Type);
            DustType = DustID.Tungsten;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(SOTSTile.NaturePlatingColor, name);
            TileID.Sets.DisableSmartCursor[Type] = true;
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return false;
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            GetTopLeft(ref i, ref j);
            noBreak = true;
            resetFrame = false;
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    Tile tile = Framing.GetTileSafely(i + x, j + y);
                    if (y == 6)
                    {
                        if (!SOTSWorldgenHelper.TileTopCapable(i + x, j + y))
                        {
                            noBreak = false;
                            break;
                        }
                    }
                    else if (tile != null && (!tile.HasTile || tile.TileType != ModContent.TileType<Hydroponics>()))
                    {
                        noBreak = false;
                        break;
                    }
                }
            }
            if (!noBreak)
                Kill(i, j);
            return false;
        }
        public static void Kill(int i, int j)
        {
            Tile tileTL = Framing.GetTileSafely(i, j);
            if (tileTL.TileFrameX == 0 && tileTL.TileFrameY == 0 && tileTL.TileType == ModContent.TileType<Hydroponics>() && tileTL.HasTile)
            {
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        if (x >= 1 && x <= 4 && y % 2 == 0)
                            DropPlant(i + x, j + y);
                        WorldGen.KillTile(i + x, j + y, false, false, true);
                        NetMessage.SendData(MessageID.TileManipulation, Main.myPlayer, Main.myPlayer, null, 0, i + x, j + y, 0f, 0, 0, 0);
                    }
                }
            }
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            type = Main.rand.NextBool(3) ? DustID.Dirt : DustType;
            return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 2;
        }
        public override void RandomUpdate(int i, int j)
        {
            GetTopLeft(ref i, ref j);
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (x >= 1 && x <= 4 && y % 2 == 0 && WorldGen.genRand.NextBool(240))
                    {
                        GrowPlant(i + x, j + y);
                        NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
                    }
                }
            }
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            PlaceInWorldStatic(i, j, item);
        }
        public static void PlaceInWorldStatic(int i, int j, Item item)
        {
            GetTopLeft(ref i, ref j);
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    Tile growTile = Main.tile[i + x, j + y];
                    if (x >= 1 && x <= 4 && y % 2 == 0 && growTile.TileType == ModContent.TileType<Hydroponics>())
                    {
                        SetPlant(i + x, j + y, Main.rand.Next(HydroponicsHerbSystem.Herbs.Count));
                        NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
                    }
                }
            }
        }

        public static void SetPlant(int i, int j, int type)
        {
            Tile tile = Main.tile[i, j];
            int count = HydroponicsHerbSystem.Herbs.Count;
            if (count <= 0) type = 0;
            else type = Utils.Clamp(type, 0, count - 1);

            tile.TileFrameX = (short)(18 * (type + 1));
            tile.TileFrameY = 18;
        }

        private static int GetPlantStyle(Tile tile) => tile.TileFrameX / 18 - 1;
        private static int GetGrowthStage(Tile tile) => tile.TileFrameY / 18 - 1;

        public static void GrowPlant(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY < 72)
                tile.TileFrameY += 18;
        }
        public static bool DropPlant(int i, int j)
        {

            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY != 72) // If a herb isn't fully grown...
                return false;

            int style = GetPlantStyle(tile);
            if (!HydroponicsHerbSystem.TryGet(style, out var herb))
                return false;

            int itemType = herb.herbType;
            if (itemType <= 0)
                return false;

            int item = Item.NewItem(
                new EntitySource_TileInteraction(Main.LocalPlayer, i, j),
                i * 16, j * 16, 16, 16, itemType, 1);

            NetMessage.SendData(MessageID.SyncItem, Main.myPlayer, Main.myPlayer, null, item);

            // Replant a random plant
            SetPlant(i, j, Main.rand.Next(HydroponicsHerbSystem.Herbs.Count));
            NetMessage.SendTileSquare(Main.myPlayer, i, j, 1, TileChangeType.None);

            SOTSUtils.PlaySound(SoundID.Grass, i * 16, j * 16, 1f, 0f);
            for (int a = 0; a < 10; a++)
                Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, herb.harvestDust);

            return true;
        }
        public static void GetTopLeft(ref int i, ref int j)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0 && tile.TileType == ModContent.TileType<Hydroponics>())
                return;
            if (tile.TileFrameX != 0)
            {
                for (int k = 0; k < 6; k++)
                {
                    i--;
                    tile = Main.tile[i, j];
                    if (tile.TileFrameX == 0)
                    {
                        break;
                    }
                }
            }
            if (tile.TileFrameY != 0)
            {
                for (int k = 0; k < 6; k++)
                {
                    j--;
                    tile = Main.tile[i, j];
                    if (tile.TileFrameY == 0)
                    {
                        break;
                    }
                }
            }
        }
        public override bool RightClick(int i, int j)
        {
            GetTopLeft(ref i, ref j);
            for (int y = 0; y < 6; y += 2)
            {
                for (int x = 1; x <= 4; x++)
                {
                    Tile growTile = Main.tile[i + x, j + y];
                    if (x >= 1 && x <= 4 && y % 2 == 0 && growTile.TileType == Type)
                    {
                        if (growTile.TileFrameY > 72)
                            SetPlant(i + x, j + y, Main.rand.Next(HydroponicsHerbSystem.Herbs.Count));
                        DropPlant(i + x, j + y);
                    }
                }
            }
            return false;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            //Main.SmartInteractTileCoordsSelected
            Tile tile = Main.tile[i, j];
            bool topLeft = tile.TileFrameY == 0 && tile.TileFrameX == 0;
            if (topLeft) //check for it being the top left tile
            {
                //int highlightCapable = -1;
                Texture2D texture = SOTSTile.GetTileDrawTexture(i, j); //hopefully should get paint properly
                Texture2D textureGlow = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Furniture/Functional/HydroponicsGlow");
                for (int layer = 0; layer < 2; layer++)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        for (int x = 0; x < 6; x++)
                        {
                            Vector2 drawOffset = new Vector2(16 * x, 16 * y);
                            Vector2 drawPosition = new Vector2(i * 16, j * 16) + zero - Main.screenPosition + drawOffset;
                            int frameHeight = 16;
                            if (y == 5)
                            {
                                frameHeight = 18;
                            }
                            Color lightColor = Lighting.GetColor(i + x, j + y);
                            Rectangle frame = new Rectangle(18 * x, 18 * y, 16, frameHeight);
                            if (layer == 0)
                            {
                                spriteBatch.Draw(texture, drawPosition, frame, lightColor, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
                                spriteBatch.Draw(textureGlow, drawPosition, frame, Color.White, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
                            }
                            else if (layer == 1)
                            {
                                /*if (highlightCapable != -1)
									DrawHighlight(highlightCapable == 2, drawPosition, frame, spriteBatch, lightColor);*/
                                if (x >= 1 && x <= 4 && y % 2 == 0)
                                {
                                    Tile growTile = Main.tile[i + x, j + y];
                                    int growthStage = growTile.TileFrameY / 18 - 1;
                                    if (growthStage > 0)
                                    {
                                        int style = GetPlantStyle(growTile);

                                        int direction = ((i + x) % 2 * 2 - 1);
                                        drawPosition.Y -= 6;

                                        if (HydroponicsHerbSystem.TryGet(style, out var herb))
                                        {
                                            if (herb.vanillaDraw) // if it's a vanilla plant...
                                            {
                                                if (!TextureAssets.Tile[81 + growthStage].IsLoaded)
                                                    Main.instance.LoadTiles(81 + growthStage);

                                                Texture2D tex = TextureAssets.Tile[81 + growthStage].Value;

                                                if (style == 1) drawPosition.X += 4 * direction;
                                                if (growthStage == 3) DoDust(i + x, j + y, style, ref lightColor);

                                                spriteBatch.Draw(
                                                    tex, drawPosition, new Rectangle(style * 18, 0, 16, 20),
                                                    lightColor, 0f, default, 1f,
                                                    direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                                                    0f);
                                            }
                                            else // otherwise, it's a modded plant
                                            {
                                                Rectangle src = herb.GetSourceRect(growthStage);

                                                spriteBatch.Draw(
                                                    TextureAssets.Tile[herb.tileType].Value, drawPosition, src, lightColor, 0f, default, 1f,
                                                    direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                                                    0f);

                                                //herb.ambientDust?.Invoke(i + x, j + y, growthStage);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                /*if (highlightCapable != -1)
					for(int a = 0; a < Main.SmartInteractTileCoordsSelected.Count; a++)
					{
						spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Fragments/NaturePlating"), Main.SmartInteractTileCoordsSelected[a].ToVector2() * 16 - Main.screenPosition + zero, null, Color.White * 0.5f, 0f, default(Vector2), 0.5f, SpriteEffects.None, 0f);
					}*/
            }
            return false;
        }
        public void DoDust(int i, int j, int type, ref Color color1)
        {
            if (type == 0 && Main.rand.NextBool(100))
            {
                int dust = Dust.NewDust(new Vector2(i * 16, (float)(j * 16 - 4)), 16, 16, DustID.Sunflower, 0.0f, 0.0f, 160, new Color(), 0.1f);
                Main.dust[dust].velocity.X /= 2f;
                Main.dust[dust].velocity.Y /= 2f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 1f;
            }
            if (type == 1 && Main.rand.NextBool(100))
                Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.GlowingMushroom, 0.0f, 0.0f, 250, new Color(), 0.8f);
            if (type == 3)
            {
                if (Main.rand.NextBool(200))
                {
                    int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.Demonite, 0.0f, 0.0f, 100, new Color(), 0.2f);
                    Main.dust[dust].fadeIn = 1.2f;
                }

                if (Main.rand.NextBool(75))
                {
                    int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.Shadowflame);
                    Main.dust[dust].velocity.X /= 2f;
                    Main.dust[dust].velocity.Y /= 2f;
                }
            }
            if (type == 4 && Main.rand.NextBool(150))
            {
                int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 8, DustID.Cloud);
                Main.dust[dust].velocity.X /= 3f;
                Main.dust[dust].velocity.Y /= 3f;
                Main.dust[dust].velocity.Y -= 0.7f;
                Main.dust[dust].alpha = 50;
                Main.dust[dust].scale *= 0.1f;
                Main.dust[dust].fadeIn = 0.9f;
                Main.dust[dust].noGravity = true;
            }
            if (type == 5)
            {
                if (Main.rand.NextBool(40))
                {
                    int dust = Dust.NewDust(new Vector2(i * 16, (float)(j * 16 - 6)), 16, 16, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[dust].velocity.Y -= 2f;
                    Main.dust[dust].noGravity = true;
                }
                color1.A = (byte)(Main.mouseTextColor / 2U);
                color1.G = Main.mouseTextColor;
                color1.B = Main.mouseTextColor;
            }
            if (type == 6)
            {
                if (Main.rand.NextBool(30))
                {
                    var newColor = new Color(50, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    int dust = Dust.NewDust(new Vector2((i * 16), (j * 16)), 16, 16, DustID.TintableDustLighted, 0.0f, 0.0f, 254, newColor, 0.5f);
                    Main.dust[dust].velocity *= 0.0f;
                }
                var num9 = (byte)((Main.mouseTextColor + color1.G * 2) / 3);
                var num10 = (byte)((Main.mouseTextColor + color1.B * 2) / 3);
                if (num9 > color1.G)
                    color1.G = num9;
                if (num10 > color1.B)
                    color1.B = num10;
            }
        }
        /*public void DrawHighlight(bool selected, Vector2 drawPosition, Rectangle frame, SpriteBatch spriteBatch, Color lightColor)
		{
			int ave = ((int)lightColor.R + (int)lightColor.G + (int)lightColor.B) / 3;
			if (ave > 10)
			{
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>(HighlightTexture);
				Color drawColor = !selected ? new Color(ave / 2, ave / 2, ave / 2, ave) : new Color(ave, ave, ave / 3, ave);
				spriteBatch.Draw(texture2, drawPosition, frame, drawColor, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
			}
		}*/
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0;
            g = 0;
            b = 0;
            int top = j;
            int left = i;
            GetTopLeft(ref left, ref top);
            Tile growTile = Main.tile[i, j];
            int x = i - left;
            int y = j - top;
            if (x >= 1 && x <= 4 && y % 2 == 0 && growTile.TileType == Type)
            {
                if (growTile.TileFrameY == 72)
                {
                    int style = GetPlantStyle(growTile);

                    if (HydroponicsHerbSystem.TryGet(style, out var temp) && temp.lightColor != null)
                    {
                        var tempCol = temp.lightColor(i, j, 3);
                        if (tempCol is not null)
                        {
                            // Only overwrite a value if it's already present
                            if (tempCol.Value.i is float rr) r = rr;
                            if (tempCol.Value.j is float gg) g = gg;
                            if (tempCol.Value.type is float bb) b = bb;
                        }
                        return;
                    }
                }
            }
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                if (!noItem)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        for (int x = 0; x < 6; x++)
                        {
                            if (y > 0 && x > 0)
                            {
                                WorldGen.KillTile(i + x, j + y, false, false, true);
                                NetMessage.SendData(MessageID.TileManipulation, Main.myPlayer, Main.myPlayer, null, 0, i + x, j + y, 0f, 0, 0, 0);
                            }
                        }
                    }
                }
                else
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 96, 96, ModContent.ItemType<NatureHydroponics>());
                noItem = true;
            }
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ModContent.ItemType<NatureHydroponics>();
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
                player.cursorItemIconID = ItemID.None;
            }
        }
    }

    public class HydroponicsHerbSystem : ModSystem
    {
        public sealed class HydroponicsHerb
        {
            #region General
            public string key; // The identifier for a herb, such as "ExampleMod:ExampleHerb"
            public int tileType = TileID.BloomingHerbs; // The default tile for herbs
            public int herbType; // The Herb that's dropped upon harvest
            public int harvestDust = DustID.GrassBlades; // Dust used upon harvest - DustID.GrassBlades is the default

            public Func<int, int, int, (float? i, float? j, float? type)?> lightColor; // For optional light
            //public Action<int, int, int> ambientDust; // For optional dust and sparkles - currently does nothing
            #endregion

            #region Draw settings
            public bool vanillaDraw = false; // If true, this is a vanilla tile and should use TileID._Herbs as a texture
            public Texture texturePath; // If it's a modded herb, use its asset path
            public int frameWidth = 18;
            public int frameHeight = 20;
            #endregion

            // Map the growthStage to its source frame index along the X cordinate
            // By default, herbs, both vanilla and modded, have three stages: planted(0), grown(1), and blooming(2)
            public int[] StageToFrameX = [0, 1, 2];

            public Rectangle GetSourceRect(int growthStage)
            {
                int idx = Utils.Clamp(growthStage - 1, 0, StageToFrameX.Length - 1);
                int frameX = StageToFrameX[idx];
                return new Rectangle(frameX * frameWidth, 0, 16, frameHeight);
            }
        }

        public static readonly List<HydroponicsHerb> Herbs = [];

        public override void OnModLoad()
        {
            float tempCol;

            // Register the regular vanilla herbs first
            RegisterHerb("Terraria:Daybloom", ItemID.Daybloom, DustID.GrassBlades, 0);
            RegisterHerb("Terraria:Moonglow", ItemID.Moonglow, DustID.GrassBlades, 1);
            tempCol = MathHelper.Clamp((270 - Main.mouseTextColor) / 800f, 0f, 1f); // tempCol will always be "static" and set to whatever value was used upon loading, and won't oscillate - could be implemented better
            RegisterHerb("Terraria:Blinkroot", ItemID.Blinkroot, DustID.WoodFurniture, 2, tempCol * 0.7f, tempCol, tempCol * 0.1f);
            RegisterHerb("Terraria:Deathweed", ItemID.Deathweed, DustID.CorruptPlants, 3);
            RegisterHerb("Terraria:Waterleaf", ItemID.Waterleaf, DustID.GrassBlades, 4);
            tempCol = 0.9f;
            RegisterHerb("Terraria:Fireblossom", ItemID.Fireblossom, DustID.Torch, 5, tempCol, tempCol * 0.8f, tempCol * 0.2f);
            tempCol = 0.08f;
            RegisterHerb("Terraria:Shiverthorn", ItemID.Shiverthorn, DustID.Shiverthorn, 6, 0, tempCol * 0.7f, tempCol);
        }

        public override void OnModUnload()
        {
            Herbs.Clear();
        }

        private static void RegisterHerb(string key, int itemType, int dustType, int vanillaStyle, float r = 0f, float g = 0f, float b = 0f)
        {
            while (Herbs.Count <= vanillaStyle)
                Herbs.Add(null);

            bool hasLight = r != 0f || g != 0f || b != 0f;

            Herbs[vanillaStyle] = new HydroponicsHerb
            {
                key = key,
                herbType = itemType,
                harvestDust = dustType,
                vanillaDraw = true,

                lightColor = hasLight ? (i, j, type) => (
                r == 0f ? null : r,
                g == 0f ? null : g,
                b == 0f ? null : b
                ) : null
            };
        }

        public static int RegisterHerb(HydroponicsHerb herb)
        {
            for (int i = 0; i < Herbs.Count; i++)
                if (Herbs[i]?.key == herb.key) return i; // If there's a duplicated herb, remove the dupe

            Herbs.Add(herb);
            return Herbs.Count - 1;
        }

        public static bool TryGet(int type, out HydroponicsHerb herb)
        {
            if (type >= 0 && type < Herbs.Count && Herbs[type] != null)
            {
                herb = Herbs[type];
                return true;
            }
            herb = null;
            return false;
        }

        /// <summary>
        /// Registers a new herb that can grow in Hydroponics.
        /// </summary>
        /// <param name="args"> The arguments for the mod call. This is seperated into the following:
        /// <list type="args">
        /// <item><description><c>herbItem (int)</c>  ItemID of the herb.</description></item>
        /// <item><description><c>herbTile (int)</c>  TileID of the herb.</description></item>
        /// <item><description><c>tileOffset (int)</c>  If multiple herb textures are in the same file, use this to offset it. Set to 0 for no offset. (Currently does nothing)</description></item>
		/// <item><description><c>harvestDust (int)</c>  <c>Optional</c>: DustID used upon harvesting a herb. If set to 0, it'll be set to DustID.GrassBlades by default.</description></item>
		/// <item><description><c>colorR (float)</c>  <c>Optional</c>: The red hue that the blooming herb emits.</description></item>
		/// <item><description><c>colorG (float)</c>  <c>Optional</c>: The green hue that the blooming herb emits.</description></item>
		/// <item><description><c>colorB (float)</c>  <c>Optional</c>: The blue hue that the blooming herb emits.</description></item>
        /// </list>
        /// </param>
        internal static bool ParseNewHerb(params object[] args)
        {
            if (args.Length < 3)
                throw new ArgumentException("ParseNewHerb requires int (herbItem), int (herbTile), int/0 (tileOffset), int/0 (harvestDust), float/0 (colorR), float/0 (colorG), float/0 (colorB)!");
            if (args[0] is not int)
                throw new ArgumentException("ParseNewHerb paramater 0 (herbItem) should be an int!");
            if (args[1] is not int)
                throw new ArgumentException("ParseNewHerb paramater 1 (herbTile) should be an int!");
            if (args[2] is not int)
                throw new ArgumentException("ParseNewHerb paramater 2 (tileOffset) should be an int!");
            /* if (args[2] is not 0)
				// Implement code for that here - right now it does nothing*/

            int dustType = DustID.GrassBlades;
            if (args[3] is not int)
                throw new ArgumentException("ParseNewHerb paramater 3 (harvestDust) should be an int!");
            else if (args[3] is not 0) dustType = (int)args[3];

            var r = 0f;
            var g = 0f;
            var b = 0f;
            if (args.Length > 3)
            {
                if (args[4] is not float && args[4] is not null)
                    throw new ArgumentException("ParseNewHerb paramater 4 (colorR) should be a float!");
                else r = (float)args[4];

                if (args[5] is not float && args[5] is not null)
                    throw new ArgumentException("ParseNewHerb paramater 5 (colorG) should be a float!");
                else g = (float)args[4];

                if (args[6] is not float && args[6] is not null)
                    throw new ArgumentException("ParseNewHerb paramater 6 (colorB) should be a float!");
                else b = (float)args[5];
            }

            bool hasLight = r != 0f || g != 0f || b != 0f;

            var herb = new HydroponicsHerb
            {
                key = ItemLoader.GetItem((int)args[0]).FullName,
                tileType = (int)args[1],
                herbType = (int)args[0],
                texturePath = TextureAssets.Tile[(int)args[1]].Value,
                harvestDust = dustType,

                lightColor = hasLight ? (i, j, type) => (
                r == 0f ? null : r,
                g == 0f ? null : g,
                b == 0f ? null : b
                ) : null
            };
            RegisterHerb(herb);

            return true;
        }
    }
}