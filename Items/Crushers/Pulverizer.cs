using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class Pulverizer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 76;
            Item.height = 76;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 7f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Crushers.PulverizerCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		public override bool CanShoot(Player player)
		{
			return true; // player.ownedProjectileCounts[Item.shoot] <= 0;
		}
		public override bool BeforeDrainVoid(Player player)
		{
			return false;
		}
		public override int GetVoid(Player player)
		{
			return 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<BoneClapper>(1).AddIngredient<MantisGrip>(1).AddIngredient(ItemID.HallowedBar, 10).AddIngredient(ItemID.SoulofFright, 10).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
