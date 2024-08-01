using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;

namespace SOTS.Items.Crushers
{
	public class MantisGrip : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
		public override void SafeSetDefaults()
		{
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 50;
            Item.height = 50;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 10f;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MantisGripCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool CanShoot(Player player)
        {
            return true; //player.ownedProjectileCounts[Item.shoot] <= 0;
        }
        public override bool BeforeDrainVoid(Player player)
        {
            return false;
        }
        public override int GetVoid(Player player)
		{
			return 7;
		}
	}
}