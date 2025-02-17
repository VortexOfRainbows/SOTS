using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class LevMirror : VoidItem
	{
		public override void SafeSetDefaults()
		{
			Item.CloneDefaults(ItemID.MagicMirror);
			Item.width = 52;
			Item.height = 38;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item6;
		}
        public override void UseItemFrame(Player player)
        {
            player.itemLocation.X -= 24 * player.direction;
			Color c = Helpers.ColorHelper.EmeraldColor;
			c.A = 0;
			if(Main.rand.NextBool(2))
			{
                PixelDust.Spawn(player.itemLocation + new Vector2(Item.width / 2 * player.direction, -player.gravDir * Item.height / 2), 0, 0, Main.rand.NextVector2Circular(6, 6), c, Main.rand.Next(4, 7)).scale = Main.rand.NextFloat(1, 1.5f);
            }
            if (player.itemTime == player.itemAnimationMax / 2)
            {
                for(int i = 0; i < 60; i ++)
                    PixelDust.Spawn(player.position, player.width, player.height, Main.rand.NextVector2Circular(8, 8), c, Main.rand.Next(2, 6)).scale = Main.rand.NextFloat(1.5f, 2.5f);
                if (Common.Systems.ImportantTilesWorld.InvidiaPortal.HasValue)
                {
                    Vector2 dest = Common.Systems.ImportantTilesWorld.InvidiaPortal.Value.ToVector2() * 16 + new Vector2(0, 32);
                    player.Teleport(dest, -1);
                }
                for (int i = 0; i < 60; i ++)
                    PixelDust.Spawn(player.position, player.width, player.height, Main.rand.NextVector2Circular(8, 8), c, Main.rand.Next(2, 6)).scale = Main.rand.NextFloat(1.5f, 2.5f);
            }
        }
        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }
        public override int GetVoid(Player player)
        {
            return 20;
        }
    }
}