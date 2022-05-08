using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Ores;

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
			Item.damage = 34;
			Item.magic = true;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;     
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<ArkBolt>(); 
            Item.shootSpeed = 12.5f;
		}
		public override int GetVoid(Player player)
		{
			return 4;
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