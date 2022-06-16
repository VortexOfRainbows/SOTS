using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.EpicWings;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	[AutoloadEquip(EquipType.Wings)]
	public class GelWings : ModItem
	{
		public override void SetStaticDefaults()
		{	
			DisplayName.SetDefault("Gel Wings");
			Tooltip.SetDefault("Allows flight and slow fall\n'It really shouldn't hold up well'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 40;
            Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 30;
		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 1.35f;
			constantAscend = 0.195f;
		}
        public override bool WingUpdate(Player player, bool inUse)
		{
			if (player.controlJump && player.wingTime > 0 && player.velocity.Y != 0)
			{
				player.wingFrameCounter++;
				if (player.wingFrameCounter >= 20)
					player.wingFrameCounter = 0;
				if(player.wingFrameCounter < 5)
                {
					player.wingFrame = 1;
				}
				else if (player.wingFrameCounter < 10)
				{
					player.wingFrame = 2;
				}
				else if (player.wingFrameCounter < 15)
				{
					player.wingFrame = 3;
				}
				else if (player.wingFrameCounter < 20)
				{
					player.wingFrame = 4;
				}
			}
			else
			{
				player.wingFrameCounter = 0;
				player.wingFrame = 0;
				if (player.velocity.Y != 0)
				{
					player.wingFrame = 1;
					if (player.controlJump && player.velocity.Y > 0)
						player.wingFrame = 3;
				}
			}
			return true;
        }
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 8f;
			acceleration *= 1.01f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CorrosiveGel>(), 8).AddIngredient<Wormwood>(24).AddIngredient(ItemID.Feather, 10).AddTile(TileID.Anvils).Register();
		}
	}
}