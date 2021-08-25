using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{	[AutoloadEquip(EquipType.Shoes)]
	public class ShoeIce : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shoe Ice");
			Tooltip.SetDefault("Allows the ability to slide down walls\nImproved ability if combined with Climbing Claws or Shoe Spikes\n40% increased movement speed\n'Ice physics, everyone's favorite!'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 26;     
            item.height = 26;   
            item.value = Item.sellPrice(0, 1, 20, 0);
            item.rare = 2;
			item.accessory = true;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShoeSpikes, 1); 
			recipe.AddIngredient(null, "FragmentOfPermafrost", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}