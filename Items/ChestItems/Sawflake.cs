using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.ChestItems
{
    public class Sawflake : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sawflake");
			Tooltip.SetDefault("Throw a Sawflake that exudes a spiral of razor ice mist in every direction");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.width = 50;
            Item.height = 50;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.knockBack = 7f;
            Item.noUseGraphic = true; 
            Item.shoot = ModContent.ProjectileType<Projectiles.BiomeChest.Sawflake>();
            Item.shootSpeed = 13.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee; 
            Item.autoReuse = true;
        }
    }
}