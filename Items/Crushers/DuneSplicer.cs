using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Crushers;

namespace SOTS.Items.Crushers
{
	public class DuneSplicer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Splicer");
			Tooltip.SetDefault("Launches out a swarm of short-range, homing spikes\nCharge to increase damage up to 350%\nTakes 4 seconds to reach max charge");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 54;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 54;
            Item.height = 62;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DuneSplicerCrusher>(); 
            Item.shootSpeed = 12f;
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
			return 8;
		}
	}
}
