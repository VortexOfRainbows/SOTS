using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.AbandonedVillage
{
	public class AVAmbient : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 0;
			Item.consumable = true;
		}
		int type = 0;
        public override bool? UseItem(Player player)
        {
			int modU = type % 6;
            Item.createTile = ModContent.TileType<AVAmbientTile1x1>();
            if(modU == 1)
                Item.createTile = ModContent.TileType<AVAmbientTile2x1>();
            if (modU == 2)
                Item.createTile = ModContent.TileType<AVAmbientTile3x1>();
            if (modU == 3)
                Item.createTile = ModContent.TileType<AVAmbientTile2x2>();
            if (modU == 4)
                Item.createTile = ModContent.TileType<AVAmbientTile1x2>();
            if (modU == 5)
                Item.createTile = ModContent.TileType<AVAmbientTile3x2>();
            type++;
			return base.UseItem(player);
        }
    }	
    public abstract class AVAmbientTile : ModTile
    {
        private short frameOffX;
        private short frameOffY;
        public override string Texture => "SOTS/Items/AbandonedVillage/AVAmbientTile";
        public sealed override void SetStaticDefaults()
        {
            TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            SafeSetDefaults(TileObjectData.newTile, ref frameOffX, ref frameOffY);
            TileObjectData.newTile.DrawYOffset -= 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(98, 79, 68));
            DustType = ModContent.DustType<SootDust>();
            HitSound = SoundID.Dig;
            MineResist = 0.1f;
        }
        public virtual void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            
        }
        public sealed override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            tileFrameX += frameOffX;
            tileFrameY += frameOffY;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
            Color color;
            color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
            color.A = 0;
            float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
            Tile tile = Main.tile[i, j];
            int TileFrameX = tile.TileFrameX + frameOffX;
            int TileFrameY = tile.TileFrameY + frameOffY;
            Vector2 location = new Vector2(i * 16, j * 16);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            Vector2 offsets = -Main.screenPosition + zero + new Vector2(0, tile.TileType == ModContent.TileType<AVAmbientTile2x2>() ? -4 : -2);
            Vector2 drawCoordinates = location + offsets;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/AVAmbientTileGlow").Value, drawCoordinates, new Rectangle(TileFrameX, TileFrameY, 16, 20), color * alphaMult, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            int style = Main.tile[i, j].TileFrameX / 18;
            DustStyle(new Vector2(i * 16, j * 16), style, ref type);
            return true;
        }
        public virtual void DustStyle(Vector2 pos, int style, ref int type)
        {

        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 4;
        }
        public override bool CanDrop(int i, int j)
        {
            return false;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            return null;
        }
        public sealed override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
            float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
            int style = Main.tile[i, j].TileFrameX / 18;
            Vector3 c = new Vector3(0, 0, 0);
            ModifyLight2(style, Main.tile[i, j].TileFrameY, ref c);
            c *= alphaMult * 0.2f;
            r = c.X;
            g = c.Y;
            b = c.Z;
        }
        public virtual void ModifyLight2(int style, int frameY, ref Vector3 color)
        {

        }
    }
	public class AVAmbientTile1x1 : AVAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
		{
            d.CopyFrom(TileObjectData.Style1x1);
			d.CoordinateHeights = new[] { 20 };
			d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 13;
		}
        public override void DustStyle(Vector2 centerPos, int style, ref int type)
        {
            if (style == 0 || style == 1)
            {
                type = ModContent.DustType<SootDust>();
                if (Main.rand.NextBool(3))
                    type = DustID.Bone;
            }
            if (style == 2 || style == 3)
            {
                type = ModContent.DustType<SootDust>();
                if (Main.rand.NextBool(3))
                    type = DustID.CorruptionThorns;
            }
            if (style == 4 || style == 5 || style == 10)
            {
                type = DustID.CorruptionThorns;
                if (!Main.rand.NextBool(3))
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.CursedTorch);
            }
            if (style == 6)
            {
                type = Main.rand.NextBool() ? DustID.CrimsonPlants : DustID.CorruptionThorns;
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.IchorTorch);
                }
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.CursedTorch);
                }
            }
            if (style == 7 || style == 8 || style == 9)
            {
                type = DustID.CrimsonPlants;
                if(!Main.rand.NextBool(3))
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.IchorTorch);
            }
            if (style == 11 || style == 12)
            {
                type = ModContent.DustType<SootDust>();
                if (!Main.rand.NextBool(3))
                {
                    Dust dust = PixelDust.Spawn(centerPos, 16, 16, Main.rand.NextVector2Circular(2, 2), ColorHelpers.AVDustColor, -2);
                    dust.scale = Main.rand.NextFloat(1f, 2f);
                    dust.alpha = 150;
                }
            }
        }
        public override void ModifyLight2(int style, int frameY, ref Vector3 color)
        {
            if (style == 4 || style == 5 || style == 6)
                color += ColorHelpers.AVCursedLight;
            if (style == 7 || style == 8 || style == 9 || style == 6)
                color += ColorHelpers.AVIchorLight;
        }
    }
    public class AVAmbientTile2x1 : AVAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            d.CopyFrom(TileObjectData.Style2x1);
            d.CoordinateHeights = new[] { 20 };
            d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 4;
            StartingY = 22;
        }
        public override void DustStyle(Vector2 centerPos, int style, ref int type)
        {
            if(style < 4)
            {
                type = ModContent.DustType<SootDust>();
                if (!Main.rand.NextBool(3))
                {
                    Dust dust = PixelDust.Spawn(centerPos, 16, 16, Main.rand.NextVector2Circular(2, 2), ColorHelpers.AVDustColor, -2);
                    dust.scale = Main.rand.NextFloat(1f, 2f);
                    dust.alpha = 150;
                }
            }
            else
            {
                type = ModContent.DustType<SootDust>();
                if (!Main.rand.NextBool(3))
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.Bone);
            }
        }
    }
    public class AVAmbientTile3x1 : AVAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            d.CopyFrom(TileObjectData.Style2x1);
            d.Width = 3;
            d.CoordinateHeights = new[] { 20 };
            d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 2;
            StartingY = 22;
            StartingX = 144;
        }
        public override void DustStyle(Vector2 centerPos, int style,ref int type)
        {
            if (style >= 3)
            {
                type = ModContent.DustType<SootDust>();
                if (!Main.rand.NextBool(3))
                {
                    Dust dust = PixelDust.Spawn(centerPos, 16, 16, Main.rand.NextVector2Circular(2, 2), ColorHelpers.AVDustColor, -2);
                    dust.scale = Main.rand.NextFloat(1f, 2f);
                    dust.alpha = 150;
                }
            }
            else
            {
                type = ModContent.DustType<SootDust>();
                if (!Main.rand.NextBool(3))
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.Bone);
            }
        }
    }
    public class AVAmbientTile3x2 : AVAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            d.CopyFrom(TileObjectData.Style3x2);
            d.CoordinateHeights = new[] { 16, 20 };
            d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 4;
            StartingY = 44;
            StartingX = 0;
        }
        public override void DustStyle(Vector2 centerPos, int style, ref int type)
        {
            if(style < 9)
            {
                if (Main.rand.NextBool(4))
                    type = DustID.Bone;
                else
                {
                    Dust dust = PixelDust.Spawn(centerPos, 16, 16, Main.rand.NextVector2Circular(2, 2), ColorHelpers.AVDustColor, -2);
                    dust.scale = Main.rand.NextFloat(1f, 2f);
                    dust.alpha = 150;
                }
            }
            else
            {
                type = Main.rand.NextBool() ? DustID.CrimsonPlants : DustID.CorruptionThorns;
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.IchorTorch);
                }
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.CursedTorch);
                }
            }
        }
        public override void ModifyLight2(int style, int frameY, ref Vector3 color)
        {
            if (style >= 9)
            {
                color += ColorHelpers.AVCursedLight;
                color += ColorHelpers.AVIchorLight;
            }
        }
    }
    public class AVAmbientTile2x2 : AVAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            d.CopyFrom(TileObjectData.Style2x2);
            d.CoordinateHeights = new[] { 16, 22 };
            d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 4;
            d.DrawYOffset -= 2;
            StartingY = 84;
            StartingX = 0;
        }
        public override void DustStyle(Vector2 centerPos, int style, ref int type)
        {
            if(style < 4)
            {
                type = DustID.CorruptionThorns;
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.CursedTorch);
                }
            }
            else
            {
                type = DustID.CrimsonPlants;
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustDirect(centerPos, 16, 16, DustID.IchorTorch);
                }
            }
        }
        public override void ModifyLight2(int style, int frameY, ref Vector3 color)
        {
            if (style < 4)
                color += ColorHelpers.AVCursedLight;
            else
                color += ColorHelpers.AVIchorLight;
        }
    }
    public class AVAmbientTile1x2 : AVAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            d.CopyFrom(TileObjectData.Style1x2);
            d.CoordinateHeights = new[] { 16, 22 };
            d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 4;
            d.DrawYOffset -= 2;
            StartingY = 84;
            StartingX = 144;
        }
        public override void DustStyle(Vector2 centerPos, int style, ref int type)
        {
            type = ModContent.DustType<CharredWoodDust>();
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<CharredWood>(), Main.rand.Next(1, 4));
        }
        public override bool CanDrop(int i, int j)
        {
            return true;
        }
    }
}