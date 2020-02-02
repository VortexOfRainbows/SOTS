using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
 
 
namespace SOTS.Items.GelGear    
{
    public class WormWoodScepter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Scepter");
			
			Tooltip.SetDefault("Summons a Wormwood Turret to fire upon your enemies");
		}
 
        public override void SetDefaults()
        {
            item.damage = 21;  
            item.mana = 20;     
            item.width = 44;  
            item.height = 44;    
            item.useTime = 45; 
            item.useAnimation = 45;   
            item.useStyle = 1;  
            item.noMelee = true;
            item.knockBack = 0;  
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = 4;  
            item.UseSound = SoundID.Item44; 
            item.autoReuse = true;   
            item.shoot = mod.ProjectileType("PinkyTurret");  
            item.summon = true;  
            item.sentry = true;
        } 
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "Wormwood", 28);
			recipe.AddIngredient(ItemID.PinkGel, 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 cursorPos = Main.MouseWorld;
			position = cursorPos;
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