using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{	
	public class HeartInAJar : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart in a Jar");
			Tooltip.SetDefault("Curses enemies within a small radius around you, draining 10 life per second\nCursed enemies release Curse Fragments upon death, which seek out other enemies\nIncreases max life by 20");
		}
		public override void SetDefaults()
		{
            item.width = 38;     
            item.height = 48;   
            item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
			item.expert = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.CurseAura = true;
			if(!hideVisual)
				for(int j = 0; j < 120; j++)
				{
					Vector2 circular = new Vector2(320, 0).RotatedBy(MathHelper.ToRadians(j * 3 + modPlayer.orbitalCounter * 0.2f));
					Dust dust = Dust.NewDustDirect(player.Center + circular - new Vector2(5), 0, 0, ModContent.DustType<ShortlivedCurseDust>());
					dust.velocity *= 0f;
					dust.scale = 1.25f;
					dust.noGravity = true;
					dust.color = new Color(150, 100, 130, 0);
					dust.alpha = 210;
				}
			player.statLifeMax2 += 20;
		}
	}
}