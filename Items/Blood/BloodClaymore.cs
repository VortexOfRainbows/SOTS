using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Blood
{
	public class BloodClaymore : VoidItem
	{	int timer = 0;
		int amount = 0;
		int fatigueTimer = 0;
		int fatigueAmount = 0;
		bool fatigue = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Claymore");
			Tooltip.SetDefault("Revitalizes your health upon striking an enemy");

		}
		public override void SafeSetDefaults()
		{

			item.damage = 21;
			item.melee = true;
			item.width = 36;
			item.height = 38;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 2.2f;
			item.value = 50000;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;       
            item.shootSpeed = 17;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 8;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) 
		{
			player.statLife += (int)(9 + (damage / 7));
			player.HealEffect((int)(9 + (damage / 7)));
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BloodEssence", 12);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}