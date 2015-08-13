﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Prism.Injector.Patcher
{
    static class ProjectilePatcher
    {
        static CecilContext   context;
        static MemberResolver  memRes;

        static TypeSystem typeSys;
        static TypeDefinition typeDef_Projectile;

        static void WrapSetDefaults()
        {
            typeDef_Projectile.GetMethod("SetDefaults", MethodFlags.Public | MethodFlags.Instance, typeSys.Int32).Wrap(context);
        }
        static void AddFieldForBHandler()
        {
            typeDef_Projectile.Fields.Add(new FieldDefinition("P_BHandler", FieldAttributes.Public, typeSys.Object));
        }

        internal static void Patch()
        {
            context = TerrariaPatcher.context;
            memRes = TerrariaPatcher.memRes;

            typeSys = context.PrimaryAssembly.MainModule.TypeSystem;
            typeDef_Projectile = memRes.GetType("Terraria.Projectile");

            WrapSetDefaults();
            AddFieldForBHandler();
        }
    }
}
