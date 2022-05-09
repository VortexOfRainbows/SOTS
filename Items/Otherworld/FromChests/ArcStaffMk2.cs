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
			Item.damage = 120;
			Item.DamageType = DamageClass.Magic;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 3, 80, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;     
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<GenesisCore>(); 
            Item.shootSpeed = 12.5f;
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