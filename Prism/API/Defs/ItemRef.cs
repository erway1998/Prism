﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Prism.Mods;
using Prism.Mods.DefHandlers;
using Prism.Util;
using Terraria;
using Terraria.ID;

namespace Prism.API.Defs
{
    public class ItemRef : EntityRefWithId<ItemDef>
    {
        static string ToResName(int id)
        {
            ItemDef ii = null;
            if (Handler.ItemDef.DefsByType.TryGetValue(id, out ii))
                return ii.InternalName;

            string r = null;
            if (Handler.ItemDef.IDLUT.TryGetValue(id, out r))
                return r;

            throw new ArgumentException("id", "Unknown Item ID '" + id + "'.");
        }

        public ItemRef(int resourceId)
            : base(resourceId, ToResName)
        {
            if (resourceId >= ItemID.Count)
                throw new ArgumentOutOfRangeException("resourceId", "The resourceId must be a vanilla Item type or netID.");
        }
        public ItemRef(ObjectRef objRef)
            : base(objRef, Assembly.GetCallingAssembly())
        {

        }
        public ItemRef(string resourceName, ModInfo mod)
            : base(new ObjectRef(resourceName, mod), Assembly.GetCallingAssembly())
        {

        }
        public ItemRef(string resourceName, string modName = null)
            : base(new ObjectRef(resourceName, modName, Assembly.GetCallingAssembly()), Assembly.GetCallingAssembly())
        {

        }

        [ThreadStatic]
        static ItemDef d;
        ItemRef(int resourceId, object ignore)
            : base(resourceId, id => Handler.ItemDef.DefsByType.TryGetValue(id, out d) ? d.InternalName : String.Empty)
        {

        }

        public static ItemRef FromIDUnsafe(int resourceId)
        {
            return new ItemRef(resourceId, null);
        }

        public override ItemDef Resolve()
        {
            ItemDef r;

            if (ResourceID.HasValue && Handler.ItemDef.DefsByType.TryGetValue(ResourceID.Value, out r))
                return r;

            if (String.IsNullOrEmpty(ModName) && Requesting != null && Requesting.ItemDefs.TryGetValue(ResourceName, out r))
                return r;

            if (IsVanillaRef)
            {
                if (!Handler.ItemDef.VanillaDefsByName.TryGetValue(ResourceName, out r))
                    throw new InvalidOperationException("Vanilla item reference '" + ResourceName + "' not found.");

                return r;
            }

            ModDef m;
            if (!ModData.ModsFromInternalName.TryGetValue(ModName, out m))
                throw new InvalidOperationException("Item reference '" + ResourceName + "' in mod '" + ModName + "' could not be resolved because the mod is not loaded.");
            if (!m.ItemDefs.TryGetValue(ResourceName, out r))
                throw new InvalidOperationException("Item reference '" + ResourceName + "' in mod '" + ModName + "' could not be resolved because the item is not loaded.");

            return r;
        }

        public static implicit operator Either<ItemRef, CraftGroup<ItemDef, ItemRef>>(ItemRef r)
        {
            return Either<ItemRef, CraftGroup<ItemDef, ItemRef>>.NewRight(r);
        }
        public static implicit operator Either<ItemRef, ItemGroup>(ItemRef r)
        {
            return Either<ItemRef, ItemGroup>.NewRight(r);
        }

        public static implicit operator ItemRef(Item it)
        {
            if (it.netID < ItemID.Count)
                return new ItemRef(it.netID);

            ItemDef d;
            if (Handler.ItemDef.DefsByType.TryGetValue(it.netID, out d))
                return d;

            throw new InvalidOperationException("Item '" + it + "' (" + it.netID + ") is not in the def database.");
        }
    }
}
