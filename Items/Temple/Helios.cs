using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Temple
{
	public class Helios : ModItem
	{
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios");
			Tooltip.SetDefault("Attacks scorch with each hit, increasing damage done by 3% permanently\nStacks up to 5 times\nLaunches waves that deal 40% damage");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 70;
			Item.DamageType = DamageClass.Melee;
			Item.width = 94;
			Item.height = 94;
			Item.useTime = 33;
			Item.useAnimation = 33;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Temple.Helios>(); 
            Item.shootSpeed = 10.5f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			//Item.channel = true;
		}
        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] < 1 && NPC.downedPlantBoss;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LunarTabletFragment, 20).AddIngredient(ItemID.LihzahrdPowerCell, 1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 10.5f, type, damage, knockback, player.whoAmI, SOTSPlayer.ApplyAttackSpeedClassModWithGeneric(player, Item.DamageType, Item.useTime));
			return false; 
		}
    }
}
	
