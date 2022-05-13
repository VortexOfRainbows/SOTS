using SOTS.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Permafrost
{
    public class IceDeployer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snow Crystal");
			Tooltip.SetDefault("Summons the snow-shield mount\nWhile mounted, gain 8 defense");
		}
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
			Item.expert = false;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<IceShield>();
        }
    }
}