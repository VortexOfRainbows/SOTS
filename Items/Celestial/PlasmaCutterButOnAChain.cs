using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Projectiles.Celestial;

namespace SOTS.Items.Celestial
{
	public class PlasmaCutterButOnAChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 130;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 64;
            Item.height = 54;  
            Item.useTime = 28; 
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;    
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item22;
            Item.shoot = ModContent.ProjectileType<PlasmaCutter>(); 
            Item.shootSpeed = 0f;
			Item.axe = 200;
            Item.autoReuse = false;
            Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
        public override float UseTimeMultiplier(Player player)
		{
			float distance = Vector2.Distance(player.Center, Main.MouseWorld);
			if (distance < 48)
				distance = 48;
			if (distance > 896)
				distance = 896;
			float spinSpeed = 1 + (2574f / distance);
			int speedMod = (int)(28f - spinSpeed);
			if (speedMod < 6)
				speedMod = 6;
			int useTime = speedMod;
			Item.useTime = useTime;
			Item.useAnimation = useTime;
			return base.UseTimeMultiplier(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.UseSound = SoundID.Item22;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, (float)Math.Atan2(velocity.Y, velocity.X) + 90f, 0);
            return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SanguiteBar>(10).AddIngredient<ChainedPlasma>(1).AddIngredient(ItemID.ButchersChainsaw, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}