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
            Item.damage = 26;
            Item.melee = true;  
            Item.width = 42;
            Item.height = 42;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldingOut;    
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
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 5;
		}
	}
}
