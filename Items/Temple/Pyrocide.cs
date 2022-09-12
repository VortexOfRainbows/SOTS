using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Temple;

namespace SOTS.Items.Temple
{
	public class Pyrocide : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrocide");
            Tooltip.SetDefault("Strike with 5 slashes\nThe final slash does 150% damage");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 76;
            Item.height = 82;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PyrocideSlash>(); 
            Item.shootSpeed = 18f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		int i = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			i++;
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, (i % 2 * 2 -1) * 5, Main.rand.NextFloat(0.8f, 0.9f));
			return false;
		}
        public override int GetVoid(Player player)
        {
            return 6;
        }
    }
}
