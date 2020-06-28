using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
 
 
namespace SOTS.Items.Celestial    
{
    public class VoidspaceAuraStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidspace Aura Staff");
			Tooltip.SetDefault("Summons a voidspace cell on your cursor\nGrants stat boosts while in the radius of the cell\nVoid regen increased by 5, life regen by 5, defense by 5, and reduces damage taken by 10%\nDamages all enemies within range\nRange scales with summon damage");
		}
        public override void SetDefaults()
        {
            item.damage = 48;  
            item.mana = 12;   
            item.width = 44;    
            item.height = 44;    
            item.useTime = 45;  
            item.useAnimation = 45;   
            item.useStyle = 1;  
            item.noMelee = true; 
            item.knockBack = 0; 
            item.value = Item.sellPrice(0, 9, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item44; 
            item.autoReuse = true;  
            item.shoot = mod.ProjectileType("AuraCell");  
            item.summon = true; 
            item.sentry = true; 
        } 
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			return player.altFunctionUse != 2;
		}
    }
}