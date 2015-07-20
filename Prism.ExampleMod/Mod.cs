﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Prism.API;
using Terraria;
using Terraria.ID;

namespace Prism.ExampleMod
{
    public class Mod : ModDef
    {
        bool hasPizza = false;
        bool hasAnt = false;
        bool hasPizzant = false;
        bool hasPizzantzioli = false;

        protected override Dictionary<string, ItemDef> GetItemDefs()
        {
            return new Dictionary<string, ItemDef>
            {
                { "Pizza", new ItemDef("Pizza", getTex: () => GetResource<Texture2D>("Resources\\Textures\\Items\\Pizza.png"),
                    descr: new ItemDescription("LOTZA SPA-pizza. It's pizza.", "'MMmmmmmmm'", false, true),
                    useTime: 15,
                    useAnimation: 15,
                    consumable: true,
                    maxStack: 999,
                    rare: ItemRarity.Lime,
                    useSound: 2,
                    useStyle: ItemUseStyle.Eat,
                    holdStyle: ItemHoldStyle.HeldLightSource,
                    healLife: 400,
                    value: new ItemValue(50, 10, 2),
                    buff: new ItemBuff(BuffID.WellFed, 60 * 60 * 30)
                    ) },
                { "Ant", new ItemDef("Ant", getTex: () => GetResource<Texture2D>("Resources\\Textures\\Items\\Ant.png"),
                    descr: new ItemDescription("By ants, for ants.", "'B-but...ants aren't this big!'", false, true),
                    damageType: ItemDamageType.Melee,
                    autoReuse: true,
                    useTime: 12,
                    useAnimation: 20,
                    maxStack: 1,
                    rare: ItemRarity.LightPurple,
                    useSound: 1,
                    damage: 60,
                    knockback: 4f,
                    width: 30,
                    height: 30,
                    useStyle: ItemUseStyle.Stab,
                    holdStyle: ItemHoldStyle.Default,
                    value: new ItemValue(0, 40, 8, 25),
                    scale: 1.1f
                    ) },
                { "Pizzant", new ItemDef("Pizzant", getTex: () => GetResource<Texture2D>("Resources\\Textures\\Items\\Pizzant.png"),
                    descr: new ItemDescription("The chaotic forces of italian spices and insects and bread.", "", false, true),
                    damageType: ItemDamageType.Melee,
                    autoReuse: true,
                    useTime: 15,
                    useAnimation: 20,
                    maxStack: 1,
                    rare: ItemRarity.Yellow,
                    useSound: 1,
                    damage: 80,
                    knockback: 4f,
                    width: 30,
                    height: 30,
                    useStyle: ItemUseStyle.Stab,
                    holdStyle: ItemHoldStyle.Default,
                    value: new ItemValue(1, 34, 1, 67),                    
                    scale: 1.1f
                    ) },
                { "Pizzantzioli", new ItemDef("Pizzantzioli", getTex: () => GetResource<Texture2D>("Resources\\Textures\\Items\\Pizzantzioli.png"),
                    descr: new ItemDescription("The forces of ants and pizza come together as one.", "The name is Italian for 'KICKING ASS'! YEAH!", false, true),
                    damageType: ItemDamageType.Melee,
                    autoReuse: true,
                    useTime: 20,
                    useAnimation: 16,
                    maxStack: 1,
                    rare: ItemRarity.Cyan,
                    useSound: 1,
                    damage: 150,
                    knockback: 10f,
                    width: 30,
                    height: 30,
                    useStyle: ItemUseStyle.Swing,
                    holdStyle: ItemHoldStyle.Default,
                    value: new ItemValue(2, 51, 3, 9),
                    scale: 1.1f
                    ) }
            };
        }

        public override void PostUpdate()
        {
            if (Main.keyState.IsKeyDown(Keys.Y) && !hasPizza && !Main.gameMenu && Main.hasFocus)
            {
                var inv = Main.player[Main.myPlayer].inventory;

                for (int i = 0; i < inv.Length; i++)
                {
                    if (inv[i].type == 0)
                    {
                        if (!hasPizza)
                        {
                            inv[i].SetDefaults(ItemDef.ByName["Pizza", Info.InternalName].Type);
                            hasPizza = true;
                            inv[i].stack = inv[i].maxStack;
                            continue;
                        }
                        else if (!hasAnt)
                        {
                            inv[i].SetDefaults(ItemDef.ByName["Ant", Info.InternalName].Type);
                            hasAnt = true;
                            inv[i].stack = inv[i].maxStack;
                            continue;
                        }
                        else if (!hasPizzant)
                        {
                            inv[i].SetDefaults(ItemDef.ByName["Pizzant", Info.InternalName].Type);
                            hasPizzant = true;
                            inv[i].stack = inv[i].maxStack;
                            continue;
                        }
                        else if (!hasPizzantzioli)
                        {
                            inv[i].SetDefaults(ItemDef.ByName["Pizzantzioli", Info.InternalName].Type);
                            hasPizzantzioli = true;
                            inv[i].stack = inv[i].maxStack;
                            continue;
                        }
                    }
                }
            }
        }
    }
}