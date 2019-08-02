using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.BiomeItems
{
	public class GeodeBlock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 62; 
			dustType = 0;
			AddMapEntry(new Color(76, 33, 0));
		}
		public override bool Drop(int i, int j)
		{
			int baseSelection = Main.rand.Next(3);
			if(baseSelection == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.StoneBlock);
			}
			if(baseSelection == 1)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SiltBlock);
			}
			if(baseSelection == 2)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Obsidian);
			}
			
			if(Main.rand.Next(56) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Amethyst);
			}
			if(Main.rand.Next(56) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Topaz);
			}
			if(Main.rand.Next(66) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Sapphire);
			}
			if(Main.rand.Next(66) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Emerald);
			}
			if(Main.rand.Next(76) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Ruby);
			}
			if(Main.rand.Next(76) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Diamond);
			}
			if(Main.rand.Next(15) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.CopperOre);
			}
			if(Main.rand.Next(16) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.TinOre);
			}
			if(Main.rand.Next(22) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.IronOre);
			}
			if(Main.rand.Next(23) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.LeadOre);
			}
			if(Main.rand.Next(36) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SilverOre);
			}
			if(Main.rand.Next(37) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.TungstenOre);
			}
			if(Main.rand.Next(49) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.GoldOre);
			}
			if(Main.rand.Next(49) == 0)
			{
			Item.NewItem(i * 16, j * 16, 16, 16, ItemID.PlatinumOre);
			}
			
			if(NPC.downedBoss2)
			{
				if(Main.rand.Next(50) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.CrimtaneOre);
				}
				if(Main.rand.Next(50) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.DemoniteOre);
				}
			}
			
			
			if(Main.hardMode)
			{
				if(Main.rand.Next(50) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.PalladiumOre);
				}
				if(Main.rand.Next(50) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.CobaltOre);
				}
				if(Main.rand.Next(60) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.MythrilOre);
				}
				if(Main.rand.Next(60) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.OrichalcumOre);
				}
				if(Main.rand.Next(80) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.TitaniumOre);
				}
				if(Main.rand.Next(80) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.AdamantiteOre);
				}
			}
			
			if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				if(Main.rand.Next(100) == 0)
				{
				Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ChlorophyteOre);
				}
			}
			return true;
		}
		
	}
}