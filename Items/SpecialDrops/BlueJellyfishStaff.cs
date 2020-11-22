using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class BlueJellyfishStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Jellyfish Staff");
			Tooltip.SetDefault("Fires an energy ball that detonates into blue lighting after traveling forward");
		}
		public override void SetDefaults()
		{
            item.damage = 17;
            item.magic = true; 
            item.width = 32;    
            item.height = 32; 
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 25, 0);
            item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = false;
            item.shootSpeed = 14.5f; 
			item.shoot = mod.ProjectileType("BlueThunderCluster");
			Item.staff[item.type] = true; 
			item.mana = 15;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire, 5);
			recipe.AddIngredient(null, "FragmentOfTide", 2);
			recipe.AddIngredient(null, "FragmentOfPermafrost", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
