using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SOTS.Projectiles.Minions;

namespace SOTS.Items.Slime    
{
    public class WormWoodScepter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Scepter");
			Tooltip.SetDefault("Summons a Wormwood Turret to fire upon up to 4 enemies at a time");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 21;  
            Item.mana = 20;     
            Item.width = 44;  
            Item.height = 44;    
            Item.useTime = 45; 
            Item.useAnimation = 45;   
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.noMelee = true;
            Item.knockBack = 0;  
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = 4;  
            Item.UseSound = SoundID.Item44; 
            Item.autoReuse = true;   
            Item.shoot = ModContent.ProjectileType<PinkyTurret>();  
            Item.DamageType = DamageClass.Summon; 
            Item.sentry = true;
        } 
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32).AddIngredient<Wormwood>(28).AddTile(TileID.Anvils).Register();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int index = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                Main.projectile[index].originalDamage = Item.damage;
            }
            return false;
        }
    }
}