using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Items.OreItems;
using SOTS.Projectiles.Planetarium;
using SOTS.Dusts;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Temple;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Blades;
using SOTS.Items.ChestItems;
using System;
using Terraria.DataStructures;
using static SOTS.ItemHelpers;

namespace SOTS.Items.Conduit
{
	public class SkipScythe : VoidItem
	{ 	
		public override void SetStaticDefaults()
		{
			ItemID.Sets.UsesCursedByPlanteraTooltip[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 105;  
            Item.DamageType = DamageClass.Melee; 
            Item.width = 78;    
            Item.height = 82;  
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;   
            Item.autoReuse = true; 
            Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ModContent.RarityType<AnomalyRarity>();
            Item.UseSound = SoundID.Item71;
			Item.crit = 11;
			Item.shoot = ModContent.ProjectileType<SeleneSlash>();
			Item.shootSpeed = 15f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 3 * SOTSUtils.SignNoZero(velocity.X) * player.gravDir, 1);
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255 - Item.alpha) / 255f);
		}
        public override bool BeforeUseItem(Player player)
        {
			return NPC.downedPlantBoss;
		}
		public override int GetVoid(Player player)
		{
			return 15;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Helios>(1).AddIngredient(ItemID.DeathSickle).AddIngredient<SectionChiefsScythe>(1).AddIngredient<BetrayersKnife>(1).AddIngredient<SkipSoul>(30).AddIngredient<SkipShard>(15).AddIngredient<DissolvingUmbra>(1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
