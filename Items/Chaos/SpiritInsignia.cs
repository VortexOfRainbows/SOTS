using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;
using SOTS.Items.Fragments;

namespace SOTS.Items.Chaos
{
	public class SpiritInsignia : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 34;     
            Item.height = 44;   
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
			Item.expert = true;
		}
		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (equippedItem.type == ModContent.ItemType<SpiritSymphony>() || 
				incomingItem.type == ModContent.ItemType<SpiritSymphony>())
				return false;
			return true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.bonusVoidGain += 1f;
			vPlayer.voidRegenSpeed += 0.25f;
			player.empressBrooch = true;
			player.moveSpeed += 0.1f;
			modPlayer.SpiritSymphony = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SpiritSymphony>(1)
				.AddIngredient(ItemID.EmpressFlightBooster)
				.AddIngredient<DissolvingBrilliance>(1)
				.AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}

