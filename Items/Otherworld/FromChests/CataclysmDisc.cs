using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Otherworld.FromChests
{
	public class CataclysmDisc : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Disc");
			Tooltip.SetDefault("Deals more damage at the peak of its trajectory and explodes for 300% damage");
		}
		public override void SafeSetDefaults()
		{

			item.damage = 32;
			item.ranged = true;
			item.width = 48;
			item.height = 32;
			item.useTime = 34;
			item.useAnimation = 34;
			item.useStyle = 1;
			item.knockBack = 4.5f;
            item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item18;
			item.autoReuse = true;     
			item.noMelee = true;
			item.shoot = mod.ProjectileType("CataclysmDisc"); 
            item.shootSpeed = 13.5f;
            item.noUseGraphic = true;

		}
		public override void GetVoid(Player player)
		{
			voidMana = 12;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GoldChakram", 1);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}