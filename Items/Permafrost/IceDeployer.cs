
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
            
            item.width = 30;
            item.height = 30;
			item.expert = false;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 80000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.noMelee = true;
            item.mountType = mod.MountType("IceShield");
        }
    }
}