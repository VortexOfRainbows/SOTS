using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.OreItems
{
	public class GoldArkStaff : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Arc Staff");
			Tooltip.SetDefault("Deal 150% damage to up to 2 enemies surrounding the initially hit enemy");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 34;
			item.magic = true;
			item.width = 36;
			item.height = 36;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.knockBack = 3.5f;
            item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;     
			item.noMelee = true;
			item.shoot = mod.ProjectileType("ArkBolt"); 
            item.shootSpeed = 12.5f;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 4;
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