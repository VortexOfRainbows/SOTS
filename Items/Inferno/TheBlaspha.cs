using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Fragments;
using SOTS.Projectiles;
using SOTS.Projectiles.Inferno;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{
	public class TheBlaspha : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;  
            Item.width = 68;   
            Item.height = 26;
            Item.useTime = 33; 
            Item.useAnimation = 33;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = null;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = 10;
            Item.shootSpeed = 12.0f;
            Item.useAmmo = AmmoID.Bullet;
            Item.channel = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Blaspha>(), damage, knockback, player.whoAmI, (int)(Item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod), type);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SniperRifle, 1).AddIngredient(ItemID.TacticalShotgun, 1).AddIngredient(ModContent.ItemType<Doomstick>(), 1).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
