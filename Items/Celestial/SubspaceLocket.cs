using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Otherworld;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{	
	public class SubspaceLocket : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subspace Locket");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.width = 40;     
            item.height = 50;   
            item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Red;
			item.accessory = true;
			item.expert = true;
		}
        public override void UpdateVanity(Player player, EquipType type)
		{
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SubspacePlayer modPlayer = SubspacePlayer.ModPlayer(player);
			if(!hideVisual)
				modPlayer.servantActive = true;
		}
	}
	public class SubspacePlayer : ModPlayer
	{
		public bool servantActive = false;
		public static SubspacePlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<SubspacePlayer>();
		}
        public override void ResetEffects()
		{
			if(servantActive)
				Summon();
			servantActive = false;
        }
		int Probe = -1;
		public void Summon()
		{
			int type = ModContent.ProjectileType<SubspaceServant>();
			if (Main.myPlayer == player.whoAmI)
			{
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.Center, Vector2.Zero, type, 0, 0, player.whoAmI, 0);
				}
				Projectile temp = Main.projectile[Probe];
				if (!temp.active || temp.type != type || temp.owner != player.whoAmI)
				{
					Probe = Projectile.NewProjectile(player.Center, Vector2.Zero, type, 0, 0, player.whoAmI, 0);
				}
				Main.projectile[Probe].timeLeft = 6;
			}
		}
		public void UseVanillaItemProjectile(Vector2 Center, Item sItem, out float shouldBeRotation, ref int shouldBeDirection, bool modded = false)
        {
            Vector2 temp = player.position;
            player.position = Center - new Vector2(10, 22);
            float shouldBeAnimation = 0;
            shouldBeRotation = 0;
            int i = player.whoAmI;
            var weaponDamage = player.GetWeaponDamage(sItem);
            if (true)
            {
                var flag2 = true;
                if ((sItem.type == 65 || sItem.type == 676 || (sItem.type == 723 || sItem.type == 724) ||
                     (sItem.type == 757 || sItem.type == 674 || (sItem.type == 675 || sItem.type == 989)) ||
                     (sItem.type == 1226 || sItem.type == 1227)))//&& player.itemAnimation != player.maxAnimation - 1)
                    flag2 = true; //= false; 
                if (sItem.shoot > 0 && flag2)
                {
                    var shoot = sItem.shoot;
                    var speed = sItem.shootSpeed;
                    if (player.inventory[player.selectedItem].thrown && (double)speed < 16.0)
                    {
                        speed *= player.thrownVelocity;
                        if ((double)speed > 16.0)
                            speed = 16f;
                    }

                    if (sItem.melee)
                        speed /= player.meleeSpeed;
                    var canShoot = false;
                    var Damage = weaponDamage;
                    var knockBack = sItem.knockBack;
                    if (sItem.useAmmo > 0)
                        player.PickAmmo(sItem, ref shoot, ref speed, ref canShoot, ref Damage, ref knockBack, ItemID.Sets.gunProj[sItem.type]);
                    else
                        canShoot = true;
                    if (ItemID.Sets.gunProj[sItem.type])
                    {
                        knockBack = sItem.knockBack;
                        Damage = weaponDamage;
                        speed = sItem.shootSpeed;
                    }

                    if (sItem.type == 71)
                        canShoot = false;
                    if (sItem.type == 72)
                        canShoot = false;
                    if (sItem.type == 73)
                        canShoot = false;
                    if (sItem.type == 74)
                        canShoot = false;
                    if (sItem.type == 1254 && shoot == 14)
                        shoot = 242;
                    if (sItem.type == 1255 && shoot == 14)
                        shoot = 242;
                    if (sItem.type == 1265 && shoot == 14)
                        shoot = 242;
                    if (sItem.type == 3542)
                    {
                        if (Main.rand.Next(100) < 20)
                        {
                            ++shoot;
                            Damage *= 3;
                        }
                        else
                            --speed;
                    }

                    if (shoot == 73)
                    {
                        for (var index = 0; index < 1000; ++index)
                        {
                            if (Main.projectile[index].active && Main.projectile[index].owner == i)
                            {
                                if (Main.projectile[index].type == 73)
                                    shoot = 74;
                                if (shoot == 74 && Main.projectile[index].type == 74)
                                    canShoot = false;
                            }
                        }
                    }

                    if (canShoot)
                    {
                        var num1 = player.GetWeaponKnockback(sItem, knockBack);
                        if (shoot == 228)
                            num1 = 0.0f;
                        if (shoot == 1 && sItem.type == 120)
                            shoot = 2;
                        if (sItem.type == 682)
                            shoot = 117;
                        if (sItem.type == 725)
                            shoot = 120;
                        if (sItem.type == 2796)
                            shoot = 442;
                        if (sItem.type == 2223)
                            shoot = 357;
                        var vector2_1 = player.RotatedRelativePoint(Center, true);
                        var vector2_2 = Vector2.UnitX.RotatedBy(player.fullRotation, new Vector2());
                        var v1 = Main.MouseWorld - vector2_1;
                        var vector2_3 = shouldBeRotation.ToRotationVector2() * shouldBeDirection;
                        if (sItem.type == ItemID.BookStaff)// && player.itemAnimation != player.maxAnimation - 1)
                            v1 = vector2_3;
                        if (v1 != Vector2.Zero)
                            v1.Normalize();
                        var num2 = Vector2.Dot(vector2_2, v1);
                        if (num2 > 0.0)
                            shouldBeDirection = 1;
                        else
                            shouldBeDirection = -1;

                        if (sItem.type == 3094 || sItem.type == 3378 || sItem.type == 3543)
                            vector2_1.Y = player.position.Y + (float)(player.height / 3);
                        if (sItem.type == 2611)
                        {
                            var vector2_4 = v1;
                            if (vector2_4 != Vector2.Zero)
                                vector2_4.Normalize();
                            vector2_1 += vector2_4;
                        }

                        if (sItem.type == ItemID.DD2SquireBetsySword)
                            vector2_1 += v1.SafeNormalize(Vector2.Zero).RotatedBy((double)shouldBeDirection * -1.57079637050629, new Vector2()) * 24f;
                        if (shoot == ProjectileID.Starfury)
                        {
                            vector2_1 = new Vector2((float)(player.position.X + player.width * 0.5 + (Main.rand.Next(201) * -shouldBeDirection) + (Main.mouseX + Main.screenPosition.X - player.position.X)), player.MountedCenter.Y - 600f);
                            num1 = 0.0f;
                            Damage *= 2;
                        }

                        if (sItem.type == ItemID.Blowgun || sItem.type == ItemID.Blowpipe)
                        {
                            vector2_1.X += (float)(6 * shouldBeDirection);
                            vector2_1.Y -= 6f * player.gravDir;
                        }

                        if (sItem.type == ItemID.DartPistol)
                        {
                            vector2_1.X -= (float)(4 * shouldBeDirection);
                            vector2_1.Y -= 1f * player.gravDir;
                        }

                        var f1 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                        var f2 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                        if ((double)player.gravDir == -1.0)
                            f2 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2_1.Y;
                        var num3 = (float)Math.Sqrt((double)f1 * (double)f1 + (double)f2 * (double)f2);
                        var num4 = num3;
                        float num5;
                        if (float.IsNaN(f1) && float.IsNaN(f2) || (double)f1 == 0.0 && (double)f2 == 0.0)
                        {
                            f1 = shouldBeDirection;
                            f2 = 0.0f;
                            num5 = speed;
                        }
                        else
                            num5 = speed / num3;

                        if (sItem.type == 1929 || sItem.type == 2270)
                        {
                            f1 += (float)Main.rand.Next(-50, 51) * 0.03f / num5;
                            f2 += (float)Main.rand.Next(-50, 51) * 0.03f / num5;
                        }

                        var num6 = f1 * num5;
                        var num7 = f2 * num5;
                        if (sItem.type == 757)
                            Damage = (int)((double)Damage * 1.25);
                        if (shoot == 250)
                        {
                            for (var index = 0; index < 1000; ++index)
                            {
                                if (Main.projectile[index].active && Main.projectile[index].owner == player.whoAmI &&
                                    (Main.projectile[index].type == 250 || Main.projectile[index].type == 251))
                                    Main.projectile[index].Kill();
                            }
                        }

                        if (shoot == 12)
                        {
                            vector2_1.X += num6 * 3f;
                            vector2_1.Y += num7 * 3f;
                        }

                        if (sItem.useStyle == 5)
                        {
                            if (sItem.type == ItemID.DaedalusStormbow)
                            {
                                var vector2_4 = new Vector2(num6, num7);
                                vector2_4.X = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                                vector2_4.Y = (float)((double)Main.mouseY + (double)Main.screenPosition.Y -
                                                       (double)vector2_1.Y - 1000.0);
                                shouldBeRotation = (float)Math.Atan2((double)vector2_4.Y * shouldBeDirection, (double)vector2_4.X * shouldBeDirection);
                                //NetMessage.SendData(13, -1, -1, (NetworkText)null, player.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                //NetMessage.SendData(41, -1, -1, (NetworkText)null, player.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            }
                            else if (sItem.type == ItemID.SpiritFlame)
                            {
                                shouldBeRotation = 0.0f;
                                //NetMessage.SendData(13, -1, -1, (NetworkText)null, player.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                //NetMessage.SendData(41, -1, -1, (NetworkText)null, player.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            }
                            else
                            {
                                shouldBeRotation = (float)Math.Atan2((double)num7 * shouldBeDirection, (double)num6 * shouldBeDirection);// - player.fullRotation;
                                //NetMessage.SendData(13, -1, -1, (NetworkText)null, player.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                //NetMessage.SendData(41, -1, -1, (NetworkText)null, player.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0,0);
                            }
                        }

                        if (shoot == 17)
                        {
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            if ((double)player.gravDir == -1.0)
                                vector2_1.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                        }

                        if (shoot == 76)
                        {
                            shoot += Main.rand.Next(3);
                            var num8 = num4 / (float)(Main.screenHeight / 2);
                            if ((double)num8 > 1.0)
                                num8 = 1f;
                            var num9 = num6 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            var num10 = num7 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            var SpeedX = num9 * (num8 + 0.25f);
                            var SpeedY = num10 * (num8 + 0.25f);
                            var number = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot,
                                Damage, num1, i, 0.0f, 0.0f);
                            Main.projectile[number].ai[1] = 1f;
                            var num11 = (float)((double)num8 * 2.0 - 1.0);
                            if ((double)num11 < -1.0)
                                num11 = -1f;
                            if ((double)num11 > 1.0)
                                num11 = 1f;
                            Main.projectile[number].ai[0] = num11;
                            NetMessage.SendData(27, -1, -1, (NetworkText)null, number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                        else if (sItem.type == ItemID.DaedalusStormbow)
                        {
                            var num8 = 3;
                            if (Main.rand.Next(3) == 0)
                                ++num8;
                            for (var index1 = 0; index1 < num8; ++index1)
                            {
                                vector2_1 = new Vector2(
                                    (float)((double)player.position.X + (double)player.width * 0.5 +
                                             (double)(Main.rand.Next(201) * -shouldBeDirection) +
                                             ((double)Main.mouseX + (double)Main.screenPosition.X -
                                              (double)player.position.X)), player.MountedCenter.Y - 600f);
                                vector2_1.X = (float)(((double)vector2_1.X * 10.0 + (double)player.Center.X) / 11.0) +
                                              (float)Main.rand.Next(-100, 101);
                                vector2_1.Y -= (float)(150 * index1);
                                var num9 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                                var num10 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                                if ((double)num10 < 0.0)
                                    num10 *= -1f;
                                if ((double)num10 < 20.0)
                                    num10 = 20f;
                                var num11 =
                                    (float)Math.Sqrt((double)num9 * (double)num9 + (double)num10 * (double)num10);
                                var num12 = speed / num11;
                                var num13 = num9 * num12;
                                var num14 = num10 * num12;
                                var num15 = num13 + (float)Main.rand.Next(-40, 41) * 0.03f;
                                var SpeedY = num14 + (float)Main.rand.Next(-40, 41) * 0.03f;
                                var SpeedX = num15 * ((float)Main.rand.Next(75, 150) * 0.01f);
                                vector2_1.X += (float)Main.rand.Next(-50, 51);
                                var index2 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot,
                                    Damage, num1, i, 0.0f, 0.0f);
                                Main.projectile[index2].noDropItem = true;
                            }
                        }
                        else if (sItem.type == 98 || sItem.type == 533)
                        {
                            var SpeedX = num6 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            var SpeedY = num7 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 1319)
                        {
                            var SpeedX = num6 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            var SpeedY = num7 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            var index = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot,
                                Damage, num1, i, 0.0f, 0.0f);
                            Main.projectile[index].ranged = true;
                            Main.projectile[index].thrown = false;
                        }
                        else if (sItem.type == 3107)
                        {
                            var SpeedX = num6 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            var SpeedY = num7 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 3053)
                        {
                            var vector2_4 = new Vector2(num6, num7);
                            vector2_4.Normalize();
                            var vector2_5 = new Vector2((float)Main.rand.Next(-100, 101),
                                (float)Main.rand.Next(-100, 101));
                            vector2_5.Normalize();
                            var vector2_6 = vector2_4 * 4f + vector2_5;
                            vector2_6.Normalize();
                            vector2_6 *= sItem.shootSpeed;
                            var ai1 = (float)Main.rand.Next(10, 80) * (1f / 1000f);
                            if (Main.rand.Next(2) == 0)
                                ai1 *= -1f;
                            var ai0 = (float)Main.rand.Next(10, 80) * (1f / 1000f);
                            if (Main.rand.Next(2) == 0)
                                ai0 *= -1f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, vector2_6.X, vector2_6.Y, shoot, Damage,
                                num1, i, ai0, ai1);
                        }
                        else if (sItem.type == 3019)
                        {
                            var vector2_4 = new Vector2(num6, num7);
                            var num8 = vector2_4.Length();
                            vector2_4.X += (float)((double)Main.rand.Next(-100, 101) * 0.00999999977648258 *
                                                    (double)num8 * 0.150000005960464);
                            vector2_4.Y += (float)((double)Main.rand.Next(-100, 101) * 0.00999999977648258 *
                                                    (double)num8 * 0.150000005960464);
                            var num9 = num6 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            var num10 = num7 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            vector2_4.Normalize();
                            var vector2_5 = vector2_4 * num8;
                            var vector2_6 = new Vector2(num9 * ((float)Main.rand.Next(50, 150) * 0.01f),
                                num10 * ((float)Main.rand.Next(50, 150) * 0.01f));
                            vector2_6.X += (float)Main.rand.Next(-100, 101) * 0.025f;
                            vector2_6.Y += (float)Main.rand.Next(-100, 101) * 0.025f;
                            vector2_6.Normalize();
                            vector2_6 *= num8;
                            var x = vector2_6.X;
                            var y = vector2_6.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, x, y, shoot, Damage, num1, i,
                                vector2_5.X, vector2_5.Y);
                        }
                        else if (sItem.type == 2797)
                        {
                            var vector2_4 = Vector2.Normalize(new Vector2(num6, num7)) * 40f * sItem.scale;
                            if (Collision.CanHit(vector2_1, 0, 0, vector2_1 + vector2_4, 0, 0))
                                vector2_1 += vector2_4;
                            var rotation = new Vector2(num6, num7).ToRotation();
                            var num8 = 2.094395f;
                            var num9 = Main.rand.Next(4, 5);
                            if (Main.rand.Next(4) == 0)
                                ++num9;
                            for (var index1 = 0; index1 < num9; ++index1)
                            {
                                var num10 = (float)(Main.rand.NextDouble() * 0.200000002980232 + 0.0500000007450581);
                                var vector2_5 =
                                    new Vector2(num6, num7).RotatedBy(
                                        (double)num8 * Main.rand.NextDouble() - (double)num8 / 2.0, new Vector2()) *
                                    num10;
                                var index2 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, vector2_5.X,
                                    vector2_5.Y, 444, Damage, num1, i, rotation, 0.0f);
                                Main.projectile[index2].localAI[0] = (float)shoot;
                                Main.projectile[index2].localAI[1] = speed;
                            }
                        }
                        else if (sItem.type == 2270)
                        {
                            var SpeedX = num6 + (float)Main.rand.Next(-40, 41) * 0.05f;
                            var SpeedY = num7 + (float)Main.rand.Next(-40, 41) * 0.05f;
                            if (Main.rand.Next(3) == 0)
                            {
                                SpeedX *= (float)(1.0 + (double)Main.rand.Next(-30, 31) * 0.0199999995529652);
                                SpeedY *= (float)(1.0 + (double)Main.rand.Next(-30, 31) * 0.0199999995529652);
                            }

                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 1930)
                        {
                            var num8 = 2 + Main.rand.Next(3);
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var num11 = 0.025f * (float)index;
                                var num12 = num9 + (float)Main.rand.Next(-35, 36) * num11;
                                var num13 = num10 + (float)Main.rand.Next(-35, 36) * num11;
                                var num14 =
                                    (float)Math.Sqrt((double)num12 * (double)num12 +
                                                      (double)num13 * (double)num13);
                                var num15 = speed / num14;
                                var SpeedX = num12 * num15;
                                var SpeedY = num13 * num15;
                                Projectile.NewProjectile(
                                    vector2_1.X + (float)((double)num6 * (double)(num8 - index) * 1.75),
                                    vector2_1.Y + (float)((double)num7 * (double)(num8 - index) * 1.75), SpeedX,
                                    SpeedY, shoot, Damage, num1, i, (float)Main.rand.Next(0, 10 * (index + 1)), 0.0f);
                            }
                        }
                        else if (sItem.type == 1931)
                        {
                            var num8 = 2;
                            for (var index = 0; index < num8; ++index)
                            {
                                vector2_1 = new Vector2(
                                    (float)((double)player.position.X + (double)player.width * 0.5 +
                                             (double)(Main.rand.Next(201) * -shouldBeDirection) +
                                             ((double)Main.mouseX + (double)Main.screenPosition.X -
                                              (double)player.position.X)), player.MountedCenter.Y - 600f);
                                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) +
                                              (float)Main.rand.Next(-200, 201);
                                vector2_1.Y -= (float)(100 * index);
                                var num9 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                                var num10 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                                if ((double)num10 < 0.0)
                                    num10 *= -1f;
                                if ((double)num10 < 20.0)
                                    num10 = 20f;
                                var num11 =
                                    (float)Math.Sqrt((double)num9 * (double)num9 + (double)num10 * (double)num10);
                                var num12 = speed / num11;
                                var num13 = num9 * num12;
                                var num14 = num10 * num12;
                                var SpeedX = num13 + (float)Main.rand.Next(-40, 41) * 0.02f;
                                var SpeedY = num14 + (float)Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, (float)Main.rand.Next(5));
                            }
                        }
                        else if (sItem.type == 2750)
                        {
                            var num8 = 1;
                            for (var index = 0; index < num8; ++index)
                            {
                                vector2_1 = new Vector2(
                                    (float)((double)player.position.X + (double)player.width * 0.5 +
                                             (double)(Main.rand.Next(201) * -shouldBeDirection) +
                                             ((double)Main.mouseX + (double)Main.screenPosition.X -
                                              (double)player.position.X)), player.MountedCenter.Y - 600f);
                                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) +
                                              (float)Main.rand.Next(-200, 201);
                                vector2_1.Y -= (float)(100 * index);
                                var num9 = (float)((double)Main.mouseX + (double)Main.screenPosition.X -
                                                      (double)vector2_1.X + (double)Main.rand.Next(-40, 41) *
                                                      0.0299999993294477);
                                var num10 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                                if ((double)num10 < 0.0)
                                    num10 *= -1f;
                                if ((double)num10 < 20.0)
                                    num10 = 20f;
                                var num11 =
                                    (float)Math.Sqrt((double)num9 * (double)num9 + (double)num10 * (double)num10);
                                var num12 = speed / num11;
                                var num13 = num9 * num12;
                                var num14 = num10 * num12;
                                var num15 = num13;
                                var num16 = num14 + (float)Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num15 * 0.75f, num16 * 0.75f,
                                    shoot + Main.rand.Next(3), Damage, num1, i, 0.0f,
                                    (float)(0.5 + Main.rand.NextDouble() * 0.300000011920929));
                            }
                        }
                        else if (sItem.type == 3570)
                        {
                            var num8 = 3;
                            for (var index = 0; index < num8; ++index)
                            {
                                vector2_1 = new Vector2(
                                    (float)((double)player.position.X + (double)player.width * 0.5 +
                                             (double)(Main.rand.Next(201) * -shouldBeDirection) +
                                             ((double)Main.mouseX + (double)Main.screenPosition.X -
                                              (double)player.position.X)), player.MountedCenter.Y - 600f);
                                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) +
                                              (float)Main.rand.Next(-200, 201);
                                vector2_1.Y -= (float)(100 * index);
                                var num9 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                                var num10 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                                var ai1 = num10 + vector2_1.Y;
                                if ((double)num10 < 0.0)
                                    num10 *= -1f;
                                if ((double)num10 < 20.0)
                                    num10 = 20f;
                                var num11 =
                                    (float)Math.Sqrt((double)num9 * (double)num9 + (double)num10 * (double)num10);
                                var num12 = speed / num11;
                                var vector2_4 = new Vector2(num9 * num12, num10 * num12) / 2f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, vector2_4.X, vector2_4.Y, shoot,
                                    Damage, num1, i, 0.0f, ai1);
                            }
                        }
                        else if (sItem.type == 3065)
                        {
                            var vector2_4 = Main.screenPosition +
                                                new Vector2((float)Main.mouseX, (float)Main.mouseY);
                            var ai1 = vector2_4.Y;
                            if ((double)ai1 > (double)player.Center.Y - 200.0)
                                ai1 = player.Center.Y - 200f;
                            for (var index = 0; index < 3; ++index)
                            {
                                vector2_1 = player.Center +
                                            new Vector2((float)(-Main.rand.Next(0, 401) * shouldBeDirection), -600f);
                                vector2_1.Y -= (float)(100 * index);
                                var vector2_5 = vector2_4 - vector2_1;
                                if ((double)vector2_5.Y < 0.0)
                                    vector2_5.Y *= -1f;
                                if ((double)vector2_5.Y < 20.0)
                                    vector2_5.Y = 20f;
                                vector2_5.Normalize();
                                vector2_5 *= speed;
                                var x = vector2_5.X;
                                var y = vector2_5.Y;
                                var SpeedX = x;
                                var SpeedY = y + (float)Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage * 2,
                                    num1, i, 0.0f, ai1);
                            }
                        }
                        else if (sItem.type == 2624)
                        {
                            var num8 = 0.3141593f;
                            var num9 = 5;
                            var spinningpoint = new Vector2(num6, num7);
                            spinningpoint.Normalize();
                            spinningpoint *= 40f;
                            var flag4 = Collision.CanHit(vector2_1, 0, 0, vector2_1 + spinningpoint, 0, 0);
                            for (var index1 = 0; index1 < num9; ++index1)
                            {
                                var num10 = (float)index1 - (float)(((double)num9 - 1.0) / 2.0);
                                var vector2_4 =
                                    spinningpoint.RotatedBy((double)num8 * (double)num10, new Vector2());
                                if (!flag4)
                                    vector2_4 -= spinningpoint;
                                var index2 = Projectile.NewProjectile(vector2_1.X + vector2_4.X,
                                    vector2_1.Y + vector2_4.Y, num6, num7, shoot, Damage, num1, i, 0.0f, 0.0f);
                                Main.projectile[index2].noDropItem = true;
                            }
                        }
                        else if (sItem.type == 1929)
                        {
                            var SpeedX = num6 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            var SpeedY = num7 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 1553)
                        {
                            var SpeedX = num6 + (float)Main.rand.Next(-40, 41) * 0.005f;
                            var SpeedY = num7 + (float)Main.rand.Next(-40, 41) * 0.005f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 518)
                        {
                            var num8 = num6;
                            var num9 = num7;
                            var SpeedX = num8 + (float)Main.rand.Next(-40, 41) * 0.04f;
                            var SpeedY = num9 + (float)Main.rand.Next(-40, 41) * 0.04f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 1265)
                        {
                            var num8 = num6;
                            var num9 = num7;
                            var SpeedX = num8 + (float)Main.rand.Next(-30, 31) * 0.03f;
                            var SpeedY = num9 + (float)Main.rand.Next(-30, 31) * 0.03f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 534)
                        {
                            var num8 = Main.rand.Next(4, 6);
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var SpeedX = num9 + (float)Main.rand.Next(-40, 41) * 0.05f;
                                var SpeedY = num10 + (float)Main.rand.Next(-40, 41) * 0.05f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 2188)
                        {
                            var num8 = 4;
                            if (Main.rand.Next(3) == 0)
                                ++num8;
                            if (Main.rand.Next(4) == 0)
                                ++num8;
                            if (Main.rand.Next(5) == 0)
                                ++num8;
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var num11 = 0.05f * (float)index;
                                var num12 = num9 + (float)Main.rand.Next(-35, 36) * num11;
                                var num13 = num10 + (float)Main.rand.Next(-35, 36) * num11;
                                var num14 =
                                    (float)Math.Sqrt((double)num12 * (double)num12 +
                                                      (double)num13 * (double)num13);
                                var num15 = speed / num14;
                                var SpeedX = num12 * num15;
                                var SpeedY = num13 * num15;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 1308)
                        {
                            var num8 = 3;
                            if (Main.rand.Next(3) == 0)
                                ++num8;
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var num11 = 0.05f * (float)index;
                                var num12 = num9 + (float)Main.rand.Next(-35, 36) * num11;
                                var num13 = num10 + (float)Main.rand.Next(-35, 36) * num11;
                                var num14 =
                                    (float)Math.Sqrt((double)num12 * (double)num12 +
                                                      (double)num13 * (double)num13);
                                var num15 = speed / num14;
                                var SpeedX = num12 * num15;
                                var SpeedY = num13 * num15;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 1258)
                        {
                            var num8 = num6;
                            var num9 = num7;
                            var SpeedX = num8 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            var SpeedY = num9 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            vector2_1.X += (float)Main.rand.Next(-40, 41) * 0.05f;
                            vector2_1.Y += (float)Main.rand.Next(-45, 36) * 0.05f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 964)
                        {
                            var num8 = Main.rand.Next(3, 5);
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var SpeedX = num9 + (float)Main.rand.Next(-35, 36) * 0.04f;
                                var SpeedY = num10 + (float)Main.rand.Next(-35, 36) * 0.04f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 1569)
                        {
                            var num8 = 4;
                            if (Main.rand.Next(2) == 0)
                                ++num8;
                            if (Main.rand.Next(4) == 0)
                                ++num8;
                            if (Main.rand.Next(8) == 0)
                                ++num8;
                            if (Main.rand.Next(16) == 0)
                                ++num8;
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var num11 = 0.05f * (float)index;
                                var num12 = num9 + (float)Main.rand.Next(-35, 36) * num11;
                                var num13 = num10 + (float)Main.rand.Next(-35, 36) * num11;
                                var num14 =
                                    (float)Math.Sqrt((double)num12 * (double)num12 +
                                                      (double)num13 * (double)num13);
                                var num15 = speed / num14;
                                var SpeedX = num12 * num15;
                                var SpeedY = num13 * num15;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 1572 || sItem.type == 2366 || (sItem.type == 3571 || sItem.type == 3569))
                        {
                            var flag4 = sItem.type == 3571 || sItem.type == 3569;
                            var i1 = (int)((double)Main.mouseX + (double)Main.screenPosition.X) / 16;
                            var j = (int)((double)Main.mouseY + (double)Main.screenPosition.Y) / 16;
                            if ((double)player.gravDir == -1.0)
                                j = (int)((double)Main.screenPosition.Y + (double)Main.screenHeight -
                                           (double)Main.mouseY) / 16;
                            if (!flag4)
                            {
                                while (j < Main.maxTilesY - 10 && Main.tile[i1, j] != null &&
                                       (!WorldGen.SolidTile2(i1, j) && Main.tile[i1 - 1, j] != null) &&
                                       (!WorldGen.SolidTile2(i1 - 1, j) && Main.tile[i1 + 1, j] != null &&
                                        !WorldGen.SolidTile2(i1 + 1, j)))
                                    ++j;
                                --j;
                            }

                            Projectile.NewProjectile((float)Main.mouseX + Main.screenPosition.X, (float)(j * 16 - 24),
                                0.0f, 15f, shoot, Damage, num1, i, 0.0f, 0.0f);
                            player.UpdateMaxTurrets();
                        }
                        else if (sItem.type == 1244 || sItem.type == 1256)
                        {
                            var index = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, shoot, Damage,
                                num1, i, 0.0f, 0.0f);
                            Main.projectile[index].ai[0] = (float)Main.mouseX + Main.screenPosition.X;
                            Main.projectile[index].ai[1] = (float)Main.mouseY + Main.screenPosition.Y;
                        }
                        else if (sItem.type == 1229)
                        {
                            var num8 = Main.rand.Next(2, 4);
                            if (Main.rand.Next(5) == 0)
                                ++num8;
                            for (var index1 = 0; index1 < num8; ++index1)
                            {
                                var SpeedX = num6;
                                var SpeedY = num7;
                                if (index1 > 0)
                                {
                                    SpeedX += (float)Main.rand.Next(-35, 36) * 0.04f;
                                    SpeedY += (float)Main.rand.Next(-35, 36) * 0.04f;
                                }

                                if (index1 > 1)
                                {
                                    SpeedX += (float)Main.rand.Next(-35, 36) * 0.04f;
                                    SpeedY += (float)Main.rand.Next(-35, 36) * 0.04f;
                                }

                                if (index1 > 2)
                                {
                                    SpeedX += (float)Main.rand.Next(-35, 36) * 0.04f;
                                    SpeedY += (float)Main.rand.Next(-35, 36) * 0.04f;
                                }

                                var index2 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot,
                                    Damage, num1, i, 0.0f, 0.0f);
                                Main.projectile[index2].noDropItem = true;
                            }
                        }
                        else if (sItem.type == 1121)
                        {
                            var num8 = Main.rand.Next(1, 4);
                            if (Main.rand.Next(6) == 0)
                                ++num8;
                            if (Main.rand.Next(6) == 0)
                                ++num8;
                            if (player.strongBees && Main.rand.Next(3) == 0)
                                ++num8;
                            for (var index1 = 0; index1 < num8; ++index1)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var SpeedX = num9 + (float)Main.rand.Next(-35, 36) * 0.02f;
                                var SpeedY = num10 + (float)Main.rand.Next(-35, 36) * 0.02f;
                                var index2 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY,
                                    player.beeType(), player.beeDamage(Damage), player.beeKB(num1), i, 0.0f, 0.0f);
                                Main.projectile[index2].magic = true;
                            }
                        }
                        else if (sItem.type == 1155)
                        {
                            var num8 = Main.rand.Next(2, 5);
                            if (Main.rand.Next(5) == 0)
                                ++num8;
                            if (Main.rand.Next(5) == 0)
                                ++num8;
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var SpeedX = num9 + (float)Main.rand.Next(-35, 36) * 0.02f;
                                var SpeedY = num10 + (float)Main.rand.Next(-35, 36) * 0.02f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 1801)
                        {
                            var num8 = Main.rand.Next(1, 4);
                            for (var index = 0; index < num8; ++index)
                            {
                                var num9 = num6;
                                var num10 = num7;
                                var SpeedX = num9 + (float)Main.rand.Next(-35, 36) * 0.05f;
                                var SpeedY = num10 + (float)Main.rand.Next(-35, 36) * 0.05f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 679)
                        {
                            for (var index = 0; index < 6; ++index)
                            {
                                var num8 = num6;
                                var num9 = num7;
                                var SpeedX = num8 + (float)Main.rand.Next(-40, 41) * 0.05f;
                                var SpeedY = num9 + (float)Main.rand.Next(-40, 41) * 0.05f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 2623)
                        {
                            for (var index = 0; index < 3; ++index)
                            {
                                var num8 = num6;
                                var num9 = num7;
                                var SpeedX = num8 + (float)Main.rand.Next(-40, 41) * 0.1f;
                                var SpeedY = num9 + (float)Main.rand.Next(-40, 41) * 0.1f;
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1,
                                    i, 0.0f, 0.0f);
                            }
                        }
                        else if (sItem.type == 3210)
                        {
                            var vector2_4 = new Vector2(num6, num7);
                            vector2_4.X += (float)Main.rand.Next(-30, 31) * 0.04f;
                            vector2_4.Y += (float)Main.rand.Next(-30, 31) * 0.03f;
                            vector2_4.Normalize();
                            vector2_4 *= (float)Main.rand.Next(70, 91) * 0.1f;
                            vector2_4.X += (float)Main.rand.Next(-30, 31) * 0.04f;
                            vector2_4.Y += (float)Main.rand.Next(-30, 31) * 0.03f;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, vector2_4.X, vector2_4.Y, shoot, Damage,
                                num1, i, (float)Main.rand.Next(20), 0.0f);
                        }
                        else if (sItem.type == ItemID.ClockworkAssaultRifle)
                        {
                            var SpeedX = num6;
                            var SpeedY = num7;
                            if (shouldBeAnimation < 5)
                            {
                                var num8 = SpeedX + (float)Main.rand.Next(-40, 41) * 0.01f;
                                var num9 = SpeedY + (float)Main.rand.Next(-40, 41) * 0.01f;
                                SpeedX = num8 * 1.1f;
                                SpeedY = num9 * 1.1f;
                            }
                            else if (shouldBeAnimation < 10)
                            {
                                var num8 = SpeedX + (float)Main.rand.Next(-20, 21) * 0.01f;
                                var num9 = SpeedY + (float)Main.rand.Next(-20, 21) * 0.01f;
                                SpeedX = num8 * 1.05f;
                                SpeedY = num9 * 1.05f;
                            }
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 1157)
                        {
                            shoot = Main.rand.Next(191, 195);
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            var index = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot,
                                Damage, num1, i, 0.0f, 0.0f);
                            Main.projectile[index].localAI[0] = 30f;
                        }
                        else if (sItem.type == 1802)
                        {
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 2364 || sItem.type == 2365)
                        {
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 2535)
                        {
                            var x = 0.0f;
                            var y = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            var spinningpoint = new Vector2(x, y).RotatedBy(1.57079637050629, new Vector2());
                            Projectile.NewProjectile(vector2_1.X + spinningpoint.X, vector2_1.Y + spinningpoint.Y,
                                spinningpoint.X, spinningpoint.Y, shoot, Damage, num1, i, 0.0f, 0.0f);
                            spinningpoint = spinningpoint.RotatedBy(-3.14159274101257, new Vector2());
                            Projectile.NewProjectile(vector2_1.X + spinningpoint.X, vector2_1.Y + spinningpoint.Y,
                                spinningpoint.X, spinningpoint.Y, shoot + 1, Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 2551)
                        {
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY,
                                shoot + Main.rand.Next(3), Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 2584)
                        {
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY,
                                shoot + Main.rand.Next(3), Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 2621)
                        {
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 2749 || sItem.type == 3249 || sItem.type == 3474)
                        {
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.type == 3531)
                        {
                            var num8 = -1;
                            var index1 = -1;
                            for (var index2 = 0; index2 < 1000; ++index2)
                            {
                                if (Main.projectile[index2].active && Main.projectile[index2].owner == Main.myPlayer)
                                {
                                    if (num8 == -1 && Main.projectile[index2].type == 625)
                                        num8 = index2;
                                    if (index1 == -1 && Main.projectile[index2].type == 628)
                                        index1 = index2;
                                    if (num8 != -1 && index1 != -1)
                                        break;
                                }
                            }

                            if (num8 == -1 && index1 == -1)
                            {
                                var SpeedX = 0.0f;
                                var SpeedY = 0.0f;
                                vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                                vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                                var num9 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot,
                                    Damage, num1, i, 0.0f, 0.0f);
                                var num10 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY,
                                    shoot + 1, Damage, num1, i, (float)num9, 0.0f);
                                var index2 = num10;
                                var num11 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY,
                                    shoot + 2, Damage, num1, i, (float)num10, 0.0f);
                                Main.projectile[index2].localAI[1] = (float)num11;
                                var index3 = num11;
                                var num12 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY,
                                    shoot + 3, Damage, num1, i, (float)num11, 0.0f);
                                Main.projectile[index3].localAI[1] = (float)num12;
                            }
                            else if (num8 != -1 && index1 != -1)
                            {
                                var num9 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, shoot + 1,
                                    Damage, num1, i,
                                    (float)Projectile.GetByUUID(Main.myPlayer, Main.projectile[index1].ai[0]), 0.0f);
                                var index2 = num9;
                                var index3 = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, shoot + 2,
                                    Damage, num1, i, (float)num9, 0.0f);
                                Main.projectile[index2].localAI[1] = (float)index3;
                                Main.projectile[index2].netUpdate = true;
                                Main.projectile[index2].ai[1] = 1f;
                                Main.projectile[index3].localAI[1] = (float)index1;
                                Main.projectile[index3].netUpdate = true;
                                Main.projectile[index3].ai[1] = 1f;
                                Main.projectile[index1].ai[0] = (float)Main.projectile[index3].projUUID;
                                Main.projectile[index1].netUpdate = true;
                                Main.projectile[index1].ai[1] = 1f;
                            }
                        }
                        else if (sItem.type == 1309)
                        {
                            var SpeedX = 0.0f;
                            var SpeedY = 0.0f;
                            vector2_1.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_1.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, shoot, Damage, num1, i,
                                0.0f, 0.0f);
                        }
                        else if (sItem.shoot > 0 &&
                                 (Main.projPet[sItem.shoot] || sItem.shoot == 72 ||
                                  (sItem.shoot == 18 || sItem.shoot == 500) || sItem.shoot == 650) && !sItem.summon)
                        {
                            for (var index = 0; index < 1000; ++index)
                            {
                                if (Main.projectile[index].active && Main.projectile[index].owner == player.whoAmI)
                                {
                                    if (sItem.shoot == 72)
                                    {
                                        if (Main.projectile[index].type == 72 || Main.projectile[index].type == 86 ||
                                            Main.projectile[index].type == 87)
                                            Main.projectile[index].Kill();
                                    }
                                    else if (sItem.shoot == Main.projectile[index].type)
                                        Main.projectile[index].Kill();
                                }
                            }

                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, shoot, Damage, num1, i, 0.0f,
                                0.0f);
                        }
                        else if (sItem.type == 3006)
                        {
                            Vector2 vector2_4;
                            vector2_4.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_4.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            while (Collision.CanHitLine(player.position, player.width, player.height, vector2_1, 1, 1))
                            {
                                vector2_1.X += num6;
                                vector2_1.Y += num7;
                                if ((double)(vector2_1 - vector2_4).Length() <
                                    20.0 + (double)Math.Abs(num6) + (double)Math.Abs(num7))
                                {
                                    vector2_1 = vector2_4;
                                    break;
                                }
                            }

                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, 0.0f, 0.0f, shoot, Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 3014)
                        {
                            Vector2 vector2_4;
                            vector2_4.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2_4.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            while (Collision.CanHitLine(player.position, player.width, player.height, vector2_1, 1, 1))
                            {
                                vector2_1.X += num6;
                                vector2_1.Y += num7;
                                if ((double)(vector2_1 - vector2_4).Length() <
                                    20.0 + (double)Math.Abs(num6) + (double)Math.Abs(num7))
                                {
                                    vector2_1 = vector2_4;
                                    break;
                                }
                            }

                            var flag4 = false;
                            var j1 = (int)vector2_1.Y / 16;
                            var i1 = (int)vector2_1.X / 16;
                            var num8 = j1;
                            while (j1 < Main.maxTilesY - 10 && j1 - num8 < 30 &&
                                   (!WorldGen.SolidTile(i1, j1) &&
                                    !TileID.Sets.Platforms[(int)Main.tile[i1, j1].type]))
                                ++j1;
                            if (!WorldGen.SolidTile(i1, j1) && !TileID.Sets.Platforms[(int)Main.tile[i1, j1].type])
                                flag4 = true;
                            var num9 = (float)(j1 * 16);
                            var j2 = num8;
                            while (j2 > 10 && num8 - j2 < 30 && !WorldGen.SolidTile(i1, j2))
                                --j2;
                            var num10 = (float)(j2 * 16 + 16);
                            var ai1 = num9 - num10;
                            var num11 = 10;
                            if ((double)ai1 > (double)(16 * num11))
                                ai1 = (float)(16 * num11);
                            var ai0 = num9 - ai1;
                            vector2_1.X = (float)((int)((double)vector2_1.X / 16.0) * 16);
                            if (!flag4)
                                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, 0.0f, 0.0f, shoot, Damage, num1, i,
                                    ai0, ai1);
                        }
                        else if (sItem.type == 3473)
                        {
                            var ai1 = (float)(((double)Main.rand.NextFloat() - 0.5) * 0.785398185253143);
                            var vector2_4 = new Vector2(num6, num7);
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, vector2_4.X, vector2_4.Y, shoot, Damage,
                                num1, i, 0.0f, ai1);
                        }
                        else if (sItem.type == 3836)
                        {
                            var ai0 = (float)((double)Main.rand.NextFloat() * (double)speed * 0.75) *
                                        (float)shouldBeDirection;
                            var velocity = new Vector2(num6, num7);
                            Projectile.NewProjectile(vector2_1, velocity, shoot, Damage, num1, i, ai0, 0.0f);
                        }
                        else if (sItem.type == 3858)
                        {
                            var flag4 = player.altFunctionUse == 2;
                            var velocity = new Vector2(num6, num7);
                            if (flag4)
                            {
                                velocity *= 1.5f;
                                var ai0 = (float)((0.300000011920929 +
                                                      0.699999988079071 * (double)Main.rand.NextFloat()) *
                                                     (double)speed * 1.75) * (float)shouldBeDirection;
                                Projectile.NewProjectile(vector2_1, velocity, 708, (int)((double)Damage * 0.75),
                                    num1 + 4f, i, ai0, 0.0f);
                            }
                            else
                                Projectile.NewProjectile(vector2_1, velocity, shoot, Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 3859)
                        {
                            var vector2_4 = new Vector2(num6, num7);
                            shoot = 710;
                            Damage = (int)((double)Damage * 0.699999988079071);
                            var v2 = vector2_4 * 0.8f;
                            var vector2_5 = v2.SafeNormalize(-Vector2.UnitY);
                            var num8 = (float)Math.PI / 180f * (float)-shouldBeDirection;
                            for (var num9 = -2.5f; (double)num9 < 3.0; ++num9)
                                Projectile.NewProjectile(vector2_1,
                                    (v2 + vector2_5 * num9 * 0.5f).RotatedBy((double)num9 * (double)num8,
                                        new Vector2()), shoot, Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 3870)
                        {
                            var vector2_4 = Vector2.Normalize(new Vector2(num6, num7)) * 40f * sItem.scale;
                            if (Collision.CanHit(vector2_1, 0, 0, vector2_1 + vector2_4, 0, 0))
                                vector2_1 += vector2_4;
                            var v2 = new Vector2(num6, num7) * 0.8f;
                            var vector2_5 = v2.SafeNormalize(-Vector2.UnitY);
                            var num8 = (float)Math.PI / 180f * (float)-shouldBeDirection;
                            for (var index = 0; index <= 2; ++index)
                                Projectile.NewProjectile(vector2_1,
                                    (v2 + vector2_5 * (float)index * 1f).RotatedBy((double)index * (double)num8,
                                        new Vector2()), shoot, Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 3542)
                        {
                            var num8 = (float)(((double)Main.rand.NextFloat() - 0.5) * 0.785398185253143 *
                                                  0.699999988079071);
                            for (var index = 0;
                                index < 10 && !Collision.CanHit(vector2_1, 0, 0,
                                    vector2_1 + new Vector2(num6, num7).RotatedBy((double)num8, new Vector2()) * 100f,
                                    0, 0);
                                ++index)
                                num8 = (float)(((double)Main.rand.NextFloat() - 0.5) * 0.785398185253143 *
                                                0.699999988079071);
                            var vector2_4 = new Vector2(num6, num7).RotatedBy((double)num8, new Vector2()) *
                                                (float)(0.949999988079071 +
                                                         (double)Main.rand.NextFloat() * 0.300000011920929);
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, vector2_4.X, vector2_4.Y, shoot, Damage,
                                num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 3779)
                        {
                            var num8 = Main.rand.NextFloat() * 6.283185f;
                            for (var index = 0;
                                index < 10 && !Collision.CanHit(vector2_1, 0, 0,
                                    vector2_1 + new Vector2(num6, num7).RotatedBy((double)num8, new Vector2()) * 100f,
                                    0, 0);
                                ++index)
                                num8 = Main.rand.NextFloat() * 6.283185f;
                            var vector2_4 = new Vector2(num6, num7).RotatedBy((double)num8, new Vector2()) *
                                                (float)(0.949999988079071 +
                                                         (double)Main.rand.NextFloat() * 0.300000011920929);
                            Projectile.NewProjectile(vector2_1 + vector2_4 * 30f, Vector2.Zero, shoot, Damage, num1, i,
                                -2f, 0.0f);
                        }
                        else if (sItem.type == 3787)
                        {
                            var f3 = Main.rand.NextFloat() * 6.283185f;
                            var num8 = 20f;
                            var num9 = 60f;
                            var position = vector2_1 + f3.ToRotationVector2() *
                                               MathHelper.Lerp(num8, num9, Main.rand.NextFloat());
                            for (var index = 0; index < 50; ++index)
                            {
                                position = vector2_1 + f3.ToRotationVector2() *
                                           MathHelper.Lerp(num8, num9, Main.rand.NextFloat());
                                if (!Collision.CanHit(vector2_1, 0, 0,
                                    position + (position - vector2_1).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                                    f3 = Main.rand.NextFloat() * 6.283185f;
                                else
                                    break;
                            }

                            var v2 = Main.MouseWorld - position;
                            var defaultValue = new Vector2(num6, num7).SafeNormalize(Vector2.UnitY) * speed;
                            var velocity = Vector2.Lerp(v2.SafeNormalize(defaultValue) * speed, defaultValue,
                                0.25f);
                            Projectile.NewProjectile(position, velocity, shoot, Damage, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == ItemID.OnyxBlaster)
                        {
                            var v2 = new Vector2(num6, num7);
                            var num8 = 0.7853982f;
                            for (var index = 0; index < 2; ++index)
                            {
                                Projectile.NewProjectile(vector2_1, v2 + v2.SafeNormalize(Vector2.Zero).RotatedBy((double)num8 * ((double)Main.rand.NextFloat() * 0.5 + 0.5), new Vector2()) * Main.rand.NextFloatDirection() * 2f, shoot, Damage, num1, i, 0.0f, 0.0f);
                                Projectile.NewProjectile(vector2_1, v2 + v2.SafeNormalize(Vector2.Zero).RotatedBy(-(double)num8 * ((double)Main.rand.NextFloat() * 0.5 + 0.5), new Vector2()) * Main.rand.NextFloatDirection() * 2f, shoot, Damage, num1, i, 0.0f, 0.0f);
                            }
                            Projectile.NewProjectile(vector2_1,
                                v2.SafeNormalize(Vector2.UnitX * (float)shouldBeDirection) * (speed * 1.3f), 661,
                                Damage * 2, num1, i, 0.0f, 0.0f);
                        }
                        else if (sItem.type == 3475)
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, 615, Damage, num1, i,
                                (float)(5 * Main.rand.Next(0, 20)), 0.0f);
                        else if (sItem.type == 3540)
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, 630, Damage, num1, i, 0.0f,
                                0.0f);
                        else if (sItem.type == 3854)
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, 705, Damage, num1, i, 0.0f,
                                0.0f);
                        else if (sItem.type == 3546)
                        {
                            for (var index = 0; index < 2; ++index)
                            {
                                var num8 = num6;
                                var num9 = num7;
                                var num10 = num8 + (float)Main.rand.Next(-40, 41) * 0.05f;
                                var num11 = num9 + (float)Main.rand.Next(-40, 41) * 0.05f;
                                var vector2_4 =
                                    vector2_1 + Vector2.Normalize(
                                        new Vector2(num10, num11).RotatedBy(-1.57079637050629 * (double)shouldBeDirection,
                                            new Vector2())) * 6f;
                                Projectile.NewProjectile(vector2_4.X, vector2_4.Y, num10, num11,
                                    167 + Main.rand.Next(4), Damage, num1, i, 0.0f, 1f);
                            }
                        }
                        else if (sItem.type == 3350)
                        {
                            var num8 = num6;
                            var num9 = num7;
                            var num10 = num8 + (float)Main.rand.Next(-1, 2) * 0.5f;
                            var num11 = num9 + (float)Main.rand.Next(-1, 2) * 0.5f;
                            if (Collision.CanHitLine(player.Center, 0, 0, vector2_1 + new Vector2(num10, num11) * 2f, 0,
                                0))
                                vector2_1 += new Vector2(num10, num11);
                            Projectile.NewProjectile(vector2_1.X, vector2_1.Y - player.gravDir * 4f, num10, num11, shoot,
                                Damage, num1, i, 0.0f, (float)Main.rand.Next(12) / 6f);
                        }
                        else if(!modded)
                        {
                               var index = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, shoot, Damage, num1, i, 0.0f, 0.0f);
                            if (sItem.type == 726)
                                Main.projectile[index].magic = true;
                            if (sItem.type == 724 || sItem.type == 676)
                                Main.projectile[index].melee = true;
                            if (shoot == 80)
                            {
                                Main.projectile[index].ai[0] = (float)Player.tileTargetX;
                                Main.projectile[index].ai[1] = (float)Player.tileTargetY;
                            }

                            if (shoot == 442)
                            {
                                Main.projectile[index].ai[0] = (float)Player.tileTargetX;
                                Main.projectile[index].ai[1] = (float)Player.tileTargetY;
                            }

                            if ((player.thrownCost50 || player.thrownCost33) && player.inventory[player.selectedItem].thrown)
                                Main.projectile[index].noDropItem = true;
                        }
                    }
                    else if (sItem.useStyle == 5)
                    {
                        shouldBeRotation = 0.0f;
                    }
                }
            }
            player.position = temp;
        }
        public Vector2 UseVanillaItemAnimation(Vector2 Center, Item sItem, int itemAnimation, int maxAnimation, ref int direction, ref float itemRotation)
        {
            Vector2 itemLocation = Center;
            if (sItem.useStyle == 1)
            {

                itemLocation = Center -= new Vector2(0, 22);
                if(itemAnimation == maxAnimation)
                { 
                    var vector2_1 = player.RotatedRelativePoint(Center, true);
                    var vector2_2 = Vector2.UnitX.RotatedBy(player.fullRotation, new Vector2());
                    var v1 = Main.MouseWorld - vector2_1;
                    var vector2_3 = itemRotation.ToRotationVector2() * direction;
                    if (sItem.type == ItemID.BookStaff)// && player.itemAnimation != player.maxAnimation - 1)
                        v1 = vector2_3;
                    if (v1 != Vector2.Zero)
                        v1.Normalize();
                    var speedX = Vector2.Dot(vector2_2, v1);
                    if (speedX > 0.0)
                        direction = 1;
                    else
                        direction = -1;
                }
                if (sItem.type > -1 && Item.claw[sItem.type])
                {
                    if ((double)itemAnimation < (double)maxAnimation * 0.333)
                    {
                        var num = 10f;
                        itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - num) * direction);
                        itemLocation.Y = Center.Y + 26f;
                    }
                    else if ((double)itemAnimation < (double)maxAnimation * 0.666)
                    {
                        var num = 8f;
                        itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - num) * direction);
                        itemLocation.Y = Center.Y + 24f;
                    }
                    else
                    {
                        var num = 6f;
                        itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - num) * direction);
                        itemLocation.Y = Center.Y + 20f;
                    }

                    itemRotation = (float)(((double)itemAnimation / (double)maxAnimation - 0.5) * (double)-direction * 3.5 - direction * 0.300000011920929);
                }
                else
                {
                    if ((double)itemAnimation < (double)maxAnimation * 0.333)
                    {
                        var num = 10f;
                        if (Main.itemTexture[sItem.type].Width > 32)
                            num = 14f;
                        if (Main.itemTexture[sItem.type].Width >= 52)
                            num = 24f;
                        if (Main.itemTexture[sItem.type].Width >= 64)
                            num = 28f;
                        if (Main.itemTexture[sItem.type].Width >= 92)
                            num = 38f;
                        if (sItem.type == 2330 || sItem.type == 2320 || sItem.type == 2341)
                            num += 8f;
                        itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - num) * direction);
                        itemLocation.Y = Center.Y + 24f;
                    }
                    else if ((double)itemAnimation < (double)maxAnimation * 0.666)
                    {
                        var num1 = 10f;
                        if (Main.itemTexture[sItem.type].Width > 32)
                            num1 = 18f;
                        if (Main.itemTexture[sItem.type].Width >= 52)
                            num1 = 24f;
                        if (Main.itemTexture[sItem.type].Width >= 64)
                            num1 = 28f;
                        if (Main.itemTexture[sItem.type].Width >= 92)
                            num1 = 38f;
                        if (sItem.type == 2330 || sItem.type == 2320 || sItem.type == 2341)
                            num1 += 4f;
                        itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - num1) * direction);
                        var num2 = 10f;
                        if (Main.itemTexture[sItem.type].Height > 32)
                            num2 = 8f;
                        if (Main.itemTexture[sItem.type].Height > 52)
                            num2 = 12f;
                        if (Main.itemTexture[sItem.type].Height > 64)
                            num2 = 14f;
                        if (sItem.type == 2330 || sItem.type == 2320 || sItem.type == 2341)
                            num2 += 4f;
                        itemLocation.Y = Center.Y + num2;
                    }
                    else
                    {
                        var num1 = 6f;
                        if (Main.itemTexture[sItem.type].Width > 32)
                            num1 = 14f;
                        if (Main.itemTexture[sItem.type].Width >= 48)
                            num1 = 18f;
                        if (Main.itemTexture[sItem.type].Width >= 52)
                            num1 = 24f;
                        if (Main.itemTexture[sItem.type].Width >= 64)
                            num1 = 28f;
                        if (Main.itemTexture[sItem.type].Width >= 92)
                            num1 = 38f;
                        if (sItem.type == 2330 || sItem.type == 2320 || sItem.type == 2341)
                            num1 += 4f;
                        itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - num1) * direction);
                        var num2 = 10f;
                        if (Main.itemTexture[sItem.type].Height > 32)
                            num2 = 10f;
                        if (Main.itemTexture[sItem.type].Height > 52)
                            num2 = 12f;
                        if (Main.itemTexture[sItem.type].Height > 64)
                            num2 = 14f;
                        if (sItem.type == 2330 || sItem.type == 2320 || sItem.type == 2341)
                            num2 += 4f;
                        itemLocation.Y = Center.Y + num2;
                    }

                    itemRotation =
                        (float)(((double)itemAnimation / (double)maxAnimation - 0.5) *
                                 (double)-direction * 3.5 - direction * 0.300000011920929);
                }
            }
            else if (sItem.useStyle == 2)
            {
                itemRotation =
                    (float)((double)itemAnimation / (double)maxAnimation *
                             direction * 2.0 + -1.39999997615814 * direction);
                if ((double)itemAnimation < (double)maxAnimation * 0.5)
                {
                    itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - 9.0 - itemRotation * 12.0 * direction) * direction);
                    itemLocation.Y = (float)(Center.Y + 38.0 + (double)itemRotation * direction * 4.0);
                }
                else
                {
                    itemLocation.X = (float)(Center.X + (Main.itemTexture[sItem.type].Width * 0.5 - 9.0 - itemRotation * 16.0 * direction) * direction);
                    itemLocation.Y = (float)(Center.Y + 38.0 + (double)itemRotation * direction);
                }
            }
            else if (sItem.useStyle == 3)
            {
                if (itemAnimation > maxAnimation * 0.666)
                {
                    itemLocation.X = -1000f;
                    itemLocation.Y = -1000f;
                    itemRotation = -1.3f * direction;
                }
                else
                {
                    itemLocation.X = (float)((Center.X +Main.itemTexture[sItem.type].Width * 0.5 - 4.0) * direction);
                    itemLocation.Y = Center.Y + 24f;
                    var num = (float)((double)itemAnimation / (double)maxAnimation * (double)Main.itemTexture[sItem.type].Width * direction * (double)sItem.scale * 1.20000004768372) -(float)(10 * direction);
                    if ((double)num > -4.0 && direction == -1)
                        num = -8f;
                    if ((double)num < 4.0 && direction == 1)
                        num = 8f;
                    itemLocation.X -= num;
                    itemRotation = 0.8f * (float)direction;
                }
            }
            else if (sItem.useStyle == 4)
            {
                var num = 0;
                if (sItem.type == ItemID.CelestialSigil)
                    num = 10;
                itemRotation = 0.0f;
                itemLocation.X = (float)(Center.X + ((double)Main.itemTexture[sItem.type].Width * 0.5 - 9.0 - (double)itemRotation * 14.0 * direction - 4.0 - (double)num) * direction);
                itemLocation.Y = (float)((double)Center.Y + (double)Main.itemTexture[sItem.type].Height * 0.5 + 4.0);
            }
            else if (sItem.useStyle == 5)
            {
                if (sItem.type == ItemID.SpiritFlame)
                {
                    itemRotation = 0.0f;
                    itemLocation.X = Center.X + (float)(6 * direction);
                    itemLocation.Y = Center.Y + 6f;
                }
                else if (Item.staff[sItem.type])
                {
                    var num = 6f;
                    if (sItem.type == ItemID.NebulaArcanum)
                        num = 14f;
                    itemLocation = Center;
                    itemLocation += itemRotation.ToRotationVector2() * num * (float)direction;
                }
                else
                {
                    itemLocation.X = (float)((double)Center.X -(double)Main.itemTexture[sItem.type].Width * 0.5) - (float)(direction * 2);
                    itemLocation.Y = Center.Y - (float)Main.itemTexture[sItem.type].Height * 0.5f;
                }
            }
            return itemLocation;
        }
	}
}