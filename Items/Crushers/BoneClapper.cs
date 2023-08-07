using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Crushers;

namespace SOTS.Items.Crushers
{
	public class BoneClapper : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 26;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 42;
            Item.height = 42;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 3, 60, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BoneClapperCrusher>(); 
            Item.shootSpeed = 12f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 5;
		}
	}
}
