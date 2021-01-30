using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace SOTS.Items.ChestItems
{
	public class StrangeKeystone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strange Keystone");
			Tooltip.SetDefault("It suffers from small cracks under intense sunlight\n'Feels hollow'");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 36;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.consumable = true;
			item.createTile = mod.TileType("StrangeKeystoneTile");
		}
	}
	public class StrangeKeystoneTile : ModTile
    {
        public override void SetDefaults()
        {
		    mineResist = 0.01f;
		    minPick = 0;
		    Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.addTile(Type);
            dustType = 32;
            ModTranslation name = CreateMapEntryName();
		    name.SetDefault("Strange Keystone");		
		    AddMapEntry(new Color(90, 80, 45), name);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
             if(frameX == 0)
             {
                Item.NewItem(i * 16, j * 16, 48, 64, mod.ItemType("StrangeKeystone"));
             }
        }
	    public override bool CanExplode(int i, int j)
		{
			return true;
		}
    	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
        }
    }
}