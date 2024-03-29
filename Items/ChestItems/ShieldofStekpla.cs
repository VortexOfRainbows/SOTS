using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{	[AutoloadEquip(EquipType.Shield)]
	public class ShieldofStekpla : ModItem
	{ 	
		int critbonus = 0;
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 28;     
            Item.height = 36;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			critbonus = 0;
			for(int i = 0; i < 50; i++)
			{
				Item inventoryItem = player.inventory[i];
				if(inventoryItem.type != 0)
				{
					critbonus++;
				}
			}
			player.GetCritChance(DamageClass.Generic) += (int)(critbonus * 0.25f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingAurora>(1).AddIngredient(ItemID.PlatinumBar, 20).AddTile(TileID.Anvils).Register();
		}
	}
}
