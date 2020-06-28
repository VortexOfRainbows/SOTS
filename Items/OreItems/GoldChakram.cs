using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.OreItems
{
	public class GoldChakram : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Chakram");
			Tooltip.SetDefault("Deals more damage at the peak of its trajectory");
		}
		public override void SafeSetDefaults()
		{

			item.damage = 18;
			item.ranged = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.knockBack = 4.5f;
            item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item18;
			item.autoReuse = true;     
			item.noMelee = true;
			item.shoot = mod.ProjectileType("GoldChakram"); 
            item.shootSpeed = 13.5f;
            item.noUseGraphic = true; 

		}
		public override void GetVoid(Player player)
		{
			voidMana = 7;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}