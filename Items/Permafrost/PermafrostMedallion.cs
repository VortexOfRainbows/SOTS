using SOTS.Items.Earth.Glowmoth;
using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class PermafrostMedallion : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Summon;
            Item.width = 34;     
            Item.height = 38;   
            Item.value = Item.sellPrice(0, 5, 50, 0);
            Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<AbsoluteBar>(7).AddIngredient<NightIlluminator>(1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.sotsPlayer().artifactProbeDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
			player.sotsPlayer().artifactProbeNum += 8;
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}