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
			DisplayName.SetDefault("Strange Obelisk");
			Tooltip.SetDefault("It suffers from small cracks under intense sunlight\n'Feels hollow'");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 42;
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
	public class StrangeKeystoneBroken : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Obelisk");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 20;
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
			item.placeStyle = 1;
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
			if(frameX >= 32)
			{
				Item.NewItem(i * 16, j * 16, 48, 64, mod.ItemType("StrangeKeystoneBroken"));
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