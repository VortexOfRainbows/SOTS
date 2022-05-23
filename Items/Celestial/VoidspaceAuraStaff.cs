using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SOTS.Projectiles.Minions;
using Terraria.DataStructures;

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
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.noMelee = true; 
            Item.knockBack = 1f; 
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44; 
            Item.autoReuse = true;  
            Item.shoot = ModContent.ProjectileType<VoidspaceCell>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true; 
        } 
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SanguiteBar>(), 15).AddTile(TileID.MythrilAnvil).Register();
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse != 2)
            {
                int index = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                Main.projectile[index].originalDamage = Item.damage;
            }
            return false;
        }
    }
}