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
using SOTS.Projectiles.Blades;
using System;

namespace SOTS.Items.Temple
{
	public class Pyrocide : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyrocide");
            Tooltip.SetDefault("Strike with 5 slashes\nThe final slash does 150% damage");
            ItemID.Sets.UsesCursedByPlanteraTooltip[Type] = true;
            this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 76;
            Item.height = 82;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PyrocideSlash>(); 
            Item.shootSpeed = 18f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.LunarTabletFragment, 20).AddIngredient(ItemID.LihzahrdPowerCell, 1).AddTile(TileID.MythrilAnvil).Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 5 * Math.Sign(velocity.X), Main.rand.NextFloat(0.8f, 0.9f));
			return false;
		}
        public override int GetVoid(Player player)
        {
            return 15;
        }
        public override bool BeforeUseItem(Player player)
        {
            return NPC.downedPlantBoss;
        }
    }
}
