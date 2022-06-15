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
			Tooltip.SetDefault("Curses enemies within a small radius around you, draining 10 life per second\nCursed enemies release a Curse Fragment upon death, which seeks out other enemies\nCurse fragments deal damage equivalent to 10% of the killed enemies max health plus an additional 10 damage\nIncreases max life by 20");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 38;     
            Item.height = 48;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
			Item.expert = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CurseAura = true;
			if(!hideVisual)
				for(int j = 0; j < 90; j++)
				{
					Vector2 circular = new Vector2(270, 0).RotatedBy(MathHelper.ToRadians(j * 4 + modPlayer.orbitalCounter * 0.3f));
					int i2 = (int)(circular.X + player.Center.X) / 16;
					int j2 = (int)(circular.Y + player.Center.Y) / 16;
					bool disable = false;
					if (!WorldGen.InWorld(i2, j2, 20) || Main.tile[i2, j2].HasTile && Main.tileSolidTop[Main.tile[i2, j2].TileType] == false && Main.tileSolid[Main.tile[i2, j2].TileType] == true)
						disable = true;
					if (!disable)
					{
						Dust dust = Dust.NewDustDirect(player.Center + circular - new Vector2(5), 0, 0, ModContent.DustType<ShortlivedCurseDust>());
						dust.velocity *= 0f;
						dust.scale = 1.25f;
						dust.noGravity = true;
						dust.color = new Color(150, 100, 130, 0);
						dust.alpha = 210;
					}
				}
			player.statLifeMax2 += 20;
		}
	}
}