using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Invidia
{
	public class EvostoneAmbient : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 16;
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
		private int type = 0;
        public override bool? UseItem(Player player)
        {
			int modU = type % 3;
            Item.createTile = ModContent.TileType<EvostoneAmbientTile1x1>();
            if(modU == 1)
                Item.createTile = ModContent.TileType<EvostoneAmbientTile2x1>();
            if (modU == 2)
                Item.createTile = ModContent.TileType<EvostoneAmbientTile1x2>();
            type++;
			return base.UseItem(player);
        }
    }	
    public abstract class EvostoneAmbientTile : ModTile
    {
        private short frameOffX;
        private short frameOffY;
        public override string Texture => "SOTS/Items/Invidia/EvostoneAmbientTile";
        public sealed override void SetStaticDefaults()
        {
            TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            SafeSetDefaults(TileObjectData.newTile, ref frameOffX, ref frameOffY);
            //TileObjectData.newTile.DrawYOffset -= 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(46, 63, 77));
            DustType = ModContent.DustType<EvostoneDust>();
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

        }
        public virtual void ModifyLight2(int style, int frameY, ref Vector3 color)
        {

        }
    }
	public class EvostoneAmbientTile1x1 : EvostoneAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
		{
            d.CopyFrom(TileObjectData.Style1x1);
			d.CoordinateHeights = new[] { 18 };
			d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 4;
            //d.DrawYOffset = 2;
		}
    }
    public class EvostoneAmbientTile2x1 : EvostoneAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            d.CopyFrom(TileObjectData.Style2x1);
            d.CoordinateHeights = new[] { 18 };
            d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 3;
            //d.DrawYOffset = 2;
            StartingY = 58;
        }
    }
    public class EvostoneAmbientTile1x2 : EvostoneAmbientTile
    {
        public override void SafeSetDefaults(TileObjectData d, ref short StartingX, ref short StartingY)
        {
            d.CopyFrom(TileObjectData.Style1x2);
            d.CoordinateHeights = new[] { 16, 18 };
            d.RandomStyleRange = TileObjectData.newTile.StyleWrapLimit = 2;
            //d.DrawYOffset = 2;
            StartingY = 20;
        }
    }
}