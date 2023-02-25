using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;
using SOTS.Projectiles.Pyramid;

namespace SOTS.Items.Pyramid
{
	public class SandstoneWarhammer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			Item.shoot = ModContent.ProjectileType<Bloodaxe>();  
            Item.shootSpeed = 13.5f;
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
	}
}