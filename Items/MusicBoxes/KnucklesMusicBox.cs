using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SOTS.Items.MusicBoxes
{
	public class KnucklesMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Knuckles)");
			Tooltip.SetDefault("'Can I put my...'");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = mod.TileType("KnucklesMusicBoxTile");
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}

		public static Color ColorSwap(Color firstColor, Color secondColor, float seconds)
		{
			float colorMePurple = (float)((Math.Sin((double)((float)Math.PI * 2f / seconds) * (double)Main.GlobalTime) + 1.0) * 0.5);
			return Color.Lerp(firstColor, secondColor, colorMePurple);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault((TooltipLine x) => x.Name == "ItemName" && x.mod == "Terraria");
			if (tt != null)
			{
				tt.overrideColor = ColorSwap(new Color(214, 6, 96), new Color(214, 152, 180), 0.75f);
			}
		}
	}
	public class KnucklesMusicBoxTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(255, 0, 0), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType("KnucklesMusicBox"));
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("KnucklesMusicBox");
		}
	}
}