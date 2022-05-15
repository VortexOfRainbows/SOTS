using Terraria;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class SandstoneWarhammer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandstone Warhammer");
			Tooltip.SetDefault("Launches homing hammers");
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.noMelee = false;
			Item.shoot = Mod.Find<ModProjectile>("Bloodaxe").Type;  
            Item.shootSpeed = 13.5f;
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
	}
}