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
 
namespace SOTS.Items.Pyramid        
{
    public class ManaStatue : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(0, 0, 155), name);
            HitSound = SoundID.Tink;
            TileObjectData.addTile(Type);
			MineResist = 2.5f;
			DustType = 15;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Pyramid/ManaStatueDraw");
                Vector2 location = new Vector2(i * 16, j * 16) + new Vector2(-4, -4);
                Color color = Lighting.GetColor(i, j, WorldGen.paintColor(Main.tile[i, j].TileColor));
                spriteBatch.Draw(texture, location + zero - Main.screenPosition, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.ManaCrystal);//this defines what to drop when this tile is destroyed
        }
    }
}