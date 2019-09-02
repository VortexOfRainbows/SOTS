using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
 
 
namespace SOTS.Items.BiomeItems      ///We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class MargritSentryStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Sentry Staff");
			
			Tooltip.SetDefault("Summons a Margrit portal on your cursor\nThe portal works like a vacuum\n10 second duration");
		}
 
        public override void SetDefaults()
        {
            item.damage = 4; 
            item.mana = 100; 
            item.width = 44; 
            item.height = 44;
            item.useTime = 45;  
            item.useAnimation = 45; 
            item.useStyle = 1;
            item.noMelee = true; 
            item.knockBack = 0; 
            item.value = 110000;
            item.rare = 6; 
            item.UseSound = SoundID.Item44;  
            item.autoReuse = true; 
            item.shoot = mod.ProjectileType("MargritSentry");  
            item.summon = true;    
            item.sentry = true;
        } 
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 24);
			recipe.AddIngredient(3081, 12);
			recipe.AddIngredient(3086, 24);
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