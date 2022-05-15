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

			Item.damage = 18;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 34;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4.5f;
            Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item18;
			Item.autoReuse = true;     
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ores.GoldChakram>(); 
            Item.shootSpeed = 13.5f;
            Item.noUseGraphic = true; 

		}
		public override int GetVoid(Player player)
		{
			return 7;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.GoldBar, 15).AddTile(TileID.Anvils).Register();
		}
	}
}