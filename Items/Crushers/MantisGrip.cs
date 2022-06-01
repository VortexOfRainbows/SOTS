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
			DisplayName.SetDefault("Mantis Grip");
			Tooltip.SetDefault("Charge to increase damage up to 500%\nThe initial charge consumes no void\nTakes 4 seconds to reach max charge");
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
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
        public override bool BeforeDrainMana(Player player)
        {
            return false;
        }
        public override int GetVoid(Player player)
		{
			return 7;
		}
	}
}