using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SOTS.Projectiles.Minions;

namespace SOTS.Items.Celestial    
{
    public class VoidspaceAuraStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidspace Aura Staff");
			Tooltip.SetDefault("Summons a voidspace cell on your cursor that releases flames at nearby enemies\nGrants stat boosts while in the radius of the cell\nVoid regeneration speed increased by 4%, life regen by 4, defense by 4, and reduces damage taken by 4%\nDamages all enemies within range\nRange scales with summon damage");
		}
        public override void SetDefaults()
        {
            item.damage = 90;  
            item.mana = 12;   
            item.width = 44;    
            item.height = 40;    
            item.useTime = 45;  
            item.useAnimation = 45;   
            item.useStyle = ItemUseStyleID.SwingThrow;  
            item.noMelee = true; 
            item.knockBack = 1f; 
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item44; 
            item.autoReuse = true;  
            item.shoot = ModContent.ProjectileType<VoidspaceCell>();  
            item.summon = true; 
            item.sentry = true; 
        } 
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SanguiteBar>(), 15);
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