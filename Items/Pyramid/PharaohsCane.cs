using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class PharaohsCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pharaoh's Cane");
			Tooltip.SetDefault("Terrible for combat, but makes enemies drop more gold\n'Who would ever want such a thing?'");
		}
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.melee = true;
			Item.width = 36;
			Item.height = 26;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = 1;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;     
		}
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			target.AddBuff(BuffID.Midas, 1200);
        }
    }
}