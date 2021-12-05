using Microsoft.Xna.Framework;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
    [AutoloadEquip(EquipType.Shoes)]
    public class FlashsparkBoots : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flashspark Boots");
			Tooltip.SetDefault("Provides tremendous acceleration while running\nAlso provides flight and extra mobility on ice\nIncreases movement speed greatly\nProvides the ability to walk on water and lava\nGrants immunity to fire blocks and 10 seconds of immunity to lava\n'Recipro Burst!'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 5));
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
			recipe.AddIngredient(ModContent.ItemType<AbsoluteBar>(), 12);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        bool activateParticle = false;
        int hasActivate = -1;
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
                if(hasActivate == -1)
                {
                    Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 45, 1.3f, -0.4f);
                    Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 14, 1.0f, -0.3f);
                    hasActivate = 180;
                    for(int i = 0; i < 3; i++)
                    {
                        int amt = 45 + i * 15;
                        for (int j = 0; j < amt; j++)
                        {
                            Vector2 circularLocation = new Vector2(30, 0).RotatedBy(MathHelper.ToRadians(360f * j / amt));
                            circularLocation.X *= 0.6f;
                            var index = Dust.NewDust(player.Center + circularLocation - new Vector2(5), 0, 0, DustID.Fire, -player.velocity.X * 1.5f, player.velocity.Y * 0.5f, 50, new Color(), 5f - i * 0.8f);
                            circularLocation = circularLocation.SafeNormalize(Vector2.Zero) * 7.5f;
                            circularLocation.X -= player.velocity.X * (1.5f + (i * 0.4f));
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
                    for (int j = 0; j < 20; j++)
                    {
                        Vector2 velo = new Vector2(-player.velocity.X * (1f + (j * 0.1f)) + Main.rand.NextFloat(-1.00f, 1.00f), Main.rand.NextFloat(-6.0f, 6.0f));
                        velo.X *= Main.rand.NextFloat(0.5f, 1.5f);
                        MakeDustShape(player, player.Center, velo, j % 4);
                    }
                    player.velocity.X *= 2.5f;
                }
            }
            else
            {
                hasActivate = -1;
            }
            if(hasActivate > 0)
            {
                hasActivate--;
            }
            if (particles && doAccel)
            {
                player.accRunSpeed = 0f;
                player.velocity *= 1 / 0.96f;
            }
        }
        public void MakeDustShape(Player player, Vector2 spawnPos, Vector2 velocity, int type = 0, float scale = 1f)
        {
            float size = Main.rand.NextFloat(0.75f, 1.25f) * scale;
            if(type == 0)
            {
                Vector2 position = spawnPos + new Vector2(-24, -24) * size;
                float rotateDiff = Main.rand.NextFloat(-0.2f, 0.2f);
                for (int j = 0; j < 4; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        position += new Vector2(3 * size, 0).RotatedBy(rotateDiff + MathHelper.ToRadians(90 * j));
                        Dust dust = Dust.NewDustDirect(position - new Vector2(5), 0, 0, DustID.Fire, -player.velocity.X * 1.5f, player.velocity.Y * 0.5f, 50, new Color(), 3.6f * size);
                        dust.velocity *= 0.5f;
                        dust.velocity = velocity;
                        dust.noGravity = true;
                        dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                    }
            }
            else if (type == 1)
            {
                Vector2 position = spawnPos + new Vector2(-32, -20) * size;
                float rotateDiff = Main.rand.NextFloat(-0.2f, 0.2f);
                for(int j = 0; j < 5; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        position += new Vector2(4 * size, 0).RotatedBy(rotateDiff + MathHelper.ToRadians(144 * j));
                        Dust dust = Dust.NewDustDirect(position - new Vector2(5), 0, 0, DustID.Fire, -player.velocity.X * 1.5f, player.velocity.Y * 0.5f, 50, new Color(), 3.2f * size);
                        dust.velocity *= 0.5f;
                        dust.velocity = velocity;
                        dust.noGravity = true;
                        dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                    }
            }
            else if (type == 2)
            {
                Vector2 position = spawnPos + new Vector2(-24, -16) * size;
                float rotateDiff = Main.rand.NextFloat(-0.2f, 0.2f);
                for (int j = 0; j < 3; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        position += new Vector2(3 * size, 0).RotatedBy(rotateDiff + MathHelper.ToRadians(120 * j));
                        Dust dust = Dust.NewDustDirect(position - new Vector2(5), 0, 0, DustID.Fire, -player.velocity.X * 1.5f, player.velocity.Y * 0.5f, 50, new Color(), 3.5f * size);
                        dust.velocity *= 0.5f;
                        dust.velocity = velocity;
                        dust.noGravity = true;
                        dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                    }
            }
            else
            {
                Vector2 position = spawnPos;
                for (int i = 0; i < 24; i++)
                {
                    Vector2 spawn = position + new Vector2(12 * size, 0).RotatedBy(MathHelper.ToRadians(15 * i));
                    Dust dust = Dust.NewDustDirect(spawn - new Vector2(5), 0, 0, DustID.Fire, -player.velocity.X * 1.5f, player.velocity.Y * 0.5f, 50, new Color(), 3.5f * size);
                    dust.velocity *= 0.5f;
                    dust.velocity = velocity;
                    dust.noGravity = true;
                    dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                }
            }
        }
        bool doAccel = false;
		public bool FireBoostPlayer(Player player)
        {
            doAccel = false;
            bool doAccel1 = false;
            bool doAccel2 = false;
            float doubleRun = player.runAcceleration * 2.5f;
            float doubleAcc = 13f;
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
                    for (float i = 0; i <= 3; i += 0.6f)
                    {
                        int length = 1;
                        if (Main.rand.NextBool(15))
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
                    for (float i = 0; i < 2; i ++)
                    {
                        if (Main.rand.NextBool(26 - hasActivate / 9))
                        {
                            Vector2 velo = new Vector2(-player.velocity.X * (0.6f + 0.6f * hasActivate / 180f) + Main.rand.NextFloat(-1.00f, 1.00f), Main.rand.NextFloat(-2.4f, 2.4f));
                            velo.X *= Main.rand.NextFloat(0.5f, 1.25f);
                            MakeDustShape(player, player.Center, velo, Main.rand.Next(4) % 4, 0.6f + 0.4f * hasActivate / 180f);
                        }
                    }
                }
            }
            return doDust;
        }
	}
}
