using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class FlashsparkBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flashspark Boots");
			Tooltip.SetDefault("Provides tremendous acceleration while running\nAlso provides flight and extra mobility on ice\nIncreases movement speed greatly\nProvides the ability to walk on water and lava\nGrants immunity to fire blocks and 10 seconds of immunity to lava");
		}
		public override void SetDefaults()
		{
            item.width = 42;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
			item.expert = false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
		    recipe.AddIngredient(ItemID.FrostsparkBoots, 1);
			recipe.AddIngredient(ItemID.LavaWaders, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 12);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        bool activateParticle = false;
        bool hasActivate = false;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[BuffID.Burning] = true;
			player.waterWalk = true; 
			player.fireWalk = true; 
			player.lavaMax += 600; 
			player.rocketBoots = 2; 
			player.iceSkate = true;
			player.moveSpeed += 0.2f;
            player.accRunSpeed = 7f;
            bool particles = FireBoostPlayer(player);
            if(activateParticle)
            {
                if(!hasActivate)
                {
                    Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 45, 1.3f, -0.4f);
                    Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 14, 1.0f, -0.3f);
                    hasActivate = true;
                    for(int i = 0; i < 3; i++)
                    {
                        int amt = 45 + i * 15;
                        for (int j = 0; j < amt; j++)
                        {
                            Vector2 circularLocation = new Vector2(30, 0).RotatedBy(MathHelper.ToRadians(360f * j / amt));
                            circularLocation.X *= 0.6f;
                            var index = Dust.NewDust(player.Center + circularLocation - new Vector2(5), 0, 0, DustID.Fire, -player.velocity.X * 1.5f, player.velocity.Y * 0.5f, 50, new Color(), 5f - i * 0.8f);
                            circularLocation = circularLocation.SafeNormalize(Vector2.Zero) * 7.5f;
                            circularLocation.X -= player.velocity.X * (1.5f + (i * 0.5f));
                            if(i == 0)
                            {
                                circularLocation *= 0.33f;
                            }
                            if (i == 1)
                            {
                                circularLocation *= 0.66f;
                            }
                            Main.dust[index].velocity.X += circularLocation.X;
                            Main.dust[index].velocity.Y = circularLocation.Y;
                            Main.dust[index].noGravity = true;
                            Main.dust[index].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                        }
                    }
                    player.velocity.X *= 2.2f;
                }
            }
            else
            {
                hasActivate = false;
            }
            if (particles && doAccel)
            {
                player.accRunSpeed = 0f;
                player.velocity *= 1 / 0.96f;
            }
        }
        bool doAccel = false;
		public bool FireBoostPlayer(Player player)
        {
            doAccel = false;
            bool doAccel1 = false;
            bool doAccel2 = false;
            float doubleRun = player.runAcceleration * 2.5f;
            float doubleAcc = 14f;
            bool doDust = false;
            bool otherDust = false;
            float num1 = (doubleAcc + player.maxRunSpeed) / 1.4f;
            if (player.controlLeft && player.velocity.X > -doubleAcc && player.dashDelay >= 0)
            {
                if (player.mount.Active && player.mount.Cart)
                {
                    if (player.velocity.X < 0.0)
                        player.direction = -1;
                }
                else if ((player.itemAnimation == 0 || player.inventory[player.selectedItem].useTurn) && player.mount.AllowDirectionChange)
                    player.direction = -1;
                doAccel1 = true;
                float speedMod = 0.25f;
                if(player.velocity.X < -player.accRunSpeed && player.velocity.Y == 0.0)
                {
                    otherDust = true;
                    player.accRunSpeed = doubleAcc;
                    speedMod = 1f;
                }
                if (player.velocity.Y == 0.0 || player.wingsLogic > 0 || player.mount.CanFly)
                {
                    player.velocity.X -= doubleRun * 0.2f * speedMod;
                    if (player.wingsLogic > 0)
                        player.velocity.X -= doubleRun * 0.2f * speedMod;
                }
            }
            else if (player.controlRight && player.velocity.X < doubleAcc && player.dashDelay >= 0)
            {
                if (player.mount.Active && player.mount.Cart)
                {
                    if (player.velocity.X > 0.0)
                        player.direction = -1;
                }
                else if ((player.itemAnimation == 0 || player.inventory[player.selectedItem].useTurn) && player.mount.AllowDirectionChange)
                    player.direction = 1;
                doAccel2 = true;
                float speedMod = 0.25f;
                if (player.velocity.X > player.accRunSpeed && player.velocity.Y == 0.0)
                {
                    otherDust = true;
                    player.accRunSpeed = doubleAcc;
                    speedMod = 1f;
                }
                if (player.velocity.Y == 0.0 || player.wingsLogic > 0 || player.mount.CanFly)
                {
                    player.velocity.X += doubleRun * 0.2f * speedMod;
                    if (player.wingsLogic > 0)
                        player.velocity.X += doubleRun * 0.2f * speedMod;
                }
            }
            if (player.velocity.X < -num1 && player.velocity.Y == 0.0 && !player.mount.Active)
            {
                doDust = true;
                if (doAccel1 && !doAccel2)
                {
                    if (player.mount.Active && player.mount.Cart)
                    {
                        if (player.velocity.X < 0.0)
                            player.direction = -1;
                    }
                    else if ((player.itemAnimation == 0 || player.inventory[player.selectedItem].useTurn) && player.mount.AllowDirectionChange)
                        player.direction = -1;
                    doAccel = true;
                }
            }
            if (player.velocity.X > num1 && player.velocity.Y == 0.0 && !player.mount.Active)
            {
                doDust = true;
                if (doAccel2 && !doAccel1)
                {
                    if (player.mount.Active && player.mount.Cart)
                    {
                        if (player.velocity.X > 0.0)
                            player.direction = -1;
                    }
                    else if ((player.itemAnimation == 0 || player.inventory[player.selectedItem].useTurn) && player.mount.AllowDirectionChange)
                        player.direction = 1;
                    doAccel = true;
                }
            }
            if (doDust || otherDust)
            {
                int height = player.height;
                var num3 = 0;
                if (player.gravDir == -1.0)
                    num3 -= height;
                if (player.runSoundDelay == 0 && player.velocity.Y == 0.0)
                {
                    if(!doDust)
                    {
                        Main.PlaySound(player.hermesStepSound.SoundType, (int)player.Center.X, (int)player.Center.Y, player.hermesStepSound.SoundStyle, 1f, -0.075f);
                        player.runSoundDelay = (int)(player.hermesStepSound.IntendedCooldown / 1.25f);
                    }    
                    else
                    {
                        Main.PlaySound(player.hermesStepSound.SoundType, (int)player.Center.X, (int)player.Center.Y, player.hermesStepSound.SoundStyle, 1f, 0.125f);
                        player.runSoundDelay = (int)(player.hermesStepSound.IntendedCooldown / 1.6f);
                    }
                }
                if(!doDust)
                {
                    var index = Dust.NewDust(new Vector2(player.position.X - 4f, player.position.Y + player.height + num3), player.width + 8, 4, 16, -player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 50, new Color(), 1.5f);
                    Main.dust[index].velocity.X *= 0.2f;
                    Main.dust[index].velocity.Y *= 0.2f;
                    Main.dust[index].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                    activateParticle = false;
                }
                else
                {
                    activateParticle = true;
                    for (float i = 0; i <= 3; i += 0.5f)
                    {
                        int length = 1;
                        if (Main.rand.NextBool(10))
                            length = 2;
                        for(int j = 0; j < length; j++)
                        {
                            var index = Dust.NewDust(new Vector2(player.Center.X, player.position.Y + (-j + 1) * (height + num3) + Main.rand.Next(j * player.height)) + player.velocity * (1.5f - i) - new Vector2(5), 0, (int)(8 * player.gravDir * (-j + 1)), DustID.Fire, -player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 50, new Color(), 3f);
                            Main.dust[index].velocity.X *= 0.65f;
                            Main.dust[index].velocity.Y *= 0.65f;
                            Main.dust[index].noGravity = true;
                            Main.dust[index].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                        }
                    }
                }
            }
            return doDust;
        }
	}
}
