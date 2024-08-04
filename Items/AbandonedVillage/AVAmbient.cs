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
			int modU = type % 2;
            Item.createTile = ModContent.TileType<AVAmbientTile1x1>();
            if(modU == 1)
                Item.createTile = ModContent.TileType<AVAmbientTile2x1>();
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
            TileObjectData.newTile.DrawYOffset = -2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(52, 36, 29));
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
            Vector2 offsets = -Main.screenPosition + zero + new Vector2(0, -2);
            Vector2 drawCoordinates = location + offsets;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/AVAmbientTileGlow").Value, drawCoordinates, new Rectangle(TileFrameX, TileFrameY, 16, 20), color * alphaMult, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            int style = Main.tile[i, j].TileFrameX / 18;
            int verticalStyle = Main.tile[i, j].TileFrameY / 22;
            DustStyle(new Vector2(i * 16 + 8, j * 16 + 8), style, verticalStyle, ref type);
            return base.CreateDust(i, j, ref type);
        }
        public virtual void DustStyle(Vector2 centerPos, int style, int verticalStyle, ref int type)
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
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            int style = Main.tile[i, j].TileFrameX / 18;
            int verticalStyle = Main.tile[i, j].TileFrameY / 22;
            if (verticalStyle == 0)
            {
                if (style == 4)
                {
                    //corruption light here
                }
                if (style == 6)
                {
                    //combiend light here
                }
                if (style == 7 || style == 8)
                {
                    //crimson light here
                }
            }
            //for taller tiles, only the bottom should be lighted
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
        public override void DustStyle(Vector2 centerPos, int style, int verticalStyle, ref int type)
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
                    type = 24; //Corrupt thorns dust
            }
            if (style == 4 || style == 5 || style == 10)
            {
                type = DustID.TintableDust; //Will need to color this to flesh color
                //also spawn pixel dust here with green color
            }
            if (style == 6)
            {
                //Combination of 4,5,7,8
            }
            if (style == 7 || style == 8 || style == 9)
            {
                type = DustID.CrimsonPlants;
                //spawn ichor dust here for styles 7 and 8
            }
            if (style == 11 || style == 12)
            {
                type = ModContent.DustType<SootDust>();
                //Spawn ash dust here (pixel dust with certain presets)
            }
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
        public override void DustStyle(Vector2 centerPos, int style, int verticalStyle, ref int type)
        {
            base.DustStyle(centerPos, style, verticalStyle, ref type);
        }
    }
}