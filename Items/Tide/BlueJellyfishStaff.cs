using SOTS.Items.Fragments;
using SOTS.Projectiles.Lightning;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
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
            Item.damage = 17;
            Item.magic = true; 
            Item.width = 34;    
            Item.height = 32; 
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 1, 25, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item92;
            Item.noMelee = true; 
            Item.autoReuse = false;
            Item.shootSpeed = 14.5f; 
			Item.shoot = ModContent.ProjectileType<BlueThunderCluster>();
			Item.staff[Item.type] = true; 
			Item.mana = 15;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire, 15);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfTide>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
