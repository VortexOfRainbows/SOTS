using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Slime;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Crushers
{
	public class AquaticEclipse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 92;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 56;
            Item.height = 56;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 10f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EclipseCrusher>(); 
            Item.shootSpeed = 20f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
        public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 7;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Hellbreaker>(), 1).AddIngredient(ModContent.ItemType<WormWoodCollapse>(), 1).AddIngredient(ModContent.ItemType<CrabClaw>(), 1).AddIngredient(ModContent.ItemType<CursedMatter>(), 5).AddIngredient(ItemID.SoulofNight, 10).AddIngredient(ItemID.SoulofLight, 10).AddIngredient(ItemID.Amethyst, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
