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
            Item.damage = 90;  
            Item.mana = 12;   
            Item.width = 44;    
            Item.height = 40;    
            Item.useTime = 45;  
            Item.useAnimation = 45;   
            Item.useStyle = ItemUseStyleID.SwingThrow;  
            Item.noMelee = true; 
            Item.knockBack = 1f; 
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44; 
            Item.autoReuse = true;  
            Item.shoot = ModContent.ProjectileType<VoidspaceCell>();  
            Item.summon = true; 
            Item.sentry = true; 
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