using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Otherworld;

namespace SOTS.Items.Otherworld.FromChests
{
	public class ArcStaffMk2 : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arc Staff Mk2");
			Tooltip.SetDefault("Launch a massive sphere of lightning\nDetonates at the end of its lifespan, doing 250% damage to low health, non-boss enemies, but only 100% to bosses");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 120;
			item.magic = true;
			item.width = 44;
			item.height = 44;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 1;
			item.knockBack = 3.5f;
            item.value = Item.sellPrice(0, 3, 80, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;     
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<GenesisCore>(); 
            item.shootSpeed = 12.5f;
		}
		public override int GetVoid(Player player)
		{
			return 60;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GoldArkStaff", 1);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 16);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}