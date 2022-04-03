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
            item.damage = 15;
            item.melee = true;  
            item.width = 50;
            item.height = 50;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 10f;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<MantisGripCrusher>(); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
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