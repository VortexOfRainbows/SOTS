using Terraria;                    
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Permafrost
{
    public class Metalmalgamation : ModItem   
    {
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.Size = new Microsoft.Xna.Framework.Vector2(34, 30);
            Item.damage = 47;
            Item.DamageType = DamageClass.Melee; 
            Item.useTime = 22;  
            Item.useAnimation = 22;   
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 7, 0, 0);
			Item.rare = ItemRarityID.Lime;
            Item.autoReuse = false; 
            Item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.Metalmalgamation>(); 
            Item.noUseGraphic = true; 
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1; 
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<AbsoluteBar>(11).AddTile(TileID.MythrilAnvil).Register();
		}
    }
}