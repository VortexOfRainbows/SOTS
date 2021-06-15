using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class InfectionTester : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infection Tester");
			Tooltip.SetDefault("Creates a clump of infected pyramid brick\nCode within is also used for the Pyramid Worldgen (static method)");
		}
		public override void SetDefaults()
		{
            item.useTime = 15; 
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.knockBack = 0;
            item.rare = 12;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
            item.shoot =  10; 
            item.shootSpeed = 24;
			item.expert = true;

		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position = Main.MouseWorld;
			if (player.altFunctionUse == 2)
			{
				SOTSWorldgenHelper.GenerateAcediaRoom((int)position.X /16, (int)position.Y / 16, mod, Main.rand.Next(-1, 2));
				return false;
			}
			if (player.altFunctionUse != 2)
			{
				PyramidWorldgenHelper.GenerateBossRoom((int)position.X / 16, (int)position.Y / 16);
				return false;
			}
			//Generate(position, mod, true);
			return false;
		}
	}
}
