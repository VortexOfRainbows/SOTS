using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{	[AutoloadEquip(EquipType.Shoes)]
	public class ShoeIce : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 26;     
            Item.height = 26;   
            Item.value = Item.sellPrice(0, 1, 20, 0);
            Item.rare = 2;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.40f;
			player.maxRunSpeed += 0.40f;
			player.spikedBoots++;
			if(player.velocity.Y == 0)
				player.slippy = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.ShoeSpikes, 1).AddIngredient<FragmentOfPermafrost>(4).AddTile(TileID.Anvils).Register();
		}
	}
}