using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Fragments;
using SOTS.Projectiles;
using SOTS.Projectiles.Inferno;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{
	public class TheBlaspha : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blaspha");
            Tooltip.SetDefault("Launch a scatter of inferno-infused bullets which release homings embers for 50% damage");
        }
		public override void SetDefaults()
		{
            item.damage = 47;
            item.ranged = true;  
            item.width = 68;   
            item.height = 26;
            item.useTime = 33; 
            item.useAnimation = 33;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 3f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = null;
            item.autoReuse = false;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = 10;
            item.shootSpeed = 12.0f;
            item.useAmmo = AmmoID.Bullet;
            item.channel = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Blaspha>(), damage, knockBack, player.whoAmI, (int)(item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod), type);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SniperRifle, 1);
            recipe.AddIngredient(ItemID.TacticalShotgun, 1);
            recipe.AddIngredient(ModContent.ItemType<Doomstick>(), 1);
            recipe.AddIngredient(ModContent.ItemType<DissolvingNether>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
