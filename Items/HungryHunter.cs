using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Items
{
    public class HungryHunter : VoidItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hungry Hunter");
			Tooltip.SetDefault("Latches onto enemies and regenerates void upon hit");
		}
        public override void SafeSetDefaults()
        {
            Item.damage = 21;
            Item.width = 32;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Pink;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.HoldingOut;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 4.5f;
            Item.noUseGraphic = true; 
            Item.shoot = ModContent.ProjectileType<Projectiles.HungryHunter>();
            Item.shootSpeed = 21.5f;
            Item.UseSound = SoundID.Item1;
            Item.melee = true; 
            Item.channel = true;
        }
		public override int GetVoid(Player player)
		{
			return  10;
		}
    }
}