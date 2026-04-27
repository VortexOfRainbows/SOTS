using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Buffs;
using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.FakePlayer;
using static SOTS.SOTS;
using SOTS.Void;
using System.Collections.Generic;
using System.Linq;

namespace SOTS.Items.Potions
{
	public class VigorPotion : ModItem
	{
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string bufftime = "BuffTime";
            tooltips.Remove(tooltips.Where(c => c.Name == bufftime).First());
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 34;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
            Item.buffType = ModContent.BuffType<Vigor>();   
            Item.buffTime = 72100;  
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if (!modPlayer.VigorActive)
            {
                modPlayer.VigorDashes = 0;
            }
            modPlayer.VigorDashes += 50;
            return true;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient<Invidia.InvidiaPetal>(1).AddIngredient(ItemID.Shiverthorn, 1).AddIngredient(ItemID.Daybloom, 1).AddTile(TileID.Bottles).Register();
		}
	}
    public class SOTSVigorDashPlayer : ModPlayer
    {
        public void SendPacket()
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            var packet = Instance.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncVigorDash);
            packet.Write((byte)Player.whoAmI);
            packet.Write(DashDir);
            packet.Write(DashTimer);
            packet.Write(Player.SOTSPlayer().VigorDashes);
            packet.Send(-1, Player.whoAmI);
        }
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public int DashDir = -1;

        public bool DashActive => DashTimer > 0;
        public int DashDelay = 0;
        public int DashTimer = 0;
        public static readonly int MaxDashDelay = 40;
        public static readonly int MaxDashTimer = 20;
        public override void ResetEffects()
        {
            if(DashActive)
            {
                if (DashTimer == MaxDashTimer)
                {
                    Player.velocity.X *= 0.2f;
                    Player.velocity += new Vector2(DashDir * 8, 0);
                    for (int i = 0; i < 30; i++)
                    {
                        PixelDust.Spawn(Player.position, Player.width, Player.height, -Player.velocity * Main.rand.NextFloat() + Main.rand.NextVector2Circular(2, 2), ColorHelper.PinkPetal, Main.rand.Next(3, 7)).scale *= Main.rand.NextFloat(1.5f, 2.25f);
                    }
                }
                Player.armorEffectDrawShadowEOCShield = true;
                Player.velocity += new Vector2(DashDir * 0.12f * DashTimer / (float)MaxDashTimer, 0);
                Vector2 dashMove = new Vector2(DashDir * 14 * DashTimer / (float)MaxDashTimer, 0);
                dashMove = Collision.TileCollision(Player.position, dashMove, Player.width, Player.height);
                Player.position += dashMove;
                --DashTimer;
                for (float i = 0; i < 1; i += 0.34f)
                {
                    Vector2 offset = Player.velocity * i + dashMove * i;
                    Vector2 speed = -Player.velocity * Main.rand.NextFloat(0.8f) + Main.rand.NextVector2Circular(1.4f, 1.4f);
                    PixelDust.Spawn(Player.Center - new Vector2(12) + offset, 24, 24, speed, ColorHelper.PinkPetal, Main.rand.Next(9, 11)).scale *= Main.rand.NextFloat(1, 1.7f);

                    Dust d = Dust.NewDustDirect(Player.position + offset + new Vector2(0, Player.height - 4), Player.width, 4, SOTSUtils.TypeHelper.CopyDust4Type, newColor: ColorHelper.PinkPetal);
                    d.velocity = speed * 0.4f + d.velocity * 0.1f;
                    d.noGravity = true;
                    d.fadeIn = 0.2f;
                    d.scale = d.scale * 0.5f + 1.0f;
                    d.color.A = 0;
                }
                return;
            }
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(Player);
            if(Main.myPlayer == Player.whoAmI)
            {
                DashDelay--;

                if (!modPlayer.VigorActive || Player.mount.Active || DashActive || Player.grappling[0] >= 0)
                    return;

                if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
                    DashDir = 1;
                else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
                    DashDir = -1;
                else
                    return;

                if (DashDelay <= 0)
                {
                    --modPlayer.VigorDashes;
                    DashDelay = MaxDashDelay;
                    DashTimer = MaxDashTimer;
                    if (Main.myPlayer == Player.whoAmI)
                        SendPacket();
                }
            }
        }
    }
}