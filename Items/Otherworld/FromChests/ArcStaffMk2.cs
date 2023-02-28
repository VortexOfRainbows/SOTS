using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.OreItems;
using SOTS.Items.Otherworld.Furniture;

namespace SOTS.Items.Otherworld.FromChests
{
	public class ArcStaffMk2 : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			CreateRecipe(1).AddIngredient<GoldArkStaff>(1).AddIngredient<OtherworldlyAlloy>(16).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}