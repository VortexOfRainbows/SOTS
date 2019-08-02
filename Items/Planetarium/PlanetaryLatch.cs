using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
 
 
namespace SOTS.Items.Planetarium      ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class PlanetaryLatch : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Staff");
			
			Tooltip.SetDefault("Summons a Planetary Latch on your cursor\nThe planetary latch will try to rotate around you, and will attack enemies that get close to it");
		}
 
        public override void SetDefaults()
        {
            item.damage = 56;
            item.mana = 33;     
            item.width = 56;    
            item.height = 56;  
            item.useTime = 40;  
            item.useAnimation = 40;    
            item.useStyle = 1; 
            item.noMelee = true; 
            item.knockBack = 0; 
            item.value = 150000; 
            item.rare = 9;  
            item.UseSound = SoundID.Item44;  
            item.autoReuse = true;  
            item.shoot = mod.ProjectileType("PlanetaryLatch");   
            item.summon = true;   
            item.sentry = true; 
        } public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 SPos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			position = SPos;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}
			return player.altFunctionUse != 2;
		}

    }
}