using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.OreItems;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Items.ChestItems;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Permafrost;
using SOTS.Items.Earth;

namespace SOTS.Items.Conduit
{
	public class AccretionDisc : VoidItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SafeSetDefaults()
		{
			Item.damage = 48;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 48;
			Item.height = 52;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ModContent.RarityType<AnomalyRarity>();
            Item.UseSound = SoundID.Item18;
			Item.autoReuse = true;     
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Anomaly.AccretionDisc>(); 
            Item.shootSpeed = 13.5f;
            Item.noUseGraphic = true;
        }
        public override int GetVoid(Player player)
        {
            return 24;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<CataclysmDisc>().AddIngredient<EchoDisk>().AddIngredient<CoconutGun>()
				.AddIngredient<SkipSoul>(30).AddIngredient<SkipShard>(15).AddIngredient<SoulOfPlight>(5).AddIngredient(ItemID.Ectoplasm, 5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}