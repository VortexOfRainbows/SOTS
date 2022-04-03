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
			DisplayName.SetDefault("Bone Clapper");
			Tooltip.SetDefault("Charge to increase damage up to 450%\nTakes 2 seconds to reach max charge");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 27;
            item.melee = true;  
            item.width = 42;
            item.height = 42;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 6f;
            item.value = Item.sellPrice(0, 3, 60, 0);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<BoneClapperCrusher>(); 
            item.shootSpeed = 12f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 4;
		}
	}
}
