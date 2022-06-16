using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
    public class WormWoodSpike : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Spike");
			Tooltip.SetDefault("Enemies get stuck on it");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = 4;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 4.5f;
            Item.noUseGraphic = true; 
            Item.shoot = ModContent.ProjectileType<Projectiles.WormWoodSpike>();
            Item.shootSpeed = 14.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee; 
            Item.channel = true;
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32).AddIngredient(ModContent.ItemType<Wormwood>(), 16).AddTile(TileID.Anvils).Register();
		}
    }
}