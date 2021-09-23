using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Celestial;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
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
			Tooltip.SetDefault("Summons a Subspace Servant to assist in combat\nUses the weapon in your last inventory slot for you\nDoesn't work with all weapons\n'You've proven yourself plenty'");
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
        bool accessory = true;
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SubspacePlayer modPlayer = SubspacePlayer.ModPlayer(player);
			modPlayer.servantActive = true;
        }
        public override void UpdateInventory(Player player)
        {
            accessory = false;
        }
        Texture2D itemTextureOutline;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            SubspacePlayer modPlayer = SubspacePlayer.ModPlayer(player);
            Texture2D texture = Main.itemTexture[item.type];
            if(itemTextureOutline == null)
            {
                Color[] data = SubspaceServant.Greenify(texture, new Color(0, 255, 0));
                itemTextureOutline = new Texture2D(Main.graphics.GraphicsDevice, texture.Width, texture.Height);
                itemTextureOutline.SetData(0, null, data, 0, texture.Width * texture.Height);
            }
            if(modPlayer.foundItem && accessory)
                for (int i = 0; i < 4; i++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * i));
                    spriteBatch.Draw(itemTextureOutline, position + circular, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
                }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
    public class SubspaceItem : GlobalItem
    {
        public static int itemID = -1;
        public static Texture2D itemTextureOutline;
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            if(player.inventory[49] == item)
            {
                SubspacePlayer modPlayer = SubspacePlayer.ModPlayer(player);
                Texture2D texture = Main.itemTexture[item.type];
                if (itemID != item.type || itemTextureOutline == null)
                {
                    Color[] data = SubspaceServant.Greenify(texture, new Color(0, 255, 0));
                    itemTextureOutline = new Texture2D(Main.graphics.GraphicsDevice, texture.Width, texture.Height);
                    itemTextureOutline.SetData(0, null, data, 0, texture.Width * texture.Height);
                    itemID = item.type;
                }
                if (modPlayer.foundItem)
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * i));
                        spriteBatch.Draw(itemTextureOutline, position + circular, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
                    }
            }
            return true;
        }
    }
	public class SubspacePlayer : ModPlayer
	{
        public bool foundItem = false;
		public bool servantActive = false;
        public bool servantIsVanity = false;
		public static SubspacePlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<SubspacePlayer>();
        }
        public static readonly PlayerLayer DrawServant = new PlayerLayer("SOTS", "DrawServant", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            Mod mod = ModLoader.GetMod("SOTS");
            Player drawPlayer = drawInfo.drawPlayer;
            SubspacePlayer modPlayer = SubspacePlayer.ModPlayer(drawPlayer);
            int Probe = modPlayer.Probe;
            for(int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                SubspaceServant subServ = proj.modProjectile as SubspaceServant;
                if(subServ != null && proj.owner == drawInfo.drawPlayer.whoAmI && proj.active)
                {
                    float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
                    float drawY = (int)drawInfo.position.Y + drawPlayer.height / 2;
                    Color color = Color.White.MultiplyRGBA(Lighting.GetColor((int)drawX / 16, (int)drawY / 16));
                    subServ.PreDraw(Main.spriteBatch, color);
                    subServ.PostDraw(Main.spriteBatch, color);
                }
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            DrawServant.visible = true;
            layers.Insert(0, DrawServant);
        }
        public int subspaceServantShader = 0;
        public override void ResetEffects()
        {
            subspaceServantShader = 0;
            servantIsVanity = false;
            for (int i = 9 + player.extraAccessorySlots; i < player.armor.Length; i++) //checking vanity slots
            {
                Item item = player.armor[i];
                if (item.type == ModContent.ItemType<SubspaceLocket>())
                {
                    servantActive = true;
                    servantIsVanity = true;
                }
                //if (item.type == ModContent.ItemType<SubspaceLocket>())
                //{
                //    SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
                //}
            }
            //for (int i = 0; i < 10; i++) //iterating through armor + accessories
            //{
            //    Item item = player.armor[i];
            //    if (item.type == ModContent.ItemType<SubspaceLocket>())
            //    {
            //        SubspacePlayer.ModPlayer(player).subspaceServantShader = GameShaders.Armor.GetShaderIdFromItemId(player.dye[i].type);
            //    }
            //}
            if (servantActive)
				Summon();
			servantActive = false;
            foundItem = false;
        }
		public int Probe = -1;
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
            if (i == Main.myPlayer)
            {
                var flag2 = true;
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
                    {
                        Item obj = new Item();
                        var flag1 = false;
                        for (var index = 54; index < 58; ++index)
                        {
                            if (player.inventory[index].ammo == sItem.useAmmo && player.inventory[index].stack > 0)
                            {
                                obj = player.inventory[index];
                                break;
                            }
                        }
                        if (!flag1)
                        {
                            for (var index = 0; index < 54; ++index)
                            {
                                if (player.inventory[index].ammo == sItem.useAmmo && player.inventory[index].stack > 0)
                                {
                                    obj = player.inventory[index];
                                    break;
                                }
                            }
                        }
                        PickAmmo(sItem, ref shoot, ref speed, ref canShoot, ref Damage, ref knockBack, !ItemLoader.ConsumeAmmo(sItem, obj, player));
                    }
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
                        bool Shoot1 = ItemLoader.Shoot(sItem, player, ref vector2_1, ref num6, ref num7, ref shoot, ref Damage, ref knockBack);
                        bool Shoot2 = PlayerHooks.Shoot(player, sItem, ref vector2_1, ref num6, ref num7, ref shoot, ref Damage, ref knockBack);
                        if (!Shoot1 || !Shoot2)
                        {
                            player.position = temp;
                            return;
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
                        else
                        {
                            int index = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, num6, num7, shoot, Damage, num1, i, 0.0f, 0.0f);
                            if (sItem.type == 726)
                                Main.projectile[index].magic = true;
                            if (sItem.type == 724 || sItem.type == 676)
                                Main.projectile[index].melee = true;
                            if (shoot == 80)
                            {
                                Main.projectile[index].ai[0] = Player.tileTargetX;
                                Main.projectile[index].ai[1] = Player.tileTargetY;
                            }
                            if (shoot == 442)
                            {
                                Main.projectile[index].ai[0] = Player.tileTargetX;
                                Main.projectile[index].ai[1] = Player.tileTargetY;
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
            if (sItem.mana > 0 && (sItem.type != (int)sbyte.MaxValue || !player.spaceGun))
                player.manaRegenDelay = (int)player.maxRegenDelay;
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
                        itemLocation.X = (float)(Center.X - (Main.itemTexture[sItem.type].Width * 0.5 - num) * direction);
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
                        itemLocation.X = (float)(Center.X - (Main.itemTexture[sItem.type].Width * 0.5 - num1) * direction);
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
                    //Center -= new Vector2(10, 22);
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
        public void UseVanillaItemHitbox(Vector2 itemLocation, Vector2 Center, Vector2 Velocity, Item sItem, int itemAnimation, int maxAnimation, ref int direction, ref float itemRotation)
        {
            Center -= new Vector2(10, 22);
            if ((sItem.damage >= 0 && sItem.type > 0 && (!sItem.noMelee || sItem.type == ItemID.NebulaBlaze || sItem.type == ItemID.SpiritFlame)) && itemAnimation > 0)
            {
                var flag2 = false;
                Rectangle r = new Rectangle((int)itemLocation.X, (int)itemLocation.Y, 32, 32);
                if (!Main.dedServ) r = new Rectangle((int)itemLocation.X, (int)itemLocation.Y, Main.itemTexture[sItem.type].Width, Main.itemTexture[sItem.type].Height);
                r.Width = (int)((double)r.Width * sItem.scale);
                r.Height = (int)((double)r.Height * sItem.scale);
                if (direction == -1)
                    r.X -= r.Width;
                r.Y -= r.Height;
                if (sItem.useStyle == 1)
                {
                    if ((double)itemAnimation < (double)maxAnimation * 0.333)
                    {
                        if (direction == -1)
                            r.X -= (int)(r.Width * 1.4 - r.Width);
                        r.Width = (int)(r.Width * 1.4);
                        r.Y += (int)(r.Height * 0.5);
                        r.Height = (int)(r.Height * 1.1);
                    }
                    else if ((double)itemAnimation >= (double)maxAnimation * 0.666)
                    {
                        if (direction == 1)
                            r.X -= (int)(r.Width * 1.2);
                        r.Width *= 2;
                        r.Y -= (int)(r.Height * 1.4 - r.Height);
                        r.Height = (int)(r.Height * 1.4);
                    }
                }
                else if (sItem.useStyle == 3)
                {
                    if ((double)itemAnimation > (double)maxAnimation * 0.666)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        if (direction == -1)
                            r.X -= (int)((double)r.Width * 1.4 - (double)r.Width);
                        r.Width = (int)((double)r.Width * 1.4);
                        r.Y += (int)((double)r.Height * 0.6);
                        r.Height = (int)((double)r.Height * 0.6);
                    }
                }
                ItemLoader.MeleeEffects(sItem, player, r);
                if (sItem.type == ItemID.NebulaBlaze)
                    flag2 = true;
                if (sItem.type == ItemID.SpiritFlame)
                {
                    flag2 = true;
                    var vector2_1 = itemLocation + new Vector2((float)(direction * 30), -8f);
                    var num = maxAnimation - 2;
                    var vector2_2 = vector2_1 - Center - Velocity;
                    var amount = 0.0f;
                    while ((double)amount < 1.0)
                    {
                        var vector2_3 = Vector2.Lerp(Center + vector2_2, vector2_1, amount);
                        var dust = Main.dust[Dust.NewDust(vector2_1 - Vector2.One * 8f, 16, 16, 27, 0.0f, -2f, 0, new Color(), 1f)];
                        dust.noGravity = true;
                        dust.position = vector2_3;
                        dust.velocity = new Vector2(0.0f, -2.0f);
                        dust.scale = 1.2f;
                        dust.alpha = 200;
                        amount += 0.2f;
                    }
                }

                if (!flag2)
                {
                    if (sItem.type == ItemID.EnchantedSword && Main.rand.Next(5) == 0)
                    {
                        int Type;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Type = 15;
                                break;
                            case 1:
                                Type = 57;
                                break;
                            default:
                                Type = 58;
                                break;
                        }

                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, Type,
                            (float)(direction * 2), 0.0f, 150, new Color(), 1.3f);
                        Main.dust[index].velocity *= 0.2f;
                    }

                    if (sItem.type == ItemID.InfluxWaver && Main.rand.Next(2) == 0)
                    {
                        var Type = Utils.SelectRandom<int>(Main.rand, new int[2]
                        {
                            226,
                            229
                        });
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, Type,
                            (float)(direction * 2), 0.0f, 150, new Color(), 1f);
                        Main.dust[index].velocity *= 0.2f;
                        Main.dust[index].noGravity = true;
                    }

                    if ((sItem.type == 44 || sItem.type == 45 || (sItem.type == 46 || sItem.type == 103) || sItem.type == 104) && Main.rand.Next(15) == 0)
                        Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 14, (float)(direction * 2), 0.0f, 150, new Color(), 1.3f);
                    if (sItem.type == 273 || sItem.type == 675)
                    {
                        if (Main.rand.Next(5) == 0)
                            Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 14,
                                (float)(direction * 2), 0.0f, 150, new Color(), 1.4f);
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 27,
                            Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 100,
                            new Color(), 1.2f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity.X /= 2f;
                        Main.dust[index].velocity.Y /= 2f;
                    }

                    if (sItem.type == 723 && Main.rand.Next(2) == 0)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 64, 0.0f,
                            0.0f, 150, new Color(), 1.2f);
                        Main.dust[index].noGravity = true;
                    }

                    if (sItem.type == ItemID.Starfury)
                    {
                        if (Main.rand.Next(5) == 0)
                            Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 58, 0.0f, 0.0f, 150, new Color(), 1.2f);
                        if (Main.rand.Next(10) == 0)
                            Gore.NewGore(new Vector2((float)r.X, (float)r.Y), new Vector2(), Main.rand.Next(16, 18), 1f);
                    }

                    if (sItem.type == 3065)
                    {
                        var index1 = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 58, 0.0f,
                            0.0f, 150, new Color(), 1.2f);
                        Main.dust[index1].velocity *= 0.5f;
                        if (Main.rand.Next(8) == 0)
                        {
                            var index2 = Gore.NewGore(new Vector2((float)r.Center.X, (float)r.Center.Y),
                                new Vector2(), 16, 1f);
                            Main.gore[index2].velocity *= 0.5f;
                            Main.gore[index2].velocity += new Vector2((float)direction, 0.0f);
                        }
                    }

                    if (sItem.type == 190)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 40,
                            Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 0,
                            new Color(), 1.2f);
                        Main.dust[index].noGravity = true;
                    }
                    else if (sItem.type == 213)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 3,
                            Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 0,
                            new Color(), 1.2f);
                        Main.dust[index].noGravity = true;
                    }

                    if (sItem.type == 121)
                    {
                        for (var index1 = 0; index1 < 2; ++index1)
                        {
                            var index2 = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 6,
                                Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 100,
                                new Color(), 2.5f);
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].velocity.X *= 2f;
                            Main.dust[index2].velocity.Y *= 2f;
                        }
                    }

                    if (sItem.type == 122 || sItem.type == 217)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 6,
                            Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 100,
                            new Color(), 1.9f);
                        Main.dust[index].noGravity = true;
                    }

                    if (sItem.type == 155)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 172,
                            Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 100,
                            new Color(), 0.9f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity *= 0.1f;
                    }

                    if (sItem.type == 676 && Main.rand.Next(3) == 0)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 67,
                            Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 90,
                            new Color(), 1.5f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity *= 0.2f;
                    }

                    if (sItem.type == 3063)
                    {
                        var index = Dust.NewDust(r.TopLeft(), r.Width, r.Height, 66, 0.0f, 0.0f, 150, Color.Transparent,
                            0.85f);
                        Main.dust[index].color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity /= 2f;
                    }

                    if (sItem.type == 3823)
                    {
                        var dust = Dust.NewDustDirect(r.TopLeft(), r.Width, r.Height, 6, Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 100, Color.Transparent, 0.7f);
                        dust.noGravity = true;
                        dust.velocity *= 2f;
                        dust.fadeIn = 0.9f;
                    }

                    if (sItem.type == 724 && Main.rand.Next(5) == 0)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 67,
                            Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 90,
                            new Color(), 1.5f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity *= 0.2f;
                    }

                    if (sItem.type >= 795 && sItem.type <= 802 && Main.rand.Next(3) == 0)
                    {
                        var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 115, Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 140, new Color(), 1.5f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity *= 0.25f;
                    }

                    if (sItem.type == 367 || sItem.type == 368 || sItem.type == 674)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 57, Velocity.X * 0.2f + (float)(direction * 3), Velocity.Y * 0.2f, 100, new Color(), 1.1f);
                            Main.dust[index].noGravity = true;
                            Main.dust[index].velocity.X /= 2f;
                            Main.dust[index].velocity.Y /= 2f;
                            Main.dust[index].velocity.X += (float)(direction * 2);
                        }

                        if (Main.rand.Next(4) == 0)
                        {
                            var index = Dust.NewDust(new Vector2((float)r.X, (float)r.Y), r.Width, r.Height, 43, 0.0f,
                                0.0f, 254, new Color(), 0.3f);
                            Main.dust[index].velocity *= 0.0f;
                        }
                    }

                    if (sItem.type >= 198 && sItem.type <= 203 || sItem.type >= 3764 && sItem.type <= 3769)
                    {
                        var R = 0.5f;
                        var G = 0.5f;
                        var B = 0.5f;
                        if (sItem.type == 198 || sItem.type == 3764)
                        {
                            R *= 0.1f;
                            G *= 0.5f;
                            B *= 1.2f;
                        }
                        else if (sItem.type == 199 || sItem.type == 3765)
                        {
                            R *= 1f;
                            G *= 0.2f;
                            B *= 0.1f;
                        }
                        else if (sItem.type == 200 || sItem.type == 3766)
                        {
                            R *= 0.1f;
                            G *= 1f;
                            B *= 0.2f;
                        }
                        else if (sItem.type == 201 || sItem.type == 3767)
                        {
                            R *= 0.8f;
                            G *= 0.1f;
                            B *= 1f;
                        }
                        else if (sItem.type == 202 || sItem.type == 3768)
                        {
                            R *= 0.8f;
                            G *= 0.9f;
                            B *= 1f;
                        }
                        else if (sItem.type == 203 || sItem.type == 3769)
                        {
                            R *= 0.9f;
                            G *= 0.9f;
                            B *= 0.1f;
                        }

                        Lighting.AddLight(
                            (int)(((double)itemLocation.X + 6.0 + (double)Velocity.X) / 16.0),
                            (int)(((double)itemLocation.Y - 14.0) / 16.0), R, G, B);
                    }
                    if (Main.myPlayer == player.whoAmI && sItem.damage > 0)
                    {
                        var num1 = sItem.damage;
                        if (sItem.melee)
                            num1 = (int)(sItem.damage * player.meleeDamage);
                        if (sItem.ranged)
                            num1 = (int)(sItem.damage * player.rangedDamage);
                        if (sItem.magic)
                            num1 = (int)(sItem.damage * player.magicDamage);
                        if (sItem.summon)
                            num1 = (int)(sItem.damage * player.minionDamage);
                        if (sItem.thrown)
                            num1 = (int)(sItem.damage * player.thrownDamage);
                        var knockBack = sItem.knockBack;
                        var num2 = 1f;
                        if (player.kbGlove)
                            ++num2;
                        if (player.kbBuff)
                            num2 += 0.5f;
                        var num3 = knockBack * num2;
                        if (sItem.type != ItemID.GoldenBugNet)
                        {
                            for (var index1 = 0; index1 < 200; ++index1)
                            {
                                if (Main.npc[index1].active && Main.npc[index1].immune[player.whoAmI] == 0)
                                {
                                    if (!Main.npc[index1].dontTakeDamage)
                                    {
                                        if (!Main.npc[index1].friendly ||
                                            Main.npc[index1].type == NPCID.Guide && player.killGuide ||
                                            Main.npc[index1].type == NPCID.Clothier && player.killClothier)
                                        {
                                            var rectangle = new Rectangle((int)Main.npc[index1].position.X, (int)Main.npc[index1].position.Y, Main.npc[index1].width, Main.npc[index1].height);
                                            if (r.Intersects(rectangle))
                                            {
                                                var crit = false;
                                                if (sItem.melee && Main.rand.Next(1, 101) <= player.meleeCrit)
                                                    crit = true;
                                                if (sItem.ranged && Main.rand.Next(1, 101) <= player.rangedCrit)
                                                    crit = true;
                                                if (sItem.magic && Main.rand.Next(1, 101) <= player.magicCrit)
                                                    crit = true;
                                                if (sItem.thrown && Main.rand.Next(1, 101) <= player.thrownCrit)
                                                    crit = true;
                                                var banner = Item.NPCtoBanner(Main.npc[index1].BannerID());
                                                if (banner > 0 && player.NPCBannerBuff[banner])
                                                    num1 = !Main.expertMode ? (int)(num1 * ItemID.Sets.BannerStrength[Item.BannerToItem(banner)].NormalDamageDealt) : (int)(num1 * ItemID.Sets.BannerStrength[Item.BannerToItem(banner)].ExpertDamageDealt);
                                                var num8 = Main.DamageVar((float)num1);
                                                player.OnHit(Main.npc[index1].Center.X, Main.npc[index1].Center.Y, Main.npc[index1]);
                                                if (player.armorPenetration > 0)
                                                    num8 += Main.npc[index1].checkArmorPenetration(player.armorPenetration);
                                                NPCLoader.ModifyHitByItem(Main.npc[index1], player, sItem, ref num8, ref num3, ref crit);
                                                PlayerHooks.ModifyHitNPC(player, sItem, Main.npc[index1], ref num8, ref num3, ref crit);
                                                var num9 = (int)Main.npc[index1].StrikeNPC(num8, num3, direction, crit, false, false);
                                                NPCLoader.OnHitByItem(Main.npc[index1], player, sItem, num8, num3, crit);
                                                PlayerHooks.OnHitNPC(player, sItem, Main.npc[index1], num8, num3, crit);
                                                player.StatusNPC(sItem.type, index1);
                                                if (sItem.type == ItemID.Bladetongue)
                                                {
                                                    var vector2_1 = new Vector2((direction * 100 + Main.rand.Next(-25, 26)), Main.rand.Next(-75, 76));
                                                    vector2_1.Normalize();
                                                    vector2_1 *= (float)Main.rand.Next(30, 41) * 0.1f;
                                                    var vector2_2 = new Vector2(
                                                        (float)(r.X + Main.rand.Next(r.Width)),
                                                        (float)(r.Y + Main.rand.Next(r.Height)));
                                                    vector2_2 = (vector2_2 + Main.npc[index1].Center * 2f) / 3f;
                                                    Projectile.NewProjectile(vector2_2.X, vector2_2.Y, vector2_1.X, vector2_1.Y, ProjectileID.IchorSplash, (int)((double)num1 * 0.7), num3 * 0.7f, player.whoAmI, 0.0f, 0.0f);
                                                }

                                                var flag3 = !Main.npc[index1].immortal;
                                                if (player.beetleOffense && flag3)
                                                {
                                                    player.beetleCounter += (float)num9;
                                                    player.beetleCountdown = 0;
                                                }

                                                if (sItem.type == 1826 && (Main.npc[index1].value > 0.0 || Main.npc[index1].damage > 0 && !Main.npc[index1].friendly)) pumpkinSword(index1, (int)((double)num1 * 1.5), num3);

                                                if (sItem.type == 1123 && flag3)
                                                {
                                                    var num10 = Main.rand.Next(1, 4);
                                                    if (player.strongBees && Main.rand.Next(3) == 0)
                                                        ++num10;
                                                    for (var index2 = 0; index2 < num10; ++index2)
                                                    {
                                                        var num11 =
                                                            (float)(direction * 2) +
                                                            (float)Main.rand.Next(-35, 36) * 0.02f;
                                                        var num12 = (float)Main.rand.Next(-35, 36) * 0.02f;
                                                        var SpeedX = num11 * 0.2f;
                                                        var SpeedY = num12 * 0.2f;
                                                        Projectile.NewProjectile((float)(r.X + r.Width / 2), (float)(r.Y + r.Height / 2), SpeedX, SpeedY,player.beeType(), player.beeDamage(num8 / 3), player.beeKB(0.0f), player.whoAmI, 0.0f, 0.0f);
                                                    }
                                                }

                                                if (Main.npc[index1].value > 0.0 && player.coins && Main.rand.Next(5) == 0)
                                                {
                                                    var Type = 71;
                                                    if (Main.rand.Next(10) == 0)
                                                        Type = 72;
                                                    if (Main.rand.Next(100) == 0)
                                                        Type = 73;
                                                    var number = Item.NewItem((int)Main.npc[index1].position.X, (int)Main.npc[index1].position.Y, Main.npc[index1].width, Main.npc[index1].height, Type, 1, false, 0, false, false);
                                                    Main.item[number].stack = Main.rand.Next(1, 11);
                                                    Main.item[number].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                                                    Main.item[number].velocity.X = Main.rand.Next(10, 31) * 0.2f * direction;
                                                    if (Main.netMode == 1)
                                                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                                }

                                                var num13 = Item.NPCtoBanner(Main.npc[index1].BannerID());
                                                if (num13 >= 0)
                                                    player.lastCreatureHit = num13;
                                                if (Main.netMode != 0)
                                                {
                                                    if (crit)
                                                        NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, index1, num8, num3, direction, 1, 0, 0);
                                                    else
                                                        NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, index1, num8, num3, direction, 0, 0, 0);
                                                }

                                                if (player.accDreamCatcher)
                                                    player.addDPS(num8);
                                                Main.npc[index1].immune[player.whoAmI] = itemAnimation;
                                            }
                                        }
                                    }
                                }
                            }

                            if (player.hostile)
                            {
                                for (var index1 = 0; index1 < (int)byte.MaxValue; ++index1)
                                {
                                    if (index1 != player.whoAmI && Main.player[index1].active && Main.player[index1].hostile && !Main.player[index1].immune && !Main.player[index1].dead && (Main.player[player.whoAmI].team == 0 || Main.player[player.whoAmI].team != Main.player[index1].team))
                                    {
                                        var rectangle = new Rectangle((int)Main.player[index1].position.X, (int)Main.player[index1].position.Y, Main.player[index1].width, Main.player[index1].height);
                                        if (r.Intersects(rectangle) && player.CanHit((Entity)Main.player[index1]))
                                        {
                                            var flag3 = false;
                                            if (Main.rand.Next(1, 101) <= 10)
                                                flag3 = true;
                                            var num8 = Main.DamageVar((float)num1);
                                            player.StatusPvP(sItem.type, index1);
                                            player.OnHit(Main.player[index1].Center.X, Main.player[index1].Center.Y,
                                                (Entity)Main.player[index1]);
                                            var playerDeathReason = PlayerDeathReason.ByPlayer(player.whoAmI);
                                            ItemLoader.ModifyHitPvp(sItem, player, Main.player[index1], ref num8, ref flag3);
                                            PlayerHooks.ModifyHitPvp(player, sItem, Main.player[index1], ref num8, ref flag3);
                                            var num9 = (int)Main.player[index1].Hurt(playerDeathReason, num8, direction, true, false, flag3, -1);
                                            ItemLoader.OnHitPvp(sItem, player, Main.player[index1], num8, flag3);
                                            PlayerHooks.OnHitPvp(player, sItem, Main.player[index1], num8, flag3);
                                            player.StatusPvP(sItem.type, index1);
                                            if (sItem.type == ItemID.Bladetongue)
                                            {
                                                var vector2_1 = new Vector2(
                                                    (float)(direction * 100 + Main.rand.Next(-25, 26)),
                                                    (float)Main.rand.Next(-75, 76));
                                                vector2_1.Normalize();
                                                vector2_1 *= (float)Main.rand.Next(30, 41) * 0.1f;
                                                var vector2_2 = new Vector2((float)(r.X + Main.rand.Next(r.Width)),
                                                    (float)(r.Y + Main.rand.Next(r.Height)));
                                                vector2_2 = (vector2_2 + Main.player[index1].Center * 2f) / 3f;
                                                Projectile.NewProjectile(vector2_2.X, vector2_2.Y, vector2_1.X, vector2_1.Y, ProjectileID.IchorSplash, (int)((double)num1 * 0.7), num3 * 0.7f, player.whoAmI, 0.0f, 0.0f);
                                            }

                                            if (player.beetleOffense)
                                            {
                                                player.beetleCounter += (float)num9;
                                                player.beetleCountdown = 0;
                                            }
                                            if (sItem.type == ItemID.BeeKeeper)
                                            {
                                                var num10 = Main.rand.Next(1, 4);
                                                if (player.strongBees && Main.rand.Next(3) == 0)
                                                    ++num10;
                                                for (var index2 = 0; index2 < num10; ++index2)
                                                {
                                                    var num11 =
                                                        (float)(direction * 2) +
                                                        (float)Main.rand.Next(-35, 36) * 0.02f;
                                                    var num12 = (float)Main.rand.Next(-35, 36) * 0.02f;
                                                    var SpeedX = num11 * 0.2f;
                                                    var SpeedY = num12 * 0.2f;
                                                    Projectile.NewProjectile(r.X + r.Width / 2, r.Y + r.Height / 2, SpeedX, SpeedY, player.beeType(), player.beeDamage(num8 / 3), player.beeKB(0.0f), player.whoAmI, 0.0f, 0.0f);
                                                }
                                            }
                                            //if (sItem.type == ItemID.TheHorsemansBlade && Main.npc[index1].value > 0.0)
                                            //    pumpkinSword(index1, (int)(num1 * 1.5), num3);
                                            if (Main.netMode != 0)
                                                NetMessage.SendPlayerHurt(index1, playerDeathReason, num8, direction, flag3, true, -1, -1, -1);
                                        }
                                    }
                                }
                            }

                            if (sItem.type == ItemID.Hammush && (itemAnimation == (int)(maxAnimation * 0.1) || itemAnimation == (int)(maxAnimation * 0.3) || (itemAnimation == (int)(maxAnimation * 0.5) || itemAnimation == (int)(maxAnimation * 0.7)) || itemAnimation == (int)(maxAnimation * 0.9)))
                            {
                                var num8 = 0.0f;
                                var num9 = 0.0f;
                                var num10 = 0.0f;
                                var num11 = 0.0f;
                                if (itemAnimation == (int)(maxAnimation * 0.9))
                                    num8 = -7f;
                                if (itemAnimation == (int)(maxAnimation * 0.7))
                                {
                                    num8 = -6f;
                                    num9 = 2f;
                                }
                                if (itemAnimation == (int)(maxAnimation * 0.5))
                                {
                                    num8 = -4f;
                                    num9 = 4f;
                                }
                                if (itemAnimation == (int)((double)maxAnimation * 0.3))
                                {
                                    num8 = -2f;
                                    num9 = 6f;
                                }
                                if (itemAnimation == (int)((double)maxAnimation * 0.1))
                                    num9 = 7f;
                                if (itemAnimation == (int)((double)maxAnimation * 0.7))
                                    num11 = 26f;
                                if (itemAnimation == (int)((double)maxAnimation * 0.3))
                                {
                                    num11 -= 4f;
                                    num10 -= 20f;
                                }
                                if (itemAnimation == (int)((double)maxAnimation * 0.1))
                                    num10 += 6f;
                                if (direction == -1)
                                {
                                    if (itemAnimation == (int)((double)maxAnimation * 0.9))
                                        num11 -= 8f;
                                    if (itemAnimation == (int)((double)maxAnimation * 0.7))
                                        num11 -= 6f;
                                }
                                var num12 = num8 * 1.5f;
                                var num13 = num9 * 1.5f;
                                var num14 = num11 * (float)direction;
                                var num15 = num10;
                                Projectile.NewProjectile((float)(r.X + r.Width / 2) + num14, (float)(r.Y + r.Height / 2) + num15, (float)direction * num13, num12 , ProjectileID.Mushroom, num1 / 2, 0.0f, player.whoAmI, 0.0f, 0.0f);
                            }
                        }
                    }
                }
            }
        }
        private void pumpkinSword(int i, int dmg, float kb)
        {
            var checkScreenHeight = Main.LogicCheckScreenHeight;
            var checkScreenWidth = Main.LogicCheckScreenWidth;
            var num1 = Main.rand.Next(100, 300);
            var num2 = Main.rand.Next(100, 300);
            var num3 = Main.rand.Next(2) != 0
                ? num1 + (checkScreenWidth / 2 - num1)
                : num1 - (checkScreenWidth / 2 + num1);
            var num4 = Main.rand.Next(2) != 0
                ? num2 + (checkScreenHeight / 2 - num2)
                : num2 - (checkScreenHeight / 2 + num2);
            var num5 = num3 + (int)player.Center.X;
            var num6 = num4 + (int)player.Center.Y;
            var num7 = 8f;
            var vector2 = new Vector2(num5, num6);
            var num8 = Main.npc[i].Center.X - vector2.X;
            var num9 = Main.npc[i].Center.Y - vector2.Y;
            var num10 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
            var num11 = num7 / num10;
            var SpeedX = num8 * num11;
            var SpeedY = num9 * num11;
            Projectile.NewProjectile(num5, num6, SpeedX, SpeedY, ProjectileID.FlamingJack, dmg, kb, player.whoAmI, i, 0.0f);
        }
        public void PickAmmo(Item sItem, ref int shoot, ref float speed, ref bool canShoot, ref int Damage, ref float KnockBack, bool dontConsume = false)
        {
            var obj = new Item();
            var flag1 = false;
            for (var index = 54; index < 58; ++index)
            {
                if (player.inventory[index].ammo == sItem.useAmmo && player.inventory[index].stack > 0)
                {
                    obj = player.inventory[index];
                    canShoot = true;
                    flag1 = true;
                    break;
                }
            }

            if (!flag1)
            {
                for (var index = 0; index < 54; ++index)
                {
                    if (player.inventory[index].ammo == sItem.useAmmo && player.inventory[index].stack > 0)
                    {
                        obj = player.inventory[index];
                        canShoot = true;
                        break;
                    }
                }
            }

            if (!canShoot)
                return;
            if (sItem.type == ItemID.SnowmanCannon)
            {
                shoot = 338 + obj.shoot / 3;
            }
            else if (sItem.useAmmo == AmmoID.Rocket)
            {
                shoot += obj.shoot;
            }
            else if (sItem.useAmmo == 780)
                shoot += obj.shoot;
            else if (obj.shoot > 0)
                shoot = obj.shoot;
            if (sItem.type == 3019 && shoot == 1)
                shoot = 485;
            if (sItem.type == 3052)
                shoot = 495;
            if (shoot == 42)
            {
                if (obj.type == ItemID.EbonsandBlock)
                {
                    shoot = 65;
                    Damage += 5;
                }
                else if (obj.type == ItemID.PearlsandBlock)
                {
                    shoot = 68;
                    Damage += 5;
                }
                else if (obj.type == ItemID.CrimsandBlock)
                {
                    shoot = 354;
                    Damage += 5;
                }
            }

            if (sItem.type == ItemID.BeesKnees && shoot == 1)
                shoot = 469;
            if (player.magicQuiver && (sItem.useAmmo == AmmoID.Arrow || sItem.useAmmo == AmmoID.Stake))
            {
                KnockBack = (float)(int)((double)KnockBack * 1.1);
                speed *= 1.1f;
            }

            speed += obj.shootSpeed;
            if (obj.ranged)
            {
                if (obj.damage > 0)
                    Damage += (int)((double)obj.damage * (double)player.rangedDamage);
            }
            else
                Damage += obj.damage;

            if (sItem.useAmmo == AmmoID.Arrow && player.archery)
            {
                if ((double)speed < 20.0)
                {
                    speed *= 1.2f;
                    if ((double)speed > 20.0)
                        speed = 20f;
                }

                Damage = (int)((double)Damage * 1.2);
            }

            KnockBack += obj.knockBack;
            var flag2 = dontConsume;
            if (sItem.type == 3475 && Main.rand.Next(3) != 0)
                flag2 = true;
            if (sItem.type == 3540 && Main.rand.Next(3) != 0)
                flag2 = true;
            if (player.magicQuiver && sItem.useAmmo == AmmoID.Arrow && Main.rand.Next(5) == 0)
                flag2 = true;
            if (player.ammoBox && Main.rand.Next(5) == 0)
                flag2 = true;
            if (player.ammoPotion && Main.rand.Next(5) == 0)
                flag2 = true;
            if (sItem.type == 1782 && Main.rand.Next(3) == 0)
                flag2 = true;
            if (sItem.type == 98 && Main.rand.Next(3) == 0)
                flag2 = true;
            if (sItem.type == 2270 && Main.rand.Next(2) == 0)
                flag2 = true;
            if (sItem.type == 533 && Main.rand.Next(2) == 0)
                flag2 = true;
            if (sItem.type == 1929 && Main.rand.Next(2) == 0)
                flag2 = true;
            if (sItem.type == 1553 && Main.rand.Next(2) == 0)
                flag2 = true;
            if (sItem.type == 434 && player.itemAnimation < sItem.useAnimation - 2)
                flag2 = true;
            if (player.ammoCost80 && Main.rand.Next(5) == 0)
                flag2 = true;
            if (player.ammoCost75 && Main.rand.Next(4) == 0)
                flag2 = true;
            if (shoot == 85 && player.itemAnimation < player.itemAnimationMax - 6)
                flag2 = true;
            if ((shoot == 145 || shoot == 146 || (shoot == 147 || shoot == 148) || shoot == 149) &&
                player.itemAnimation < player.itemAnimationMax - 5)
                flag2 = true;
            if (flag2 || !obj.consumable)
                return;
            --obj.stack;
            if (obj.stack > 0)
                return;
            obj.active = false;
            obj.TurnToAir();
        }
        public void PickFrame(Item sItem, int itemAnimation, int maxAnimation, int direction, float itemRotation, ref int frame)
        {
            if (itemAnimation > 0 && sItem.useStyle != 10)
            {
                if (sItem.useStyle == 1 || sItem.type == 0)
                {
                    if ((double)itemAnimation < (double)maxAnimation * 0.333)
                        frame = 3;
                    else if ((double)itemAnimation < (double)maxAnimation * 0.666)
                        frame = 2;
                    else
                        frame = 1;
                }
                else if (sItem.useStyle == 2)
                {
                    if ((double)itemAnimation > (double)maxAnimation * 0.5)
                        frame = 3;
                    else
                        frame = 2;
                }
                else if (sItem.useStyle == 3)
                {
                    frame = 3;
                }
                else if (sItem.useStyle == 4)
                {
                    frame = 2;
                }
                else
                {
                    if (sItem.useStyle != 5)
                        return;
                    if (sItem.type == 281 || sItem.type == 986)
                    {
                        frame = 2;
                    }
                    else
                    {
                        var num4 = itemRotation * (float)direction;
                        frame = 3;
                        if ((double)num4 < -0.75)
                        {
                            frame = 2;
                        }
                        if ((double)num4 <= 0.6)
                            return;
                        frame = 4;
                    }
                }
            }
        }
        public Vector2 DrawPlayerItemPos(int itemtype)
        {
            var num = 10f;
            var vector2 = new Vector2((float)(Main.itemTexture[itemtype].Width / 2), (float)(Main.itemTexture[itemtype].Height / 2));
            if (itemtype == 95)
            {
                num = 6f;
                vector2.Y += 2f;
            }
            else if (itemtype == 1295)
                num = 4f;
            else if (itemtype == 3611)
                num = 2f;
            else if (itemtype == 3350)
                num = 2f;
            else if (itemtype == 2624)
                num = 4f;
            else if (itemtype == 3018)
                num = 2f;
            else if (itemtype == 3007)
            {
                num = 4f;
                vector2.Y += 4f;
            }
            else if (itemtype == 3107)
            {
                num = 4f;
                vector2.Y += 2f;
            }
            else if (itemtype == 3008)
            {
                num = -12f;
                vector2.Y += 2f;
            }
            else if (itemtype == 1255)
            {
                num = 6f;
                vector2.Y += 0.0f;
            }
            else if (itemtype == 2269)
            {
                num = 2f;
                vector2.Y += 2f;
            }
            else if (itemtype == 1265)
            {
                num = -8f;
                vector2.Y += 4f;
            }
            else if (itemtype == 2272)
            {
                num = 0.0f;
                vector2.Y += 4f;
            }
            else if (itemtype == 3029)
                num = 4f;
            else if (itemtype == 2796)
            {
                num = -28f;
                vector2.Y += 2f;
            }
            else if (itemtype == 2797)
                num = 0.0f;
            else if (itemtype == 2610)
                num = 0.0f;
            else if (itemtype == 2623)
            {
                num = -30f;
                vector2.Y -= 4f;
            }
            else if (itemtype == 3546)
            {
                num = -14f;
                vector2.Y -= 6f;
            }
            else if (itemtype == 1835)
            {
                num = -2f;
                vector2.Y += 2f;
            }
            else if (itemtype == 2624)
                num = -4f;
            else if (itemtype == 3859)
                num = -2f;
            else if (itemtype == 2888)
                num = 6f;
            else if (itemtype == 2223)
            {
                num = 2f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 1782)
            {
                num = 0.0f;
                vector2.Y += 4f;
            }
            else if (itemtype == 1929)
            {
                num = 0.0f;
                vector2.Y += 2f;
            }
            else if (itemtype == 2270)
                num = -4f;
            else if (itemtype == 1784)
            {
                num = 0.0f;
                vector2.Y += 4f;
            }
            else if (itemtype == 1000)
            {
                num = 6f;
                vector2.Y += 0.0f;
            }
            else if (itemtype == 1178)
            {
                num = 4f;
                vector2.Y += 0.0f;
            }
            else if (itemtype == 1319)
            {
                num = 0.0f;
                vector2.Y += 0.0f;
            }
            else if (itemtype == 1297)
            {
                num = -8f;
                vector2.Y += 0.0f;
            }
            else if (itemtype == 1121)
            {
                num = 6f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 1314)
                num = 2f;
            else if (itemtype == 1258)
            {
                num = 2f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 1155)
            {
                num = -10f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 1156)
                num = -2f;
            else if (itemtype == 96)
            {
                num = -8f;
                vector2.Y += 2f;
            }
            else if (itemtype == 1870)
            {
                num = -8f;
                vector2.Y += 2f;
            }
            else if (itemtype == 1260)
            {
                num = -8f;
                vector2.Y += 2f;
            }
            else if (itemtype == 1254)
            {
                num = -6f;
                vector2.Y += 2f;
            }
            else if (itemtype == 98)
            {
                num = -5f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 534)
            {
                num = -2f;
                vector2.Y += 1f;
            }
            else if (itemtype == 679)
            {
                num = 0.0f;
                vector2.Y += 2f;
            }
            else if (itemtype == 964)
            {
                num = 0.0f;
                vector2.Y += 0.0f;
            }
            else if (itemtype == 533)
            {
                num = -7f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 1553)
            {
                num = -10f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 506)
            {
                num = 0.0f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 1910)
            {
                num = 0.0f;
                vector2.Y -= 2f;
            }
            else if (itemtype == 494 || itemtype == 508)
            {
                num = -2f;
            }
            else
            {
                switch (itemtype)
                {
                    case 434:
                        num = 0.0f;
                        vector2.Y -= 2f;
                        break;
                    case 514:
                        num = 0.0f;
                        vector2.Y += 3f;
                        break;
                    default:
                        if (itemtype == 435 || itemtype == 436 || (itemtype == 481 || itemtype == 578) || (itemtype == 1187 || itemtype == 1194 || (itemtype == 1201 || itemtype == 1229)))
                        {
                            num = -2f;
                            vector2.Y -= 2f;
                            break;
                        }
                        switch (itemtype)
                        {
                            case 126:
                                num = 4f;
                                vector2.Y += 4f;
                                break;
                            case (int)sbyte.MaxValue:
                                num = 4f;
                                vector2.Y += 2f;
                                break;
                            case 157:
                                num = 6f;
                                vector2.Y += 2f;
                                break;
                            case 160:
                                num = -8f;
                                break;
                            case 197:
                                num = -5f;
                                vector2.Y += 4f;
                                break;
                            case 800:
                                num = 4f;
                                vector2.Y += 2f;
                                break;
                            default:
                                if (itemtype == 164 || itemtype == 219)
                                {
                                    num = 0.0f;
                                    vector2.Y += 2f;
                                    break;
                                }

                                if (itemtype == 165 || itemtype == 272)
                                {
                                    num = 4f;
                                    vector2.Y += 4f;
                                    break;
                                }

                                switch (itemtype)
                                {
                                    case 266:
                                        num = 0.0f;
                                        vector2.Y += 2f;
                                        break;
                                    case 281:
                                        num = 6f;
                                        vector2.Y -= 6f;
                                        break;
                                    case 682:
                                        num = 4f;
                                        break;
                                    case 758:
                                        num -= 20f;
                                        vector2.Y += 0.0f;
                                        break;
                                    case 759:
                                        num -= 18f;
                                        vector2.Y += 2f;
                                        break;
                                    case 760:
                                        num -= 12f;
                                        vector2.Y += 2f;
                                        break;
                                    case 779:
                                        num = 0.0f;
                                        vector2.Y += 2f;
                                        break;
                                    case 905:
                                        num = -5f;
                                        vector2.Y += 0.0f;
                                        break;
                                    case 930:
                                        num = 4f;
                                        vector2.Y += 2f;
                                        break;
                                    case 986:
                                        num = 6f;
                                        vector2.Y -= 10f;
                                        break;
                                    case 1946:
                                        num -= 12f;
                                        vector2.Y += 2f;
                                        break;
                                    case 3788:
                                        num = 2f;
                                        vector2.Y += 2f;
                                        break;
                                    case 3870:
                                        num = 4f;
                                        vector2.Y += 4f;
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            vector2.X = num;
            return vector2;
        }
    }
}