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

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			minPick = 50; 
			dustType = 72;
			drop = mod.ItemType("IceCreamBrick");
			AddMapEntry(new Color(255, 105, 180));
		}
		}	
	}
