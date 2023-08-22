using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.OreItems;
using SOTS.Items.Planetarium.Furniture;

namespace SOTS.Items.Planetarium.FromChests
{
	public class CataclysmDisc : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4.5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item18;
			Item.autoReuse = true;     
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Planetarium.CataclysmDisc>(); 
            Item.shootSpeed = 13.5f;
            Item.noUseGraphic = true;
		}
		public override int GetVoid(Player player)
		{
			return 12;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<GoldChakram>(), 1).AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 12).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}